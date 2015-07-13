//& IM-3927
using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using System.Data;


namespace ImardaNotificationBusiness
{
	partial class ImardaNotification
	{
		public GetItemResponse<FtpPending> GetFtpPending(IDRequest request)
		{
			try
			{
				return GenericGetEntity<FtpPending>(request);
			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle<GetItemResponse<FtpPending>>(ex);
			}
		}

		public GetUpdateCountResponse GetFtpPendingUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<FtpPending>("FtpPending", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<FtpPending> GetFtpPendingListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<FtpPending>("FtpPending", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle<GetListResponse<FtpPending>>(ex);
			}
		}

		public GetListResponse<FtpPending> GetFtpPendingList(IDRequest request)
		{

			var result = new GetListResponse<FtpPending>();

			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<FtpPending>());
				int numRecords = 100; //process 100 Ftps at a time
				if (request.ContainsKey("NumRecords"))  //! IM-2342
					int.TryParse(request["NumRecords"], out numRecords);
				object[] args = new object[] { numRecords };
				using (IDataReader dr = db.ExecuteDataReader("SPGetFtpPendingList", args))
				{
					while (dr.Read())
					{
						result.List.Add(GetFromData<FtpPending>(dr));
					}

					return result;
				}

			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle<GetListResponse<FtpPending>>(ex);
			}
		}

		public BusinessMessageResponse SaveFtpPending(SaveRequest<FtpPending> request)
		{
			try
			{
				FtpPending entity = request.Item;
				BaseEntity.ValidateThrow(entity);

				object[] properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.IPAddress,
						entity.Port,
						entity.Username,
						entity.Password,
						entity.PSK,
						entity.DestinationPath,
						entity.AttachmentFiles,
						entity.Retry,
						entity.TimeToSend,
						entity.Status,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.LastRetryAt,
						entity.Active,
						entity.Deleted
						,entity.Priority
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<FtpPending>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveFtpPendingList(SaveListRequest<FtpPending> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (FtpPending entity in request.List)
				{
					BaseEntity.ValidateThrow(entity);
					object[] properties = new object[]
					{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.IPAddress,
						entity.Port,
						entity.Username,
						entity.Password,
						entity.PSK,
						entity.DestinationPath,
						entity.AttachmentFiles,
						entity.Retry,
						entity.TimeToSend,
						entity.Status,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.LastRetryAt,
						entity.Active,
						entity.Deleted
						,entity.Priority
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<FtpPending>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteFtpPending(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<FtpPending>("FtpPending", request.ID);
			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle(ex);
			}
		}
	}
}

