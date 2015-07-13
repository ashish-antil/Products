using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaConfigurationBusiness 
{
	partial interface IImardaConfiguration 
	{

		#region Operation Contracts for IncidentCompanyConfiguration
		
		[OperationContract]
		GetListResponse<IncidentConfigurationCompany> GetIncidentConfigurationCompanyListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<IncidentConfigurationCompany> GetIncidentConfigurationCompanyList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveIncidentConfigurationCompanyList(SaveListRequest<IncidentConfigurationCompany> request);

		[OperationContract]
		BusinessMessageResponse SaveIncidentConfigurationCompany(SaveRequest<IncidentConfigurationCompany> request);

		[OperationContract]
		BusinessMessageResponse DeleteIncidentConfigurationCompany(IDRequest request);

		[OperationContract]
		BusinessMessageResponse UpdateIncidentConfigurationCompanyList(SaveListRequest<IncidentConfigurationCompany> request);

		[OperationContract]
		GetItemResponse<IncidentConfigurationCompany> GetIncidentConfigurationCompany(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetIncidentConfigurationCompanyUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}