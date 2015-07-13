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

		#region Operation Contracts for Task
		[OperationContract]
		GetListResponse<Task> GetTaskListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<Task> GetTaskList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveTaskList(SaveListRequest<Task> request);

		[OperationContract]
		BusinessMessageResponse SaveTask(SaveRequest<Task> request);

		[OperationContract]
		BusinessMessageResponse DeleteTask(IDRequest request);

		[OperationContract]
		GetItemResponse<Task> GetTask(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetTaskUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}

