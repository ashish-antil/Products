using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness
{
	partial interface IImardaCRM
	{
		[OperationContract]
		GetListResponse<Company> GetCompanyExtent(GetFilteredExtentRequest request);

		[OperationContract]
		SimpleResponse<int> GetCompanyExtentCount(GetFilteredExtentRequest request);

		[OperationContract]
		GetListResponse<Company> GetCompanyListManagedBy(IDRequest request);

		[OperationContract]
		GetListResponse<CustomerInfo> GetCustomerInfoList(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetScopedCompanyCount(IDRequest request);

		[OperationContract]
		SimpleResponse<bool> CheckIfUniqueForCompany(IDRequest request);
 
	}
}