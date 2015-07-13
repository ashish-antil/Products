using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaVehicleBusiness {

	[ServiceContract]
	public partial interface IImardaVehicle : IServerFacadeBase 
	{
		[OperationContract]
		GetUpdateCountResponseList GetUpdateObjectList(GetUpdateCountRequestList request);
	}
}
