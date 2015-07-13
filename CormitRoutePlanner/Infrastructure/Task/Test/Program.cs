/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\maurice.verheijen
Date Generated : 24/06/2009 9:55 a.m.
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using TestBusinessHost.Services;

namespace TestBusinessHost
{
	class Program
	{
		//private static ILog _Log = LogManager.GetLogger(typeof(Program));

		static void Main(string[] args)
		{
			Console.WindowWidth = 120;

			//_Log.Info("Starting Up Imarda Business Host.");

			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			Run();

			//_Log.Info("All Business Services Have Started.");

			Console.ReadLine();
		}

		private static void Run()
		{
			// start all the services here
			Start(new ImardaTaskBusinessHost ());
		}

		private static void Start(ServiceHostBase service)
		{
			service.Start();
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			try
			{
				Console.Title = "FATAL - Unhanded Exception in AppDomain";
				//_Log.Fatal("Unhandled Exception in Host AppDomain. Host Will Now Quit.", e.ExceptionObject as Exception);
				Console.ReadLine();
			}
			catch
			{
				// don't throw an exception in the exception handler
			}
		}
	}
}


