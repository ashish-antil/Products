using System;
using Imarda.Lib;
using Imarda.Logging;
using ImardaConfigurationBusiness.Interfaces.Profiles;

namespace ImardaConfigurationBusiness.Model.ExtensionProfile
{
	public class ExtensionProfileCacheManager : ProfileCacheManagerBase
	{
		public ExtensionProfileCacheManager(ILogger log, IProfileCache profileCache, IProfileProvider profileProvider) 
			: base(log, profileCache, profileProvider)
		{
		}

		protected override IProfile MakeDefaultInactiveProfile(Guid companyId, Guid extensionId)
		{
			return new Model.ExtensionProfile.ExtensionProfile
				{
					ID = SequentialGuid.NewDbGuid(),
					Active = false,
					CompanyID = companyId,
					Description = "Default Inactive Profile",
					ExtensionID = extensionId,
					Name = "Default Inactive"
				};
		}
	}
}
