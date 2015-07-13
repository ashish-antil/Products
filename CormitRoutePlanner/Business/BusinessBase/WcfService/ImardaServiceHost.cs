using Imarda.Logging;

#region

using System;
using System.ServiceModel;
using FernBusinessBase.Errors;
using System.Reflection;
using FernServiceBase;
using System.Threading;

#endregion

// ReSharper disable once CheckNamespace
namespace Imarda.Lib
{
    /// <summary>
    ///     All Imarda ServiceHosts are created in with class. Do not use System.ServiceModel.ServiceHost
    ///     directly elsewhere, because the host has to be re-created on failure.
    /// </summary>
    [Serializable]
    public sealed class ImardaServiceHost : IRestartableHost
	{
		private Type _serviceType;
		private string _serviceName;
        private string _logName;
        private ServiceHost _serviceHost;

        /// <summary>
        /// Instantiates a new service host object
        /// </summary>
        public ImardaServiceHost()
        {
        }

        public ImardaServiceHost(Type serviceType)
		{
			_serviceType = serviceType;
			_logName = "Svc_" + _serviceType.Name.Replace('.', '_');

			// Get a description from the service class
			FieldInfo fi = serviceType.GetField("Description");
			_serviceName = fi == null ? _serviceType.Name : (string) fi.GetValue(null);
		}

        /// <summary>
        /// Sets the service host's service type
        /// </summary>
        /// <param name="serviceType">Type of service to be instantiated</param>
        public void SetServiceType(Type serviceType)
        {
            this._serviceType = serviceType;
            this._logName = "Svc_" + this._serviceType.Name.Replace('.', '_');

            // Get a description from the service class
            FieldInfo fi = serviceType.GetField("Description");
            _serviceName = fi == null ? _serviceType.Name : (string)fi.GetValue(null);
        }
        
		public void SetThreadSettings()
		{
			int minWorkerThreads;
			int minCompletionPortThreads;
			ThreadPool.GetMinThreads(out minWorkerThreads, out minCompletionPortThreads);
			int minThreads = ConfigUtils.GetInt("MinThreads", 16);

			ThreadPool.SetMinThreads(minThreads, minThreads);

			int maxWorkerThreads;
			int maxCompletionPortThreads;
			ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);
			int maxThreads = ConfigUtils.GetInt("MaxThreads", 64);

			ThreadPool.SetMaxThreads(maxThreads, maxThreads);
		}

		public void Start()
		{
			try
			{
				// Start WCF service
				CreateServiceHost();

				SetThreadSettings();

				Log(FullServiceName + " Started");
			}
			catch (Exception ex)
			{
				ErrorHandler.HandleInternal(ex);
				throw;
			}
		}

		public void Stop()
		{
			try
			{
				// Stop WCF Service
				DestroyServiceHost();

				Log(FullServiceName + " Stopped");
			}
			catch (Exception e)
			{
				ErrorHandler.HandleInternal(e);
				throw;
			}
		}

		private void CreateServiceHost()
		{
			if (_serviceHost != null)
			{
				DestroyServiceHost();
			}

			_serviceHost = new ServiceHost(_serviceType);
			_serviceHost.Closed += ServiceHost_Closed;
			_serviceHost.Closing += ServiceHost_Closing;
			_serviceHost.Opened += ServiceHost_Opened;
			_serviceHost.Opening += ServiceHost_Opening;
			_serviceHost.UnknownMessageReceived += ServiceHost_UnknownMessageReceived;
			
            _serviceHost.Open();
			_serviceHost.Faulted += ServiceHost_Faulted;
		}

		private void DestroyServiceHost()
		{
		    if (_serviceHost == null)
		    {
		        return;
		    }

		    _serviceHost.Faulted -= ServiceHost_Faulted;
		    _serviceHost.Closed -= ServiceHost_Closed;
		    _serviceHost.Closing -= ServiceHost_Closing;
		    _serviceHost.Opened -= ServiceHost_Opened;
		    _serviceHost.Opening -= ServiceHost_Opening;
		    _serviceHost.UnknownMessageReceived -= ServiceHost_UnknownMessageReceived;

            //Abort also invokes Dispose of the underlying ServiceType object if the latter implements IDisposable
		    _serviceHost.Abort();
		    _serviceHost = null;
		}

		private void ServiceHost_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
		{
			Log(FullServiceName + " Service Host UnknownMessageReceived");
		}

		private void ServiceHost_Opening(object sender, EventArgs e)
		{
			Log(FullServiceName + " Opening");
		}

		private void ServiceHost_Opened(object sender, EventArgs e)
		{
			string msg = FullServiceName + " Opened";
			Log(msg);
			Console.WriteLine(msg);
		}

		private void ServiceHost_Closing(object sender, EventArgs e)
		{
			Log(FullServiceName + " Closing");
		}

		private void ServiceHost_Closed(object sender, EventArgs e)
		{
			Log(FullServiceName + " Closed");
		}

		private void ServiceHost_Faulted(object sender, EventArgs e)
		{
			try
			{
				Log(ServiceType + " Faulted; Resetting");

				// Discard
				DestroyServiceHost();

				// Recreate
				CreateServiceHost();

				Log(FullServiceName + " Service Host Faulted; Reset");
			}
			catch (Exception ex)
			{
				ErrorHandler.HandleInternal(ex);
			}
		}

		public Type ServiceType
		{
			get { return _serviceType; }
		}

		public string ServiceName
		{
			get { return _serviceName; }
		}

		public string FullServiceName
		{
			get { return ServiceName + " (" + ServiceType.Name + ")"; }
		}

		public ServiceHost Host
		{
			get { return _serviceHost; }
		}

		private void Log(string msg)
		{
			ErrorLogger.GetLogger(_logName).Info(msg);
		}

	}
}