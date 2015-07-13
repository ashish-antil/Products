using System;
using System.Collections.Generic;
using Imarda.Lib;
using Imarda.Logging;
using ImardaTaskBusiness;
using System.Linq;


namespace ImardaTask
{
	/// <summary>
	/// Takes a ScheduledTask and returns the queue ID where task should be added to.
	/// </summary>
	/// <param name="task">scheduled task</param>
	/// <returns>queue ID; null to reject</returns>
	public delegate string SchedulingAlgorithm(ScheduledTask task);

	/// <summary>
	/// Code to run to execute the task. Delegate can throw exception on failure.
	/// </summary>
	/// <param name="task">contains all info (by value + by ref) necessary for execution</param>
	public delegate void Program(ScheduledTask task);


	/// <summary>
	/// Handles a number of task queues.
	/// </summary>
	public class TaskQueueCache
	{
        private static Imarda.Logging.ErrorLogger _Log = Imarda.Logging.ErrorLogger.GetLogger("TQC");
        private static readonly int _MaxTime = ConfigUtils.GetInt("TaskQueueTimeOut", 8000); 

		/// <summary>
		/// Extend the Job class and add delegate and msg contents.
		/// </summary>
        private class TaskJob : QJob<DateTime>
		{
			internal ScheduledTask Task;
			internal Program Program;

			internal TaskJob(ScheduledTask task, string queueID, Program program)
				: base(queueID, ProcessQueuedTask)
			{
				Program = program;
				Task = task;
			}

			public override DateTime Earliest
			{
				get { return Task.StartTime; }
				set { Task.StartTime = value; }
			}

			public override string ToString()
			{
				return string.Format("Task(Q={0}, ID={1}, {2:s} UTC, {3})", Task.QueueID, Task.ID, Earliest, Task.Recurrence);
			}
		}


		public TaskQueueCache()
		{
			_Log.Info("Creating TaskQueueCache");
            _QH = new JobQueueHandler<DateTime>(SchedulerThreadPool.Instance, _Log, SortOrder.Desc, DateTime.MaxValue, "TaskQ");
            _QH.TimeKeeping = ConfigUtils.GetFlag("JobTimeKeeping");
			_QH.Open();
			_QH.Start();
		}

		public SchedulingAlgorithm Schedule { set; get; }
		public Program Execute { set; get; }

        private JobQueueHandler<DateTime> _QH;

		public int Total()
		{
			return _QH.Total();
		}

        public JobQueueHandler<DateTime> QueueHandler
		{
			get { return _QH; }
		}


		public bool RunNow(string qID)
		{
			return _QH.Wake(qID);
		}

		/// <summary>
		/// Cancel all jobs from the given queue
		/// </summary>
		/// <param name="qID">name of queue, or null to mean ALL queues</param>
		/// <returns>number of jobs cancelled</returns>
		public int CancelAll(string qID)
		{
			var all = _QH.GetQueueIDsWhere(q => qID == null || qID == q.ID);
			return all.Select(qid => _QH.Cancel(qid, job => true)).Select(jobs => jobs.Count).Sum();
		}


		public List<Guid> Cancel(string qID, Guid scheduledTaskID)
		{
            List<Imarda.Logging.QJob<DateTime>> cancelled = _QH.Cancel(qID, job => ((TaskJob)job).Task.ID == scheduledTaskID);
			_Log.DebugFormat("Cancelled {0} scheduledTaskID {1} from queue {2}", cancelled.Count, scheduledTaskID, qID);
			return cancelled.Select(job => ((TaskJob)job).Task.ID).ToList();
		}

		public List<Guid> CancelSeries(string qID, Guid taskID)
		{
            List<Imarda.Logging.QJob<DateTime>> cancelled = _QH.Cancel(qID, job => ((TaskJob)job).Task.TaskID == taskID);
			_Log.DebugFormat("Cancelled {0} taskID series {1} from queue {2}", cancelled.Count, taskID, qID);
			return cancelled.Select(job => ((TaskJob)job).Task.ID).ToList();
		}


		public bool Enqueue(ScheduledTask task, SchedulingAlgorithm scheduler, Program program)
		{
			_Log.InfoFormat("Enqueue {0}", task);
			string qID = task.QueueID = scheduler(task);
			_Log.InfoFormat("..Q = {0}", qID);
			if (string.IsNullOrEmpty(qID)) return false;

			var job = new TaskJob(task, qID, program) { MaxTime = _MaxTime, Pause = task.Pause }; 
			job.Priority = task.StartTime;
			job.MaxTime = (int)(task.DueTime - task.StartTime).TotalMilliseconds;
			return _QH.Submit(job);
		}

		private static void ProcessQueuedTask(object arg)
		{
			var job = (TaskJob)arg;
			ScheduledTask task = job.Task;

			if (task != null) job.Program(task);
			else _Log.WarnFormat("null Task");
		}

	}
}


