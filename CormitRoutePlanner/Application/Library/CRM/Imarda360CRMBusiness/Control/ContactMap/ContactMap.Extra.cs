using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using ImardaCRMBusiness;
using System.ServiceModel;
using Imarda360Base;
using Imarda.Lib;

namespace Imarda360Application.CRM
{
	partial class ImardaCRM
	{
		public BusinessMessageResponse SaveContactMap(SaveRequest<ContactMap> request)
		{
			try
			{
				BusinessMessageResponse response = null;
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveContactMap(request);
					ErrorHandler.Check(response);
					 
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<BusinessMessageResponse>(ex);
			}
		}
	public BusinessMessageResponse DeleteContactMapByContactNPerson(IDRequest request)
		{
			 
			try
			{
				BusinessMessageResponse response = null;
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.DeleteContactMapByContactNPerson(request);
					ErrorHandler.Check(response);
					 
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<BusinessMessageResponse>(ex);
			}
		}
}
}