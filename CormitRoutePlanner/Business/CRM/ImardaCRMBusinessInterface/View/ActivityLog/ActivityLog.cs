
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{

		#region Operation Contracts for ActivityLog
		
		[OperationContract]
		GetListResponse<ActivityLogEntry> GetActivityLogEntryListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<ActivityLogEntry> GetActivityLogEntryList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveActivityLogEntryList(SaveListRequest<ActivityLogEntry> request);

		[OperationContract]
		BusinessMessageResponse SaveActivityLogEntry(SaveRequest<ActivityLogEntry> request);

		[OperationContract]
		BusinessMessageResponse DeleteActivityLogEntry(IDRequest request);

		[OperationContract]
		GetItemResponse<ActivityLogEntry> GetActivityLogEntry(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetActivityLogEntryUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}