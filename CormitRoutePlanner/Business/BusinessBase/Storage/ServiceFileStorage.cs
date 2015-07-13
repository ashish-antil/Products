using System;
using System.IO;
using Imarda.Lib.Utilities.FileStorage;
using Imarda.Lib.Utilities.KnownPaths;

namespace FernBusinessBase.Storage
{
	public class ServiceFileStorage : IServiceFileStorage
	{
		//the ServiceYmdFileStorage replaces the IFileStorage and IKnownPathManager by Ymd specific version
		//hence the implementation as a Decorator instead of Inheriting FileStorage

		private IFileStorage _storage;
		private IKnownPathManager _pathManager;

		public ServiceFileStorage()
		{
			_storage = new FileStorage();
			_pathManager = new KnownPathManager();
		}

		protected IFileStorage Storage
		{
			get { return _storage; }
			set { _storage = value; }
		}

		protected IKnownPathManager PathManager
		{
			get { return _pathManager; }
			set { _pathManager = value; }
		}

		public Func<string, string> MakeStorageDirPath
		{
			get { return _storage.MakeStorageDirectoryPath; }
			protected set { _storage.MakeStorageDirectoryPath = value; }
		}

		public bool CheckDirectory
		{
			get { return _storage.CheckDirectory; }
			set { _storage.CheckDirectory = value; }
		}

		public bool Retry
		{
			get { return _storage.Retry; }
			set { _storage.Retry = value; }
		}

		public string DestDir
		{
			get { return _storage.DestDir; }
			set { _storage.DestDir = value; }
		}

		public string SourceDir
		{
			get { return _storage.SourceDir; }
			set { _storage.SourceDir = value; }
		}

		public bool MoveFile(string shortFileName)
		{
			return _storage.MoveFile(shortFileName);
		}

		public bool CopyFile(string shortFileName)
		{
			return _storage.CopyFile(shortFileName);
		}

		public bool MakeDirectory(string path)
		{
			return PathManager.CreatePathIfNotExists(path);
		}

		public string Extension
		{
			get { return _storage.Extension; }
			set { _storage.Extension = value; }
		}

		public bool MoveAll(bool checkFiles = false)
		{
			return _storage.MoveAll(checkFiles);
		}

		public bool CopyAll(bool checkFiles = false)
		{
			return _storage.CopyAll(checkFiles);
		}

		public bool MakeAllDirectories()
		{
			var processFunc = new Func<string, bool>(fullSourceFileName =>
			{
				var fileName = Path.GetFileName(fullSourceFileName);
				var path = Storage.MakeStorageDirectoryPath(fileName);
				return PathManager.CreatePathIfNotExists(path);
			});
			return _storage.ProcessAll(processFunc);
		}
	}
}