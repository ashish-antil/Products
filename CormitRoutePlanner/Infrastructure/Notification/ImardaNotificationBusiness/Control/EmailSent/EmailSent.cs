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


namespace ImardaNotificationBusiness
{
	partial class ImardaNotification
	{
		public GetItemResponse<EmailSent> GetEmailSent(IDRequest request)
		{
			try
			{
				return GenericGetEntity<EmailSent>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<EmailSent>>(ex);
			}
		}

		public GetUpdateCountResponse GetEmailSentUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<EmailSent>("EmailSent", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<EmailSent> GetEmailSentListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<EmailSent>("EmailSent", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<EmailSent>>(ex);
			}
		}

		public GetListResponse<EmailSent> GetEmailSentList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<EmailSent>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<EmailSent>>(ex);
			}
		}

		public BusinessMessageResponse SaveEmailSent(SaveRequest<EmailSent> request)
		{
			try
			{
				EmailSent entity = request.Item;
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
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<EmailSent>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveEmailSentList(SaveListRequest<EmailSent> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (EmailSent entity in request.List)
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
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<EmailSent>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteEmailSent(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<EmailSent>("EmailSent", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}
