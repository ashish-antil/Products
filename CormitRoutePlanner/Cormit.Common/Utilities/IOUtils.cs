using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Imarda.Lib.Extensions;

// ReSharper disable once CheckNamespace
namespace Imarda.Lib
{
	public static class IoUtils
	{
		public static string GetFileText(string fileName)
		{
			return File.ReadAllText(fileName);
		}

		public static bool WriteFileText2(string fileName, List<string> lines)
		{
			var sb = new StringBuilder();
			lines.ForEach(l => sb.AppendLine(l));
			var fileContent = sb.ToString();
			File.WriteAllText(fileName, fileContent);
			return true;
		}

	    public static string GetfileName(string filepath)
	    {
	        string filename = string.Empty;

	        if (!string.IsNullOrEmpty(filepath))
	        {
                filename = filepath.Split("\\".ToCharArray())[filepath.Split("\\".ToCharArray()).Length - 1];
	        }
	        return filename;
	    }

		public static bool WriteFileText2(string fileName, string fileContent)
		{
			File.WriteAllText(fileName, fileContent);
			return true;
		}

		[Obsolete("use version 2 which throws exception and handle them")]
		public static bool WriteFileText(string fileName, List<string> lines)
		{
			try
			{
				var sb = new StringBuilder();
				lines.ForEach(l => sb.AppendLine(l));
				var fileContent = sb.ToString();
				File.WriteAllText(fileName, fileContent);
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			return false;
		}

		[Obsolete("use version 2 which throws exception and handle them")]
		public static bool WriteFileText(string fileName, string fileContent)
		{
			try
			{
				File.WriteAllText(fileName, fileContent);
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			return false;
		}

		/// <summary>
		/// Creates a series of folder in root based on year\month\day found in datetime
		/// </summary>
		/// <param name="root"></param>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string MakeYmdDirectoryPath(string root, DateTime dt)
		{
			var path = MakeYmdDirectoryPath(dt);
			path = Path.Combine(root, path);
			return path;
		}

		public static string MakeInjectorFileYmdDirectoryPath(string root, string fileName)
		{
			var dt = DateFromInjectorFileName(fileName);
			return MakeYmdDirectoryPath(root, dt);
		}

		public static string MakeYmdDirectoryPath(DateTime dt)
		{
			var path = string.Format("{0}\\{1}\\{2}\\", dt.Year, dt.Month.ToString("00"), dt.Day.ToString("00"));
			return path;
		}

		public static DateTime GetDateTimeFromYmdPath(string path)
		{
			var splits = path.Split(new[] { '\\' },StringSplitOptions.RemoveEmptyEntries);
			var c = splits.Length;
			var ymd = splits.Skip(c - 3).Select(int.Parse).ToArray();
			return new DateTime(ymd[0], ymd[1], ymd[2]);
		}

		/// <summary>
		/// Provided for handling of videos in IDS injector cf. DateFromVideoFileName
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="posFromLast">Added to handle video with name like xxx_20150112T003224Z_NF.mp4</param>
		/// <returns>DateTime from 20150112T003224Z</returns>
		public static DateTime DateFromInjectorFileName(string fileName, int posFromLast)
		{
			var splits = fileName.Split('_');
			var dateString = splits.Take(splits.Length - posFromLast).Last();
			dateString = dateString.Split('.')[0];
			const string dtFormat = "yyyyMMddTHHmmssZ";
			var dt = DateTime.ParseExact(dateString, dtFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
			return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
		}

		/// <summary>
		/// Returns date from IDS or EMS files such as whatever_20150112T003224Z.ext - Default DateFromInjectorFileName used by IDS injector and viewer
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns>DateTime from 20150112T003224Z</returns>
		public static DateTime DateFromInjectorFileName(string fileName)
		{
			return DateFromInjectorFileName(fileName, 0);
		}

		/// <summary>
		/// Move a files in the file system - common to EMS and IDS injectors
		/// </summary>
		/// <param name="originFullPath"></param>
		/// <param name="destFolder"></param>
		/// <param name="destFileName"></param>
		/// <param name="checkDirectory"></param>
		/// <param name="withRetries"></param>
		/// <param name="isCopy">Copies file to destination but does not remove original</param>
		/// <returns></returns>
		public static bool MoveFile(string originFullPath, string destFolder, string destFileName, bool checkDirectory = false, bool withRetries = true, bool isCopy = false)
		{
			var fullFilePath = Path.Combine(destFolder, destFileName);
			return MoveFile(originFullPath, fullFilePath, checkDirectory, withRetries, isCopy);
		}

		public static bool MoveFile(string originFullPath, string fullFilePath, bool checkDirectory = false, bool withRetries = true, bool isCopy=false)
		{
			var moveRetries = 0;
			var numRetries = withRetries ? 3 : 0;
			while (moveRetries <= numRetries)
			{
				try
				{
					//IM-5924 no directory check to reduce file system load except for videos - all other usages are relying on dirs which we check on start
					if (checkDirectory)
					{
						//check that directory exists
						fullFilePath.ThrowIfNull("fullFilePath cannot be null.");
						// ReSharper disable once AssignNullToNotNullAttribute
						Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath));
					}
					var fn = fullFilePath;
					//Check for existing file
					if (File.Exists(fn))
					{
						File.Delete(fn);
					}
					if(isCopy)
					{
						File.Copy(originFullPath, fn,true);
					}
					else
					{
						File.Move(originFullPath, fn);
					}
					return true;
				}
				catch (Exception)
				{
					if (moveRetries == numRetries)
					{
						throw;
					}
					moveRetries++;
					Thread.Sleep(50);
				}
			}
			return true;
		}

		public static bool CheckFileCreationCompleted(string filename)
		{
			// If the file can be opened for exclusive access it means that the file
			// is no longer locked by another process.
			try
			{
				using (File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
				{
					return true;
				}
			}
			catch (IOException)
			{
				return false;
			}
		}

	}
}