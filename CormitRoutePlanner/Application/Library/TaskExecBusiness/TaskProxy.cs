
using System;
using System.Configuration;
using System.ServiceModel;
using System.Xml;
using System.ServiceModel.Channels;

//using System.Web.Services.Description;
using ImardaTaskBusiness;
using FernBusinessBase;


namespace Imarda360Application.Task
{
	public class ImardaProxyManager
	{
		#region ImardaProxyManager is a Singleton

		public static ImardaProxyManager Instance
		{
			get
			{
				if (_Instance == null) _Instance = new ImardaProxyManager();
				return _Instance;
			}
		}

		private static ImardaProxyManager _Instance;

		/// <summary>
		/// Constructs the one and only ImardaProxyManager.
		/// </summary>
		private ImardaProxyManager()
		{

		}

		#endregion

		private Proxy<ImardaTaskBusiness.IImardaTask> _IImardaTaskProxy
			= new Proxy<ImardaTaskBusiness.IImardaTask>("TaskTcpEndpoint");
		public ImardaTaskBusiness.IImardaTask IImardaTaskProxy
		{
			get { return _IImardaTaskProxy.GetChannel(); }
		}

		/// <summary>
		/// Access to the Proxy required for the TaskManager.
		/// </summary>
		/// <returns></returns>
		public Proxy<ImardaTaskBusiness.IImardaTask> GetTaskServiceProxy()
		{
			return _IImardaTaskProxy;
		}

		

		private Proxy<ImardaConfigurationBusiness.IImardaConfiguration> _IImardaConfiguration
			= new Proxy<ImardaConfigurationBusiness.IImardaConfiguration>("ConfigurationTcpEndpoint");
		public ImardaConfigurationBusiness.IImardaConfiguration IImardaConfigurationProxy
		{
			get { return _IImardaConfiguration.GetChannel(); }
		}

		private Proxy<ImardaSecurityBusiness.IImardaSecurity> _IImardaSecurity
			= new Proxy<ImardaSecurityBusiness.IImardaSecurity>("SecurityTcpEndpoint");
		public ImardaSecurityBusiness.IImardaSecurity IImardaSecurityProxy
		{
			get { return _IImardaSecurity.GetChannel(); }
		}


        private Proxy<ImardaCRMBusiness.IImardaCRM> _IImardaCRM
                = new Proxy<ImardaCRMBusiness.IImardaCRM>("CRMTcpEndpoint");
        public ImardaCRMBusiness.IImardaCRM IImardaCRMProxy
        {
            get { return _IImardaCRM.GetChannel(); }
        }

        private Proxy<ImardaNotificationBusiness.IImardaNotification> _IImardaNotification
            = new Proxy<ImardaNotificationBusiness.IImardaNotification>("NotificationTcpEndpoint");
        public ImardaNotificationBusiness.IImardaNotification IImardaNotificationProxy
        {
            get { return _IImardaNotification.GetChannel(); }
        }
	

		
	}
}

