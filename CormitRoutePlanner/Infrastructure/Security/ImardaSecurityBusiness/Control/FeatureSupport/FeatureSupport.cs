using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		public GetItemResponse<FeatureSupport> GetFeatureSupport(IDRequest request)
		{
			try
			{
				return GenericGetEntity<FeatureSupport>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<FeatureSupport>>(ex);
			}
		}

		public GetUpdateCountResponse GetFeatureSupportUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<FeatureSupport>("FeatureSupport", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<FeatureSupport> GetFeatureSupportListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<FeatureSupport>("FeatureSupport", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<FeatureSupport>>(ex);
			}
		}

		public GetListResponse<FeatureSupport> GetFeatureSupportList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<FeatureSupport>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<FeatureSupport>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveFeatureSupport(SaveRequest<FeatureSupport> request)
		{
			try
			{
				FeatureSupport entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.FeatureID
						,entity.OwnerID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<FeatureSupport>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveFeatureSupportList(SaveListRequest<FeatureSupport> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (FeatureSupport entity in request.List)
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
						,entity.FeatureID
						,entity.OwnerID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<FeatureSupport>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteFeatureSupport(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<FeatureSupport>("FeatureSupport", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}
