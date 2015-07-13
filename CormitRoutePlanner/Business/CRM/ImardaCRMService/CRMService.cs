/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\adamson.delacruz
Date Generated : 27/04/2009 1:27 PM
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using ImardaCRMBusiness;
using System.Data.SqlClient;
using Imarda.Lib;

namespace ImardaCRM 
{

	public class ImardaCRMService : FernServiceBase.FernBusinessServiceBase 
	{

		/// <summary>
		/// Passes the specific args for this Service to the base class
		/// </summary>
		public ImardaCRMService()
			: base("ImardaCRM")
		{
			RegisterHost(new ImardaServiceHost(typeof(ImardaCRMBusiness.ImardaCRM)));
		}

		/// <summary>
		/// Main program entry point
		/// </summary>
		public static void Main() 
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new ImardaCRMService() };
			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		#region Methods to re-initialize, or update, the database.
		protected override void InternalUpdateDatabase(SqlConnection conn) 
		{
			
		}
		#endregion
	}
}

