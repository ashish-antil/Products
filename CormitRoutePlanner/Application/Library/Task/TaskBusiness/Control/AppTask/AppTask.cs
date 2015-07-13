using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using Imarda.Logging;
using ImardaTask;
using ImardaTaskBusiness;
using System.ServiceModel;
using Imarda360Base;
using Imarda.Lib;



namespace Imarda360Application.Task
{
	partial class ImardaTask
	{
		/// <summary>
		/// Schedule an AppTask.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public SolutionMessageResponse ScheduleAppTask(Save360Request<AppTask> request)
		{
			try
			{
				bool run = false;
				ErrorLogger logger = ErrorLogger.GetLogger("AppTask");
				var service = ImardaTaskProxyManager.Instance.IImardaTaskProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					AppTask appTask = request.Item;
					DateTime userTime, utc;
					run = TaskManager.CalculateStartTime(
						appTask.GetTimeZoneInfo(), appTask.Recurrence, appTask.TimeOfDay, out userTime, out utc);

					if (run)
					{
						appTask.CompanyID = request.CompanyID;
						appTask.StartTime = utc;
						logger.DebugFormat("StartTime {0} UTC", appTask.StartTime);
						appTask.TimeAllowed = TimeSpan.FromMinutes(ConfigUtils.GetInt("TaskTimeoutMinutes", 60));
						ScheduledTask scheduledTask = AppTaskHelper.Convert(appTask);
                        scheduledTask.ManagerID = (byte)ConfigUtils.GetInt("AppTaskManagerID", 1);
							// = i360 application service task manager
						scheduledTask.OwnerID = appTask.TaskOwnerID;
						scheduledTask.UserID = appTask.TaskOwnerID;

						#region TASK TYPE SPECIFIC ITEMS

						if (appTask.GetType() == typeof (ReportTask))
						{
							ReportTask repTask = (ReportTask) appTask;
							if (repTask.StartTime < TimeUtils.MinValue.AddYears(1))
							{
								logger.Error(repTask);
								throw new Exception(string.Format("Invalid Start {0}", repTask));
							}
							repTask.Definition.CalculatePeriod(userTime);
							scheduledTask.AlgorithmID = (byte) Algorithms.Reports;
							scheduledTask.ProgramID = (int) Programs.ReportHandler;
							scheduledTask.Arguments = XmlUtils.Serialize(repTask.Arguments, false);
						}

						#endregion

						logger.Debug(scheduledTask);
						var response = service.SaveScheduledTask(new SaveRequest<ScheduledTask>(scheduledTask));
						ErrorHandler.Check(response);
					}
				});
				return new SolutionMessageResponse { Status = run };
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SolutionMessageResponse>(ex);
			}
		}

		// required by interface, but handled completely in Imarda360.cs
		public SolutionMessageResponse IsNextScheduledDateInRange(Save360Request<AppTask> request)
		{
			throw new NotImplementedException(); 
		}


		/// <summary>
		/// Cancel the scheduled task.
		/// </summary>
		/// <param name="request">ID = ScheduledTask.ID, ["QueueID"]=queue where scheduled</param>
		/// <returns></returns>
		public SolutionMessageResponse CancelScheduledAppTask(IDRequest request)
		{
			try
			{
				Guid id = request.ID;
				ScheduledTask task = null;
				var service = ImardaTaskProxyManager.Instance.IImardaTaskProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					var resp1 = service.GetScheduledTask(new IDRequest(id));
					ErrorHandler.CheckItem(resp1);
					task = resp1.Item;
					if (task.Status == (byte)TaskStatus.Queued) 
					{
						var resp2 = service.SetScheduledTaskStatus(new GenericRequest(id, (byte)AppTaskStatus.Cancelled));
						ErrorHandler.Check(resp2);
					}
				});
				return new SolutionMessageResponse { StatusMessage = task.QueueID };
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SolutionMessageResponse>(ex);
			}
		}

		public Get360ItemResponse<AppTask> GetAppTask(IDRequest request)
		{
			try
			{
				GetItemResponse<ScheduledTask> response = null;
				Get360ItemResponse<AppTask> appResponse = null;
				var service = ImardaTaskProxyManager.Instance.IImardaTaskProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetScheduledTask(request);
					ErrorHandler.Check(response);
					appResponse = new Get360ItemResponse<AppTask>(AppTaskHelper.Convert<AppTask>(response.Item));
				});

				return appResponse;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<Get360ItemResponse<AppTask>>(ex);
			}
		}

		public Get360ListResponse<AppTask> GetAppTaskList(IDRequest request)
		{
			try
			{
				GetListResponse<ScheduledTask> response = null;
				Get360ListResponse<AppTask> appResponse = null;
				var service = ImardaTaskProxyManager.Instance.IImardaTaskProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetScheduledTaskList(request);
					ErrorHandler.Check(response);
					appResponse = new Get360ListResponse<AppTask>(AppTaskHelper.Convert<AppTask>(response.List));
				});

				return appResponse;

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<Get360ListResponse<AppTask>>(ex);
			}
		}

		public GetListResponse<ScheduledTask> GetScheduledTaskList(IDRequest request)
		{
			try
			{
				GetListResponse<ScheduledTask> response = null;
				var service = ImardaTaskProxyManager.Instance.IImardaTaskProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetScheduledTaskList(request);
					ErrorHandler.Check(response);
				});

				return response;

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ScheduledTask>>(ex);
			}
		}

		public SimpleResponse<string[]> GetTaskQueueCache(IDRequest request)
		{
			//only here because interface requires it; the actual work is done in the Imard360.cs which does not call this.
			throw new InvalidOperationException();
		}

		public BusinessMessageResponse ClearTaskQueueCache(IDRequest request)
		{
			//only here because interface requires it; the actual work is done in the Imard360.cs which does not call this.
			throw new InvalidOperationException();
		}


		public Get360ListResponse<ReportTask> GetReportTaskList(IDRequest request)
		{
			try
			{
				GetListResponse<ScheduledTask> response = null;
				Get360ListResponse<ReportTask> appResponse = null;
				var service = ImardaTaskProxyManager.Instance.IImardaTaskProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetScheduledTaskList(request);
					ErrorHandler.Check(response);
					appResponse = new Get360ListResponse<ReportTask>(DeserializeReportTask(response.List));
				});

				return appResponse;

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<Get360ListResponse<ReportTask>>(ex);
			}
		}

		public Get360ListResponse<ReportTask> GetReportTaskListFiltered(ReportTaskListRequest request)
		{
			try
			{
				Get360ListResponse<ReportTask> appResponse = null;
				var service = ImardaTaskProxyManager.Instance.IImardaTaskProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					var stReq = new ScheduledTaskListRequest
					            {
					            	CompanyID = request.CompanyID,
					            	OwnerID = request.OwnerID,
					            	IncludeInactive = request.IncludeInactive,
					            	TopN = request.TopN,
					            	StartTime = request.StartTime,
					            	ManagerID = TaskManagerParameters.ManagerID,
					            	ProgramID = (int) Programs.ReportHandler,
					            	AnyStatus = request.AnyStatus,
					            	IncludeNewAndQueued = request.IncludeNewAndQueued,
					            	IncludeSuccessful = request.IncludeSuccessful,
					            	IncludeFailed = request.IncludeFailed,
					            	IncludeCancelled = request.IncludeCancelled
					            };

					var response = service.GetScheduledTaskListFiltered(stReq);
					ErrorHandler.Check(response);
					appResponse = new Get360ListResponse<ReportTask>(DeserializeReportTask(response.List));
				});
				return appResponse;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<Get360ListResponse<ReportTask>>(ex);
			}
		}

		private List<ReportTask> DeserializeReportTask(List<ScheduledTask> ScheduledTaskList)
		{
			List<ReportTask> list = new List<ReportTask>();
			foreach (ScheduledTask task in ScheduledTaskList)
			{
				ReportTask rt = AppTaskHelper.Convert<ReportTask>(task);
				rt.Arguments = (ReportTask.Args)XmlUtils.Deserialize(task.Arguments, typeof(ReportTask.Args));
				list.Add(rt);
			}
			
			return list;
		}
	}
}