using System;
using Imarda.Lib;
using Imarda.Lib.Utilities.FileStorage;
using Imarda.Lib.Utilities.KnownPaths;

namespace FernBusinessBase.Storage
{
	public class ServiceYmdFileStorage : ServiceFileStorage, IServiceYmdFileStorage
	{
		private readonly Func<string, DateTime> _getDateTimeFromFileName;
		public ServiceYmdFileStorage(Func<string, DateTime> getDateTimeFromFileName)
		{
			Storage = new YmdFileStorage(getDateTimeFromFileName);
			PathManager = new KnownYmdPathManager();
			_getDateTimeFromFileName = getDateTimeFromFileName;
		}

		public bool MakeYmdDirectory(DateTime dt)
		{
			var pathYmdPart = IoUtils.MakeYmdDirectoryPath(DestDir, dt);
			return PathManager.CreatePath(pathYmdPart);
		}

		public bool MakeYmdDirectory(string fileName)
		{
			var dt = _getDateTimeFromFileName(fileName);
			return MakeYmdDirectory(dt);
		}
	}
}