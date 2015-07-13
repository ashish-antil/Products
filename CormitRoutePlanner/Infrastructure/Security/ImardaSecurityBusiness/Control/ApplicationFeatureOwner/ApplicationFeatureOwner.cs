using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		public GetItemResponse<ApplicationFeatureOwner> GetApplicationFeatureOwner(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ApplicationFeatureOwner>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ApplicationFeatureOwner>>(ex);
			}
		}

		public GetUpdateCountResponse GetApplicationFeatureOwnerUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<ApplicationFeatureOwner>("ApplicationFeatureOwner", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ApplicationFeatureOwner> GetApplicationFeatureOwnerListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ApplicationFeatureOwner>("ApplicationFeatureOwner", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationFeatureOwner>>(ex);
			}
		}

		public GetListResponse<ApplicationFeatureOwner> GetApplicationFeatureOwnerList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<ApplicationFeatureOwner>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationFeatureOwner>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveApplicationFeatureOwner(SaveRequest<ApplicationFeatureOwner> request)
		{
			try
			{
				ApplicationFeatureOwner entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.ApplicationFeatureID
						,entity.OwnerID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<ApplicationFeatureOwner>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveApplicationFeatureOwnerList(SaveListRequest<ApplicationFeatureOwner> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (ApplicationFeatureOwner entity in request.List)
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
						,entity.ApplicationFeatureID
						,entity.OwnerID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<ApplicationFeatureOwner>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteApplicationFeatureOwner(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ApplicationFeatureOwner>("ApplicationFeatureOwner", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}
