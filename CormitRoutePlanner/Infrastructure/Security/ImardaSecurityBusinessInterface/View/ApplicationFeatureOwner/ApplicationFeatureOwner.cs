using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness 
{
	partial interface IImardaSecurity 
	{

		#region Operation Contracts for ApplicationFeatureOwner
		
		[OperationContract]
		GetListResponse<ApplicationFeatureOwner> GetApplicationFeatureOwnerListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<ApplicationFeatureOwner> GetApplicationFeatureOwnerList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveApplicationFeatureOwnerList(SaveListRequest<ApplicationFeatureOwner> request);

		[OperationContract]
		BusinessMessageResponse SaveApplicationFeatureOwner(SaveRequest<ApplicationFeatureOwner> request);

		[OperationContract]
		BusinessMessageResponse DeleteApplicationFeatureOwner(IDRequest request);

		[OperationContract]
		GetItemResponse<ApplicationFeatureOwner> GetApplicationFeatureOwner(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetApplicationFeatureOwnerUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}
