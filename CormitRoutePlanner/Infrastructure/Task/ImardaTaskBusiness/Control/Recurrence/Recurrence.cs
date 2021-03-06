/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\maurice.verheijen
Date Generated : 24/06/2009 9:55 a.m.
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
namespace ImardaTaskBusiness
{
	partial class ImardaTask
	{

		#region Get Recurrence
		public GetItemResponse<Recurrence> GetRecurrence(IDRequest request)
		{
			try
			{
				return GenericGetEntity<Recurrence>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Recurrence>>(ex);
			}
		}
		#endregion
		#region GetRecurrenceUpdateCount
		

		public GetUpdateCountResponse GetRecurrenceUpdateCount(GetUpdateCountRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();

			try
			{
				response = GenericGetEntityUpdateCount<Recurrence>("Recurrence", request.TimeStamp, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}

			return response;
		}
		#endregion
		#region GetRecurrenceListByTimeStamp
		

		public GetListResponse<Recurrence> GetRecurrenceListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<Recurrence>("Recurrence", request.TimeStamp, request.Cap, true, request.IncludeInactive, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Recurrence>>(ex);
			}
		}
		#endregion
		#region GetRecurrenceList
		

		public GetListResponse<Recurrence> GetRecurrenceList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<Recurrence>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Recurrence>>(ex);
			}
		}
		#endregion
		#region Save Recurrence
		public BusinessMessageResponse SaveRecurrence(SaveRequest<Recurrence> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				Recurrence entity = request.Item;
				BaseEntity.ValidateThrow(entity);
				 			   
				object [] properties=new object[]{			entity.ID,
						entity.Definition,
						entity.Start,
						entity.EndCondition,
						entity.EventNumber,
						entity.Last,
						entity.Next,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Deleted,
						entity.Active
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				response = GenericSaveEntity<Recurrence>(entity.Attributes, properties);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
		#endregion
		#region SaveRecurrenceList
		

		public BusinessMessageResponse SaveRecurrenceList(SaveListRequest<Recurrence> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (Recurrence entity in request.List)
				{
					BaseEntity.ValidateThrow(entity);
					object [] properties=new object[]
					{
						entity.ID,
						entity.Definition,
						entity.Start,
						entity.EndCondition,
						entity.EventNumber,
						entity.Last,
						entity.Next,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Deleted,
						entity.Active
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<Recurrence>(entity.Attributes, properties); 				   
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
		#endregion
		#region Delete Recurrence
		

		public BusinessMessageResponse DeleteRecurrence(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<Recurrence>("Recurrence", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
	}
}


