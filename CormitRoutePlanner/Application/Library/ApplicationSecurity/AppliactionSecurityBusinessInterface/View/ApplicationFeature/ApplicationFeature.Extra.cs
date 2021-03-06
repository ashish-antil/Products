using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using ImardaSecurityBusiness;

namespace Cormit.Application.RouteApplication.Security
{
	partial interface IImardaSecurity
	{
		[OperationContract]
		GetListResponse<ApplicationFeature> GetApplicationFeatureListByCategoryID(IDRequest request);
		[OperationContract]
		GetListResponse<ApplicationFeature> GetApplicationFeatureListByOwnerID(IDRequest request);
	}
}