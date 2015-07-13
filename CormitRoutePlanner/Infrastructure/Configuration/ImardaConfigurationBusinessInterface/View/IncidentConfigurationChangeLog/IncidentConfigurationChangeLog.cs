using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaConfigurationBusiness 
{
	partial interface IImardaConfiguration 
	{

		#region Operation Contracts for IncidentConfigurationChangeLog
		
		[OperationContract]
		GetListResponse<IncidentConfigurationChangeLog> GetIncidentConfigurationChangeLogListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<IncidentConfigurationChangeLog> GetIncidentConfigurationChangeLogList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveIncidentConfigurationChangeLogList(SaveListRequest<IncidentConfigurationChangeLog> request);

		[OperationContract]
		BusinessMessageResponse SaveIncidentConfigurationChangeLog(SaveRequest<IncidentConfigurationChangeLog> request);

		[OperationContract]
		BusinessMessageResponse DeleteIncidentConfigurationChangeLog(IDRequest request);

		[OperationContract]
		GetItemResponse<IncidentConfigurationChangeLog> GetIncidentConfigurationChangeLog(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetIncidentConfigurationChangeLogUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}