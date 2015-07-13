using System;

namespace FernBusinessBase.Storage
{
	public interface IServiceFileStorage
	{
		Func<string, string> MakeStorageDirPath { get; }
		bool CheckDirectory { get; }
		bool Retry { get; }
		string DestDir { get; }
		string SourceDir { get; }
		bool MoveFile(string shortFileName);
		bool CopyFile(string shortFileName);
		bool MakeDirectory(string path); 
		string Extension { get; set; }
		bool MoveAll(bool checkFiles = true);
		bool CopyAll(bool checkFiles = true);
		bool MakeAllDirectories(); 
	}
}
