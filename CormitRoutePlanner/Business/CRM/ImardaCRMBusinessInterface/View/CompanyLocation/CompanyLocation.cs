/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\adamson.delacruz
Date Generated : 27/04/2009 1:27 PM
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{

		#region Operation Contracts for CompanyLocation
		[OperationContract]
		GetListResponse<CompanyLocation> GetCompanyLocationListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<CompanyLocation> GetCompanyLocationList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveCompanyLocationList(SaveListRequest<CompanyLocation> request);

		[OperationContract]
		BusinessMessageResponse SaveCompanyLocation(SaveRequest<CompanyLocation> request);

		[OperationContract]
		BusinessMessageResponse DeleteCompanyLocation(IDRequest request);

		[OperationContract]
		GetItemResponse<CompanyLocation> GetCompanyLocation(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetCompanyLocationUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}

