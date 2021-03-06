/***********************************************************************
Auto Generated Code.

Generated by   : IMARDAINC\Qian.Chen
Date Generated : 12/03/2010 12:10 p.m.
Copyright (c)2009 CodeGenerator 1.2
***********************************************************************/
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
		public GetItemResponse<NotificationHistory> GetNotificationHistory(IDRequest request)
		{
			try
			{
				var response = new GetItemResponse<NotificationHistory>();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetNotificationHistory(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<NotificationHistory>>(ex);
			}
		}

		public GetUpdateCountResponse GetNotificationHistoryUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = new GetUpdateCountResponse();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetNotificationHistoryUpdateCount(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<NotificationHistory> GetNotificationHistoryListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				var response = new GetListResponse<NotificationHistory>();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;				
					response = service.GetNotificationHistoryListByTimeStamp (request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NotificationHistory>>(ex);
			}
		}

		public GetListResponse<NotificationHistory> GetNotificationHistoryList(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<NotificationHistory>();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetNotificationHistoryList(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NotificationHistory>>(ex);
			}
		}

		public BusinessMessageResponse SaveNotificationHistory(SaveRequest<NotificationHistory> request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveNotificationHistory(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveNotificationHistoryList(SaveListRequest<NotificationHistory> request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveNotificationHistoryList(request); 
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteNotificationHistory(IDRequest request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.DeleteNotificationHistory(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		public GetListResponse<NotificationHistory> GetNotificationHistoryListByNotificationPlanID(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<NotificationHistory>();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetNotificationHistoryListByNotificationPlanID(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NotificationHistory>>(ex);
			}
		}
	}
}


