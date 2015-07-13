using System;
using System.Collections.Generic;
using Imarda.Lib;

namespace Imarda.Logging
{
	public interface IJobQueueControl<S> where S : IComparable
	{
		/// <summary>
		/// Get number of jobs in the queue that satisfy the condition
		/// </summary>
		/// <param name="qID">queue</param>
		/// <param name="condition"></param>
		/// <returns>number of jobs</returns>
		int GetJobCount(string qID, Func<QJob<S>, bool> condition);

		/// <summary>
		/// Get the first job that has the condition
		/// </summary>
		/// <param name="qID">queue</param>
		/// <param name="condition">if evaluates to true, select that job</param>
		/// <returns>the job or null if not found</returns>
		QJob<S> GetJob(string qID, Func<QJob<S>, bool> condition);

		/// <summary>
		/// Cancel the jobs with the given condition
		/// </summary>
		/// <param name="qID">identifies queue</param>
		/// <param name="criteria">condition selects the jobs to cancel</param>
		/// <returns>cancelled jobs or empty list; not null</returns>
		List<QJob<S>> Cancel(string qID, Func<QJob<S>, bool> criteria);

		/// <summary>
		/// Wake the queue with the given ID. 
		/// </summary>
		/// <param name="qID">identifies queue</param>
		/// <returns>true if qID exists</returns>
		bool Wake(string qID);

		/// <summary>
		/// Stop processing this queue until Wake is called
		/// </summary>
		/// <param name="qID">identifies queue</param>
		/// <returns>true if qID exists</returns>
		bool Sleep(string qID);

		/// <summary>
		/// Wake the queue if sleeping. Do nothing if not sleeping.
		/// </summary>
		/// <param name="qID">identifies queue</param>
		/// <param name="wakeup">new wakeup time</param>
		/// <param name="ifSooner">false = assign wakeup time; true = only assign new wakeup time if sooner than currently assigned wakeup time</param>
		/// <returns>true if qID exists</returns>
		bool WakeAt(string qID, DateTime wakeup, bool ifSooner);

		/// <summary>
		/// Stop processing all existing queues in given category until the given time.
		/// Any new queues added since this call will not be affected.
		/// </summary>
		/// <param name="category"></param>
		/// <param name="wakeUp"></param>
		/// <returns></returns>
		bool WakeCategoryAt(string category, DateTime wakeUp);
	}
}