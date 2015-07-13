using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace ImardaSecurityBusiness {
	partial interface IImardaSecurity {

		[OperationContract]
		GetApplicationListResponse GetApplicationList();
	}
}
