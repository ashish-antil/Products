using Imarda.Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Imarda.Logging
{
	/// <summary>
	/// Maintains a circular list of queues that get serviced by a thread. The queues contains jobs
	/// that will be executed by AbortableThreadPool threads.
	/// </summary>
	/// <example>
	/// <code>
	/// </code>
	/// </example>
	public class JobQueueHandler<S> : Imarda.Logging.IJobQueueControl<S> where S : IComparable
	{
		private readonly ErrorLogger _ExceptionLog;
		private readonly S _LowestPriority;
		private readonly TimeSpan _MaxEmptyTime = TimeSpan.FromSeconds(10);
		private readonly CircularList<JobQueue> _Qs;
		private readonly SortOrder _SortOrder;
		private readonly object _TSync = new object();
		private readonly AbortableThreadPool _WorkThreads;
		private volatile bool _Accept;
		private EventWaitHandle _CheckForJobs;
		private volatile bool _Run;
		private Thread _SchedulerThread;
		private DelayCycle _HeartbeatCycle;
		private TimeSpan _LoopSleep;

		private readonly string _Name;


		public JobQueueHandler(AbortableThreadPool threadPool, ErrorLogger exceptionLog, SortOrder sortOrder, S lowestPriority, string name)
		{
			_SortOrder = sortOrder;
			_LowestPriority = lowestPriority;
			_WorkThreads = threadPool;
			_Qs = new CircularList<JobQueue>();
			DefaultRetryDelay = TimeSpan.FromMilliseconds(500);
			_ExceptionLog = exceptionLog;
			_Name = name + "/" + (ConfigUtils.GetString("QueueHandlerName") ?? "JQH");
			_LoopSleep = ConfigUtils.GetTimeSpan("SchedulerCycleDelay", TimeSpan.FromMilliseconds(2500));

			_HeartbeatCycle = new DelayCycle(
				ConfigUtils.GetTimeSpan("HeartbeatInterval.Process", TimeSpan.FromSeconds(5)),
				() => AttentionUtils.SendHeartbeat(new Guid("48A1F726-4418-BEA7-9D70-B7B8D7543ED5")));

			AttentionUtils.Attention(new Guid("AC8BC5CC-0553-FEED-8E96-AD36F1FC93AA"), _Name + " started");
			DebugLog.Write("JobQueueHandler \"{0}\" started from {1}", _Name, new StackTrace());
		}

		public bool TimeKeeping { get; set; }
		public TimeSpan DefaultRetryDelay { get; set; }

		public int QueueCount
		{
			get { lock (_Qs) return _Qs.Count; }
		}

		public bool IsRunning
		{
			get { return _Run && _SchedulerThread.IsAlive; }
		}

		#region IJobQueueControl<S> Members

		/// <summary>
		/// Cancel in the given queue all waiting jobs matching the given criteria.
		/// </summary>
		/// <param name="qID">identifies the queue</param>
		/// <param name="criteria"></param>
		/// <returns>List of cancelled jobs</returns>
		public List<QJob<S>> Cancel(string qID, Func<QJob<S>, bool> criteria)
		{
			lock (_Qs)
			{
				var list = new List<QJob<S>>();
				JobQueue jq = _Qs.First(q => q.ID == qID);
				if (jq != _Qs.NullItem)
				{
					lock (jq)
					{
						foreach (var job in jq.Jobs.Where(criteria))
						{
							if (job.Status == JobStatus.Busy) continue;

							DebugLog.Write("## Cancelling {0}", job);
							job.Status = JobStatus.Cancel;
							job.Earliest = DateTime.MinValue;
							list.Add(job);
							DebugLog.Write("## .. done {0}", job);
						}
					}
				}
				if (list.Count > 0) _CheckForJobs.Set();
				return list;
			}
		}

		public QJob<S> DropFirst(string qID)
		{
			lock (_Qs)
			{
				QJob<S> job = null;
				JobQueue jq = _Qs.First(q => q.ID == qID);
				if (jq != _Qs.NullItem && jq.Jobs.Count > 0)
				{
					lock (jq)
					{
						job = jq.Jobs.Peek();
						job.Status = JobStatus.Cancel;
						job.Earliest = DateTime.MinValue;
					}
				}
				if (job != null) _CheckForJobs.Set();
				return job;
			}
		}

		/// <summary>
		/// Open for submitting messages.
		/// </summary>
		/// <returns></returns>
		public void Open()
		{
			_CheckForJobs = new AutoResetEvent(false);
			_Accept = true;
		}

		public bool Wake(string qID)
		{
			return WakeAt(qID, DateTime.MinValue, false);
		}

		public bool Sleep(string qID)
		{
			return WakeAt(qID, DateTime.MaxValue, false);
		}

		/// <summary>
		/// Wake the queue if sleeping. Do nothing if not sleeping.
		/// </summary>
		/// <param name="qID">identifies queue</param>
		/// <param name="wakeup">new wakeup time</param>
		/// <param name="ifSooner">false = assign wakeup time; true = only assign new wakeup time if sooner than currently assigned wakeup time</param>
		/// <returns></returns>
		public bool WakeAt(string qID, DateTime wakeup, bool ifSooner)
		{
			lock (_Qs)
			{
				JobQueue jq = _Qs.First(q => q.ID == qID);
				if (jq != _Qs.NullItem)
				{
					lock (jq)
					{
						if (jq.Jobs.Count > 0)
						{
							QJob<S> job = jq.Jobs.Peek();
							if (job.Status == JobStatus.Sleep)
							{
								DebugLog.Write("## WakeAt {0:s} {1}", wakeup, job);
								if (ifSooner)
								{
									if (wakeup < job.Earliest) job.Earliest = wakeup;
								}
								else
								{
									job.Earliest = wakeup;
								}
								job.Status = JobStatus.Retry;
								job.Retries = 0;
								_CheckForJobs.Set();
							}
							DebugLog.Write("## .. done {0}", job);
						}
					}
					return true;
				}
				return false;
			}
		}

		public bool WakeCategoryAt(string category, DateTime wakeUp)
		{
			bool woken = false;
			lock (_Qs)
			{
				_Qs.Do(delegate(JobQueue jq)
				{
					if (jq.Category == category)
					{
						lock (jq)
						{
							if (jq.Jobs.Count > 0)
							{
								QJob<S> job = jq.Jobs.Peek();
								DebugLog.Write("## Waking cat {0} queue {1}", category, job);
								job.Earliest = wakeUp;
								DebugLog.Write("## .. done {0}", job);
								woken = true;
							}
						}
					}
					return jq;
				});
			}
			if (woken) _CheckForJobs.Set();
			return woken;
		}

		/// <summary>
		/// Get the job with the given criteria in the given queue
		/// </summary>
		/// <param name="qID">identifies the queue to search</param>
		/// <param name="condition">the criteria</param>
		/// <returns>job object</returns>
		public QJob<S> GetJob(string qID, Func<QJob<S>, bool> condition)
		{
			lock (_Qs)
			{
				JobQueue jq = _Qs.First(q => q.ID == qID);
				if (jq != _Qs.NullItem)
				{
					lock (jq) return jq.Jobs.FirstOrDefault(condition);
				}
				return null;
			}
		}

		public int GetJobCount(string qID)
		{
			lock (_Qs)
			{
				JobQueue jq = _Qs.First(q => q.ID == qID);
				if (jq != _Qs.NullItem)
				{
					lock (jq) return jq.Jobs.Count;
				}
				return 0;
			}
		}

		/// <summary>
		/// Get the number of jobs with the given criteria.
		/// </summary>
		/// <param name="qID">identifies the queue to search</param>
		/// <param name="condition">the criteria</param>
		/// <returns>count</returns>
		public int GetJobCount(string qID, Func<QJob<S>, bool> condition)
		{
			lock (_Qs)
			{
				JobQueue jq = _Qs.First(q => q.ID == qID);
				if (jq != _Qs.NullItem)
				{
					lock (jq) return jq.Jobs.Count(condition);
				}
				return 0;
			}
		}

		#endregion

		public int Total()
		{
			lock (_Qs)
			{
				return _Qs.ToArray().Sum(q => q.Jobs.Count);
			}
		}

		/// <summary>
		/// Synchronized start of the cache. Return from this call after thread has started running.
		/// </summary>
		public void Start()
		{
			lock (_TSync)
			{
				if (_SchedulerThread != null && _SchedulerThread.IsAlive) return;
				_SchedulerThread = new Thread(RoundRobin)
														{
															Name = _Name + "Thread",
															IsBackground = true
														};

				var notifyStart = new ManualResetEvent(false);
				_SchedulerThread.Start(notifyStart);
				notifyStart.WaitOne();
				DebugLog.Write(GetType().Name + " started");
			}
		}

		public void StopNow()
		{
			lock (_TSync)
			{
				_Accept = false;
				if (_Run)
				{
					_Run = false;
				}
			}
		}

		public void Close()
		{
			StopNow();
			_WorkThreads.Close();
		}

		public bool Submit(QJob<S> job)
		{
			return Submit(job, null);
		}

		public bool Submit(QJob<S> job, string category)
		{
			try
			{
				if (_Accept)
				{
					if (TimeKeeping) job.Begin = Stopwatch.StartNew();
					job.Status = JobStatus.Accepted;
					lock (_Qs)
					{
						JobQueue jq = _Qs.First(x => x.ID == job.QueueID);
						if (jq != _Qs.NullItem)
						{
							lock (jq)
							{
								jq.Jobs.Enqueue(job, job.Priority);
								jq.EmptySince = DateTime.MaxValue;
							}
							DebugLog.Write("Enqueued {0} in existing queue, {1} jobs in queue", job, jq.Jobs.Count);
						}
						else
						{
							var newq = new SortedQueue<QJob<S>, S>(_SortOrder, _LowestPriority);
							newq.Enqueue(job, job.Priority);
							jq = new JobQueue { ID = job.QueueID, Category = category, Jobs = newq, EmptySince = DateTime.MaxValue };
							DebugLog.Write("Enqueued {0} in new queue", job);
							_Qs.InsertAtEnd(jq);
						}
					}
					_CheckForJobs.Set();
					return true;
				}
				DebugLog.Write("Submit to closed queue: {0}", job);
				return false;
			}
			catch (Exception ex)
			{
				DebugLog.Write(ex);
				return false;
			}
		}

		public IEnumerable<string> GetQueueIDsWhere(Predicate<IJobQueue> selector)
		{
			lock (_Qs)
			{
				return _Qs.ToArray().Where(jq => selector(jq)).Select(jq => jq.ID);
			}
		}

		///// <summary>
		///// Apply an operation to all Jobs that match the condition.
		///// </summary>
		///// <param name="qID">identifies the queue to search</param>
		///// <param name="condition">job selection predicate</param>
		///// <param name="action">apply action</param>
		//public void DoAll(string qID, Action<Job> action)
		//{
		//  lock (_Qs)
		//  {
		//	JobQueue jq = _Qs.First(q => q.ID == qID);
		//	if (jq != _Qs.NullItem)
		//	{
		//	  lock (jq)
		//	  {
		//		foreach (Job job in jq.Jobs) action(job);
		//	  }
		//	}
		//  }
		//}

		/// <summary>
		/// Scheduling algorithm. Cycle through all the queues and check 
		/// if there is a job to be executed. If so, allocate a thread from the 
		/// pool and run the job in it. 
		/// </summary>
		/// <remarks>
		/// Queues that are empty for a while will get removed by this method.
		/// Some Job properties are set by this method. Waiting clients can
		/// get notified for job completion.
		/// </remarks>
		/// <param name="notifyStarted"></param>
		private void RoundRobin(object notifyStarted)
		{
			try
			{
				((EventWaitHandle)notifyStarted).Set();
				_Run = true;

				bool removeNode = false;

				while (_Run)
				{
					if (_Qs.Count != 0)
					{
						JobQueue jq;
						lock (_Qs) jq = _Qs.CurrentItem;
						lock (jq)
						{
							DateTime now = DateTime.UtcNow;
							if (!jq.Busy && jq.PauseUntil < now)
							{
								if (jq.Jobs.Count == 0)
								{
									if (jq.EmptySince < now - _MaxEmptyTime)
									{
										removeNode = true;
									}
									else if (jq.EmptySince == DateTime.MaxValue)
									{
										jq.EmptySince = now;
									}
								}
								else
								{
									QJob<S> job = jq.Jobs.Peek();
									if (job.Status == JobStatus.Cancel)
									{
										jq.Jobs.Dequeue();
										jq.Busy = false;
										DebugLog.Write("Skip/Dequeue {0}", job);
									}
									else if (job.Earliest < now)
									{
										job.Status = JobStatus.Busy;
										IThreadControl ctl = _WorkThreads.GetThread(job.Work, job, delegate(ThreadControlStatus exec, Exception ex)
										{
											// executed on completion by abortable thread
											// Note: inside this delegate the lock (jq) above does not apply! We have to lock whereever we use jq.
											// exec is ThreadControlStatus.Success or ThreadControlStatus.TimedOut or ThreadControlStatus.Exception

											DebugLog.Write("-Finalize {0} {1} - {2}", job, exec, ex);

											switch (exec)
											{
												case ThreadControlStatus.TimedOut:
													job.Status = JobStatus.TimedOut;
													break;
												case ThreadControlStatus.Exception:
													job.Status = JobStatus.Exception;
													break;
											}

											if (ex != null && _ExceptionLog != null)
											{
												string s = string.Format("{0}  {1}", job, ex);
												_ExceptionLog.Error(s);
											}

											switch (job.Status)
											{
												case JobStatus.Busy:
													job.Status = JobStatus.Done;
													goto case JobStatus.Done;

												case JobStatus.Done:
												case JobStatus.Exception:
												case JobStatus.Cancel:
													DateTime pauseUntil = job.Pause <= 0 ? DateTime.MinValue : DateTime.UtcNow.AddMilliseconds(job.Pause);
													lock (jq) jq.PauseUntil = pauseUntil;
													goto default;

												case JobStatus.Retry:
												case JobStatus.TimedOut:
													// timeout: thread is already busy aborting
													if (job.Retries-- > 0)
													{
														SetDelay(job);
														DebugLog.Write("-Retry {0}", job);
														break;
														// default is 0 retries, which means job should succeed first time
													}
													if (job.Work == job.DeadLetterWork || job.DeadLetterWork == null)
													{
														job.Work = null;
														// processing of deadletter done and failed, or no deadletter processing defined: give up.
														goto default;
													}
													// retry job, but now with DeadLetterWork object, don't remove from queue here
													DebugLog.Write("-Fatal {0}", job);
													job.Work = job.DeadLetterWork;
													job.Earliest = DateTime.MinValue;
													job.DeadLetterStatus = job.Status;
													job.Exception = ex;
													break;

												case JobStatus.Sleep:
													job.Earliest = DateTime.MaxValue;
													DebugLog.Write("-Sleep {0}", job);
													break;

												case JobStatus.Requeue:
													// client's code has set the status to Requeue (to be put back at the end of the queue)
													// job is still in queue and should be requeued for later
													SetDelay(job);
													lock (jq)
													{
														jq.Jobs.Remove(job);
														// do not call dequeue because meanwhile a higher priority job may have jumped queue!
														jq.Jobs.Enqueue(job, job.Priority);
													}
													DebugLog.Write("-Requeue {0}", job);
													break;

												default:
													job.Exception = ex;
													if (job.Notify != null) job.Notify.Set();
													lock (jq) jq.Jobs.Remove(job);
													DebugLog.Write("-Dequeue {0}", job);
													break;
											}
											jq.Busy = false;
											DebugLog.Write("-Done {0}", jq);

											_CheckForJobs.Set();
										});

										if (ctl != null)
										{
											ctl.Due = DateTime.UtcNow + TimeSpan.FromMilliseconds(job.MaxTime);
											jq.Busy = true;

											_HeartbeatCycle.Notify();
											DebugLog.Write("Start {0}", ctl);

											//^^^^^^^^^^^^^^^^^^^^^^^
											ctl.Run(); // run the job
											//vvvvvvvvvvvvvvvvvvvvvvv
										}
										else
										{
											DebugLog.Write("# # # #  No thread available for {0}", jq.ID);
											Thread.Sleep(100);
										}
									}
								}
							}
							//DebugLog.Write("# # # #  Queue {0}", jq);
						}
					}
					bool atStart;
					lock (_Qs)
					{
						if (removeNode)
						{
							_Qs.RemoveCurrent();
							removeNode = false;
						}
						JobQueue r = _Qs.MoveNext();
						if (r == null) DebugLog.Write("# # # # No queues");
						atStart = _Qs.AtStart;
					}

					if (atStart)
					{
						var now = DateTime.UtcNow;

						if (!Array.Exists(_Qs.ToArray(), q => q.Ready(now)))
						{
							// no queue exists that is ready for processing, we sleep for a while
							bool woken = _CheckForJobs.WaitOne(_LoopSleep);
							DebugLog.Write(woken ? "Woken up" : "Sleep");
						}
						//else
						//{
						//  DebugLog.Write("Ready Queue"); 
						//}
					}
				}
				DebugLog.Write("JQH Ends here.");
			}
			catch (Exception ex)
			{
				AttentionUtils.Attention(new Guid("769F0B1D-02CC-BADD-AABE-E2B299070BAF"), "{0} has crashed: {1}", _Name, ex);
				DebugLog.Write(_Name + " crashed.");
			}
		}

		private void SetDelay(QJob<S> job)
		{
			TimeSpan delay = job.RetryDelay > TimeSpan.Zero ? job.RetryDelay : DefaultRetryDelay;
			job.Earliest = DateTime.UtcNow.Add(delay);
		}


		public IDictionary<string, string> GetInfo()
		{
			var list = new Dictionary<string, string>();
			lock (_Qs)
			{
				JobQueue[] arr = _Qs.ToArray();
				foreach (JobQueue q in arr)
				{
					list[q.ID] = q.ToString();
				}
			}
			return list;
		}

		/// <summary>
		/// Get an array of all jobs in all queues
		/// </summary>
		/// <returns>array of all jobs</returns>
		public QJob<S>[] GetJobAllJobs()
		{
			var list = new List<QJob<S>>();
			lock (_Qs)
			{
				JobQueue[] arr = _Qs.ToArray();
				foreach (JobQueue q in arr) list.AddRange(q.GetQueueCopy());
			}
			return list.ToArray();
		}


		#region Nested type: JobQueue

		private class JobQueue : IJobQueue
		{
			internal const string DeadLetter = "DeadLetter";

			internal volatile bool Busy;

			public bool IsBusy { get { return Busy; } }
			public string Category { get; internal set; }
			public DateTime EmptySince { get; internal set; }
			public string ID { get; internal set; }

			public DateTime PauseUntil;


			internal SortedQueue<QJob<S>, S> Jobs;

			public QJob<S>[] GetQueueCopy()
			{
				QJob<S>[] jobs = Jobs.GetItemArray();
				return jobs;
			}

			public bool Ready(DateTime time)
			{
				if (Busy) return false;
				lock (this)
				{
					if (Jobs.Count == 0 || Jobs.Peek().Earliest > time) return false;
				}
				return true;
			}

			public override string ToString()
			{
				var job = Jobs.FirstOrDefault();
				string jobInfo = job != null ? job.ToString() : "empty";
				return string.Format("ItemQueue({0}, {1} jobs, {2}{3},{4})",
														 ID,
														 Jobs.Count,
														 Busy ? "Busy" : "Idle",
														 EmptySince == DateTime.MaxValue ? "" : ", " + EmptySince.ToString(":mm:ss.fff"),
														 jobInfo
					);
			}
		}

		#endregion
	}
}