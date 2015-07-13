using System;
using FernBusinessBase;
using FernBusinessBase.Errors;
using ImardaConfigurationBusiness.Model.ExtensionProfile;

// ReSharper disable once CheckNamespace
namespace ImardaConfigurationBusiness
{
	partial class ImardaConfiguration
	{
		public GetListResponse<ExtensionProfileRule> GetProfileRules(IDListRequest request)
		{
			try
			{
				//IDRequest extensionId, IDRequest profileId
				var arr = request.ToArray();
				var extensionId = arr[0];
				var profileId = arr[1];
				var loadOnlyDefaultRules=false;
				if (request.HasParameters)
				{
					request.Get(ProfileConstants.ReqParamOnlyDefault, out loadOnlyDefaultRules);
				}

				return ImardaDatabase.GetList<ExtensionProfileRule>("SPGetExtensionProfileRuleList", new object[] { extensionId, profileId, loadOnlyDefaultRules });
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ExtensionProfileRule>>(ex);
			}
		}

		public GetListResponse<ExtensionProfile> GetProfileListForExtension(IDRequest extensionId)
		{
			try
			{
				return ImardaDatabase.GetList<ExtensionProfile>("SPGetExtensionProfileList", extensionId.CompanyID, extensionId.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ExtensionProfile>>(ex);
			}
		}

		public GetItemResponse<ExtensionProfile> GetExtensionProfile(IDRequest profileId)
		{
			try
			{
				return ImardaDatabase.GetItem<ExtensionProfile>("SPGetExtensionProfile", profileId.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ExtensionProfile>>(ex);
			}
		}
	}
}
