using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		public GetItemResponse<ApplicationFeatureCategory> GetApplicationFeatureCategory(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ApplicationFeatureCategory>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ApplicationFeatureCategory>>(ex);
			}
		}

		public GetUpdateCountResponse GetApplicationFeatureCategoryUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<ApplicationFeatureCategory>("ApplicationFeatureCategory", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ApplicationFeatureCategory> GetApplicationFeatureCategoryListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ApplicationFeatureCategory>("ApplicationFeatureCategory", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationFeatureCategory>>(ex);
			}
		}

		public GetListResponse<ApplicationFeatureCategory> GetApplicationFeatureCategoryList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<ApplicationFeatureCategory>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationFeatureCategory>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveApplicationFeatureCategory(SaveRequest<ApplicationFeatureCategory> request)
		{
			try
			{
				ApplicationFeatureCategory entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.ApplicationID
						,entity.Name
						,entity.Description

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<ApplicationFeatureCategory>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveApplicationFeatureCategoryList(SaveListRequest<ApplicationFeatureCategory> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (ApplicationFeatureCategory entity in request.List)
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
						,entity.ApplicationID
						,entity.Name
						,entity.Description

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<ApplicationFeatureCategory>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteApplicationFeatureCategory(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ApplicationFeatureCategory>("ApplicationFeatureCategory", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}
