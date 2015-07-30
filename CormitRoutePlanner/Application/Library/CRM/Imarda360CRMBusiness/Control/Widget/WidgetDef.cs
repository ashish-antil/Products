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
		//public List<WidgetDef> GetWidgets(Guid companyID, Guid userID)
		public GetListResponse<WidgetDef> GetWidgetDefList(IDRequest request)
		{
			try
			{
				GetListResponse<WidgetDef> response = null;
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetWidgetDefList(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<WidgetDef>>(ex);
			}
		}

		public BusinessMessageResponse SaveWidgetDefList(SaveListRequest<WidgetDef> request) //List<WidgetDef> widgets)
		{
			var response = new BusinessMessageResponse();
			try
			{
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveWidgetDefList(request);
				});
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
	}
}
