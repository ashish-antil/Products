using System;
using System.Collections.Generic;

namespace ImardaConfigurationBusiness.Interfaces.Profiles
{
	public interface IProfileCache
	{
		bool AddEntry(IProfile profile);
		void Clear();
		IProfile GetEntry(Guid key);
		IEnumerable<IProfile> GetEntries(Func<IProfile, bool> predicate);
		void Purge(int howManyKeeps);
	}
}
