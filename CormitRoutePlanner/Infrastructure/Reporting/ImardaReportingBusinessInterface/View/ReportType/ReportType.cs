/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\maurice.verheijen
Date Generated : 24/06/2009 2:48 p.m.
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaReportBusiness 
{
	partial interface IImardaReport 
	{

		#region Operation Contracts for ReportType
		[OperationContract]
		GetListResponse<ReportType> GetReportTypeListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<ReportType> GetReportTypeList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveReportTypeList(SaveListRequest<ReportType> request);

		[OperationContract]
		BusinessMessageResponse SaveReportType(SaveRequest<ReportType> request);

		[OperationContract]
		BusinessMessageResponse DeleteReportType(IDRequest request);

		[OperationContract]
		GetItemResponse<ReportType> GetReportType(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetReportTypeUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}

