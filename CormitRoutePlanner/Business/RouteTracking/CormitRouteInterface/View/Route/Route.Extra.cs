//& IM-3558
using System;
using System.Collections.Generic;
using System.ServiceModel;
using FernBusinessBase;

namespace Cormit.Business.RouteTracking
{
    partial interface ICormitRouteTracking
	{
		[OperationContract]
		GetItemResponse<Route> GetRouteByExternalID(IDRequest request);

        [OperationContract]
        GetItemResponse<Route> GetPermanentRouteByExternalId(IDRequest request);
        
	}
}