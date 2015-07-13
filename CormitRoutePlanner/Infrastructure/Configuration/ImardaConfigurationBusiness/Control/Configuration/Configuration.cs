/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\adamson.delacruz
Date Generated : 12/05/2009 12:13 PM
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
namespace ImardaConfigurationBusiness
{
	partial class ImardaConfiguration
	{

		#region Get Configuration
		public GetItemResponse<Configuration> GetConfiguration(IDRequest request)
		{
			try
			{
				return GenericGetEntity<Configuration>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Configuration>>(ex);
			}
		}
		#endregion
		#region GetConfigurationUpdateCount
		

		public GetUpdateCountResponse GetConfigurationUpdateCount(GetUpdateCountRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();

			try
			{
				response = GenericGetEntityUpdateCount<Configuration>("Configuration", request.TimeStamp, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}

			return response;
		}
		#endregion
		#region GetConfigurationListByTimeStamp
		

		public GetListResponse<Configuration> GetConfigurationListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<Configuration>("Configuration", request.TimeStamp, request.Cap, true, request.IncludeInactive, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Configuration>>(ex);
			}
		}
		#endregion
		#region GetConfigurationList
		

		public GetListResponse<Configuration> GetConfigurationList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<Configuration>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Configuration>>(ex);
			}
		}
		#endregion
		#region Save Configuration
		public BusinessMessageResponse SaveConfiguration(SaveRequest<Configuration> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				Configuration entity = request.Item;
				BaseEntity.ValidateThrow(entity);
				
				object[] properties = new object[]{			
					entity.ID,
					entity.Active,
					entity.Deleted,
					entity.DateCreated,
					entity.DateModified = DateTime.UtcNow,
					entity.CompanyID,
					entity.UserID,
					entity.Combine,
					entity.ValueType,
					entity.VersionValue,
					entity.Notes,
					entity.Hierarchy,
					entity.Level1,
					entity.Level2,
					entity.Level3,
					entity.Level4,
					entity.Level5,
					entity.UID
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				response = GenericSaveEntity<Configuration>(entity.Attributes, properties);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
		#endregion
		#region SaveConfigurationList
		

		public BusinessMessageResponse SaveConfigurationList(SaveListRequest<Configuration> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (Configuration entity in request.List)
				{
					BaseEntity.ValidateThrow(entity);
					object[] properties = new object[]
					{
						entity.ID,
						entity.Active,
						entity.Deleted,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.CompanyID,
						entity.UserID,
						entity.Combine,
						entity.ValueType,
						entity.VersionValue,
						entity.Notes,
						entity.Hierarchy,
						entity.Level1,
						entity.Level2,
						entity.Level3,
						entity.Level4,
						entity.Level5,
						entity.UID
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<Configuration>(entity.Attributes, properties);
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
		#endregion
		#region Delete Configuration
		

		public BusinessMessageResponse DeleteConfiguration(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<Configuration>("Configuration", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
	}
}

