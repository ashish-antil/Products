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
		public GetListResponse<EmailFailed> GetEmailFailedListByNotificationID(IDRequest request)
		{
			try
			{
				return GenericGetRelated<EmailFailed>("NotificationID", request.ID, false);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<EmailFailed>>(ex);
			}
		}

		public GetListResponse<EmailFailed> GetEmailFailedDueList(IDRequest request)
		{
			var result = new GetListResponse<EmailFailed>();

			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<EmailFailed>());

				object[] args = new object[] { DateTime.UtcNow };
				using (IDataReader dr = db.ExecuteDataReader("SPGetEmailFailedDueList", args))
				{
					while (dr.Read())
					{
						result.List.Add(GetFromData<EmailFailed>(dr));
					}

					return result;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<EmailFailed>>(ex);
			}
		}
	}
}
