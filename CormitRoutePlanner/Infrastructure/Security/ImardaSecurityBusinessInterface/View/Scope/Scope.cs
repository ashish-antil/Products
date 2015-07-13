using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness 
{
	partial interface IImardaSecurity 
	{

		#region Operation Contracts for Scope
		
		[OperationContract]
		GetListResponse<Scope> GetScopeListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<Scope> GetScopeList(IDRequest request);

		[OperationContract]
		GetListResponse<Scope> GetScopeExtent(GetFilteredExtentRequest request);

		[OperationContract]
		BusinessMessageResponse SaveScopeList(SaveListRequest<Scope> request);

		[OperationContract]
		BusinessMessageResponse SaveScope(SaveRequest<Scope> request);

		[OperationContract]
		BusinessMessageResponse DeleteScope(IDRequest request);

		[OperationContract]
		GetItemResponse<Scope> GetScope(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetScopeUpdateCount(GetUpdateCountRequest request);
		#endregion

		[OperationContract]
		GetListResponse<Scope> GetApiScopeList(IDRequest request);

		[OperationContract]
		GetListResponse<Scope> GetAssignedScopeList(IDRequest request);

	}
}