using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		public GetItemResponse<ScopeAccess> GetScopeAccess(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ScopeAccess>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ScopeAccess>>(ex);
			}
		}

		public GetUpdateCountResponse GetScopeAccessUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<ScopeAccess>("ScopeAccess", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ScopeAccess> GetScopeAccessListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ScopeAccess>("ScopeAccess", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ScopeAccess>>(ex);
			}
		}

		public GetListResponse<ScopeAccess> GetScopeAccessList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<ScopeAccess>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ScopeAccess>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveScopeAccess(SaveRequest<ScopeAccess> request)
		{
			try
			{
				ScopeAccess entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted
						,entity.ScopeID
						,entity.AssignedToID
						,entity.AssignedToType
						,entity.AccessPermission
						,entity.ConsentType

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<ScopeAccess>(entity.CompanyID, entity.Attributes, properties);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveScopeAccessList(SaveListRequest<ScopeAccess> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (ScopeAccess entity in request.List)
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
						,entity.ScopeID
						,entity.AssignedToID
						,entity.AssignedToType
						,entity.AccessPermission
						,entity.ConsentType

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<ScopeAccess>(entity.CompanyID, entity.Attributes, properties);
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteScopeAccess(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ScopeAccess>("ScopeAccess", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse UpdateScopeAccessList(SaveListRequest<ScopeAccess> request)
		{
			var companyID = request.CompanyID;
			var scopeType = request["ScopeType"];
			var assignedToID = new Guid(request["AssignedToID"]);
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ScopeAccess>());
				db.ExecuteNonQuery("SPClearAssignedScopeList", assignedToID, scopeType);
				return SaveScopeAccessList(request); ;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

	}
}