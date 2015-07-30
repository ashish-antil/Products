using System;
using System.Configuration;
using System.ServiceModel;
using System.Xml;
using System.ServiceModel.Channels;
using ImardaSecurityBusiness;
using System.Diagnostics;
using FernBusinessBase;

namespace Cormit.Application.RouteApplication.Security
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


		private Proxy<ImardaSecurityBusiness.IImardaSecurity> _IImardaSecurityProxy
			= new Proxy<ImardaSecurityBusiness.IImardaSecurity>("SecurityTcpEndpoint");
		public ImardaSecurityBusiness.IImardaSecurity IImardaSecurityProxy
		{
			get { return _IImardaSecurityProxy.GetChannel(); }
		}

		private Proxy<ImardaConfigurationBusiness.IImardaConfiguration> _IImardaConfigurationProxy
			= new Proxy<ImardaConfigurationBusiness.IImardaConfiguration>("ConfigurationTcpEndpoint");
		public ImardaConfigurationBusiness.IImardaConfiguration IImardaConfigurationProxy
		{
			get { return _IImardaConfigurationProxy.GetChannel(); }
		}

	}
}