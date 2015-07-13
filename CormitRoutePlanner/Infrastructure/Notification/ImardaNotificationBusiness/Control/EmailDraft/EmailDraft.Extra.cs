using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaNotificationBusiness
{
	partial class ImardaNotification
	{
		public GetListResponse<EmailDraft> GetEmailDraftListByNotificationID(IDRequest request)
		{
			try
			{
				return GenericGetRelated<EmailDraft>("NotificationID", request.ID, false);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<EmailDraft>>(ex);
			}
		}
	}
}
