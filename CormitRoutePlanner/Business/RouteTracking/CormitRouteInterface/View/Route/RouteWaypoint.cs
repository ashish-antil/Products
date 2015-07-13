using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace Cormit.Business.RouteTracking
{
    partial interface IImardaTracking
    {

        #region Operation Contracts for RouteWayPoint

        [OperationContract]
        BusinessMessageResponse SaveRouteWayPoint(SaveRequest<RouteWaypoint> request);

        [OperationContract]
        BusinessMessageResponse SaveRouteWayPointList(SaveListRequest<RouteWaypoint> request);

        [OperationContract]
        GetListResponse<RouteWaypoint> GetRouteWayPointList(IDRequest request);

        [OperationContract]
        GetListResponse<Waypoint> GetRouteWayPointListByRouteId(IDRequest request);

        #endregion

    }
}