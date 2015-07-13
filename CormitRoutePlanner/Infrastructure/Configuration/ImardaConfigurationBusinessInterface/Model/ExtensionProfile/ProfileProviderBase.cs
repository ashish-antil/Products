using System;
using System.Collections.Generic;
using System.Linq;
using FernBusinessBase;
using FernBusinessBase.Errors;
using FernBusinessBase.Extensions;
using ImardaConfigurationBusiness.Interfaces.Profiles;

namespace ImardaConfigurationBusiness.Model.ExtensionProfile
{
	public abstract class ProfileProviderBase : IProfileProvider
	{
		private readonly Proxy<IImardaConfiguration> _proxy;
		protected ProfileProviderBase(Proxy<IImardaConfiguration> proxy)
		{
			_proxy = proxy;
		}
		public List<IProfileRule> GetProfileRules(Guid extensionId, Guid profileId, bool loadOnlyDefaultRules)
		{
			var request = new IDListRequest(new[] {extensionId, profileId});
			request.AddParameters(ProfileConstants.ReqParamOnlyDefault, loadOnlyDefaultRules);
			var response = _proxy.Invoke(service => service.GetProfileRules(request));
			ErrorHandler.Check(response);
			return response.List.OfType<IProfileRule>().ToList();
		}

		public List<IProfile> GetProfileListForExtension(Guid companyId, Guid extensionId)
		{
			var request = new IDRequest(extensionId) { CompanyID = companyId };
			var response = _proxy.Invoke(service => service.GetProfileListForExtension(request));
			ErrorHandler.Check(response);
			return response.List.OfType<IProfile>().ToList();
		}

		public IProfile GetProfile(Guid profileId)
		{
			var request = new IDRequest(profileId);
			var response = _proxy.Invoke(service => service.GetExtensionProfile(request));
			ErrorHandler.Check(response);
			return response.Item;
		}
	}
}
