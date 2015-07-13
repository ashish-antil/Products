using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaConfigurationBusiness
{
	partial class ImardaConfiguration
	{
		public GetItemResponse<ProfileAdmission> GetProfileAdmission(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ProfileAdmission>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ProfileAdmission>>(ex);
			}
		}

		public GetItemResponse<ProfileAdmission> GetProfileAdmissionByCompanyID(IDRequest request)
		{
			try
			{
				//request.CompanyID contains CompanyID, request.ID contains ProfileID
				return ImardaDatabase.GetItem<ProfileAdmission>("SPGetProfileAdmissionByCompanyID", request.CompanyID, request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ProfileAdmission>>(ex);
			}
		}

		public GetUpdateCountResponse GetProfileAdmissionUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<ProfileAdmission>("ProfileAdmission", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ProfileAdmission> GetProfileAdmissionListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ProfileAdmission>("ProfileAdmission", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ProfileAdmission>>(ex);
			}
		}

		public GetListResponse<ProfileAdmission> GetProfileAdmissionList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<ProfileAdmission>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ProfileAdmission>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveProfileAdmission(SaveRequest<ProfileAdmission> request)
		{
			try
			{
				ProfileAdmission entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.ProfileID
						,entity.AllowCustomersToUse

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<ProfileAdmission>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveProfileAdmissionList(SaveListRequest<ProfileAdmission> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (ProfileAdmission entity in request.List)
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
						,entity.ProfileID
						,entity.AllowCustomersToUse

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<ProfileAdmission>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteProfileAdmission(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ProfileAdmission>("ProfileAdmission", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

	}
}