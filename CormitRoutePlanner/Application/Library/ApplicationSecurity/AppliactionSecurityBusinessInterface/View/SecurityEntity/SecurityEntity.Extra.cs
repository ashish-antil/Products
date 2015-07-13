using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace Imarda360Application.Security
{
	partial interface IImardaSecurity
	{
		[OperationContract]
		SimpleResponse<Guid> TransferUser(IDRequest request);
	}
}