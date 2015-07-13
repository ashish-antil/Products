using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace Imarda360Application.Tracking
{
	partial interface IImardaTracking
	{
		[OperationContract]
		BusinessMessageResponse ClearTaskQueueCache(IDRequest request);
}
}