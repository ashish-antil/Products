using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		public GetItemResponse<ActivityLogEntry> GetActivityLogEntry(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ActivityLogEntry>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ActivityLogEntry>>(ex);
			}
		}

		public GetUpdateCountResponse GetActivityLogEntryUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<ActivityLogEntry>("ActivityLog", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ActivityLogEntry> GetActivityLogEntryListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ActivityLogEntry>("ActivityLog", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ActivityLogEntry>>(ex);
			}
		}

		public GetListResponse<ActivityLogEntry> GetActivityLogEntryList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<ActivityLogEntry>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ActivityLogEntry>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveActivityLogEntry(SaveRequest<ActivityLogEntry> request)
		{
			try
			{
				ActivityLogEntry entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.LogDateTime
						,entity.Operation
						,entity.Description
						,entity.AdditionalDescription

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<ActivityLogEntry>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveActivityLogEntryList(SaveListRequest<ActivityLogEntry> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (ActivityLogEntry entity in request.List)
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

						,entity.LogDateTime
						,entity.Operation
						,entity.Description
						,entity.AdditionalDescription

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<ActivityLogEntry>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteActivityLogEntry(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ActivityLogEntry>("ActivityLog", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}