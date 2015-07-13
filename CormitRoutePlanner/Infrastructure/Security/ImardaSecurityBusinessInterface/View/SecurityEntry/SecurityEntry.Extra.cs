using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness
{
	partial interface IImardaSecurity
	{
		[OperationContract]
		BusinessMessageResponse DeleteEntriesByEntityID(IDRequest request);

		[OperationContract]
		GetListResponse<SecurityEntry> GetEntriesByEntityID(IDRequest request);
	}
}
