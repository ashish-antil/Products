using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaTaskBusiness 
{
	partial interface IImardaTask 
	{
		[OperationContract]
		GetListResponse<ScheduledTask> GetScheduledTaskListForProcessing(GenericRequest request);


		/// <summary>
		/// Called on startup to requeue all status 0 and 1 messages with the given task manager id
		/// </summary>
		/// <param name="request">[0] = task manager id</param>
		/// <returns></returns>
		[OperationContract]
		BusinessMessageResponse RequeuePendingMessages(GenericRequest request);


		/// <summary>
		/// Change the status.
		/// </summary>
		/// <param name="genericRequest">.ID = scheduled task ID, [0] = (byte)status, [1] = (string)queue ID or null</param>
		/// <returns></returns>
		[OperationContract]
		BusinessMessageResponse SetScheduledTaskStatus(GenericRequest genericRequest);
		
		/// <summary>
		/// Get a filtered scheduled task list.
		/// </summary>
		/// <param name="request">filter options</param>
		/// <returns></returns>
		[OperationContract]
		GetListResponse<ScheduledTask> GetScheduledTaskListFiltered(ScheduledTaskListRequest request);

		[OperationContract]
		GetListResponse<ScheduledTask> GetScheduledReportTaskExtent(GetFilteredExtentRequest request);

		[OperationContract]
		SimpleResponse<int> GetScheduledReportTaskExtentCount(GetFilteredExtentRequest request);
	}
}
