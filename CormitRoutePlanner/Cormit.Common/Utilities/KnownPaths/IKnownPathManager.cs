using System;

namespace Imarda.Lib.Utilities.KnownPaths
{
	/// <summary>
	/// Defines a path manager with functions to create directories (or files) and keep track of known paths
	/// </summary>
	public interface IKnownPathManager
	{
		bool PathExists(string path);
		bool CreatePath(string path);
		bool CreatePathIfNotExists(string path);
		void Flush(int param);/// <summary>
		/// Function used to create folder or file to disk
		/// </summary>
		Func<string,bool> Create { get; }
		/// <summary>
		/// Additional check on whether a path can be removed from internal list
		/// </summary>
		Func<string, bool> CanFlush { get; }
	}
}
