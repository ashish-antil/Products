/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\maurice.verheijen
Date Generated : 24/06/2009 9:55 a.m.
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace TestBusinessHost
{
	public abstract class ServiceHostBase
	{
		public ServiceHostBase()
		{
		  //  _Log = LogManager.GetLogger(typeof(ServiceHostBase));
		}

		//ILog _Log = null;
		ServiceHost _serviceHost = null;

		protected abstract Type ServiceType
		{
			get;
		}

		protected abstract string ServiceName
		{
			get;
		}

		protected string FullServiceName
		{
			get { return ServiceName + " (" + ServiceType.Name + ")"; }
		}

		public void Start()
		{
			//_Log.Info("Starting Service " + FullServiceName);
			try
			{
				_serviceHost = new ServiceHost(ServiceType);
				_serviceHost.Open();

			  //  _Log.Info(FullServiceName + " Started Without Error");
			}
			catch (Exception ex)
			{
				//_Log.Error(FullServiceName + " Error During Start. The Exception Has Been Caught And Other Services Should Continue To Run.", ex);
			}
		}
	}
}


