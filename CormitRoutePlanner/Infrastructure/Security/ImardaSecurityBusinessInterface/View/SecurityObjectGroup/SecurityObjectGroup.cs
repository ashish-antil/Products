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
		GetListResponse<SecurityObjectGroup> GetSecurityObjectGroupList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveSecurityObjectGroupList(SaveListRequest<SecurityObjectGroup> request);

		[OperationContract]
		BusinessMessageResponse DeleteSecurityObjectGroup(IDRequest request);
	}
}
