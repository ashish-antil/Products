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
		GetListResponse<CompanyLocation> GetCompanyLocationListByCompanyID(IDRequest request);
	}
}