using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		public GetItemResponse<History> GetHistory(IDRequest request)
		{
			try
			{
				return GenericGetEntity<History>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<History>>(ex);
			}
		}

		public GetUpdateCountResponse GetHistoryUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<History>("History", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<History> GetHistoryListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<History>("History", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<History>>(ex);
			}
		}

		public GetListResponse<History> GetHistoryList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<History>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<History>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveHistory(SaveRequest<History> request)
		{
			try
			{
				History entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.Path,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,BusinessBase.ReadyDateForStorage(entity.Date)
						,entity.Subject
						,entity.Description
						,entity.EventType
						,entity.OwnerType
						,entity.OwnerID

						,entity.ContactID
						,entity.EmployeeID						
						,entity.JobID
						,entity.TaskID
						,entity.TaskType
						
						,entity.AttachmentID
						,entity.Completed
						,BusinessBase.ReadyDateForStorage(entity.CompleteDate)

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<History>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveHistoryList(SaveListRequest<History> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (History entity in request.List)
				{
					var properties = new object[]
					{
						entity.ID,
						entity.CompanyID,
						entity.Path,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,BusinessBase.ReadyDateForStorage(entity.Date)
						,entity.Subject
						,entity.Description
						,entity.EventType
						,entity.OwnerType
						,entity.OwnerID

						,entity.ContactID
						,entity.EmployeeID						
						,entity.JobID
						,entity.TaskID
						,entity.TaskType
						
						,entity.AttachmentID
						,entity.Completed
						,BusinessBase.ReadyDateForStorage(entity.CompleteDate)

#if EntityProperty_NoDate
						,entity.`field`
#endif

						
						

#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<History>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteHistory(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<History>("History", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}