using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using System.Data;

namespace ImardaNotificationBusiness
{
	partial class ImardaNotification
	{
		public GetListResponse<SMSFailed> GetSMSFailedListByNotificationID(IDRequest request)
		{
			try
			{
				return GenericGetRelated<SMSFailed>("NotificationID", request.ID, false);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SMSFailed>>(ex);
			}
		}
		public GetListResponse<SMSFailed> GetSMSFailedDueList(IDRequest request)
		{
			var result = new GetListResponse<SMSFailed>();

			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SMSFailed>());

				object[] args = new object[] { DateTime.UtcNow };
				using (IDataReader dr = db.ExecuteDataReader("SPGetSMSFailedDueList", args))
				{
					while (dr.Read())
					{
						result.List.Add(GetFromData<SMSFailed>(dr));
					}

					return result;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SMSFailed>>(ex);
			}
		}
	}
}
