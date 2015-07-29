using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using ImardaCRMBusiness;
using Imarda.Lib;

namespace Imarda360Application.CRM
{
	partial class ImardaCRM
	{
		public GetListResponse<MessageItem> GetMessageListByUser(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<MessageItem>();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.GetMessageListByUser(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<MessageItem>>(ex);
			}
		}
		public BusinessMessageResponse SaveMessageViewedByUser(IDRequest request)
		{
			try
			{
				var response = new BusinessMessageResponse();
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					response = service.SaveMessageViewedByUser(request);
				});
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

        public BusinessMessageResponse SaveMessageItem(SaveRequest<MessageItem> request)
        {
            try
            {
                BusinessMessageResponse busresp = null;
                MessageItem msgitem = request.Item;
                
                


                var service = ImardaProxyManager.Instance.IImardaCRMProxy;
                ChannelInvoker.Invoke(delegate(out IClientChannel channel)
                {
                    channel = service as IClientChannel;
                    busresp = service.SaveMessageItem(request);

                });
                string[] usersId = null;
                usersId = !string.IsNullOrEmpty(request.Item.Users) ? request.Item.Users.Split(',') : usersId;
                MessageOwner ow;
                List<MessageOwner> listMessageOwner = new List<MessageOwner>();

                foreach (string userid in usersId)
                {
                    ow = new MessageOwner();
                    MessageOwner.Copy(msgitem,ow);
                    ow.CRMID = new Guid(userid);
                    ow.MessageID = request.Item.ID;
                    ow.ID = SequentialGuid.NewDbGuid();
                    listMessageOwner.Add(ow);
                }
                SaveListRequest<MessageOwner> ownerRequest = new SaveListRequest<MessageOwner>(listMessageOwner);

                service = ImardaProxyManager.Instance.IImardaCRMProxy;
                ChannelInvoker.Invoke(delegate(out IClientChannel channel)
                {
                    channel = service as IClientChannel;
                    busresp = service.SaveMessageOwnerList(ownerRequest);

                });


                return busresp;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<MessageItem>>(ex);
            }
        }

	}
}