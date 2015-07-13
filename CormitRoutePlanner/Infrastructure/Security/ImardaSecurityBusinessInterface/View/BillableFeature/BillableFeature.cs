using System;
using System.Collections.Generic;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness
{
	partial interface IImardaSecurity
	{

		[OperationContract]
		GetListResponse<BillableFeature> GetBillableFeaturesByCompanyID(IDRequest request);

	}
}
