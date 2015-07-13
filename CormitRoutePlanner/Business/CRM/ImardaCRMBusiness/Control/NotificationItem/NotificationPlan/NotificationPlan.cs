/***********************************************************************
Auto Generated Code.

Generated by   : IMARDAINC\Qian.Chen
Date Generated : 12/03/2010 12:10 p.m.
Copyright (c)2009 CodeGenerator 1.2
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;


namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		public GetItemResponse<NotificationPlan> GetNotificationPlan(IDRequest request)
		{
			try
			{
				return GenericGetEntity<NotificationPlan>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<NotificationPlan>>(ex);
			}
		}

		public GetUpdateCountResponse GetNotificationPlanUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<NotificationPlan>("NotificationPlan", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<NotificationPlan> GetNotificationPlanListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<NotificationPlan>("NotificationPlan", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NotificationPlan>>(ex);
			}
		}

		public GetListResponse<NotificationPlan> GetNotificationPlanList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<NotificationPlan>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NotificationPlan>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveNotificationPlan(SaveRequest<NotificationPlan> request)
		{
			try
			{
				NotificationPlan entity = request.Item;
				BaseEntity.ValidateThrow(entity);
				 			   
				object [] properties=new object[]
				{
						entity.ID,
						entity.Name,
						entity.Description,
						entity.CompanyID,
						entity.UserID,
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
                var response = GenericSaveEntity<NotificationPlan>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveNotificationPlanList(SaveListRequest<NotificationPlan> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (NotificationPlan entity in request.List)
				{
					BaseEntity.ValidateThrow(entity);
					object [] properties=new object[]
					{
						entity.ID,
						entity.Name,
						entity.Description,
						entity.CompanyID,
						entity.UserID,
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
                    response = GenericSaveEntity<NotificationPlan>(entity.CompanyID, entity.Attributes, properties); 				     //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteNotificationPlan(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<NotificationPlan>("NotificationPlan", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}

