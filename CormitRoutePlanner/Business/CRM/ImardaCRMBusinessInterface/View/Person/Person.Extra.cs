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
		GetListResponse<Person> GetPersonExtent(GetFilteredExtentRequest request);

		[OperationContract]
		GetListResponse<Person> GetPersonListByCompanyID(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetActiveUserCount(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetInactiveUserCount(IDRequest request);
		
	}
}
