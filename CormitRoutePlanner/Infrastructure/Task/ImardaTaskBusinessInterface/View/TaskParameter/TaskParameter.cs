/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\maurice.verheijen
Date Generated : 24/06/2009 9:55 a.m.
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaTaskBusiness 
{
	partial interface IImardaTask 
	{

		#region Operation Contracts for TaskParameter
		[OperationContract]
		GetListResponse<TaskParameter> GetTaskParameterListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<TaskParameter> GetTaskParameterList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveTaskParameterList(SaveListRequest<TaskParameter> request);

		[OperationContract]
		BusinessMessageResponse SaveTaskParameter(SaveRequest<TaskParameter> request);

		[OperationContract]
		BusinessMessageResponse DeleteTaskParameter(IDRequest request);

		[OperationContract]
		GetItemResponse<TaskParameter> GetTaskParameter(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetTaskParameterUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}

