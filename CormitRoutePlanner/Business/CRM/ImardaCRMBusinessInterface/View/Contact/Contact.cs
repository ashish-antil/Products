
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{

		#region Operation Contracts for Contact
		
		[OperationContract]
		GetListResponse<Contact> GetContactListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<Contact> GetContactList(IDRequest request);

		[OperationContract]
		GetListResponse<Contact> GetContactExtent(GetFilteredExtentRequest request);

		[OperationContract]
		BusinessMessageResponse SaveContactList(SaveListRequest<Contact> request);

		[OperationContract]
		BusinessMessageResponse SaveContact(SaveRequest<Contact> request);

		[OperationContract]
		BusinessMessageResponse DeleteContact(IDRequest request);

		[OperationContract]
		GetItemResponse<Contact> GetContact(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetContactUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}
