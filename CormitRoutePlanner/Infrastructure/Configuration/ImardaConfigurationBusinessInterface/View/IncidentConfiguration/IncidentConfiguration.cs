using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaConfigurationBusiness 
{
	partial interface IImardaConfiguration 
	{

		#region Operation Contracts for IncidentConfiguration
		
		[OperationContract]
		GetListResponse<IncidentConfiguration> GetIncidentConfigurationListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<IncidentConfiguration> GetIncidentConfigurationList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveIncidentConfigurationList(SaveListRequest<IncidentConfiguration> request);

		[OperationContract]
		BusinessMessageResponse SaveIncidentConfiguration(SaveRequest<IncidentConfiguration> request);

		[OperationContract]
		BusinessMessageResponse DeleteIncidentConfiguration(IDRequest request);

		[OperationContract]
		GetItemResponse<IncidentConfiguration> GetIncidentConfiguration(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetIncidentConfigurationUpdateCount(GetUpdateCountRequest request);

		[OperationContract]
		SimpleResponse<bool> CheckIfUniqueForIncidentConfiguration(IDRequest request);

		#endregion

	}
}