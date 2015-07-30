using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using ImardaSecurityBusiness;


namespace Cormit.Application.RouteApplication.Security
{
	partial interface IImardaSecurity 
	{

		#region Operation Contracts for FeatureSupport
		[OperationContract]
		GetListResponse<FeatureSupport> GetFeatureSupportListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<FeatureSupport> GetFeatureSupportList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveFeatureSupportList(SaveListRequest<FeatureSupport> request);

		[OperationContract]
		BusinessMessageResponse SaveFeatureSupport(SaveRequest<FeatureSupport> request);

		[OperationContract]
		BusinessMessageResponse DeleteFeatureSupport(IDRequest request);

		[OperationContract]
		GetItemResponse<FeatureSupport> GetFeatureSupport(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetFeatureSupportUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}