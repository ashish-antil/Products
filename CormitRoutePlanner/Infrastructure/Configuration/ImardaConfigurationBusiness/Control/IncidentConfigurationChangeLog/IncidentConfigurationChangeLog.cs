using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaConfigurationBusiness
{
	partial class ImardaConfiguration
	{
		public GetItemResponse<IncidentConfigurationChangeLog> GetIncidentConfigurationChangeLog(IDRequest request)
		{
			try
			{
				return GenericGetEntity<IncidentConfigurationChangeLog>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<IncidentConfigurationChangeLog>>(ex);
			}
		}

		public GetUpdateCountResponse GetIncidentConfigurationChangeLogUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<IncidentConfigurationChangeLog>("IncidentConfigurationChangeLog", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<IncidentConfigurationChangeLog> GetIncidentConfigurationChangeLogListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<IncidentConfigurationChangeLog>("IncidentConfigurationChangeLog", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<IncidentConfigurationChangeLog>>(ex);
			}
		}

		public GetListResponse<IncidentConfigurationChangeLog> GetIncidentConfigurationChangeLogList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<IncidentConfigurationChangeLog>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<IncidentConfigurationChangeLog>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveIncidentConfigurationChangeLog(SaveRequest<IncidentConfigurationChangeLog> request)
		{
			try
			{
				IncidentConfigurationChangeLog entity = request.Item; 			   
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
						,entity.DeviceType
						,entity.Filename
						,entity.FileID

#if EntityProperty_NoDate
						,entity.`field`
#endif

						,BusinessBase.ReadyDateForStorage(entity.FileCreationDate)
						,BusinessBase.ReadyDateForStorage(entity.FileModificationDate)

#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<IncidentConfigurationChangeLog>(entity.CompanyID, entity.Attributes, properties);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveIncidentConfigurationChangeLogList(SaveListRequest<IncidentConfigurationChangeLog> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (IncidentConfigurationChangeLog entity in request.List)
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
						,entity.DeviceType
						,entity.Filename
						,entity.FileID

#if EntityProperty_NoDate
						,entity.`field`
#endif

						,BusinessBase.ReadyDateForStorage(entity.FileCreationDate)
						,BusinessBase.ReadyDateForStorage(entity.FileModificationDate)

#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<IncidentConfigurationChangeLog>(entity.CompanyID, entity.Attributes, properties);
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteIncidentConfigurationChangeLog(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<IncidentConfigurationChangeLog>("IncidentConfigurationChangeLog", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}