using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaNotificationBusiness
{
	partial class ImardaNotification
	{
		public GetListResponse<SMSSent> GetSMSSentListByNotificationID(IDRequest request)
		{
			try
			{
				return GenericGetRelated<SMSSent>("NotificationID", request.ID, false);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SMSSent>>(ex);
			}
		}
	}
}
