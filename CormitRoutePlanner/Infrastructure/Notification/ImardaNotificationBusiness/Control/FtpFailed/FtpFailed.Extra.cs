//& IM-3927
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
		//public GetListResponse<FtpFailed> GetFtpFailedListByNotificationID(IDRequest request)
		//{
		//	try
		//	{
		//		return GenericGetRelated<FtpFailed>("NotificationID", request.ID, false);
		//	}
		//	catch (Exception ex)
		//	{
		//		return ErrorHandler.Handle<GetListResponse<FtpFailed>>(ex);
		//	}
		//}

		public GetListResponse<FtpFailed> GetFtpFailedDueList(IDRequest request)
		{
			var result = new GetListResponse<FtpFailed>();

			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<FtpFailed>());

				object[] args = new object[] { DateTime.UtcNow };
				using (IDataReader dr = db.ExecuteDataReader("SPGetFtpFailedDueList", args))
				{
					while (dr.Read())
					{
						result.List.Add(GetFromData<FtpFailed>(dr));
					}

					return result;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<FtpFailed>>(ex);
			}
		}
	}
}
