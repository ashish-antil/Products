using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using ImardaCRMBusiness;
namespace Imarda360Application.CRM
{
	partial interface IImardaCRM
	{

		#region Operation Contracts for Contact
		[OperationContract]
		GetListResponse<Contact> GetContactListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<Contact> GetContactList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveContactList(SaveListRequest<Contact> request);

		[OperationContract]
		BusinessMessageResponse SaveContact(SaveRequest<Contact> request);

		[OperationContract]
		GetItemResponse<Contact> GetContact(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetContactUpdateCount(GetUpdateCountRequest request);

		[OperationContract]
		BusinessMessageResponse DeleteContact(IDRequest request);

		#endregion

	}
}
