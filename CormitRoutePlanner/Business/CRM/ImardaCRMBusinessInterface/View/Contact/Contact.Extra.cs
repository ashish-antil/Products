using System;
using System.Collections.Generic;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness
{
	partial interface IImardaCRM
	{
		[OperationContract]
		GetListResponse<Contact> SearchContactList(IDRequest request);
		[OperationContract]
		GetListResponse<Contact> GetContactListByParentId(IDRequest request); 
		[OperationContract]
		BusinessMessageResponse DeleteContactRelatedPersons(IDRequest request);

        [OperationContract]
        GetItemResponse<Contact> GetContactByName(IDRequest request);


	}
}
