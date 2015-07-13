using Imarda.Lib;

namespace ImardaTask
{
    public class SchedulerThreadPool : AbortableThreadPool
	{
		#region Singleton

		static SchedulerThreadPool()
		{
		}

		private static readonly SchedulerThreadPool _Instance = new SchedulerThreadPool();

		public static SchedulerThreadPool Instance { get { return _Instance; } }

		private SchedulerThreadPool()
			: base("TM")
		{
		}

		#endregion

	}
}
