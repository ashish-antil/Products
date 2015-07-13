using System;
using System.Collections.Generic;
using System.Data;
using FernBusinessBase;
using FernBusinessBase.Errors;
using Imarda.Lib;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		#region GetSecurityEntityExtent

		#region SQL Build Helpers
		private string GetSecurityEntityInnerSelectSQL(int scope, Guid companyID)
		{
			var innerSelect = string.Format(
				"SELECT s.* " +
				"FROM [SecurityEntity] s " +
				"WHERE " +
				"( ({0} = 0 AND s.CompanyID = '{1}') " +
					"OR " +
					"({0} = 1 AND CHARINDEX('{1}', s.Path) = LEN(s.Path) - 72) " +
					"OR " +
					"({0} = 2 AND (s.Path LIKE '%{1}%' OR s.Path = '' OR s.Path IS NULL)) " +
				") " +
				"AND s.CompanyID <> '11111111-1111-1111-1111-111111111111' " +
				"AND s.Deleted = 0 ", scope, companyID.ToString());

			return innerSelect;
		}

		#endregion

		public GetListResponse<SecurityEntity> GetSecurityEntityExtent(GetFilteredExtentRequest request)
		{
			var scope = 0;
			if (request.Scope.HasValue)
				scope = request.Scope.Value;

			//for users from fleet
			var fleetID = request.OwnerID.HasValue ? request.OwnerID.Value : Guid.Empty;

			var innerSelect = GetSecurityEntityInnerSelectSQL(scope, request.CompanyID);

			try
			{
				var response = GenericGetExtentWithCustomSelect<SecurityEntity>(
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
				return ErrorHandler.Handle<GetListResponse<SecurityEntity>>(ex);
			}
		}
		#endregion

		public GetItemResponse<SecurityEntity> GetSecurityEntityByCrmID(IDRequest request)
		{
			try
			{
				var response = base.GenericGetRelated<SecurityEntity>("CRMID", request.ID, false);
				if (response.Status && response.List.Count == 1)
				{
					return new GetItemResponse<SecurityEntity>
					{
						Status = true,
						Item = response.List[0],
					};
				}
				else return new GetItemResponse<SecurityEntity> { Status = false };
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<SecurityEntity>>(ex);
			}
		}

		public GetItemResponse<SecurityEntity> GetSecurityEntityByLoginUserName(IDRequest request)
		{
			try
			{
				var response = new GetItemResponse<SecurityEntity>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityEntity>());
				string username = request.Get("username", string.Empty);

				using (IDataReader dr = db.ExecuteDataReader("SPGetSecurityEntityByLoginUserName", username,0))
				{
					if (dr.Read())
					{
						response.Item = GetFromData<SecurityEntity>(dr, false);
					}
					else
					{
						//failed to find login username
						response.Status = false;
						response.StatusMessage = "User name [" + username + "] was not found.";
					}
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<SecurityEntity>>(ex);
			}
		}

		public GetListResponse<SecurityEntity> GetSecurityEntityListByCompanyID(IDRequest request)
		{
			try
			{
				return GenericGetRelated<SecurityEntity>("CompanyID", request.ID, false);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityEntity>>(ex);
			}
		}

		#region Save User securityEntity

		public BusinessMessageResponse SaveUserSecurityEntity(SaveRequest<SecurityEntity> request)
		{

			var response = new BusinessMessageResponse();

			try
			{
				if (request.Item != null)
				{
					SecurityEntity se = request.Item;
					bool updatePassword = request.HasFlagSet("UpdatePassword");

					var resp1 = GenericGetEntity<SecurityEntity>(new IDRequest(se.ID));
					bool exists = ServiceMessageHelper.IsSuccess(resp1);
					if (exists)
					{
						se.Salt = resp1.Item.Salt;
						if (!updatePassword) se.LoginPassword = resp1.Item.LoginPassword;
					}

					if (updatePassword)
					{
						if (se.Salt == Guid.Empty) se.Salt = AuthenticationHelper.GenerateSalt();
						se.LoginPassword = Convert.ToBase64String(AuthenticationHelper.ComputePasswordHash(se.Salt, se.LoginPassword));
					}
					object date = se.LastLogonDate;
					if (se.LastLogonDate == default(DateTime)) date = null;

					//# gs-353 check for se.LoginEnabled
					bool emptyUser = se.LoginEnabled && string.IsNullOrEmpty(se.LoginUsername);
					bool emptyPwd = se.LoginEnabled && string.IsNullOrEmpty(se.LoginPassword);
					bool emptySalt = se.LoginEnabled && se.Salt == Guid.Empty;
					//. gs-353

					if (emptyUser || emptyPwd || emptySalt)
					{
						var resp0 = new BusinessMessageResponse { Status = false, StatusMessage = "Empty user, password or salt" };
						string err = "INse|ImardaSecurity.GetUserSecurityEntity:SecurityEntity|{0}{1}{2}";
						resp0.ErrorCode = string.Format(err, (emptyUser ? " Usr" : ""), (emptyPwd ? " Pwd" : ""),
																						(emptySalt ? " Salt" : ""));
						return resp0;
					}

					response = GenericSaveEntity<SecurityEntity>("SecurityEntity",
						se.ID,
						se.EntityName,
						se.EntityType,
						se.LoginEnabled,
						se.LoginUsername,
						se.LoginPassword,
						se.IsTemplate,		//& gs-353
						se.CompanyID,		//& gs-351
						se.Path,
						se.UserID,
						se.CRMId,
						se.Description,
						se.BranchID,
						date,
						se.Active,
						se.Deleted,
						string.IsNullOrEmpty(se.TimeZone) || se.TimeZone.StartsWith("(") ? "UTC" : se.TimeZone,
						se.Locale ?? "en",
						se.PreferredUnitSystemID,
						se.EnableTimeZoneSelect,
						se.IsAdmin,
						se.Salt,
						updatePassword
						);

				}
				response.Status = true;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}

			return response;
		}
		#endregion

		public BusinessMessageResponse SetDeletedSecurityEntityByCRMID(IDRequest request)
		{
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityObject>());
				var args = new object[] { request.ID, request.HasFlagSet("Deleted") };
				int n = db.ExecuteNonQuery("SPSetDeletedSecurityEntityByCRMID", args);
				return new BusinessMessageResponse { StatusMessage = n.ToString() };
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		#region SecurityObjects for SecurityEntity
		#region GetAssigned SecurityObjects

		public GetListResponse<SecurityObject> GetAssignedSecurityObjects(IDRequest request)
		{
			var response = new GetListResponse<SecurityObject>();
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityObject>());
				object[] args = new object[] { request.ID };
				using (IDataReader dr = db.ExecuteDataReader("SPGetSecurityObjectAssignedForEntity", args))
				{
					response.List = new List<SecurityObject>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<SecurityObject>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
			}
		}
		#endregion
		#region GetUnAssigned SecurityObjects

		public GetListResponse<SecurityObject> GetUnassignedSecurityObjects(IDRequest request)
		{
			var response = new GetListResponse<SecurityObject>();
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityObject>());
				object[] args = new object[] { request.ID };
				using (IDataReader dr = db.ExecuteDataReader("SPGetSecurityObjectNotAssignedForEntity", args))
				{
					response.List = new List<SecurityObject>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<SecurityObject>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
			}
		}
		#endregion
		#region GetKnown SecurityObjects

		public GetListResponse<SecurityObject> GetKnownSecurityObjects(IDRequest request)
		{
			var response = new GetListResponse<SecurityObject>();
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityObject>());
				object[] args = new object[] { request.ID };
				using (IDataReader dr = db.ExecuteDataReader("SPGetKnownSecurityObjectList", args))
				{
					response.List = new List<SecurityObject>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<SecurityObject>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
			}
		}
		#endregion
		#endregion

		public SimpleResponse<bool> CheckIfUniqueForUser(IDRequest request)
		{
			var response = new SimpleResponse<bool>();
			try
			{
				var fieldName = request["FieldName"];
				var value = request["Value"];
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityEntity>());
				string query = string.Format("SELECT COUNT(*) FROM SecurityEntity WHERE {0} = '{1}' AND ID <> '{2}'", fieldName, value, request.ID);
				using (IDataReader dr = db.ExecuteDataReader(CommandType.Text, query))
				{
					if (dr.Read() && Convert.ToInt32(dr[0]) > 0)
						response = new SimpleResponse<bool>(false);
					else
						response = new SimpleResponse<bool>(true);
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SimpleResponse<bool>>(ex);
			}
		}

		#region SecurityObjects by ApplicationID (for IAC and other new operations)
		public GetListResponse<SecurityObject> GetSecurityObjectsByApplicationID(IDRequest request)
		{
			var response = new GetListResponse<SecurityObject>();
			try
			{
				Guid applicationID;
				request.Get<Guid>("appid", out applicationID);

				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityObject>());
				object[] args = new object[] { applicationID };
				using (IDataReader dr = db.ExecuteDataReader("SPGetSecurityObjectsByApplicationID", args))
				{
					response.List = new List<SecurityObject>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<SecurityObject>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
			}
		}

		public GetListResponse<SecurityObject> GetSecurityObjectsAssignedByApplicationID(IDRequest request)
		{
			var response = new GetListResponse<SecurityObject>();
			try
			{
				Guid applicationID;
				request.Get<Guid>("appid", out applicationID);

				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityObject>());
				object[] args = new object[] { request.ID, applicationID };
				using (IDataReader dr = db.ExecuteDataReader("SPGetSecurityObjectsAssignedByApplicationID", args))
				{
					response.List = new List<SecurityObject>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<SecurityObject>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
			}
		}
		#endregion
	}

}
