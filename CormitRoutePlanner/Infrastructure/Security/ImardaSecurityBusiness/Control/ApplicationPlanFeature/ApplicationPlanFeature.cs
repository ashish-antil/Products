using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		public GetItemResponse<ApplicationPlanFeature> GetApplicationPlanFeature(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ApplicationPlanFeature>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ApplicationPlanFeature>>(ex);
			}
		}

		public GetUpdateCountResponse GetApplicationPlanFeatureUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<ApplicationPlanFeature>("ApplicationPlanFeature", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ApplicationPlanFeature> GetApplicationPlanFeatureListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ApplicationPlanFeature>("ApplicationPlanFeature", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationPlanFeature>>(ex);
			}
		}

		public GetListResponse<ApplicationPlanFeature> GetApplicationPlanFeatureList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<ApplicationPlanFeature>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationPlanFeature>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveApplicationPlanFeature(SaveRequest<ApplicationPlanFeature> request)
		{
			try
			{
				ApplicationPlanFeature entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.ApplicationPlanID
						,entity.FeatureID
						,entity.FeatureType
						,entity.Billable
						,entity.Price
						,entity.UnitCount

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<ApplicationPlanFeature>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveApplicationPlanFeatureList(SaveListRequest<ApplicationPlanFeature> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (ApplicationPlanFeature entity in request.List)
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
						,entity.ApplicationPlanID
						,entity.FeatureID
						,entity.FeatureType
						,entity.Billable
						,entity.Price
						,entity.UnitCount

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<ApplicationPlanFeature>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteApplicationPlanFeature(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ApplicationPlanFeature>("ApplicationPlanFeature", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}
