using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		public GetItemResponse<ApplicationFeature> GetApplicationFeature(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ApplicationFeature>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ApplicationFeature>>(ex);
			}
		}

		public GetUpdateCountResponse GetApplicationFeatureUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<ApplicationFeature>("ApplicationFeature", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ApplicationFeature> GetApplicationFeatureListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ApplicationFeature>("ApplicationFeature", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationFeature>>(ex);
			}
		}

		public GetListResponse<ApplicationFeature> GetApplicationFeatureList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<ApplicationFeature>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationFeature>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveApplicationFeature(SaveRequest<ApplicationFeature> request)
		{
			try
			{
				ApplicationFeature entity = request.Item; 			   
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
						,entity.CategoryID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<ApplicationFeature>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveApplicationFeatureList(SaveListRequest<ApplicationFeature> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (ApplicationFeature entity in request.List)
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
						,entity.CategoryID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<ApplicationFeature>(entity.CompanyID, entity.Attributes, properties);     //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteApplicationFeature(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ApplicationFeature>("ApplicationFeature", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}
