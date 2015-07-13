using System;
using System.IO;
using Imarda.Logging;

namespace ImardaFileSystemWatcher
{
	public class FileMonitor
    {
		private FileSystemWatcher _FileMonitor;
		public event Func<string, bool> FoundNewFile;

		public FileMonitor()
		{
			_FileMonitor = new FileSystemWatcher();
			_FileMonitor.Error += _FileMonitor_Error;
			_FileMonitor.Created += _FileMonitor_Created;
		}
		void _FileMonitor_Created(object sender, FileSystemEventArgs e)
		{
			FoundNewFile(e.FullPath);
		}

		private void _FileMonitor_Error(object sender, ErrorEventArgs e)
		{
            DebugLog.Write(e);
		}

		private void CheckExistingFiles()
		{
			string[] filePaths = Directory.GetFiles(MonitorPath, MonitorFilter);
			foreach (string filepath in filePaths)
			{
				if (FoundNewFile != null)
				{
					FoundNewFile(filepath);
				}
			}
		}
		#region property

		public string MonitorFilter
		{
			get
			{
				return _FileMonitor.Filter;
			}
			set
			{
				_FileMonitor.Filter = value;
			}
		}

		public bool MonitorIncludeSubs 
		{
			get
			{
				return (_FileMonitor.IncludeSubdirectories);
			}
			set
			{
				_FileMonitor.IncludeSubdirectories = value;
			}
		}

		public string MonitorPath
		{
			get
			{
				return (_FileMonitor.Path);
			}
			set
			{
				_FileMonitor.Path = value;
			}
		}

		public int InternalBufferSize
		{
			get
			{
				return (_FileMonitor.InternalBufferSize);
			}
			set
			{
				_FileMonitor.InternalBufferSize = value;
			}
		}

		public bool EnableRaisingEvents
		{
			get
			{
				return _FileMonitor.EnableRaisingEvents;
			}
			set
			{
				if (value)
				{
					CheckExistingFiles();
				}
				_FileMonitor.EnableRaisingEvents = value;
			}
		}
		#endregion
	}
}
