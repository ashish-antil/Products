/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\adamson.delacruz
Date Generated : 12/05/2009 12:13 PM
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaConfigurationBusiness 
{
	partial interface IImardaConfiguration 
	{

		#region Operation Contracts for Configuration
		[OperationContract]
		GetListResponse<Configuration> GetConfigurationListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<Configuration> GetConfigurationList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveConfigurationList(SaveListRequest<Configuration> request);

		[OperationContract]
		BusinessMessageResponse SaveConfiguration(SaveRequest<Configuration> request);

		[OperationContract]
		BusinessMessageResponse DeleteConfiguration(IDRequest request);

		[OperationContract]
		GetItemResponse<Configuration> GetConfiguration(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetConfigurationUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}

