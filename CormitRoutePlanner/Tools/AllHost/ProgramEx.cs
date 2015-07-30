#define DEVICECOMM
#define FULL
#define RUN_TASK

#region

using Imarda.Lib;
using Imarda.Logging;

using ImardaServiceHostManager;
using System;
using System.Collections.Generic;
using System.Linq;

using ConfigUtils = Imarda.Lib.ConfigUtils;

#endregion

namespace AllHost
{
	public class Program
	{
		public static void Main(string[] args)
		{
	

			var include = args.FirstOrDefault(c => c.StartsWith("/include:"));
			var services = new HashSet<string>();
			if (include != null)
			{
				services = new HashSet<string>(include.Substring("/include:".Length).Split('|'));
			}

			if (services.Count == 0)
			{
				services.Add("CormitLogging");
				services.Add("CormitTask");
				services.Add("CormitConfiguration");
				services.Add("CormitNotification");
				services.Add("CormitAlerting");
                services.Add("CormitTracking");
				services.Add("CormitSecurity");
				services.Add("CormitReport");
				services.Add("Cormit360");
				
			}
			

			var hostTypes = new List<Type>();



			hostTypes.Add(typeof (Cormit.Business.RouteTracking.CormitRouteTracking));


            hostTypes.Add(typeof(Cormit.Application.RouteApplication.Cormit24));
			

			Console.WindowWidth = 120;
			Console.BufferHeight = 10000;

			ErrorLogger Log = ErrorLogger.GetLogger("AllHosts");
			Log.Info("Start of Cormit360");

			var manager = new ServiceHostManager(hostTypes.ToArray());
			manager.StartHosts();


			Console.ReadLine();

			manager.StopHosts();
			manager.Dispose();

			
          
		}


	}
}
