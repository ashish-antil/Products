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

		#region Operation Contracts for ApplicationPlan
		[OperationContract]
		GetListResponse<ApplicationPlan> GetApplicationPlanListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<ApplicationPlan> GetApplicationPlanList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveApplicationPlanList(SaveListRequest<ApplicationPlan> request);

		[OperationContract]
		BusinessMessageResponse SaveApplicationPlan(SaveRequest<ApplicationPlan> request);

		[OperationContract]
		BusinessMessageResponse DeleteApplicationPlan(IDRequest request);

		[OperationContract]
		GetItemResponse<ApplicationPlan> GetApplicationPlan(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetApplicationPlanUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}