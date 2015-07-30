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

		#region Operation Contracts for ApplicationFeatureCategory
		[OperationContract]
		GetListResponse<ApplicationFeatureCategory> GetApplicationFeatureCategoryListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<ApplicationFeatureCategory> GetApplicationFeatureCategoryList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveApplicationFeatureCategoryList(SaveListRequest<ApplicationFeatureCategory> request);

		[OperationContract]
		BusinessMessageResponse SaveApplicationFeatureCategory(SaveRequest<ApplicationFeatureCategory> request);

		[OperationContract]
		BusinessMessageResponse DeleteApplicationFeatureCategory(IDRequest request);

		[OperationContract]
		GetItemResponse<ApplicationFeatureCategory> GetApplicationFeatureCategory(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetApplicationFeatureCategoryUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}