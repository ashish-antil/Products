using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		public GetItemResponse<Asset> GetAsset(IDRequest request)
		{
			try
			{
				return GenericGetEntity<Asset>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Asset>>(ex);
			}
		}

		public GetUpdateCountResponse GetAssetUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<Asset>("Asset", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<Asset> GetAssetListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<Asset>("Asset", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Asset>>(ex);
			}
		}

		public GetListResponse<Asset> GetAssetList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<Asset>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Asset>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveAsset(SaveRequest<Asset> request)
		{
			try
			{
				Asset entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.Description
						,entity.AccessCode
						,entity.User
						,entity.Email
						,entity.PasswordHash
						,entity.MobilePhone
						,entity.Salt

						,entity.Lat
						,entity.Lon
#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<Asset>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveAssetList(SaveListRequest<Asset> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (Asset entity in request.List)
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
						,entity.Description
						,entity.AccessCode
						,entity.User
						,entity.Email
						,entity.PasswordHash
						,entity.MobilePhone
						,entity.Salt

						,entity.Lat
						,entity.Lon
#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<Asset>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteAsset(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<Asset>("Asset", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}
