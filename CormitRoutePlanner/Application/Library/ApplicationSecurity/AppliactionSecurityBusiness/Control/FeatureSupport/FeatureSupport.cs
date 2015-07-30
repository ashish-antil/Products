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
		public GetItemResponse<FeatureSupport> GetFeatureSupport(IDRequest request)
		{
			try
			{
				var response = new GetItemResponse<FeatureSupport>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetFeatureSupport(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<FeatureSupport>>(ex);
			}
		}

		public GetUpdateCountResponse GetFeatureSupportUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = new GetUpdateCountResponse();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetFeatureSupportUpdateCount(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<FeatureSupport> GetFeatureSupportListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				var response = new GetListResponse<FeatureSupport>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;				
					response = service.GetFeatureSupportListByTimeStamp (request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<FeatureSupport>>(ex);
			}
		}

		public GetListResponse<FeatureSupport> GetFeatureSupportList(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<FeatureSupport>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetFeatureSupportList(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<FeatureSupport>>(ex);
			}
		}

		public BusinessMessageResponse SaveFeatureSupport(SaveRequest<FeatureSupport> request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveFeatureSupport(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveFeatureSupportList(SaveListRequest<FeatureSupport> request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveFeatureSupportList(request); 
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteFeatureSupport(IDRequest request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.DeleteFeatureSupport(request);
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