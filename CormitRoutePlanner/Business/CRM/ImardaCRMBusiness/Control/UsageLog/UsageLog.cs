using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		#region Core Methods
		public GetItemResponse<UsageLog> GetUsageLog(IDRequest request)
		{
			try
			{
				return GenericGetEntity<UsageLog>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<UsageLog>>(ex);
			}
		}

		public GetUpdateCountResponse GetUsageLogUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<UsageLog>("UsageLog", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<UsageLog> GetUsageLogListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<UsageLog>("UsageLog", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<UsageLog>>(ex);
			}
		}

		public GetListResponse<UsageLog> GetUsageLogList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<UsageLog>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<UsageLog>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveUsageLog(SaveRequest<UsageLog> request)
		{
			try
			{
				UsageLog entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted
						,entity.ApiID
						,entity.ApiVersion
						,entity.Method
						,entity.ExecutionTime

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<UsageLog>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveUsageLogList(SaveListRequest<UsageLog> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (UsageLog entity in request.List)
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
						,entity.ApiID
						,entity.ApiVersion
						,entity.Method
						,entity.ExecutionTime

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<UsageLog>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteUsageLog(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<UsageLog>("UsageLog", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion

		#region Api Summary
		public GetListResponse<UsageLog> GetUsageLogSummary(IDRequest request)
		{
			try
			{
				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				var companyID = request.CompanyID;
				var apiID = request.Get("ApiID", Guid.Empty);
				return ImardaDatabase.GetList<UsageLog>("SPGetUsageLogListSummary", includeInactive, companyID, apiID);

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<UsageLog>>(ex);
			}
		}

		public GetListResponse<UsageLog> GetUsageLogSummaryByDateRange(IDRequest request)
		{
			try
			{
				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				var startDate = request.Get("StartDate", DateTime.UtcNow.Date.AddMonths(-3));
				var endDate = request.Get("EndDate", DateTime.UtcNow.Date);
				var companyID = request.CompanyID;
				var apiID = request.Get("ApiID", Guid.Empty);
				return ImardaDatabase.GetList<UsageLog>("SPGetUsageLogListSummary", includeInactive, startDate, endDate, companyID, apiID);

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<UsageLog>>(ex);
			}
		}
		#endregion

		#region Method Summary
		public GetListResponse<UsageLog> GetApiMethodList(IDRequest request)
		{
			try
			{
				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				//request.ID contains ApiScopeID
				return ImardaDatabase.GetList<UsageLog>("SPGetApiMethodList", includeInactive, request.ID);

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<UsageLog>>(ex);
			}
		}

		public GetListResponse<UsageLog> GetApiMethodBreakDownList(IDRequest request)
		{
			try
			{
				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				//request.ID contains ApiMethodID
				return ImardaDatabase.GetList<UsageLog>("SPGetApiMethodBreakDownList", includeInactive, request.ID);

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<UsageLog>>(ex);
			}
		}
		#endregion
	}
}