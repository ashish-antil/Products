
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness 
{
	partial interface IImardaSecurity 
	{

		#region Operation Contracts for ScopeAccess
		
		[OperationContract]
		GetListResponse<ScopeAccess> GetScopeAccessListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<ScopeAccess> GetScopeAccessList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveScopeAccessList(SaveListRequest<ScopeAccess> request);

		[OperationContract]
		BusinessMessageResponse SaveScopeAccess(SaveRequest<ScopeAccess> request);

		[OperationContract]
		BusinessMessageResponse DeleteScopeAccess(IDRequest request);

		[OperationContract]
		GetItemResponse<ScopeAccess> GetScopeAccess(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetScopeAccessUpdateCount(GetUpdateCountRequest request);
		#endregion

		[OperationContract]
		BusinessMessageResponse UpdateScopeAccessList(SaveListRequest<ScopeAccess> request);
		

	}
}