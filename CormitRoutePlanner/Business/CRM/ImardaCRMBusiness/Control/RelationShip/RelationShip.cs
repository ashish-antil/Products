using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		public GetItemResponse<RelationShip> GetRelationShip(IDRequest request)
		{
			try
			{
				return GenericGetEntity<RelationShip>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<RelationShip>>(ex);
			}
		}

		public GetUpdateCountResponse GetRelationShipUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<RelationShip>("RelationShip", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<RelationShip> GetRelationShipListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<RelationShip>("RelationShip", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<RelationShip>>(ex);
			}
		}

		public GetListResponse<RelationShip> GetRelationShipList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<RelationShip>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<RelationShip>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveRelationShip(SaveRequest<RelationShip> request)
		{
			try
			{
				RelationShip entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

                        ,entity.Type
						,entity.TypeID                        
						,entity.SubjectType
						,entity.SubjectID
						,entity.ObjectType
						,entity.ObjectID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<RelationShip>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveRelationShipList(SaveListRequest<RelationShip> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (RelationShip entity in request.List)
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
                        ,entity.Type
						,entity.TypeID
						,entity.SubjectType
						,entity.SubjectID
						,entity.ObjectType
						,entity.ObjectID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<RelationShip>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteRelationShip(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<RelationShip>("RelationShip", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}