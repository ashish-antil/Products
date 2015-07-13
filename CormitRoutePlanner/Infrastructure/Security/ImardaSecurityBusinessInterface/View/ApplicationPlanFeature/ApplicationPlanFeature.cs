
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness 
{
	partial interface IImardaSecurity 
	{

		#region Operation Contracts for ApplicationPlanFeature
		
		[OperationContract]
		GetListResponse<ApplicationPlanFeature> GetApplicationPlanFeatureListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<ApplicationPlanFeature> GetApplicationPlanFeatureList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveApplicationPlanFeatureList(SaveListRequest<ApplicationPlanFeature> request);

		[OperationContract]
		BusinessMessageResponse SaveApplicationPlanFeature(SaveRequest<ApplicationPlanFeature> request);

		[OperationContract]
		BusinessMessageResponse DeleteApplicationPlanFeature(IDRequest request);

		[OperationContract]
		GetItemResponse<ApplicationPlanFeature> GetApplicationPlanFeature(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetApplicationPlanFeatureUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}
