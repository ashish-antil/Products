
using System;
using System.Diagnostics;
using System.Threading;
using Imarda.Lib;

namespace Imarda.Logging
{
	public class QJob<S> where S : IComparable
	{
		public static long JobSeqNum;
		public static int DefaultMaxTime = 8000;
		public volatile JobStatus DeadLetterStatus; // can be inspected in the DeadLetterWork
		public S Priority;
		public volatile int Retries; // number of retries of TimedOut or Exception, default = 0
		public TimeSpan RetryDelay; // delay to add for each retry, can be changed in client code
		public volatile JobStatus Status; // communicate the status of the job between system and client

		public long ID { get; private set; }
		public string QueueID { get; set; } // identifies the queue to post the job into
		public WaitCallback Work { get; internal set; } // client code to be executed by an abortable thread
		public WaitCallback DeadLetterWork { get; set; } // when job Work fails all retries, execute this in a special queue.
		public int MaxTime { get; set; }	// maximum time in millis that the job may take to finish, if exceeded then the thread will be aborted
		public Exception Exception { get; set; } // thrown by client code
		public EventWaitHandle Notify { get; set; } // optional waithandle, to be set by client and to be waited for by client
		public Stopwatch Begin { get; set; } // optional stopwatch, is assigned if JobQueueHandler.TimeKeeping is true.
		public virtual DateTime Earliest { get; set; } // earliest possible time for job too start
		public int Pause { get; set; } // minimum time to pause the queue after successful completion (millis) 

		public QJob(string qid, WaitCallback work)
			: this(qid, work, DefaultMaxTime)
		{
		}

		/// <summary>
		/// Create a job 
		/// </summary>
		/// <param name="qid">identifies the queue where to post this job too</param>
		/// <param name="work"></param>
		/// <param name="maxTime"></param>
		public QJob(string qid, WaitCallback work, int maxTime)
		{
			ID = Interlocked.Increment(ref JobSeqNum);
			QueueID = qid;
			Work = work;
			MaxTime = maxTime;
		}


		public override string ToString()
		{
			return Info();
		}

		protected string Info()
		{
			string time = Begin == null ? string.Empty : string.Format("{0} ms, ", Begin.ElapsedMilliseconds);
			string earliest = Earliest == DateTime.MinValue ? string.Empty : (", " + Earliest.ToString("HH:mm:ss.fff"));
			return string.Format("{6}(Q={0} ID={1}, {2}, {3}r{4}{5})", QueueID, ID, Status, time, Retries, earliest, GetType().Name);
		}
	}
}