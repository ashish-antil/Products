using System;
using System.IO;
using ServiceStack.Common.Extensions;

namespace Imarda.Lib.Utilities.FileStorage
{
	/// <summary>
	/// Basic implementation of IFileStorage
	/// </summary>
	public abstract class FileStorageBase : IFileStorage
	{
		protected FileStorageBase()
		{
			MakeStorageDirectoryPath = filename => Path.Combine(DestDir, filename);
		}

		/// <summary>
		/// Default implementation combining DestDir and the filename provided - can be overriden in child class
		/// </summary>
		public Func<string, string> MakeStorageDirectoryPath { get; set; }

		public bool CheckDirectory { get; set; }
		public bool Retry { get; set; }
		public string DestDir { get; set; }
		public string SourceDir { get; set; }
		public string Extension { get; set; }

		/// <summary>
		/// Default implementation using IoUtils.MoveFile
		/// </summary>
		/// <param name="shortFileName"></param>
		/// <returns></returns>
		public virtual bool MoveFile(string shortFileName)
		{
			return MoveOrCopyFile(shortFileName);
		}

		private bool MoveOrCopyFile(string shortFileName, bool isCopy = false)
		{
			return IoUtils.MoveFile(Path.Combine(SourceDir, shortFileName), Path.Combine( MakeStorageDirectoryPath(shortFileName),shortFileName), CheckDirectory, Retry,isCopy);
		}

		public bool MoveAll(bool checkFiles = false)
		{
			return MoveOrCopyAll(checkFiles);
		}
		private bool MoveOrCopyAll(bool checkFiles = false, bool isCopy = false)
		{
			Extension.ThrowIfNull("Extension cannot be null.");
			var result = true;
			var allFiles = Directory.GetFiles(SourceDir, "*" + Extension);
			foreach (var file in allFiles)
			{
				var canMove = !checkFiles || IoUtils.CheckFileCreationCompleted(file); //either we don't check OR the file can be moved
				if(!canMove) {continue;}

				var fileName = Path.GetFileName(file);
				result &= MoveOrCopyFile(fileName, isCopy);
			}
			return result;
		}

		public bool CopyFile(string shortFileName)
		{
			return MoveOrCopyFile(shortFileName,true);
		}

		public bool CopyAll(bool checkFiles = false)
		{
			return MoveOrCopyAll(checkFiles,true);
		}

		public bool ProcessAll(Func<string,bool> process )
		{
			Extension.ThrowIfNull("Extension cannot be null.");
			var result = true;
			var allFiles = Directory.GetFiles(SourceDir, "*" + Extension);
			foreach (var file in allFiles)
			{
				result &= process(file);
			}
			return result;
		}

	}

	public class FileStorage : FileStorageBase
	{
	}

}
