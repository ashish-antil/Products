using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FernBusinessBase;
using Imarda.Lib;
using Imarda.Logging;
using ImardaTask;
using ImardaTaskBusiness;
using System.ServiceModel;
using ImardaConfigurationBusiness;
using FernBusinessBase.Errors;
using System.Diagnostics;


namespace Imarda360Application.Task
{
	/// <summary>
	/// Main partial class for TaskManager in the application component.
	/// Implement one i360TaskManager_{ProgramName}.cs for each different Program.
	/// </summary>
	public partial class i360TaskManager : TaskManager
	{
		#region Singleton

		static i360TaskManager()
		{
		}

		private static readonly i360TaskManager _Instance = new i360TaskManager();

		public static i360TaskManager Instance
		{
			get { return _Instance; }
		}

		private i360TaskManager()
			: base((byte)ConfigUtils.GetInt("AppTaskManagerID", 1), ImardaProxyManager.Instance.GetTaskServiceProxy())
		{
			Initialize();
			TaskManagerParameters.ManagerID = ManagerID;
		}

		#endregion

		/// <summary>
		/// Common initialize for all partial classes
		/// </summary>
		private void Initialize()
		{
			try
			{
				
				string userpwd = String.Empty;
				var service = ImardaProxyManager.Instance.IImardaConfigurationProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;

					var response1 =
						service.GetConfigValue(new ConfigRequest(new Guid("c3b3a20b-ea86-460a-a227-1cc25e4a69a8"),
																										 null));
					ErrorHandler.CheckItem(response1);
					userpwd = response1.Item.As<string>();

					var response2 =
						service.GetConfigValue(new ConfigRequest(new Guid("9324db0c-c8db-4af4-8795-873822ad73e6"),
																										 null));
					ErrorHandler.CheckItem(response2);
					_ReportHost = response2.Item.As<string>();
				});
				var cryptoKey = new Guid("4ff9a497-d303-41ec-968e-b0de71a4f765");
				string decrypted = EncryptionHelper.Decrypt(cryptoKey, userpwd);
				string[] cred = decrypted.Split('|');
				_ReportServerLogin = new NetworkCredential(cred[0], cred[1]);
                _DownloadFolder = ConfigUtils.GetString("DownloadFolder") ?? @"C:\";
                _CleanUpAttachments = ConfigUtils.GetFlag("DeleteDownloads");

				_Log.InfoFormat("Report Host {0}, Cred {1}, Att.Folder {2}, Cleanup {3}",
												_ReportHost, userpwd, _DownloadFolder, _CleanUpAttachments);
			}
			catch (Exception ex)
			{
				_Log.ErrorFormat("Error while initializing {0}: {1}", GetType().Name, ex);
				_Log.Error("Task Scheduling may not work!");
			}
		}

		public static void LogReportProcessingTime(ReportTask reportTask, string reportName, DateTime started)
		{
			try
			{
                //var endMetric = new PerformanceMetric();
                //endMetric.OwnerID = reportTask.ID;
                //endMetric.ParentOwnerID = Guid.Empty;
                //endMetric.CompanyID = reportTask.CompanyID;
                //endMetric.Name = reportName;
                //endMetric.EntityType = (int)PerformanceMetric.EntityTypes.Business;
                //endMetric.EntitySubType = (int)PerformanceMetric.EntitySubTypes.Reporting;
                //endMetric.ValueType = (int)PerformanceMetric.ValueTypes.Duration;
                //endMetric.Value = (int)(DateTime.UtcNow.Subtract(started).TotalMilliseconds);
                //var service = ImardaProxyManager.Instance.IImardaPerformanceMonitorProxy;
                //ChannelInvoker.Invoke(delegate(out IClientChannel channel)
                //{
                //    channel = service as IClientChannel;

                //    var req = new SaveRequest<PerformanceMetric>(endMetric);
                //    var resp = service.SavePerformanceMetric(req);
                //    ErrorHandler.Check(resp);
                //});
			}
			catch (Exception ex)
			{
				_Log.ErrorFormat("Error Occured While Logging Report Metrics. Exception Details: {0}", ex);
			}
		}

		#region Overrides

		protected override TaskStatus Run(ScheduledTask task)
		{
			Stopwatch sw = null;
			if (_Log.MaxLevel >= 3) sw = Stopwatch.StartNew();
			try
			{
				switch ((Programs)task.ProgramID)
				{
					case Programs.ReportHandler:
						var started = DateTime.UtcNow;

						ReportTask repTask = AppTaskHelper.Convert<ReportTask>(task);
						repTask.Arguments = (ReportTask.Args)XmlUtils.Deserialize(task.Arguments, typeof(ReportTask.Args));

						string reportName;
				        string ext;
						Uri uri = RunReport(repTask, task.CompanyID, out reportName, out ext);
						_Log.InfoFormat("(1) RunReport -> {0} {1}", uri, reportName);
						string path = repTask.Notification.AsAttachment ? DownloadReport(uri, reportName,ext) : null;
						_Log.InfoFormat("(2) Download -> {0}", path);
						NotifyRecipients(repTask, path);
						_Log.InfoFormat("(3) Notify -> {0}", path);

						LogReportProcessingTime(repTask, reportName, started);

						break;

					

					case Programs.SystemEventHandler:
						var sysTask = AppTaskHelper.Convert<SystemEventTask>(task);
						sysTask.Arguments = (SystemEventTask.Args)XmlUtils.Deserialize(task.Arguments, typeof(SystemEventTask.Args));
						if (!RunSystemEvent(sysTask)) return TaskStatus.Cancelled;
						break;


					

#if TaskHandler
					case Programs.`ProgramName`:
						`Task`Task task`ProgramNumber` = AppTaskHelper.Convert<`Task`Task>(task);
						task`ProgramNumber`.Arguments = (`Task`Task.Args)XmlUtils.Deserialize(task.Arguments, typeof(`Task`Task.Args));
						Run`Task`(task`ProgramNumber`);
						break;

#endif
				}
				return TaskStatus.Success;
			}
			catch (Exception ex)
			{
				_Log.ErrorFormat("Error on Run {0}: {1}", task, ex);
				return TaskStatus.Failure;
			}
			finally
			{
				if (sw != null)
				{
					sw.Stop();
					_Log.InfoFormat("- P{0} {1} ({2} ms) {3}", task.ProgramID, task.ID, sw.ElapsedMilliseconds, task.Recurrence);
				}
			}
		}



		/// <summary>
		/// Determine queue for this task. Can also modify task 
		/// </summary>
		/// <param name="task"></param>
		/// <returns></returns>
		protected override string Schedule(ScheduledTask task) //TODO add IScheduler to get access to task queue
		{
			switch ((Algorithms)task.AlgorithmID)
			{
				case Algorithms.Default:
					return "Default";
				case Algorithms.Reports:
					return "Reports";
				case Algorithms.Alerts:
					return "Alerts";
				case Algorithms.System:
					ScheduleSystemTask(task);
					return "System";

				case Algorithms.Address:
					return "Address";

				default:
					return "Unknown";
			}
		}

		protected override void ScheduleSpecific(ScheduledTask newTask)
		{
			if (newTask.AlgorithmID == (byte)Algorithms.Reports)
			{
				var reportSpecific = (ReportTask.Args)XmlUtils.Deserialize(newTask.Arguments, typeof(ReportTask.Args));
				DateTime userLocalStartTime = TimeZoneInfo.ConvertTimeFromUtc(newTask.StartTime, newTask.GetTimeZoneInfo());
				reportSpecific.Definition.CalculatePeriod(userLocalStartTime);
				newTask.Arguments = XmlUtils.Serialize(reportSpecific, false);
			}
		}

		#endregion

	
		



		protected override void CheckForLateTasks()
		{
			return;

			// The code below tries to bring back half a million rows and then find late tasks, which may be none or a few, and then report
			// that there are late tasks to the attention monitor, so dev team can be informed by email.
			// This was causing a WCF error: The maximum message size quota for incoming messages (104857600) has been exceeded. 
			// To re-enable this code, we need to add a new SP for the purpose of finding late tasks. And there is no point in doing that
			// unless we also re-enable the attention monitor and actually have it report this state to us in email.
			// So for now, I'm just commenting out this code as a quick hack to stop the DB being affected by this silly request for 500,000 rows
			// every 3 hours. (Dave Jollie, 14 May 14)  (This code issue can be fixed on IM-5006, which was created for it.)

			//try
			//{
			//	GetListResponse<ScheduledTask> response = null;
			//	var service = ImardaProxyManager.Instance.IImardaTaskProxy;
			//	ChannelInvoker.Invoke(delegate(out IClientChannel channel)
			//	{
			//		channel = service as IClientChannel;
			//		response = service.GetScheduledTaskList(new IDRequest());
			//	});
			//	if (!ServiceMessageHelper.IsSuccess(response))
			//	{
			//		return;
			//	}

			//	ConfigUtils.RefreshAppSettings();
			//	var tolerance = ConfigUtils.GetTimeSpan("LateTaskTolerance", TimeSpan.FromMinutes(-10.0)); // a.k.a. MinutesFromNow in API
			//	DateTime dt = DateTime.UtcNow.Add(tolerance);
			//	var scheduledTasks = response.List;
			//	var list = from task in scheduledTasks
			//						 where task.Status < 3 && task.StartTime < dt && task.ProgramID != 2
			//						 select task.ToString();
			//	var arr = list.ToArray();
			//	string msg = string.Join(Environment.NewLine, arr);
			//	if (arr.Length > 0)
			//	{
			//		string shortMsg = arr.Length == 1 ? "There is 1 late task!" : "There are {0} late tasks!!";
			//		Attention(new Guid("42fec959-af13-badd-8ea5-2c9eca640eb3"), shortMsg, arr.Length);
			//		_Log.Info(msg);
			//	}
			//	else
			//	{
			//		_Log.InfoFormat("There are no late tasks.");
			//	}
			//}
			//catch (Exception ex)
			//{
			//	_Log.Error(ex);
			//}
		}

	}
}
