using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaNotificationBusiness
{
	partial class ImardaNotification
	{
		public GetListResponse<SMSDraft> GetSMSDraftListByNotificationID(IDRequest request)
		{
			try
			{
				return GenericGetRelated<SMSDraft>("NotificationID", request.ID, false);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SMSDraft>>(ex);
			}
		}
	}
}
