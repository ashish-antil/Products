using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace Cormit.Business.RouteTracking
{
    partial class CormitRouteTracking
	{
		public GetItemResponse<Route> GetRouteByExternalID(IDRequest request)
		{
			try
			{
				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				Guid companyID = request.CompanyID;
				string externalID = request.GetString("ExternalID");
				return ImardaDatabase.GetItem<Route>("SPGetRouteByExternalID", request.ID, includeInactive, externalID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Route>>(ex);
			}
		}

        public GetItemResponse<Route> GetPermanentRouteByExternalId(IDRequest request)
		{
			try
			{
				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				Guid companyId = request.CompanyID;
				string externalId = request.GetString("ExternalID");
                return ImardaDatabase.GetItem<Route>("SPGetPermanentRouteByExternalID", request.ID, includeInactive, externalId);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Route>>(ex);
			}
		}
	}
}
