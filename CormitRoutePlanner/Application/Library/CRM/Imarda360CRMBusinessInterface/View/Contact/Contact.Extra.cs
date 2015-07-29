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
		[OperationContract]
		GetListResponse<Contact> SearchContactList(IDRequest request);
		[OperationContract]
		GetListResponse<Contact> GetContactListByParentId(IDRequest request); 
}
}