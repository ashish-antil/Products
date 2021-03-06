/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\adamson.delacruz
Date Generated : 27/04/2009 1:27 PM
Copyright @2009 CodeGenerator
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

		#region Get Person
		public GetItemResponse<Person> GetPerson(IDRequest request)
		{
			try
			{
				return GenericGetEntity<Person>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Person>>(ex);
			}
		}
		#endregion
		#region GetPersonUpdateCount
		

		public GetUpdateCountResponse GetPersonUpdateCount(GetUpdateCountRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();

			try
			{
				response = GenericGetEntityUpdateCount<Person>("Person", request.TimeStamp, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}

			return response;
		}
		#endregion
		#region GetPersonListByTimeStamp
		

		public GetListResponse<Person> GetPersonListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<Person>("Person", request.TimeStamp, request.Cap, true, request.IncludeInactive, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Person>>(ex);
			}
		}
		#endregion
		#region GetPersonList
		

		public GetListResponse<Person> GetPersonList(IDRequest request)
		{
			try
			{
				var list = GenericGetEntityList<Person>(request);
				return list;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Person>>(ex);
			}
		}
		#endregion
		#region Save Person
		public BusinessMessageResponse SavePerson(SaveRequest<Person> request)
		{
			try
			{
				Person entity = request.Item;
				BaseEntity.ValidateThrow(entity);
				
				object[] properties = new object[]{			
							entity.ID,
							entity.CompanyID,
							entity.Path,        //& gs-351
							entity.Active,
							entity.Deleted,
							entity.DateCreated,
							entity.DateModified = DateTime.UtcNow,
							entity.UserID,
							//entity.UserName,	//$ IM-3609
							entity.FullName,
							entity.Email,
							entity.Phone,
							entity.MobilePhone,
							entity.JobTitle,
							entity.Picture
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				return GenericSaveEntity<Person>("Person", properties);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
		#region SavePersonList
		

		public BusinessMessageResponse SavePersonList(SaveListRequest<Person> request)
		{
			try
			{
				BusinessMessageResponse response = null;
				foreach (Person entity in request.List)
				{
					BaseEntity.ValidateThrow(entity);
					object[] properties = new object[]
					{
						entity.ID,
						entity.CompanyID,
						entity.Path,		//& gs-351
						entity.Active,
						entity.Deleted,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.UserID,
						//entity.UserName,	//$ IM-3609
						entity.FullName,
						entity.Email,
						entity.Phone,
						entity.MobilePhone,
						entity.JobTitle,
						entity.Picture
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<Person>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
		#region Delete Person
		

		public BusinessMessageResponse DeletePerson(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<Person>("Person", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
	}
}


