using System;
using System.Collections.Generic;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness
{
	partial interface IImardaSecurity
	{
		[OperationContract]
		GetListResponse<ApplicationFeature> GetApplicationFeatureListByCategoryID(IDRequest request);
		[OperationContract]
		GetListResponse<ApplicationFeature> GetApplicationFeatureListByOwnerID(IDRequest request);
	}
}
