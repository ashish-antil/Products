/***********************************************************************
Auto Generated Code.

Generated by   : IMARDAINC\Qian.Chen
Date Generated : 12/02/2010 3:40 p.m.
Copyright (c)2009 CodeGenerator 1.2
***********************************************************************/
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
		public GetItemResponse<EmailPending> GetEmailPending(IDRequest request)
		{
			try
			{
				return GenericGetEntity<EmailPending>(request);
			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle<GetItemResponse<EmailPending>>(ex);
			}
		}

		public GetUpdateCountResponse GetEmailPendingUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<EmailPending>("EmailPending", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<EmailPending> GetEmailPendingListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<EmailPending>("EmailPending", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle<GetListResponse<EmailPending>>(ex);
			}
		}

		public GetListResponse<EmailPending> GetEmailPendingList(IDRequest request)
		{

			var result = new GetListResponse<EmailPending>();

			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<EmailPending>());
				int numRecords = 100; //process 100 emails at a time
				if (request.ContainsKey("NumRecords"))  //! IM-2342
					int.TryParse(request["NumRecords"], out numRecords);
				object[] args = new object[] { numRecords };
				using (IDataReader dr = db.ExecuteDataReader("SPGetEmailPendingList", args))
				{
					while (dr.Read())
					{
						result.List.Add(GetFromData<EmailPending>(dr));
					}

					return result;
				}

			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle<GetListResponse<EmailPending>>(ex);
			}
		}

		public BusinessMessageResponse SaveEmailPending(SaveRequest<EmailPending> request)
		{
			try
			{
				EmailPending entity = request.Item;
				BaseEntity.ValidateThrow(entity);
				
				object[] properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.EmailDraftID,
						entity.Subject,
						entity.FromAddress,
						entity.Message,
						entity.NotificationID,
						entity.RecipientName,
						entity.ToAddress,
						entity.CC,
						entity.Bcc,
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
				var response = GenericSaveEntity<EmailPending>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveEmailPendingList(SaveListRequest<EmailPending> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (EmailPending entity in request.List)
				{
					BaseEntity.ValidateThrow(entity);
					object[] properties = new object[]
					{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.EmailDraftID,
						entity.Subject,
						entity.FromAddress,
						entity.Message,
						entity.NotificationID,
						entity.RecipientName,
						entity.ToAddress,
						entity.CC,
						entity.Bcc,
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
                    response = GenericSaveEntity<EmailPending>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteEmailPending(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<EmailPending>("EmailPending", request.ID);
			}
			catch (Exception ex)
			{
				_Log.Error(ex);
				return ErrorHandler.Handle(ex);
			}
		}
	}
}

