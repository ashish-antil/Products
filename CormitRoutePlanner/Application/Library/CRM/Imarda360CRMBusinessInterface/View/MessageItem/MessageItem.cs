using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using FernBusinessBase;
using ImardaCRMBusiness;


namespace Imarda360Application.CRM
{
	partial interface IImardaCRM
	{
		#region Operation Contracts for Message
		[OperationContract]
		GetListResponse<MessageItem> GetMessageListByUser(IDRequest request);
		[OperationContract]
		BusinessMessageResponse SaveMessageViewedByUser(IDRequest request);

        [OperationContract]
        BusinessMessageResponse SaveMessageItem(SaveRequest<MessageItem> request);
		#endregion

	}
}