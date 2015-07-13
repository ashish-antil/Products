using System;
using System.Collections.Generic;

namespace ImardaConfigurationBusiness.Interfaces.Profiles
{
	public interface IProfileProvider
	{
		List<IProfileRule> GetProfileRules(Guid extensionId,Guid profileId, bool loadOnlyDefaultRules);
		List<IProfile> GetProfileListForExtension(Guid companyId, Guid extensionId);
		IProfile GetProfile(Guid profileId);
	}

}
