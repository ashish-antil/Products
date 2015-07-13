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

		
        #region Operation Contracts for History

        

        [OperationContract]
        GetListResponse<MessageItem> GetMessageItemList(IDRequest request);

        [OperationContract]
        BusinessMessageResponse SaveMessageItemList(SaveListRequest<MessageItem> request);

        [OperationContract]
        BusinessMessageResponse SaveMessageItem(SaveRequest<MessageItem> request);

        [OperationContract]
        BusinessMessageResponse DeleteMessageItem(IDRequest request);

        [OperationContract]
        GetItemResponse<MessageItem> GetMessageItem(IDRequest request);

        
		#endregion
	}
}