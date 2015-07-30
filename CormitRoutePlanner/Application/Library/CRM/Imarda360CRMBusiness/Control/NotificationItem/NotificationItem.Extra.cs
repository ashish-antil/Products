using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using ImardaCRMBusiness;
using System.ServiceModel;

namespace Imarda360Application.CRM
{
	partial class ImardaCRM
	{
		public GetListResponse<NotificationItem> GetNotificationItemListByNotificationPlanID(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<NotificationItem>();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetNotificationItemListByNotificationPlanID(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NotificationItem>>(ex);
			}
		}

		//SRR IM-4126
		public GetListResponse<NotificationItem> GetNotificationItemListByPlanID(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<NotificationItem>();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetNotificationItemListByPlanID(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NotificationItem>>(ex);
			}
		}
		//SRR IM-4126
		public GetListResponse<NotificationItem> GetNotificationTemplateList(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<NotificationItem>();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetNotificationTemplateList(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NotificationItem>>(ex);
			}
		}

	}
}