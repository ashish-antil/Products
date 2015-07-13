using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaTrackingBusiness
{

	[Serializable]
	[MessageContract]
	public class ProcessDeviceUpdateRequest
	{
		public ProcessDeviceUpdateRequest()
		{
			Update = new Update();
		}


		[MessageBodyMember]
		public Update Update { get; set; }
	}

	partial interface IImardaTracking
	{

		[OperationContract]
		BusinessMessageResponse ProcessDeviceUpdate(ProcessDeviceUpdateRequest request);

	}
}