namespace Cormit.Application.RouteApplication.Security
{
	partial class ImardaSecurity
	{
		#region Singleton

		static ImardaSecurity() 
		{
		}

		private static readonly IImardaSecurity _Instance = new ImardaSecurity();

		public static IImardaSecurity Instance { get { return _Instance; } }

		private ImardaSecurity()
		{
		}

		#endregion
	}
}
