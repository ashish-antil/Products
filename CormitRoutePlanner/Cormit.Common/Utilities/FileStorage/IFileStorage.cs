using System;

namespace Imarda.Lib.Utilities.FileStorage
{
	public interface IFileStorage
	{
		/// <summary>
		/// Function used to create a directory full path - e.g. combining input string with DestDir
		/// </summary>
		Func<string,string> MakeStorageDirectoryPath { get; set; }
		bool CheckDirectory { get; set; }
		bool Retry { get; set; }
		string DestDir { get; set; }
		string SourceDir { get; set; }
		string Extension { get; set; }
		bool MoveFile(string shortFileName);
		bool MoveAll(bool checkFiles = false);
		bool CopyFile(string shortFileName);
		bool CopyAll(bool checkFiles = false);
		bool ProcessAll(Func<string, bool> process);
	}
}
