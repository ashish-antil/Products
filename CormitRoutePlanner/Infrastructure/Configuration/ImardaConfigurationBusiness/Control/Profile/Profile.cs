using System;
using System.Data;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaConfigurationBusiness
{
	partial class ImardaConfiguration
	{
		public GetItemResponse<Profile> GetProfile(IDRequest request)
		{
			try
			{
				return GenericGetEntity<Profile>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Profile>>(ex);
			}
		}

		public GetUpdateCountResponse GetProfileUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<Profile>("Profile", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<Profile> GetProfileListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<Profile>("Profile", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Profile>>(ex);
			}
		}

		public GetListResponse<Profile> GetProfileList(IDRequest request)
		{
			try
			{
				var areaType = request["AreaType"];
				return ImardaDatabase.GetList<Profile>("SPGetProfileList", request.CompanyID, areaType);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Profile>>(ex);
			}
		}

		#region GetProfileExtent
		#region SQL Build Helpers
		private string GetProfileInnerSelectSQL(int scope, Guid companyID)
		{
			var innerSelect = string.Format(
				"SELECT p.*, ra.AllowCustomersToUse, " +
				"(SELECT CASE WHEN owner.ID IS NULL THEN 'System' ELSE owner.Name END) AS Owner " +
				"FROM Profile p " +
				"LEFT JOIN [Imarda360.CRM].[dbo].[Company] owner ON owner.ID = p.CompanyID " +
				"LEFT JOIN [Imarda360.CRM].[dbo].[Company] requester ON requester.ID =  '{1}' " +
				"LEFT JOIN [Imarda360.CRM].[dbo].[Company] parent ON parent.ID = requester.CompanyID " +
				"/*** Admission given by parent for use - for inclusion ***/ " +
				"LEFT JOIN ProfileAdmission oa ON oa.ProfileID = p.ID AND oa.CompanyID = parent.ID " +
				"/*** Admission given by requester for use - for result ***/ " +	
				"LEFT JOIN ProfileAdmission ra ON ra.ProfileID = p.ID AND ra.CompanyID = '{1}' " +
				"WHERE " +
				"  ( p.CompanyID = '{1}' " +
				"    OR  " +
				"    (p.Active = 1 AND oa.AllowCustomersToUse = 1) " +
				"    OR " +
				"    (p.CompanyID = '11111111-1111-1111-1111-111111111111' AND '{1}' = '78c46d66-b886-44d0-a3c2-3aa9b12c4d98') " +
				"  ) " +
				"AND p.Deleted=0 ", scope, companyID.ToString());
			return innerSelect;
		}

		#endregion

		public GetListResponse<Profile> GetProfileExtent(GetFilteredExtentRequest request)
		{
			var scope = 0;
			if (request.Scope.HasValue)
				scope = request.Scope.Value;
			//note! Areatype select is typically part of the request.Conditions, scope is currently not used
			var innerSelect = GetProfileInnerSelectSQL(scope, request.CompanyID);

			try
			{
				var response = GenericGetExtentWithCustomSelect<Profile>(
							request.CompanyID,
							request.CreatedAfter, request.CreatedBefore,
							request.ModifiedAfter, request.ModifiedBefore,
							request.Deleted, request.Active, request.Template, request.Path,
							request.Limit, request.Offset, request.SortColumns,
							null, null,
							innerSelect, request.Conditions, request.LogicalOperator);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Profile>>(ex);
			}

		}
		#endregion

		public BusinessMessageResponse SaveProfile(SaveRequest<Profile> request)
		{
			try
			{
				Profile entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.Name
						,entity.Description
						,BusinessBase.ReadyDateForStorage(entity.StartDate)
						,BusinessBase.ReadyDateForStorage(entity.ExpiryDate)
						,entity.AreaType
						,entity.ProfileType
						,entity.SettingsLinkID
						,entity.Settings

#if EntityProperty_NoDate
						,entity.`field`
#endif



#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<Profile>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveProfileList(SaveListRequest<Profile> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (Profile entity in request.List)
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
						,entity.Name
						,entity.Description
						,BusinessBase.ReadyDateForStorage(entity.StartDate)
						,BusinessBase.ReadyDateForStorage(entity.ExpiryDate)
						,entity.AreaType
						,entity.ProfileType
						,entity.SettingsLinkID
						,entity.Settings

#if EntityProperty_NoDate
						,entity.`field`
#endif



#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<Profile>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteProfile(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<Profile>("Profile", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public GetListResponse<Profile> GetAssignedProfileList(IDRequest request)
		{
			try
			{
				var areaType = request["AreaType"];
				return ImardaDatabase.GetList<Profile>("SPGetAssignedProfileList", request.CompanyID, request.ID, areaType);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Profile>>(ex);
			}
		}

		public GetUpdateCountResponse GetI360ProfileCount(IDRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Profile>());
				using (IDataReader dr = db.ExecuteDataReader("SPGetProfileCount", true, request.ID, 0))
				{
					if (dr.Read()) response.Count = Convert.ToInt32(dr[0]);
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
			return response;
		}

		public GetUpdateCountResponse GetIACProfileCount(IDRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Profile>());
				using (IDataReader dr = db.ExecuteDataReader("SPGetProfileCount", true, request.ID, 1))
				{
					if (dr.Read()) response.Count = Convert.ToInt32(dr[0]);
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
			return response;
		}

	}
}