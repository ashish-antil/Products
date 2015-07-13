using System;
using System.Collections.Generic;
using ImardaVehicleBusiness;
using System.ServiceModel;
using System.Data.SqlClient;
using FernBusinessBase;
using Imarda360Application.CRM;
using Imarda360Application.VehicleManagement;
using Imarda360Application.Tracking;
using Imarda.Lib;

namespace Imarda360Application
{

	public class Imarda360Service : FernServiceBase.FernServiceBase
	{

		/// <summary>
		/// Passes the specific args for this Service to the base class
		/// </summary>
		public Imarda360Service() : base("ImardaSolutionService")
		{
			RegisterHost(new ImardaServiceHost(typeof(Imarda360Application.Imarda360), _InstanceID));
			RegisterHost(new ImardaServiceHost(typeof(Imarda360Application.Security.ImardaSecurity), _InstanceID));
		}

		/// <summary>
		/// Main program entry point
		/// </summary>
		public static void Main()
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new Imarda360Service() };
			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}
	}

}