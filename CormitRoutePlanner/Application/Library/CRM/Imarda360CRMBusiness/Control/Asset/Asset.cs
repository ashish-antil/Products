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
		public GetItemResponse<Asset> GetAsset(IDRequest request)
		{
			try
			{
				var response = new GetItemResponse<Asset>();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetAsset(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Asset>>(ex);
			}
		}

		public GetUpdateCountResponse GetAssetUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = new GetUpdateCountResponse();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetAssetUpdateCount(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<Asset> GetAssetListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				var response = new GetListResponse<Asset>();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;				
					response = service.GetAssetListByTimeStamp (request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Asset>>(ex);
			}
		}

		public GetListResponse<Asset> GetAssetList(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<Asset>();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetAssetList(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Asset>>(ex);
			}
		}

		public BusinessMessageResponse SaveAsset(SaveRequest<Asset> request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveAsset(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveAssetList(SaveListRequest<Asset> request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveAssetList(request); 
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteAsset(IDRequest request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.DeleteAsset(request);
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