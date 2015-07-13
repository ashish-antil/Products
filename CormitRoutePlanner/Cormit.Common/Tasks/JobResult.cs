using System;
using System.Diagnostics;

namespace Imarda.Lib
{
	public class JobResult
	{
		public Stopwatch Begin;
		public Exception Exception;
		public Guid ID;
		public JobStatus Status;

		public override string ToString()
		{
			return string.Format("JobResult({0}, {1}, {2}ms)",
			                     ID.ToString().Substring(0, 8), Status, Begin != null ? Begin.ElapsedMilliseconds : -1);
		}
	}
}