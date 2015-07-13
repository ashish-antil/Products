using System;
using FernBusinessBase;
using FernBusinessBase.Errors;


namespace Cormit.Business.RouteTracking
{
    partial class CormitRouteTracking
    {
        public GetListResponse<RouteWaypoint> GetRouteWayPointList(IDRequest request)
        {
            try
            {
                return GenericGetEntityList<RouteWaypoint>(request);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<RouteWaypoint>>(ex);
            }
        }

        public GetListResponse<Waypoint> GetRouteWayPointListByRouteId(IDRequest request)
        {
            try
            {
                return ImardaDatabase.GetList<Waypoint>("SPGetRouteWayPointListByRouteId", request.ID);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<Waypoint>>(ex);
            }
        }


        public BusinessMessageResponse SaveRouteWayPoint(SaveRequest<RouteWaypoint> request)
        {
            try
            {
                RouteWaypoint entity = request.Item;
                BaseEntity.ValidateThrow(entity);

                var properties = new object[]
				{
					    entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted,
                        entity.RouteId,
                        entity.WayPointId
						

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<RouteWaypoint>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveRouteWayPointList(SaveListRequest<RouteWaypoint> request)
        {
            var response = new BusinessMessageResponse();
            try
            {
                foreach (RouteWaypoint entity in request.List)
                {
                    var properties = new object[]
					{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted,
                        entity.RouteId,
                        entity.WayPointId
						

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<RouteWaypoint>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
                }
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
    }
}
