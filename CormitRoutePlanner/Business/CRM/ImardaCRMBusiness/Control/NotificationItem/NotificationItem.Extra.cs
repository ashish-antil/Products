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
		#region GetNotificationItemListByNotificationPlanID
		public GetListResponse<NotificationItem> GetNotificationItemListByNotificationPlanID(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<NotificationItem>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<NotificationItem>());
				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				object[] args = new object[] { includeInactive, request.CompanyID, request.ID };
				using (IDataReader dr = db.ExecuteDataReader("SPGetNotificationItemListByNotificationPlanID", args))
				{
					response.List = new List<NotificationItem>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<NotificationItem>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NotificationItem>>(ex);
			}
		}
		#endregion

		//SRR IM-4126
		#region GetNotificationItemListByPlanID
		public GetListResponse<NotificationItem> GetNotificationItemListByPlanID(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<NotificationItem>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<NotificationItem>());
				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				object[] args = new object[] { includeInactive, request.CompanyID, request.ID };
				using (IDataReader dr = db.ExecuteDataReader("SPGetNotificationItemListByPlanID", args))
				{
					response.List = new List<NotificationItem>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<NotificationItem>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NotificationItem>>(ex);
			}
		}
		#endregion
		//SRR IM-4126
		#region GetNotificationTemplateList
		public GetListResponse<NotificationItem> GetNotificationTemplateList(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<NotificationItem>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<NotificationItem>());
				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				object[] args = new object[] { includeInactive, request.ID };
				using (IDataReader dr = db.ExecuteDataReader("SPGetNotificationTemplateList", args))
				{
					response.List = new List<NotificationItem>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<NotificationItem>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NotificationItem>>(ex);
			}
		}
		#endregion
		#region DeleteNotificationPlanItemsByPlanID
		public BusinessMessageResponse DeleteNotificationPlanItemsByPlanID(IDRequest request)
		{
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<NotificationItem>());
				db.ExecuteNonQuery("SPDeleteNotificationPlanItemsByPlanID", request.CompanyID, request.ID);
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
	}
}
