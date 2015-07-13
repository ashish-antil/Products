using System.ServiceModel;
using FernBusinessBase;
using ImardaConfigurationBusiness.Model.ExtensionProfile;

// ReSharper disable once CheckNamespace
namespace ImardaConfigurationBusiness
{
	[ServiceContract]
	public interface IProfileProviderService
	{
		[OperationContract]
		GetListResponse<ExtensionProfileRule> GetProfileRules(IDListRequest request);
		[OperationContract]
		GetListResponse<ExtensionProfile> GetProfileListForExtension(IDRequest extensionId);
		[OperationContract]
		GetItemResponse<ExtensionProfile> GetExtensionProfile(IDRequest profileId);
	}
}
