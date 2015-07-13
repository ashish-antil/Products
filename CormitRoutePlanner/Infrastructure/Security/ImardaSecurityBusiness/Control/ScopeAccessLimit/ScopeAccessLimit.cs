using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		public GetItemResponse<ScopeAccessLimit> GetScopeAccessLimit(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ScopeAccessLimit>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ScopeAccessLimit>>(ex);
			}
		}

		public GetUpdateCountResponse GetScopeAccessLimitUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<ScopeAccessLimit>("ScopeAccessLimit", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ScopeAccessLimit> GetScopeAccessLimitListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ScopeAccessLimit>("ScopeAccessLimit", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ScopeAccessLimit>>(ex);
			}
		}

		public GetListResponse<ScopeAccessLimit> GetScopeAccessLimitList(IDRequest request)
		{
			try
			{
				bool includeAttributes = request.HasSome(RetrievalOptions.Attributes);
				return ImardaDatabase.GetList<ScopeAccessLimit>("SPGetScopeAccessLimitList", includeAttributes, request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ScopeAccessLimit>>(ex);
			}
		}


		public BusinessMessageResponse SaveScopeAccessLimit(SaveRequest<ScopeAccessLimit> request)
		{
			try
			{
				ScopeAccessLimit entity = request.Item;
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
						,entity.Limit
						,entity.PerTimeSpan

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<ScopeAccessLimit>(entity.CompanyID, entity.Attributes, properties);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveScopeAccessLimitList(SaveListRequest<ScopeAccessLimit> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (ScopeAccessLimit entity in request.List)
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
						,entity.Limit
						,entity.PerTimeSpan

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<ScopeAccessLimit>(entity.CompanyID, entity.Attributes, properties);
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteScopeAccessLimit(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ScopeAccessLimit>("ScopeAccessLimit", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}



	}
}