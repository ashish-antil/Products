using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness {
	partial interface IImardaSecurity {
		[OperationContract]
		GetListResponse<SecurityEntry> GetEntitySecurityEntryList(IDRequest request);

		[OperationContract]
		GetListResponse<SecurityEntry> GetEntitySecurityEntryListForI360(IDRequest request);

		[OperationContract]
		GetListResponse<SecurityEntry> GetEntitySecurityEntryListForIac(IDRequest request);

		[OperationContract]
		GetListResponse<SecurityEntry> GetSecurityEntryList();

		[OperationContract]
		BusinessMessageResponse SaveSecurityEntryList(SaveListRequest<SecurityEntry> request);

		[OperationContract]
		BusinessMessageResponse DeleteSecurityEntry(IDRequest request);

		[OperationContract]
		BusinessMessageResponse DeleteEntitySecurityEntry(IDRequest request);

		[OperationContract]
		BusinessMessageResponse DeleteSecurityEntryList(SaveListRequest<SecurityEntryKey> request);
	}
}
