using System;
using System.Collections.Generic;
using System.Data;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaConfigurationBusiness
{
	partial class ImardaConfiguration
	{
		public GetItemResponse<Shortcut> GetShortcut(IDRequest request)
		{
			try
			{
				return GenericGetEntity<Shortcut>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Shortcut>>(ex);
			}
		}

		public GetUpdateCountResponse GetShortcutUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<Shortcut>("Shortcut", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<Shortcut> GetShortcutListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<Shortcut>("Shortcut", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Shortcut>>(ex);
			}
		}

		public GetListResponse<Shortcut> GetShortcutList(IDRequest request)
		{
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Shortcut>());
				using (IDataReader dr = db.ExecuteDataReader("SPGetShortCutList", false, request.CompanyID, request.ID))
				{
					var response = new GetListResponse<Shortcut> { List = new List<Shortcut>() };
					while (dr.Read())
					{
						response.List.Add(GetFromData<Shortcut>(dr));
					}
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Shortcut>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveShortcut(SaveRequest<Shortcut> request)
		{
			try
			{
				Shortcut entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.ItemType
						,entity.ItemID
						,entity.Name
						,entity.OwnerID
						,entity.Sequence

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<Shortcut>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveShortcutList(SaveListRequest<Shortcut> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (Shortcut entity in request.List)
				{
					var properties = new object[]
					{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted
						,entity.ItemType
						,entity.ItemID
						,entity.Name
						,entity.OwnerID
						,entity.Sequence

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<Shortcut>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse UpdateShortcutList(SaveListRequest<Shortcut> request)
		{
			var companyID = request.CompanyID;
			var ownerID = new Guid(request["OwnerID"]);
			var itemType = request["ItemType"];
			if (itemType == null) itemType = "0";
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Shortcut>());
				db.ExecuteNonQuery("SPClearShortcutList", companyID, ownerID, itemType);
				return SaveShortcutList(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}		
		}

		public BusinessMessageResponse DeleteShortcut(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<Shortcut>("Shortcut", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}