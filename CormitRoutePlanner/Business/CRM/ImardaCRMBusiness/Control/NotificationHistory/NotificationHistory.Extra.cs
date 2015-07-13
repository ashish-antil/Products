using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using System.Data;


namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		#region GetNotificationHistoryByNotificationPlanID
		public GetListResponse<NotificationHistory> GetNotificationHistoryListByNotificationPlanID(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<NotificationHistory>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<NotificationHistory>());
				using (IDataReader dr = db.ExecuteDataReader("SPGetNotificationHistoryListByNotificationPlanID", request.ID))
				{
					response.List = new List<NotificationHistory>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<NotificationHistory>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NotificationHistory>>(ex);
			}
		}
		#endregion

	}
}
