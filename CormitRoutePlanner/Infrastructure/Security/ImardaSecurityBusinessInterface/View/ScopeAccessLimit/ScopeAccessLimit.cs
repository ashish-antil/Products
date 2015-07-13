
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness 
{
	partial interface IImardaSecurity 
	{

		#region Operation Contracts for ScopeAccessLimit
		
		[OperationContract]
		GetListResponse<ScopeAccessLimit> GetScopeAccessLimitListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<ScopeAccessLimit> GetScopeAccessLimitList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveScopeAccessLimitList(SaveListRequest<ScopeAccessLimit> request);

		[OperationContract]
		BusinessMessageResponse SaveScopeAccessLimit(SaveRequest<ScopeAccessLimit> request);

		[OperationContract]
		BusinessMessageResponse DeleteScopeAccessLimit(IDRequest request);

		[OperationContract]
		GetItemResponse<ScopeAccessLimit> GetScopeAccessLimit(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetScopeAccessLimitUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}