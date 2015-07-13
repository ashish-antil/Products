using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		public GetListResponse<MessageItem> GetMessageListByUser(IDRequest request)
		{
			try
			{
				return ImardaDatabase.GetList<MessageItem>("SPGetMessageListByUser", new Guid(request.GetString("userid")), request.GetString("ViewDate"));
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
				bool deleted = false;
				if (!string.IsNullOrEmpty(request.GetString("deleted")))
					deleted = true;
				object[] properties = new object[]{			
                    new Guid(request.GetString("userid")), 
                    new Guid(request.GetString("messageid")),
                    deleted
                };
				return GenericSaveEntity<MessageItem>("MessageViewedByUser", properties);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}