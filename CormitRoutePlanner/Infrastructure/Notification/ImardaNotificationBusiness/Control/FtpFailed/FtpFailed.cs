//& IM-3927
using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;


namespace ImardaNotificationBusiness
{
	partial class ImardaNotification
	{
		//public GetItemResponse<FtpFailed> GetFtpFailed(IDRequest request)
		//{
		//	try
		//	{
		//		return GenericGetEntity<FtpFailed>(request);
		//	}
		//	catch (Exception ex)
		//	{
		//		return ErrorHandler.Handle<GetItemResponse<FtpFailed>>(ex);
		//	}
		//}

		//public GetUpdateCountResponse GetFtpFailedUpdateCount(GetUpdateCountRequest request)
		//{
		//	try
		//	{
		//		var response = GenericGetEntityUpdateCount<FtpFailed>("FtpFailed", request.TimeStamp, true, request.ID, request.LastRecordID);
		//		return response;
		//	}
		//	catch (Exception ex)
		//	{
		//		return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
		//	}
		//}

		//public GetListResponse<FtpFailed> GetFtpFailedListByTimeStamp(GetListByTimestampRequest request)
		//{
		//	try
		//	{
		//		return GenericGetEntityListByTimestamp<FtpFailed>("FtpFailed", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
		//	}
		//	catch (Exception ex)
		//	{
		//		return ErrorHandler.Handle<GetListResponse<FtpFailed>>(ex);
		//	}
		//}

		//public GetListResponse<FtpFailed> GetFtpFailedList(IDRequest request)
		//{
		//	try
		//	{
		//		return GenericGetEntityList<FtpFailed>(request);
		//	}
		//	catch (Exception ex)
		//	{
		//		return ErrorHandler.Handle<GetListResponse<FtpFailed>>(ex);
		//	}
		//}

		public BusinessMessageResponse SaveFtpFailed(SaveRequest<FtpFailed> request)
		{
			try
			{
				FtpFailed entity = request.Item;
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
                var response = GenericSaveEntity<FtpFailed>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

//		public BusinessMessageResponse SaveFtpFailedList(SaveListRequest<FtpFailed> request)
//		{
//			var response = new BusinessMessageResponse();
//			try
//			{
//				foreach (FtpFailed entity in request.List)
//				{
//					BaseEntity.ValidateThrow(entity);
//					object[] properties = new object[]
//					{
//						entity.ID,
//						entity.CompanyID,
//						entity.UserID,
//						entity.FtpDraftID,
//						entity.Subject,
//						entity.FromAddress,
//						entity.Message,
//						entity.NotificationID,
//						entity.RecipientName,
//						entity.ToAddress,
//						entity.CC,
//						entity.Bcc,
//						entity.AttachmentFiles,
//						entity.Retry,
//						entity.TimeToSend,
//						entity.Status,
//						entity.DateCreated,
//						entity.DateModified = DateTime.UtcNow,
//						entity.LastRetryAt,
//						entity.Active,
//						entity.Deleted
//						,entity.Priority
//#if EntityProperty_NoDate
//						,entity.`field`
//#endif
//#if EntityProperty_Date
//						,BusinessBase.ReadyDateForStorage(entity.`field`)
//#endif
//					};
//					response = GenericSaveEntity<FtpFailed>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
//				}
//				return response;
//			}
//			catch (Exception ex)
//			{
//				return ErrorHandler.Handle(ex);
//			}
//		}

		public BusinessMessageResponse DeleteFtpFailed(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<FtpFailed>("FtpFailed", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}

