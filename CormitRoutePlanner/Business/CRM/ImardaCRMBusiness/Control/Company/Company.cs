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

		#region Get Company
		public GetItemResponse<Company> GetCompany(IDRequest request)
		{
			try
			{
				return GenericGetEntity<Company>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Company>>(ex);
			}
		}
		#endregion
		#region GetCompanyUpdateCount


		public GetUpdateCountResponse GetCompanyUpdateCount(GetUpdateCountRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();

			try
			{
				response = GenericGetEntityUpdateCount<Company>("Company", request.TimeStamp, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}

			return response;
		}
		#endregion
		#region GetCompanyListByTimeStamp


		public GetListResponse<Company> GetCompanyListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<Company>("Company", request.TimeStamp, request.Cap, true, request.IncludeInactive, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Company>>(ex);
			}
		}
		#endregion
		#region GetCompanyList


		public GetListResponse<Company> GetCompanyList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<Company>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Company>>(ex);
			}
		}
		#endregion
		#region Save Company
		public BusinessMessageResponse SaveCompany(SaveRequest<Company> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				Company entity = request.Item;
				BaseEntity.ValidateThrow(entity);
				
				object[] properties = new object[]{			
					    entity.ID,
						entity.CompanyID,
						entity.Path,		 
						entity.Active,
						entity.Deleted,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.UserID,
						entity.Name,
						entity.StreetAddress,
						entity.Suburb,
						entity.City,
						entity.State,
						entity.PostCode,
						entity.DisplayName,
						entity.Phone,
						entity.Fax,
						entity.Mobile,
						entity.Email,
						entity.ServiceEmail,
						entity.RunPrograms,
						entity.AccountManagerID,
						entity.AutoLogoffPeriod,
						entity.GracePeriod,
						entity.MinorBreak,
						entity.MajorBreak,
						entity.WorkPeriod,
						entity.MapLocationID,
						entity.MasterPassword,
						entity.Country ?? "XX",
						entity.FatigueRuleDefault
						,entity.UnlockCode
						,entity.LinkID 
						,entity.ClientType
						,entity.TimeZone
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                response = GenericSaveEntity<Company>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
		#endregion
		#region SaveCompanyList


		public BusinessMessageResponse SaveCompanyList(SaveListRequest<Company> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (Company entity in request.List)
				{
					BaseEntity.ValidateThrow(entity);
					object[] properties = new object[]
					{
						entity.ID,
						entity.CompanyID,
						entity.Path,		 
						entity.Active,
						entity.Deleted,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.UserID,
						entity.Name,
						entity.StreetAddress,
						entity.Suburb,
						entity.City,
						entity.State,
						entity.PostCode,
						entity.DisplayName,
						entity.Phone,
						entity.Fax,
						entity.Mobile,
						entity.Email,
						entity.ServiceEmail,
						entity.RunPrograms,
						entity.AccountManagerID,
						entity.AutoLogoffPeriod,
						entity.GracePeriod,
						entity.MinorBreak,
						entity.MajorBreak,
						entity.WorkPeriod,
						entity.MapLocationID,
						entity.MasterPassword,
						entity.Country ?? "XX",
						entity.FatigueRuleDefault
						,entity.UnlockCode
						,entity.LinkID 
						,entity.ClientType
						,entity.TimeZone
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<Company>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
		#endregion
	}
}


