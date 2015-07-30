using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using ImardaSecurityBusiness;
using System.ServiceModel;

namespace Cormit.Application.RouteApplication.Security
{
	partial class ImardaSecurity
	{
		public GetItemResponse<ApplicationPlan> GetApplicationPlan(IDRequest request)
		{
			try
			{
				var response = new GetItemResponse<ApplicationPlan>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetApplicationPlan(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ApplicationPlan>>(ex);
			}
		}

		public GetUpdateCountResponse GetApplicationPlanUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = new GetUpdateCountResponse();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetApplicationPlanUpdateCount(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ApplicationPlan> GetApplicationPlanListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				var response = new GetListResponse<ApplicationPlan>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;				
					response = service.GetApplicationPlanListByTimeStamp (request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationPlan>>(ex);
			}
		}

		public GetListResponse<ApplicationPlan> GetApplicationPlanList(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<ApplicationPlan>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetApplicationPlanList(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationPlan>>(ex);
			}
		}

		public BusinessMessageResponse SaveApplicationPlan(SaveRequest<ApplicationPlan> request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveApplicationPlan(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveApplicationPlanList(SaveListRequest<ApplicationPlan> request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveApplicationPlanList(request); 
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteApplicationPlan(IDRequest request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.DeleteApplicationPlan(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}