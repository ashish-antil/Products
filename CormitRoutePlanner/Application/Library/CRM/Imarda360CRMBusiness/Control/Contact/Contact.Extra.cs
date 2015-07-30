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
		public GetListResponse<Contact> SearchContactList(IDRequest request)
		{
			try
			{
				GetListResponse<Contact> appResponse = null;
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					appResponse = service.SearchContactList(request);
					ErrorHandler.Check(appResponse);
					 
				});
				return appResponse;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
			}
		}

		public GetListResponse<Contact> GetContactListByParentId(IDRequest request)
		{
			try
			{
				GetListResponse<Contact> appResponse = null;
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					appResponse = service.GetContactListByParentId(request);
					ErrorHandler.Check(appResponse);

				});
				return appResponse;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
			}
		} 
}
}