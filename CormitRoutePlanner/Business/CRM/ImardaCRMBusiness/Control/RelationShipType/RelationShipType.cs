using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		public GetItemResponse<RelationShipType> GetRelationShipType(IDRequest request)
		{
			try
			{
				return GenericGetEntity<RelationShipType>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<RelationShipType>>(ex);
			}
		}

		public GetUpdateCountResponse GetRelationShipTypeUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<RelationShipType>("RelationShipType", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<RelationShipType> GetRelationShipTypeListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<RelationShipType>("RelationShipType", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<RelationShipType>>(ex);
			}
		}

		public GetListResponse<RelationShipType> GetRelationShipTypeList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<RelationShipType>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<RelationShipType>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveRelationShipType(SaveRequest<RelationShipType> request)
		{
			try
			{
				RelationShipType entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.Name
						,entity.Description

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<RelationShipType>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveRelationShipTypeList(SaveListRequest<RelationShipType> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (RelationShipType entity in request.List)
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
						,entity.Name
						,entity.Description

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<RelationShipType>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteRelationShipType(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<RelationShipType>("RelationShipType", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}