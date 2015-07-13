using System.ServiceModel;
using FernBusinessBase;

// ReSharper disable once CheckNamespace
namespace ImardaConfigurationBusiness
{
	partial interface IImardaConfiguration
	{

		#region Operation Contracts for Profile

		[OperationContract]
		GetListResponse<Profile> GetProfileListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<Profile> GetProfileList(IDRequest request);

		[OperationContract]
		GetListResponse<Profile> GetProfileExtent(GetFilteredExtentRequest request);

		[OperationContract]
		BusinessMessageResponse SaveProfileList(SaveListRequest<Profile> request);

		[OperationContract]
		BusinessMessageResponse SaveProfile(SaveRequest<Profile> request);

		[OperationContract]
		BusinessMessageResponse DeleteProfile(IDRequest request);

		[OperationContract]
		GetItemResponse<Profile> GetProfile(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetProfileUpdateCount(GetUpdateCountRequest request);
		#endregion

		[OperationContract]
		GetListResponse<Profile> GetAssignedProfileList(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetI360ProfileCount(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetIACProfileCount(IDRequest request);

	}
}