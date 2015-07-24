//& IM-3558
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace Cormit.Business.RouteTracking 
{
    partial interface ICormitRouteTracking 
	{

		#region Operation Contracts for Route
		
		[OperationContract]
		GetListResponse<Route> GetRouteListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<Route> GetRouteList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveRouteList(SaveListRequest<Route> request);

		[OperationContract]
		BusinessMessageResponse SaveRoute(SaveRequest<Route> request);

		[OperationContract]
		BusinessMessageResponse DeleteRoute(IDRequest request);

		[OperationContract]
		GetItemResponse<Route> GetRoute(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetRouteUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}