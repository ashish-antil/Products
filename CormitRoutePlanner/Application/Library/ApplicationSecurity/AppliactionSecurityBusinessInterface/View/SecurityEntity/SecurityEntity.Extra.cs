using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace Cormit.Application.RouteApplication.Security
{
	partial interface IImardaSecurity
	{
		[OperationContract]
		SimpleResponse<Guid> TransferUser(IDRequest request);
	}
}