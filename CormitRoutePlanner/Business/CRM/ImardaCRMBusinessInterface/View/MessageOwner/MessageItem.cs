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


        #region Operation Contracts for MessageOwner



        [OperationContract]
        GetListResponse<MessageOwner> GetMessageOwnerList(IDRequest request);

        [OperationContract]
        BusinessMessageResponse SaveMessageOwnerList(SaveListRequest<MessageOwner> request);

        [OperationContract]
        BusinessMessageResponse SaveMessageOwner(SaveRequest<MessageOwner> request);

        [OperationContract]
        BusinessMessageResponse DeleteMessageOwner(IDRequest request);

        [OperationContract]
        GetItemResponse<MessageOwner> GetMessageOwner(IDRequest request);

        
		#endregion
	}
}