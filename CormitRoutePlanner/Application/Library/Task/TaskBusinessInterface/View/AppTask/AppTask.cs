using System.ServiceModel;
using FernBusinessBase;
using Imarda360Base;
using ImardaTaskBusiness;

namespace Imarda360Application.Task
{
	partial interface IImardaTask
	{
		[OperationContract]
		Get360ListResponse<AppTask> GetAppTaskList(IDRequest request);

		[OperationContract]
		Get360ListResponse<ReportTask> GetReportTaskList(IDRequest request);

		[OperationContract]
		SolutionMessageResponse ScheduleAppTask(Save360Request<AppTask> request);

		[OperationContract]
		SolutionMessageResponse IsNextScheduledDateInRange(Save360Request<AppTask> request);

		[OperationContract]
		SolutionMessageResponse CancelScheduledAppTask(IDRequest request);

		[OperationContract]
		Get360ItemResponse<AppTask> GetAppTask(IDRequest request);

		[OperationContract]
		Get360ListResponse<ReportTask> GetReportTaskListFiltered(ReportTaskListRequest request);

		[OperationContract]
		GetListResponse<ScheduledTask> GetScheduledTaskList(IDRequest request);

		[OperationContract]
		SimpleResponse<string[]> GetTaskQueueCache(IDRequest request);

		[OperationContract]
		BusinessMessageResponse ClearTaskQueueCache(IDRequest request);

#if NewAppMethod
		[OperationContract]
		`return` `method`(`reqtype` request);

#endif
	}
}
