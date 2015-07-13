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
		public GetItemResponse<SMSDraft> GetSMSDraft(IDRequest request)
		{
			try
			{
				return GenericGetEntity<SMSDraft>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<SMSDraft>>(ex);
			}
		}

		public GetUpdateCountResponse GetSMSDraftUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<SMSDraft>("SMSDraft", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<SMSDraft> GetSMSDraftListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<SMSDraft>("SMSDraft", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SMSDraft>>(ex);
			}
		}

		public GetListResponse<SMSDraft> GetSMSDraftList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<SMSDraft>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SMSDraft>>(ex);
			}
		}

		public BusinessMessageResponse SaveSMSDraft(SaveRequest<SMSDraft> request)
		{
			try
			{
				SMSDraft entity = request.Item;
				BaseEntity.ValidateThrow(entity);
				
				object[] properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.Subject,
						entity.Message,
						entity.NotificationID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<SMSDraft>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveSMSDraftList(SaveListRequest<SMSDraft> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (SMSDraft entity in request.List)
				{
					BaseEntity.ValidateThrow(entity);
					object[] properties = new object[]
					{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.Subject,
						entity.Message,
						entity.NotificationID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<SMSDraft>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteSMSDraft(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<SMSDraft>("SMSDraft", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}