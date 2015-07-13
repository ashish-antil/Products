using System;
using FernBusinessBase;
using FernBusinessBase.Errors;
using System.Data;
using System.Collections.Generic;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		public GetItemResponse<Scope> GetScope(IDRequest request)
		{
			try
			{
				return GenericGetEntity<Scope>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Scope>>(ex);
			}
		}

		public GetUpdateCountResponse GetScopeUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<Scope>("Scope", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<Scope> GetScopeListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<Scope>("Scope", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Scope>>(ex);
			}
		}

		public GetListResponse<Scope> GetScopeList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<Scope>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Scope>>(ex);
			}
		}

		public GetListResponse<Scope> GetApiScopeList(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<Scope>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Scope>());

				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				using (IDataReader dr = db.ExecuteDataReader("SPGetApiScopeList", includeInactive, request.CompanyID))
				{
					response.List = new List<Scope>();
					while (dr.Read()) response.List.Add(GetFromData<Scope>(dr));
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Scope>>(ex);
			}
		}

		#region GetScopeExtent
		#region SQL Build Helpers
		private string GetScopeInnerSelectSQL(int scope, Guid? clientID)
		{
			string innerSelect;
			if (clientID.HasValue)
			{
				innerSelect = string.Format(
				"SELECT s.*, cs.ConsentType " +
				"FROM Scope s " +
				"INNER JOIN ClientScope cs on cs.ScopeID = s.ID " +
				"WHERE cs.ClientID = '{0}' " +
				" AND s.Active = 1 AND s.Deleted=0 ", clientID.ToString());
			}
			else
			{
				innerSelect = string.Format(
				"SELECT s.* " +
				"FROM Scope s " +
				"WHERE s.Active = 1 AND s.Deleted=0 ", scope, clientID.ToString());
			}

			return innerSelect;
		}

		#endregion

		public GetListResponse<Scope> GetScopeExtent(GetFilteredExtentRequest request)
		{
			var scope = 0;
			if (request.Scope.HasValue)
				scope = request.Scope.Value;
			
			var innerSelect = GetScopeInnerSelectSQL(scope, request.OwnerID);

			try
			{
				var response = GenericGetExtentWithCustomSelect<Scope>(
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
				return ErrorHandler.Handle<GetListResponse<Scope>>(ex);
			}

		}
		#endregion
		
		public BusinessMessageResponse SaveScope(SaveRequest<Scope> request)
		{
			try
			{
				Scope entity = request.Item; 			   
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
						,entity.DisplayName
						,entity.Description
						,entity.ScopeType

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<Scope>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveScopeList(SaveListRequest<Scope> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (Scope entity in request.List)
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
						,entity.DisplayName
						,entity.Description
						,entity.ScopeType

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<Scope>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteScope(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<Scope>("Scope", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public GetListResponse<Scope> GetAssignedScopeList(IDRequest request)
		{
			try
			{
				var scopeType = request["ScopeType"];
				return ImardaDatabase.GetList<Scope>("SPGetAssignedScopeList", request.CompanyID, request.ID, scopeType);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Scope>>(ex);
			}
		}
	}
}