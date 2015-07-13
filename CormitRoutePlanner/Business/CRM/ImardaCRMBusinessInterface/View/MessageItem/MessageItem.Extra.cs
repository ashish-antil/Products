using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using FernBusinessBase;

namespace ImardaCRMBusiness
{
	partial interface IImardaCRM
	{

		#region Operation Contracts for Message

		[OperationContract]
		GetListResponse<MessageItem> GetMessageListByUser(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveMessageViewedByUser(IDRequest request);

		#endregion
	}
}