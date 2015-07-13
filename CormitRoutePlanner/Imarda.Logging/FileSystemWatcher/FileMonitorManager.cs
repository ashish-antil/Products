using Imarda.Lib;
using ImardaFileSystemWatcher;
using System;
using System.IO;
using System.Threading;

namespace Imarda.Logging
{
	public class FileMonitorManager
	{
		private string _MonitorFilter;
		private bool _MonitorIncludeSubs;
		private string _MonitorPath;
		private string _MonitorProcessingPath;
		private string _MonitorPassedPath;
		private string _MonitorFailedPath;
		private int _InternalBufferSize = 8 * 4096;
		private string _Name = string.Empty;

		public Func<string, bool> ProcessFile;
		private FileMonitor _FileMonitor;

		//& IM-4001
		private enum Duplicate
		{
			Error,
			Replace,
			ReplaceButBackupOriginal,
			CreateWithTimestampAppended
		};
		//. IM-4001

		public FileMonitorManager()
		{
			_FileMonitor = new FileMonitor();
			LoadSettings();
			_FileMonitor.MonitorFilter = _MonitorFilter;
			_FileMonitor.MonitorIncludeSubs = _MonitorIncludeSubs;
			_FileMonitor.MonitorPath = _MonitorPath;
			_FileMonitor.InternalBufferSize = _InternalBufferSize;
			_FileMonitor.FoundNewFile += FileMonitor_FoundNewFile;
		}

		public FileMonitorManager(string name) : this()
		{
			_Name = name;
		}

		public string MonitorPath { get { return _MonitorPath; } }

		private bool FileMonitor_FoundNewFile(string arg)
		{
            string processingPath = string.Empty ;
			Thread.Sleep(1000);
			try
			{
				//Move the file to processing Folder and pass the new path to the deligate
				processingPath = MoveToProcessing(arg, Duplicate.CreateWithTimestampAppended);	//# IM-4001
				if (ProcessFile(processingPath))
				{
					MoveToPassed(processingPath, Duplicate.CreateWithTimestampAppended);	//# IM-4001
				}
				else
				{
					MoveToFail(processingPath, Duplicate.CreateWithTimestampAppended);		//# IM-4001
				}

			}
			catch (Exception ex)
			{
                //# Move file fail folder in case of exception has occured.
                MoveToFail(processingPath, Duplicate.CreateWithTimestampAppended);
				DebugLog.Write(ex);
				return false;
			}
			return true;
		}

		public void Start()
		{
			LoadSettings();
			CheckFolder();
			if (ProcessFile == null)
			{
				DebugLog.Write("FileProcessor is not attached");
				throw new Exception("FileProcessor is not attached");
			}
			DebugLog.Write("File Monitor is starting and Watching at folder: {0} ", _MonitorPath);
			_FileMonitor.EnableRaisingEvents = true;
		}

		public void Stop()
		{
			_FileMonitor.EnableRaisingEvents = false;
		}

		private void LoadSettings()
		{
			_MonitorPath = ConfigUtils.GetString("MonitorPath." + _Name, "MonitorPath");
			_MonitorFilter = ConfigUtils.GetString("MonitorFilter." + _Name, "MonitorFilter");
			bool.TryParse(ConfigUtils.GetString("MonitorIncludeSubs." + _Name, "MonitorIncludeSubs"), out _MonitorIncludeSubs);

			_MonitorProcessingPath = ConfigUtils.GetString("MonitorProcessingPath." + _Name, "MonitorProcessingPath");
			_MonitorPassedPath = ConfigUtils.GetString("MonitorPassedPath." + _Name, "MonitorPassedPath");
			_MonitorFailedPath = ConfigUtils.GetString("MonitorFailedPath." + _Name, "MonitorFailedPath");
		}

		private void CheckFolder()
		{
			Directory.CreateDirectory(_MonitorPath);
			Directory.CreateDirectory(_MonitorProcessingPath);
			Directory.CreateDirectory(_MonitorPassedPath);
			Directory.CreateDirectory(_MonitorFailedPath);
		}

		private bool MoveToPassed(string filePath, Duplicate duplicate)		//# IM-4001
		{
			try
			{
				MoveFile(filePath, Path.Combine(_MonitorPassedPath, Path.GetFileName(filePath)), duplicate);	//# IM-4001
			}
			catch
			{
				return false;
			}
			return true;
		}

		private bool MoveToFail(string filePath, Duplicate duplicate)	//# IM-4001
		{
			try
			{
				MoveFile(filePath, Path.Combine(_MonitorFailedPath, Path.GetFileName(filePath)), duplicate);	//# IM-4001
			}
			catch
			{
				return false;
			}
			return true;

		}

		private string MoveToProcessing(string filePath, Duplicate duplicate)	//# IM-4001
		{
			try
			{
				MoveFile(filePath, Path.Combine(_MonitorProcessingPath, Path.GetFileName(filePath)), duplicate);	//# IM-4001
			}
			catch
			{
				//Not able to move file 
				throw;
			}
			return Path.Combine(_MonitorProcessingPath, Path.GetFileName(filePath));
		}

		private void MoveFile(string from, string topath, Duplicate duplicate = Duplicate.Error)	//# IM-4001
		{
			try
			{
				if (File.Exists(topath))
				{
					switch (duplicate)
					{
						case Duplicate.Error:
							DebugLog.Write("Duplicate - file already exists: " + topath);
							return;

						case Duplicate.Replace:
							File.Delete(topath);
							File.Move(from, topath);
							return;

						case Duplicate.ReplaceButBackupOriginal:
							File.Replace(from, topath, AddTimeStamp(topath));
							return;

						case Duplicate.CreateWithTimestampAppended:
							File.Move(from, AddTimeStamp(topath));
							return;
					}
				}

				File.Move(from, topath);
			}
			catch (Exception ex)
			{
				DebugLog.Write(ex);
			}

		}

		//& IM-4001
		private string AddTimeStamp(string toPath)
		{
			string extension = Path.GetExtension(toPath);
			int fileExtPos = toPath.LastIndexOf(".");
			if (fileExtPos >= 0)
				toPath = toPath.Substring(0, fileExtPos) + "_" + DateTime.UtcNow.ToString("s").Replace(":", "") + extension;
			else
				toPath += "_" + DateTime.UtcNow.ToString("s").Replace(":", "");
			return toPath;
		}
		//. IM-4001
	}
}
