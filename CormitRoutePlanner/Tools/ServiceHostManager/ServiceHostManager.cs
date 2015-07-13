using Imarda.Logging;

#region

using System;
using System.Collections.Generic;
using Imarda.Lib;
using Imarda.Lib.MVVM.Common;

#endregion

// ReSharper disable once CheckNamespace
namespace ImardaServiceHostManager
{
	public sealed class ServiceHostManager : Disposable
	{
	    private static readonly ErrorLogger Log = ErrorLogger.GetLogger("ServiceHostManager");
	    private readonly Type[] _serviceTypes;
	    private readonly List<ImardaServiceHost> _serviceHosts;

	    public ServiceHostManager(Type[] serviceTypes)
		{
            Console.WindowWidth = 120;

			Log.Info("Start of Imarda360");
			_serviceTypes = serviceTypes;
	        _serviceHosts = new List<ImardaServiceHost>();
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		}

		public void StartHosts()
		{
            StopHosts();

            foreach (Type type in _serviceTypes)
            {
                try
                {
                    var host = new ImardaServiceHost(type);
                    host.Start();

                    _serviceHosts.Add(host);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            string msg = "** All Services Have Started ** " + DateTime.Now.ToString("t") + " ** ";
            Log.Info(msg);
            Console.WriteLine(msg);
		}

        public void StopHosts()
        {
            if (_serviceHosts.Count < 1)
            {
                return;
            }

            foreach (var serviceHost in _serviceHosts)
            {
                try
                {
                    serviceHost.Stop();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            _serviceHosts.Clear();

            Console.WriteLine("** All Services Have Stopped ** " + DateTime.Now.ToString("t") + " ** ");
        }

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			try
			{
				Console.Title = "FATAL - Unhanded Exception in AppDomain";
				Log.ErrorFormat("Unhandled exception in host appdomain. Host will now quit. {0}", e.ExceptionObject as Exception);
				Console.ReadLine();
			}
			catch
			{
				// don't throw an exception in the exception handler
			}
		}

	    protected override void Dispose(bool disposing)
	    {
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
	    }
	}
}
