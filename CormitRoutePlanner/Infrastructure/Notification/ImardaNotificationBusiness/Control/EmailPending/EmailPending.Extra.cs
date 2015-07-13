using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaNotificationBusiness
{
	partial class ImardaNotification
	{
		public GetListResponse<EmailPending> GetEmailPendingListByNotificationID(IDRequest request)
		{
			try
			{
				return GenericGetRelated<EmailPending>("NotificationID", request.ID, false);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<EmailPending>>(ex);
			}
		}
	}
}
