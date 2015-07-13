using ImardaNotificationBusiness;
using Imarda.Lib;

namespace ImardaNotification
{

	public class ImardaNotificationService : FernServiceBase.FernServiceBase
	{

		/// <summary>
		/// Passes the specific args for this Service to the base class
		/// </summary>
		public ImardaNotificationService()
			: base("ImardaNotification")
		{
			RegisterHost(new ImardaServiceHost(typeof(ImardaNotificationBusiness.ImardaNotification)));
		}

		/// <summary>
		/// Main program entry point
		/// </summary>
		public static void Main()
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new ImardaNotificationService() };
			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		#region Methods to re-initialize, or update, the database.
		/*protected override void internalUpdateDatabase(SqlConnection conn) 
		{
			
		}*/
		#endregion
	}
}
