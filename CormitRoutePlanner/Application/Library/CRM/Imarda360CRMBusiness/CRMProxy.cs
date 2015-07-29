/***********************************************************************
Auto Generated Code

Generated by   : adam-Laptop\adam
Date Generated : 28/04/2009 12:23 PM
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Configuration;
using System.ServiceModel;
using System.Xml;
using System.ServiceModel.Channels;
//using System.Web.Services.Description;
using ImardaCRMBusiness ;
using FernBusinessBase;

namespace Imarda360Application.CRM
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

		#region ImardaCRMService
		private Proxy<ImardaCRMBusiness.IImardaCRM> _IImardaCRMProxy
			= new Proxy<ImardaCRMBusiness.IImardaCRM>("CRMTcpEndpoint");
		public ImardaCRMBusiness.IImardaCRM IImardaCRMProxy
		{
			get { return _IImardaCRMProxy.GetChannel(); }
		}

		#endregion
	}
}

