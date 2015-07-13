//& IM-3558
using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace Cormit.Business.RouteTracking
{
    partial class CormitRouteTracking
	{
		public GetItemResponse<Route> GetRoute(IDRequest request)
		{
			try
			{
				return GenericGetEntity<Route>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Route>>(ex);
			}
		}

		public GetUpdateCountResponse GetRouteUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<Route>("Route", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<Route> GetRouteListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<Route>("Route", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Route>>(ex);
			}
		}

		public GetListResponse<Route> GetRouteList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<Route>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Route>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveRoute(SaveRequest<Route> request)
		{
			try
			{
				Route entity = request.Item;
				BaseEntity.ValidateThrow(entity);

				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.ExternalID
						,entity.Name
						,entity.Description
						,entity.Temporary
						,entity.UnderConstruction

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<Route>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveRouteList(SaveListRequest<Route> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (Route entity in request.List)
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
						,entity.ExternalID
						,entity.Name
						,entity.Description
						,entity.Temporary
						,entity.UnderConstruction

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<Route>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteRoute(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<Route>("Route", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}