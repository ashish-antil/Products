using System;
using System.Threading;

namespace Imarda.Lib
{
	public interface IThreadControl
	{
		/// <summary>
		/// When supposed to be finished
		/// </summary>
		DateTime Due { get; set; }

		/// <summary>
		/// Start of execution.
		/// </summary>
		DateTime Begin { get; set; }

		/// <summary>
		/// Exception thrown by the delegate code.
		/// </summary>
		/// <returns></returns>
		Exception Exception { get; }

		/// <summary>
		/// Run the job.
		/// </summary>
		void Run();

		/// <summary>
		/// Milliseconds between start of execution and end.
		/// </summary>
		/// <returns></returns>
		long Duration();
	}

	public enum ThreadControlStatus
	{
		New = 1,
		Busy = 6, // in execution
		Done = 10, // finished
		TimedOut = 11, // execution timed out
		Exception = 12, // client code threw an exception
		Aborted = 14, // thread was aborted
	}


	public delegate void CompletionCallback(ThreadControlStatus status, Exception ex);


	internal class ThreadControl : IThreadControl
	{
		private static volatile int _Count;
		internal volatile ThreadControlStatus Status;


		public ThreadControl(string prefix, AbortableThreadPool pool)
		{
			Thread = new Thread(pool.ProcessWork);
			Thread.Name = prefix + (++_Count);
			//DebugLog.Write("Create new worker thread " + Thread.Name);
			Go = new AutoResetEvent(false);
		}

		internal Thread Thread { get; set; }
		internal WaitCallback Proc { get; set; }
		internal AutoResetEvent Go { get; set; }
		internal object Arg { get; set; }
		internal CompletionCallback OnCompletion { get; set; }

		#region IThreadControl Members

		public DateTime Begin { get; set; }

		public long Duration()
		{
			TimeSpan ts = DateTime.UtcNow - Begin;
			return ts.Ticks/TimeSpan.TicksPerMillisecond;
		}

		public DateTime Due { get; set; }


		public void Run()
		{
			Go.Set();
		}

		/// <summary>
		/// Exception uncaught by Proc delegate code.
		/// </summary>
		public Exception Exception { get; internal set; }

		#endregion

		internal void Start()
		{
			Thread.Start(this);
		}


		public override string ToString()
		{
			return string.Format("WorkItem({0}: arg={1} alive?{2})", Thread.Name, Arg, Thread.IsAlive); 
		}
	}
}