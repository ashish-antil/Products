using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaNotificationBusiness
{
	partial class ImardaNotification
	{
		public GetListResponse<EmailSent> GetEmailSentListByNotificationID(IDRequest request)
		{
			try
			{
				return GenericGetRelated<EmailSent>("NotificationID", request.ID, false);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<EmailSent>>(ex);
			}
		}
	}
}
