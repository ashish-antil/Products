
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{

		#region Operation Contracts for ContactMap
		
		[OperationContract]
		GetListResponse<ContactMap> GetContactMapListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<ContactMap> GetContactMapList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveContactMapList(SaveListRequest<ContactMap> request);

		[OperationContract]
		BusinessMessageResponse SaveContactMap(SaveRequest<ContactMap> request);

		[OperationContract]
		BusinessMessageResponse DeleteContactMap(IDRequest request);

		[OperationContract]
		GetItemResponse<ContactMap> GetContactMap(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetContactMapUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}