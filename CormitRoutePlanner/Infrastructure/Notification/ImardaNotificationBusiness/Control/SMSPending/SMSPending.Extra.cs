using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaNotificationBusiness
{
	partial class ImardaNotification
	{
		public GetListResponse<SMSPending> GetSMSPendingListByNotificationID(IDRequest request)
		{
			try
			{
				return GenericGetRelated<SMSPending>("NotificationID", request.ID, false);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SMSPending>>(ex);
			}
		}
	}
}
