using System.Data.SqlClient;
using Imarda.Lib;

namespace ImardaLogging 
{

    public class ImardaLoggingService : FernServiceBase.FernBusinessServiceBase 
	{

		/// <summary>
		/// Passes the specific args for this Service to the base class
		/// </summary>
		public ImardaLoggingService()
			: base("ImardaLogging")
		{
			RegisterHost(new ImardaServiceHost(typeof(ImardaLoggingBusiness.ImardaLogging)));
		}

		/// <summary>
		/// Main program entry point
		/// </summary>
		public static void Main() 
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new ImardaLoggingService() };
			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		#region Methods to re-initialize, or update, the database.
		protected override void InternalUpdateDatabase(SqlConnection conn) 
		{
			
		}
		#endregion
	}
}

