using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace RecipeLib
{
	internal class Watcher
	{
		readonly Recipe _Engine;
		private readonly string _Script;
		private FileSystemWatcher _FsWatcher;
		private readonly AutoResetEvent _Wait = new AutoResetEvent(false);
		private volatile bool _Run;
		

		internal Watcher(Recipe engine, string script)
		{
			_Engine = (Recipe)engine.Clone();
			_Script = script;
		}

		internal void ExecutionLoop(string events, string arg)
		{
			_Run = true;
			EnableWatch(events, arg);
			while (_Run)
			{
				string s = _Engine.GetMacro("timeout");
				int timeout = string.IsNullOrEmpty(s) ? Timeout.Infinite : int.Parse(s);
				string state = _Wait.WaitOne(timeout) ? "0" : "1";
				if (_Run)
				{
					_Engine.SetMacro("expired", state);
					_Engine.Run(new LineReader(_Script, name: "watch " + arg));
				}
			}
		}

		private void EnableWatch(string key, string args)
		{
			if (_FsWatcher != null) DisableWatch();
			bool recursive;
			string folder, pattern;
			Recipe.SplitPath(args.Trim(), out folder, out pattern, out recursive);
			_FsWatcher = new FileSystemWatcher(folder, pattern) { IncludeSubdirectories = recursive };
			if (key.Contains("C")) _FsWatcher.Created += FileSysEvent;
			if (key.Contains("U")) _FsWatcher.Changed += FileSysEvent;
			if (key.Contains("D")) _FsWatcher.Deleted += FileSysEvent;
			if (key.Contains("M")) _FsWatcher.Renamed += FileSysRenamedEvent;
			_FsWatcher.EnableRaisingEvents = true;
		}

		internal void DisableWatch()
		{
			_FsWatcher.EnableRaisingEvents = false;
			_FsWatcher.Created -= FileSysEvent;
			_FsWatcher.Changed -= FileSysEvent;
			_FsWatcher.Deleted -= FileSysEvent;
			_FsWatcher.Renamed -= FileSysRenamedEvent;
			_FsWatcher.Dispose();
			_FsWatcher = null;
		}

		private void FileSysEvent(object sender, FileSystemEventArgs e)
		{
			char c;
			switch (e.ChangeType)
			{
				case WatcherChangeTypes.Deleted: c = 'D'; break;
				case WatcherChangeTypes.Created: c = 'C'; break;
				case WatcherChangeTypes.Changed: c = 'U'; break;
				default: c = '?'; break;
			}
			Signal(c, e.FullPath);
		}

		private void FileSysRenamedEvent(object sender, RenamedEventArgs e)
		{
			Signal('M', e.FullPath, e.OldFullPath);
		}

		public void Signal(char c, params string[] args)
		{
			_Engine.SetMacro("signal", new string(c, 1));
			for (int i = 0; i < args.Length; i++) _Engine.SetMacro("arg" + i, args[i]);
			_Wait.Set();
		}



		internal void Stop()
		{
			DisableWatch();
			_Run = false;
			_Wait.Set();
		}
	}
}
