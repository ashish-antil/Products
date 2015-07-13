
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{

		#region Operation Contracts for UsageLog
		
		[OperationContract]
		GetListResponse<UsageLog> GetUsageLogListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<UsageLog> GetUsageLogList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveUsageLogList(SaveListRequest<UsageLog> request);

		[OperationContract]
		BusinessMessageResponse SaveUsageLog(SaveRequest<UsageLog> request);

		[OperationContract]
		BusinessMessageResponse DeleteUsageLog(IDRequest request);

		[OperationContract]
		GetItemResponse<UsageLog> GetUsageLog(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetUsageLogUpdateCount(GetUpdateCountRequest request);
		#endregion

		[OperationContract]
		GetListResponse<UsageLog> GetUsageLogSummary(IDRequest request);

		[OperationContract]
		GetListResponse<UsageLog> GetUsageLogSummaryByDateRange(IDRequest request);

		[OperationContract]
		GetListResponse<UsageLog> GetApiMethodList(IDRequest request);

		[OperationContract]
		GetListResponse<UsageLog> GetApiMethodBreakDownList(IDRequest request);
		
	}
}