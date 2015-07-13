using System;

namespace FernBusinessBase.Storage
{
	public interface IServiceYmdFileStorage : IServiceFileStorage
	{
		bool MakeYmdDirectory(DateTime dt);
		bool MakeYmdDirectory(string fileName);
	}
}