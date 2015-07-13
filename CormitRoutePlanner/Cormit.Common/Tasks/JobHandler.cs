using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Imarda.Lib
{
	public enum JobStatus
	{
		Rejected = 2, // job was not accepted by the JobQueueHandler
		NoThread = 3, // job could not start because no thread available
		NoLease = 4, // required leases for the job unavaible
		Accepted = 5, // job is queued for the first time
		Busy = 6, // in execution
		Requeue = 7, // job should be reposted to the queue
		Cancel = 8, // while queued: cancel this job, do not execute
		Done = 10, // handled and finished
		TimedOut = 11, // execution timed out
		Exception = 12, // client code threw an exception
		Retry = 13, // client code indicated job must be retried
		Sleep = 14, // client code indicates job must be blocked and externally woken up
	}

}