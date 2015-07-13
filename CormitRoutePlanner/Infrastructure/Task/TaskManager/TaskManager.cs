using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading;
using FernBusinessBase;
using FernBusinessBase.Errors;
using Imarda.Lib;
using Imarda.Logging;
using ImardaTaskBusiness;

namespace ImardaTask
{
	/// <summary>
	/// The task manager reads from the database via a task business service proxy. 
	/// It submits executed tasks to the TaskQueueCache which calls the virtual Schedule
	/// and Run methods to process the task. If a recurrence pattern is defined then 
	/// the next occurrence will be scheduled automatically on success.
	/// Implement a subclass and provide a scheduling algorithm and a run method.
	/// </summary>
	public abstract class TaskManager
	{
		private const int Chunk = 50;
		protected static ErrorLogger _Log = ErrorLogger.GetLogger("TaskManager");


		private readonly Thread _QueueWorker;
		private readonly Proxy<IImardaTask> _Proxy;
		private readonly byte _TaskManagerID;
		private readonly TaskQueueCache _QueueCache;


		/// <summary>
		/// Create a TaskManager which reads and updates ScheduledTasks from the given proxy.
		/// </summary>
		/// <param name="id">ScheduledTask.ManagerID</param>
		/// <param name="proxy"></param>
		protected TaskManager(byte id, Proxy<IImardaTask> proxy)
		{
			_TaskManagerID = id;
			_Proxy = proxy;

			SchedulerThreadPool.Instance.Start(
                ConfigUtils.GetInt("MinAPoolThreads_Task", 8),
                ConfigUtils.GetInt("MaxAPoolThreads_Task", 32)
			);

			_QueueCache = new TaskQueueCache();
			_QueueWorker = new Thread(TaskProcessingOuterLoop) { Name = "TaskProcessor" };
		}


		public byte ManagerID { get { return _TaskManagerID; } }


		public void Start()
		{
			_Log.Info("Start Processing Tasks...");
			_QueueWorker.Start();
		}

		/// <summary>
		/// Loop that reads the next bunch of scheduled tasks from the task service and processes them.
		/// </summary>
		/// <param name="state"></param>
		private void TaskProcessingOuterLoop(object state)
		{
			_Log.Debug("TaskProcessingOuterLoop entered");
			int requeued = RequeuePendingMessages(_TaskManagerID);
			int schedulerDelay = 0;

            var refreshDelay = new Cycle(10, 9,
				delegate
				{
                    ConfigUtils.RefreshAppSettings(); // get new settings from disk every 10 cycles
                    schedulerDelay = Math.Max(20, ConfigUtils.GetInt("TaskSchedulerDelay", 2000));
				});

            var lateTaskDelay = new DelayCycle(ConfigUtils.GetTimeSpan("CheckForLateTasks", TimeSpan.FromHours(3)), CheckForLateTasks, true); 

			if (requeued < 0)
			{
				_Log.ErrorFormat("Attention: Application service call via Task Service to ScheduledTask table failed. Task scheduler is not running!"
								 + " Stop app service, restart gateway and then start app service.");
			}
			else
			{
				_Log.InfoFormat("Requeued " + requeued + " ScheduledTasks");
				while (true)
				{
					try
					{
						refreshDelay.Notify();

						int n = TaskProcessingInnerLoop();
						int total = _QueueCache.Total();
						_Log.DebugFormat("Enqueued {0} tasks => total {1}, sleep {2}ms", n, total, schedulerDelay);
						Thread.Sleep(schedulerDelay);

						lateTaskDelay.Notify();
					}
					catch (Exception e)
					{
						ErrorHandler.HandleInternal(e);
						_Log.Error("Error - " + (e.InnerException != null ? e.InnerException.Message : e.Message)); 
						Thread.Sleep(5000); 
					}
				}
			}
		}

		protected abstract void CheckForLateTasks();

		/// <summary>
		/// Read in all pending tasks on start up of the TaskManager.
		/// </summary>
		/// <returns></returns>
		private int RequeuePendingMessages(byte managerID)
		{
			try
			{
				BusinessMessageResponse resp = null;
				IImardaTask service = _Proxy.GetChannel();
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					resp = service.RequeuePendingMessages(new GenericRequest(Guid.Empty, managerID));
				});
				return (resp.Status) ? int.Parse(resp.StatusMessage) : -1;
			}
			catch (Exception ex)
			{
				_Log.ErrorFormat("Requeue error: {0}", ex);
				return -1;
			}
		}


		/// <summary>
		/// Puts the new tasks that were read from the task service into a memory queue for later execution.
		/// </summary>
		/// <returns></returns>
		private int TaskProcessingInnerLoop()
		{
			int n = 0;
			try
			{
				IImardaTask service = _Proxy.GetChannel();
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;

					// Log Entry Point
					var request = new GenericRequest(Guid.Empty, _TaskManagerID, Chunk);
					var response = service.GetScheduledTaskListForProcessing(request);
					n = response.List.Count;
					if (n > 0)
					{
						_Log.Info("Enqueue " + n + " pending ScheduledTasks");
						foreach (ScheduledTask task in response.List)
						{
							if (_QueueCache.Enqueue(task, Schedule, ProcessOneTask))
							{
								SaveStatus(task.ID, TaskStatus.Queued, task.QueueID);
							}
							else
							{
								SaveStatus(task.ID, TaskStatus.QueueError, null);
							}
						}
					}
				});
			}
			catch (Exception ex)
			{
				ErrorHandler.HandleInternal(ex);
				_Log.InfoFormat("Exception caught inside ProcessMessages...", ex);
			}
			return n;
		}


		/// <summary>
		/// Execute one task and update the status in the table.
		/// </summary>
		/// <param name="task"></param>
		private void ProcessOneTask(ScheduledTask task)
		{
			TaskStatus status;

           // if (task.Recurrence != null && status != TaskStatus.Cancelled)
             if (task.Recurrence != null)
            {
                var cancelled = _QueueCache.CancelSeries(task.QueueID, task.TaskID);
                _Log.DebugFormat("Cancelled {0} of series {1}", cancelled.Count, task.TaskID);

                // Generate the very next recurring task only.

                ScheduledTask newTask = task.Copy(SequentialGuid.NewDbGuid());
                DateTime userTime, utc;
                CalculateStartTime(newTask.GetTimeZoneInfo(), newTask.Recurrence, newTask.TimeOfDay, out userTime, out utc);
                newTask.StartTime = utc;
                newTask.DueTime = newTask.StartTime + (task.DueTime - task.StartTime); //TODO make sure before next recurrence
                newTask.Status = 0;
                newTask.Active = true;
                newTask.QueueID = null;

                ScheduleSpecific(newTask);
                IImardaTask service = _Proxy.GetChannel();
                ChannelInvoker.Invoke(delegate(out IClientChannel channel)
                {
                    channel = service as IClientChannel;
                    var resp2 = service.SaveScheduledTask(new SaveRequest<ScheduledTask>(newTask));
                    ErrorHandler.Check(resp2);
                });

            }

			try
			{
				// Active tasks get run, but inactive tasks are templates only, they need to be scheduled, i.e. their start time calculated.
				status = task.Active ? Run(task) : TaskStatus.Rescheduled;
			}
			catch
			{
				// make sure Run catches all exceptions, then we'll never get here.
				status = TaskStatus.Failure;
			}

			try
			{
				SaveStatus(task.ID, status, null);				
			}
			catch (Exception ex)
			{
				_Log.ErrorFormat("Cannot save ScheduledTask: {0}", ex);
				throw;
			}
		}

		/// <summary>
		/// Override to implement changing the task's arguments after setting the new StartTime of a recurrence.
		/// </summary>
		/// <param name="task"></param>
		protected abstract void ScheduleSpecific(ScheduledTask task);

		/// <summary>
		/// Override to select a queue for this task and tweak start time, due time, retries etc,
		/// </summary>
		/// <param name="task"></param>
		/// <returns>queue ID</returns>
		protected abstract string Schedule(ScheduledTask task);

		/// <summary>
		/// Override to execute the task. Exceptions thrown will be handled by the JobQueueHandler.
		/// </summary>
		/// <param name="task"></param>
		/// <returns>status</returns>
		protected abstract TaskStatus Run(ScheduledTask task);

		/// <summary>
		/// Set new task status. Note: The status can only be upgraded to a higher number!
		/// </summary>
		/// <param name="scheduledTaskID"></param>
		/// <param name="newStatus"></param>
		/// <param name="queueID"> </param>
		public void SaveStatus(Guid scheduledTaskID, TaskStatus newStatus, string queueID)
		{
			IImardaTask service = _Proxy.GetChannel();
			ChannelInvoker.Invoke(delegate(out IClientChannel channel)
			{
				channel = service as IClientChannel;
				var resp = service.SetScheduledTaskStatus(new GenericRequest(scheduledTaskID, (byte)newStatus, queueID));
				ErrorHandler.Check(resp);
			});
		}


		public bool Cancel(Guid scheduledTaskID, string queueID)
		{
			if (queueID == null) return false;
			var cancelled = _QueueCache.Cancel(queueID, scheduledTaskID);
			if (cancelled.Count > 0)
			{
				IImardaTask service = _Proxy.GetChannel();
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					foreach (Guid id in cancelled) // should be just 1 job in the list, but loop anyway
					{
						var resp = service.SetScheduledTaskStatus(new GenericRequest(id, (byte)TaskStatus.Cancelled, queueID));
						ErrorHandler.Check(resp);
					}
				});
			}
			return cancelled.Count > 0;
		}

		/// <summary>
		/// Uses now as a basis to calculate the next scheduled date. 
		/// </summary>
		/// <param name="tzi">user's time zone</param>
		/// <param name="recurrence">pattern</param>
		/// <param name="timeOfDay">user's preferred time of day, in user's time zone</param>
		/// <param name="userTime">start time in user time zone</param>
		/// <param name="utc">start time in utc</param>
		/// <returns>false if not within start-end period, true otherwise</returns>
		public static bool CalculateStartTime(TimeZoneInfo tzi, string recurrence, TimeSpan timeOfDay, out DateTime userTime, out DateTime utc)
		{
			DateTime utcNow = DateTime.UtcNow;
			userTime = utc = DateTime.MinValue;

			TimeSpan? ts = ScheduledTask.FixedInterval(recurrence);
			if (ts.HasValue)
			{
				utc = utcNow + ts.Value;
				userTime = TimeZoneInfo.ConvertTimeFromUtc(utc, tzi);
			}
			else
			{
				DateTime userNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, tzi);

				DateTime runDate = userNow.Date;
				if (userNow.TimeOfDay > timeOfDay) runDate += TimeSpan.FromDays(1);

				DateTime dt;
				if (recurrence != null)
				{
					DateGenerator gen = DateGeneratorFactory.Create(recurrence);
					DateTime next = gen.OnOrAfter(runDate);
					if (next == default(DateTime)) return false; // outside range
					dt = next + timeOfDay;
				}
				else
				{
					dt = runDate + timeOfDay;
				}
				userTime = dt;
				utc = TimeZoneInfo.ConvertTimeToUtc(dt, tzi);
			}
			return true;
		}

		/// <summary>
		/// Cancel all jobs from a task queue.
		/// </summary>
		/// <param name="queueID">name of the queue, or null for ALL queues</param>
		/// <returns>number of jobs cancelled</returns>
		public int CancelTaskQueueCache(string queueID)
		{
			return _QueueCache.CancelAll(queueID);
		}

		public List<string> GetTaskQueueCache()
		{
			var list = new List<string>();
            QJob<DateTime>[] jobs = _QueueCache.QueueHandler.GetJobAllJobs();
			foreach (var job in jobs)
			{
				list.Add(job == null ? "" : job.ToString());
			}
			return list;
		}


		public void Attention(Guid issueID, string text, params object[] args)
		{
            _Log.Error(AttentionUtils.Attention(issueID, text, args));
		}

		public void CancelAttention(Guid issueID, string text, params object[] args)
		{
            _Log.Error(AttentionUtils.CancelAttention(issueID, text, args));
		}

	}
}
