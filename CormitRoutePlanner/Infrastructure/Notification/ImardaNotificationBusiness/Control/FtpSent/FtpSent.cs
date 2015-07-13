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
		//public GetItemResponse<FtpSent> GetFtpSent(IDRequest request)
		//{
		//	try
		//	{
		//		return GenericGetEntity<FtpSent>(request);
		//	}
		//	catch (Exception ex)
		//	{
		//		return ErrorHandler.Handle<GetItemResponse<FtpSent>>(ex);
		//	}
		//}

		//public GetUpdateCountResponse GetFtpSentUpdateCount(GetUpdateCountRequest request)
		//{
		//	try
		//	{
		//		var response = GenericGetEntityUpdateCount<FtpSent>("FtpSent", request.TimeStamp, true, request.ID, request.LastRecordID);
		//		return response;
		//	}
		//	catch (Exception ex)
		//	{
		//		return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
		//	}
		//}

		//public GetListResponse<FtpSent> GetFtpSentListByTimeStamp(GetListByTimestampRequest request)
		//{
		//	try
		//	{
		//		return GenericGetEntityListByTimestamp<FtpSent>("FtpSent", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
		//	}
		//	catch (Exception ex)
		//	{
		//		return ErrorHandler.Handle<GetListResponse<FtpSent>>(ex);
		//	}
		//}

		//public GetListResponse<FtpSent> GetFtpSentList(IDRequest request)
		//{
		//	try
		//	{
		//		return GenericGetEntityList<FtpSent>(request);
		//	}
		//	catch (Exception ex)
		//	{
		//		return ErrorHandler.Handle<GetListResponse<FtpSent>>(ex);
		//	}
		//}

		public BusinessMessageResponse SaveFtpSent(SaveRequest<FtpSent> request)
		{
			try
			{
				FtpSent entity = request.Item;
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
						entity.AttachmentFiles,
						entity.DestinationPath,
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
                var response = GenericSaveEntity<FtpSent>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

//		public BusinessMessageResponse SaveFtpSentList(SaveListRequest<FtpSent> request)
//		{
//			var response = new BusinessMessageResponse();
//			try
//			{
//				foreach (FtpSent entity in request.List)
//				{
//					BaseEntity.ValidateThrow(entity);
//					object[] properties = new object[]
//					{
//						entity.ID,
//						entity.CompanyID,
//						entity.UserID,
//						entity.IPAddress,
//						entity.Port,
//						entity.Username,
//						entity.Password,
//						entity.PSK,
//						entity.AttachmentFiles,
//						entity.DestinationPath,
//						entity.Retry,
//						entity.TimeToSend,
//						entity.Status,
//						entity.DateCreated,
//						entity.DateModified = DateTime.UtcNow,
//						entity.LastRetryAt,
//						entity.Active,
//						entity.Deleted
//#if EntityProperty_NoDate
//						,entity.`field`
//#endif
//#if EntityProperty_Date
//						,BusinessBase.ReadyDateForStorage(entity.`field`)
//#endif
//					};
//					response = GenericSaveEntity<FtpSent>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
//				}
//				return response;
//			}
//			catch (Exception ex)
//			{
//				return ErrorHandler.Handle(ex);
//			}
//		}

		//public BusinessMessageResponse DeleteFtpSent(IDRequest request)
		//{
		//	try
		//	{
		//		return GenericDeleteEntity<FtpSent>("FtpSent", request.ID);
		//	}
		//	catch (Exception ex)
		//	{
		//		return ErrorHandler.Handle(ex);
		//	}
		//}
	}
}
