using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using Common;
using RecipeLib;

namespace RecipeService
{
	public partial class RecipeService : ServiceBase
	{
		private static Recipe _RecipeExec;
		private static readonly object _LogSync = new object();

		private int _LogLevel;
		private string _ProgPath;

		public RecipeService()
		{
			InitializeComponent();
			_ProgPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			Log(0, "ProgPath={0}", _ProgPath);
		}

		private void InitializeRecipe()
		{
			try
			{
				ConfigUtils.RefreshAppSettings();
				string recipePath = ConfigUtils.GetString("Path", @"~prog\Main.rcp");
				recipePath = ExpandPath(recipePath);
				_LogLevel = ConfigUtils.GetValue("LogLevel", 1);

				var macros = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				// add all environment variables to the dictionary
				IDictionary env = Environment.GetEnvironmentVariables();
				foreach (string key in env.Keys) macros.Add(key, (string)env[key]);

				// parse config file arguments and add to dictionary
				string[] args = ConfigUtils.GetArray("Macros", "");
				var sb = new StringBuilder();
				if (args != null)
				{
					foreach (string t in args)
					{
						string[] nv = t.Split('=');
						if (nv.Length == 2)
						{
							string key = nv[0].Trim();
							string value = ExpandPath(nv[1].Trim());
							macros[key] = value;
							sb.Append(key).Append('=').Append(value).Append('|');
						}
						else throw new Exception("Invalid argument: " + t);
					}
				}
				Log(0, "Macros from configuration: [{0}]", sb.ToString().TrimEnd('|'));
				_RecipeExec = new Recipe { Write = true };
				_RecipeExec.ClearHandlers();
				_RecipeExec.Message += HandleMessage;
				_RecipeExec.SetRootPath(recipePath);

				using (LineReader sr = new LineReader(File.OpenText(recipePath), name: recipePath))
				{
					Log(0, "Running: [{0}] LogLevel: {1}", recipePath, _LogLevel);
					_RecipeExec.Run(sr, macros, string.Empty);
				}
			}
			catch (ApplicationException)
			{
				Log(0, "Recipe Exit {0}");
			}
			catch (Exception ex)
			{
				Log(1, "{0}", ex);
			}
		}

		private string ExpandPath(string s)
		{
			return s.Replace("~prog", _ProgPath);
		}

		private void Log(int level, string fmt, params object[] args)
		{
			if (level <= _LogLevel)
			{
				lock (_LogSync)
				{
					const string source = "RecipeService";
					if (!EventLog.SourceExists(source)) EventLog.CreateEventSource(source, "Application");

					EventLogEntryType type;
					switch (level)
					{
						case 1:
							type = EventLogEntryType.Error;
							break;
						case 2:
							type = EventLogEntryType.Warning;
							break;
						default:
							type = EventLogEntryType.Information; // note case 0 also is information, but has highest level (0)
							break;
					}
					string msg = string.Format(fmt, args);
					EventLog.WriteEntry(source, msg, type);
				}
			}
		}


		private void HandleMessage(string msg)
		{
			// #write will write to the log unconditionally, and so does the 'info' cmd inside a #with-block
			// these messages always start with a '~'
			if (msg != null && msg.Length > 1 && msg[0] == '~') Log(0, msg.Substring(1));
			else Log(3, "{0}", msg); // normal msg
		}

		protected override void OnStart(string[] args)
		{
			InitializeRecipe();
		}

		protected override void OnStop()
		{
			_RecipeExec.StopServer();
			_RecipeExec.Stop();
		}
	}
}
