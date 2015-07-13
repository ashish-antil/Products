using System;
using System.Collections.Generic;

namespace ImardaConfigurationBusiness.Interfaces.Profiles
{
	public interface IProfileCacheManager
	{
		void ClearProfiles();
		//defines the class providing the profiles to the extension
		IProfileProvider ProfileProvider { get; }
		IProfileCache ProfileCache { get; }
		List<IProfile> GetProfileListForExtension(Guid companyId, Guid extensionId, bool loadDefaultRules);
		int CounterMax { get; set; }
		int ProfileMaxCount { get; set; }
	}
}
