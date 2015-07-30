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

		#region Operation Contracts for ApplicationFeature
		[OperationContract]
		GetListResponse<ApplicationFeature> GetApplicationFeatureListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<ApplicationFeature> GetApplicationFeatureList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveApplicationFeatureList(SaveListRequest<ApplicationFeature> request);

		[OperationContract]
		BusinessMessageResponse SaveApplicationFeature(SaveRequest<ApplicationFeature> request);

		[OperationContract]
		BusinessMessageResponse DeleteApplicationFeature(IDRequest request);

		[OperationContract]
		GetItemResponse<ApplicationFeature> GetApplicationFeature(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetApplicationFeatureUpdateCount(GetUpdateCountRequest request);
		#endregion
	}
}