using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness 
{
	partial interface IImardaSecurity 
	{

		#region Operation Contracts for ApiMethod
		
		[OperationContract]
		GetListResponse<ApiMethod> GetApiMethodListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<ApiMethod> GetApiMethodList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveApiMethodList(SaveListRequest<ApiMethod> request);

		[OperationContract]
		BusinessMessageResponse SaveApiMethod(SaveRequest<ApiMethod> request);

		[OperationContract]
		BusinessMessageResponse DeleteApiMethod(IDRequest request);

		[OperationContract]
		GetItemResponse<ApiMethod> GetApiMethod(IDRequest request);

		[OperationContract]
		GetItemResponse<ApiMethod> GetApiMethodByName(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetApiMethodUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}