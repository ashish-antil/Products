
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness 
{
	partial interface IImardaSecurity 
	{

		#region Operation Contracts for Client
		
		[OperationContract]
		GetListResponse<Client> GetClientListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<Client> GetClientList(IDRequest request);

		[OperationContract]
		GetListResponse<Client> GetClientExtent(GetFilteredExtentRequest request);

		[OperationContract]
		BusinessMessageResponse SaveClientList(SaveListRequest<Client> request);

		[OperationContract]
		BusinessMessageResponse SaveClient(SaveRequest<Client> request);

		[OperationContract]
		BusinessMessageResponse DeleteClient(IDRequest request);

		[OperationContract]
		GetItemResponse<Client> GetClient(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetClientUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}