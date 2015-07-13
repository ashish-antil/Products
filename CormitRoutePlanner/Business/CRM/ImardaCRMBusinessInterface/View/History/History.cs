//& gs-104
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{

		#region Operation Contracts for History
		
		[OperationContract]
		GetListResponse<History> GetHistoryListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<History> GetHistoryList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveHistoryList(SaveListRequest<History> request);

		[OperationContract]
		BusinessMessageResponse SaveHistory(SaveRequest<History> request);

		[OperationContract]
		BusinessMessageResponse DeleteHistory(IDRequest request);

		[OperationContract]
		GetItemResponse<History> GetHistory(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetHistoryUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}