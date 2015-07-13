using System;
using Imarda.Common.Cache;
using ImardaConfigurationBusiness.Interfaces.Profiles;

namespace ImardaConfigurationBusiness.Model.ExtensionProfile
{
	public class ProfileCache : BaseConcurrentCache<Guid, IProfile>,IProfileCache
	{
		public override bool AddEntry(IProfile profile)
		{
			return Dictionary.TryAdd(profile.ID, profile); ;
		}
	}
}
