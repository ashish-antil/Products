using System;
using System.IO;

namespace Imarda.Lib.Utilities.FileStorage
{
	public abstract class YmdFileStorageBase : FileStorageBase
	{
		protected YmdFileStorageBase()
		{
			MakeStorageDirectoryPath = filename =>
			{
				var dt = GetDateTimeFromFileName(filename);
				var ymdPath = IoUtils.MakeYmdDirectoryPath(dt);
				return Path.Combine(DestDir, ymdPath);
			};
		}

		protected virtual Func<string, DateTime> GetDateTimeFromFileName { get; set; }
	}

	public class YmdFileStorage : YmdFileStorageBase
	{
		public YmdFileStorage(Func<string, DateTime> getDateTimeFromFileName)
		{
			GetDateTimeFromFileName = getDateTimeFromFileName;
		}
		protected override sealed Func<string, DateTime> GetDateTimeFromFileName { get; set; }
	}

}
