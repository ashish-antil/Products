using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaConfigurationBusiness
{
	partial class ImardaConfiguration
	{
		public GetItemResponse<IncidentConfigurationCompany> GetIncidentConfigurationCompany(IDRequest request)
		{
			try
			{
				return GenericGetEntity<IncidentConfigurationCompany>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<IncidentConfigurationCompany>>(ex);
			}
		}

		public GetUpdateCountResponse GetIncidentConfigurationCompanyUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<IncidentConfigurationCompany>("IncidentConfigurationCompany", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<IncidentConfigurationCompany> GetIncidentConfigurationCompanyListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<IncidentConfigurationCompany>("IncidentConfigurationCompany", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<IncidentConfigurationCompany>>(ex);
			}
		}

		public GetListResponse<IncidentConfigurationCompany> GetIncidentConfigurationCompanyList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<IncidentConfigurationCompany>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<IncidentConfigurationCompany>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveIncidentConfigurationCompany(SaveRequest<IncidentConfigurationCompany> request)
		{
			try
			{
				IncidentConfigurationCompany entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted
						,entity.IncidentConfigurationID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<IncidentConfigurationCompany>(entity.CompanyID, entity.Attributes, properties);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveIncidentConfigurationCompanyList(SaveListRequest<IncidentConfigurationCompany> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (IncidentConfigurationCompany entity in request.List)
				{
					var properties = new object[]
					{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted
						,entity.IncidentConfigurationID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<IncidentConfigurationCompany>(entity.CompanyID, entity.Attributes, properties);
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteIncidentConfigurationCompany(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<IncidentConfigurationCompany>("IncidentConfigurationCompany", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse UpdateIncidentConfigurationCompanyList(SaveListRequest<IncidentConfigurationCompany> request)
		{
			var configurationID = request.Get("ConfigurationID", Guid.Empty);
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ProfileAssignment>());
				db.ExecuteNonQuery("SPClearIncidentConfigurationCompanyList", configurationID);
				return SaveIncidentConfigurationCompanyList(request); ;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}

}