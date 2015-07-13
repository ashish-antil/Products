namespace Imarda360Application.Task
{
	partial class ImardaTask
	{
		#region Singleton

		static ImardaTask()
		{
		}

		private static readonly IImardaTask _Instance = new ImardaTask();

		public static IImardaTask Instance { get { return _Instance; } }

		private ImardaTask()
		{
		}

		#endregion
	}
}
