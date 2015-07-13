using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness {
	partial interface IImardaSecurity {
		[OperationContract]
		GetListResponse<SecurityEntityParent> GetSecurityEntityParentList();

		[OperationContract]
		BusinessMessageResponse SaveSecurityEntityParentList(SaveListRequest<SecurityEntityParent> request);

		[OperationContract]
		BusinessMessageResponse DeleteSecurityEntityParent(IDRequest request);
	}
}
