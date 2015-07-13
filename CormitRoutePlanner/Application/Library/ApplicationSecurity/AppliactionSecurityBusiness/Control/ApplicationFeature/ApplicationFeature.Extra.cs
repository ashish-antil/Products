using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using ImardaSecurityBusiness;
using System.ServiceModel;
using Imarda360Base;
using Imarda.Lib;

namespace Imarda360Application.Security
{
	partial class ImardaSecurity
	{
		public GetListResponse<ApplicationFeature> GetApplicationFeatureListByCategoryID(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<ApplicationFeature>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetApplicationFeatureListByCategoryID(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationFeature>>(ex);
			}
		}
		public GetListResponse<ApplicationFeature> GetApplicationFeatureListByOwnerID(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<ApplicationFeature>();
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetApplicationFeatureListByOwnerID(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationFeature>>(ex);
			}
		}
	}
}