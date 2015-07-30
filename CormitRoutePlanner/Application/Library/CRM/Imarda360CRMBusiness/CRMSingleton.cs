namespace Imarda360Application.CRM
{
	partial class ImardaCRM
	{
		#region Singleton

		static ImardaCRM() 
		{
		}

		private static readonly IImardaCRM _Instance = new ImardaCRM();

		public static IImardaCRM Instance { get { return _Instance; } }

		private ImardaCRM()
		{
		}

		#endregion
	}
}
