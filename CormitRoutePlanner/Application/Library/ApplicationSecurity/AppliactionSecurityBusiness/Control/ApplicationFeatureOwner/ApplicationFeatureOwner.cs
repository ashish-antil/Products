using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using ImardaSecurityBusiness;
using System.ServiceModel;
using Cormit.Application.RouteApplication.Security;

namespace Cormit.Application.RouteApplication.Security
{
	partial class ImardaSecurity
	{
		public GetItemResponse<ApplicationFeatureOwner> GetApplicationFeatureOwner(IDRequest request)
		{
			try
			{
				var response = new GetItemResponse<ApplicationFeatureOwner>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetApplicationFeatureOwner(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ApplicationFeatureOwner>>(ex);
			}
		}

		public GetUpdateCountResponse GetApplicationFeatureOwnerUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = new GetUpdateCountResponse();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetApplicationFeatureOwnerUpdateCount(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ApplicationFeatureOwner> GetApplicationFeatureOwnerListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				var response = new GetListResponse<ApplicationFeatureOwner>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;				
					response = service.GetApplicationFeatureOwnerListByTimeStamp (request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationFeatureOwner>>(ex);
			}
		}

		public GetListResponse<ApplicationFeatureOwner> GetApplicationFeatureOwnerList(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<ApplicationFeatureOwner>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetApplicationFeatureOwnerList(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationFeatureOwner>>(ex);
			}
		}

		public BusinessMessageResponse SaveApplicationFeatureOwner(SaveRequest<ApplicationFeatureOwner> request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveApplicationFeatureOwner(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveApplicationFeatureOwnerList(SaveListRequest<ApplicationFeatureOwner> request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveApplicationFeatureOwnerList(request); 
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteApplicationFeatureOwner(IDRequest request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.DeleteApplicationFeatureOwner(request);
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