using System;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Linq;
using FernBusinessBase;
using FernBusinessBase.Errors;
using Imarda.Lib;


using ImardaTaskBusiness;

using System.Collections.Generic;

namespace Imarda360Application.Task
{
	//TODO restructure this: use Command pattern

	/// <summary>
	/// This is the task handing program: SystemEventHandler.
	/// </summary>
	partial class i360TaskManager
	{
		
		private static Guid _UnitTraceStart = GuidHelper.FromDateTime(DateTime.UtcNow);

		
		private static readonly DelayCycle _SpeedingEvent
			= new DelayCycle(ConfigUtils.GetTimeSpan("HeartbeatInterval.Speeding", TimeSpan.FromMinutes(30)),
				() => AttentionUtils.SendHeartbeat(new Guid("F589AE1B-5D7D-BEA7-9D76-8575BA687407")));

		private static readonly DelayCycle _SystemTaskHeartbeat
			= new DelayCycle(ConfigUtils.GetTimeSpan("HeartbeatInterval.SystemTask", TimeSpan.FromMinutes(3)),
				() => AttentionUtils.SendHeartbeat(new Guid("6D4565E0-A0D3-BEA7-B609-99734E07718A")));
		

		

		private bool RunSystemEvent(SystemEventTask task) 
		{
			_Log.InfoFormat("Run SystemEvent {0}", task.ID);

			SystemEventTask.Args args = task.Arguments;

			
			GetItemResponse<ScheduledTask> resp1 = null;
			var service = ImardaProxyManager.Instance.IImardaTaskProxy;
			ChannelInvoker.Invoke(
				delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					resp1 = service.GetScheduledTask(new IDRequest(task.ID));
				});
			if (ServiceMessageHelper.IsSuccess(resp1))
			{
				ScheduledTask dbtask = resp1.Item;
				if (dbtask.Status != 1)
				{
					_Log.WarnFormat("System Event Task Scheduler skipping {0} because its db status is not 1", dbtask);
					return false;
				}
			}
			else
			{
				_Log.WarnFormat("System Event Task Scheduler skipping {0} because it has been deleted from the database", task);
				return false;
			}
			

			switch (args.EventID.ToString())
			{
				case "76d39571-a0f3-4079-85ab-08ce028d8f19": // Service Management
					var request = new IDRequest();
                    //request.ID = task.CompanyID;
                    //GetListResponse<ServiceRecord> response = ImardaVehicleManagement.Instance.GetVehiclesNeedSendServiceReminder(request);
                    //var srlist= response.List;

                    //if (ServiceMessageHelper.IsSuccess(response) && srlist.Count > 0)
                    //{
                    //    foreach (ServiceRecord record in srlist.Where(record => record.SendAlert))
                    //    {
                    //        CreateServiceAlert(record.Status, record.VehicleName, record.ServicePlanName, record.NextService,
                    //                           record.LastDistanceRemind, record.CompanyID,
                    //                           record.ServiceID, record.UnitID, task.GetTimeZoneInfo(), record.ID,
                    //                           record.Status == ServiceItem.ServiceStatus.Due, record.IsReminder);
                    //    }
                    //}
                    //string[] vehicleIDsArray = srlist.Select(sr => sr.VehicleID.ToString()).Distinct().ToArray();
                    //string vehicleIDs = string.Concat(vehicleIDsArray);
                    //ImardaVehicleManagement.Instance.UpdateVehicleServiceAlertFlags(new IDRequest(Guid.Empty, "VehicleIDs", vehicleIDs));
					break;

				case "7dc6f970-a1de-4b74-9afa-ac7d89c1faef": // Job Completion check
                    //var jobCompleteRequest = new IDRequest();
                    //jobCompleteRequest.CompanyID = Guid.Empty;// run this for all companies' jobs
                    //jobCompleteRequest.Put("OnDueDate", DateTime.UtcNow.AddHours(-8));
                    //ImardaJobDispatch.Instance.CompleteOnDueJobs(jobCompleteRequest); //TODO replace
					break;

				

				
				

#if AddSystemEvent
				//& `jira`
				case "`id`": // `description`
					break;
				//. `jira`

#endif
			}
			_SystemTaskHeartbeat.Notify(); 

			return true; 
		}

		private void ScheduleSystemTask(ScheduledTask task)
		{
			var duration = TimeSpan.FromMinutes(5);
			DateTime start = DateTime.UtcNow;
			string rec = task.Recurrence;
			if (rec != null && rec.Length > 1 && task.FixedInterval() == null)
			{
				if (!Char.IsDigit(task.Recurrence[0]))
				{
					task.Recurrence = start.ToString("yyyy-MM-dd") + ";2999-01-01;" + task.Recurrence;
					task.StartTime = start;
					task.DueTime = start + duration;
				}
			}
		}
		}
	}

