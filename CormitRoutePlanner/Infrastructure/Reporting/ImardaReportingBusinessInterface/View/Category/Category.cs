using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaReportBusiness
{
	partial interface IImardaReport 
	{
		[OperationContract]
		GetListResponse<Category> GetCategoryList();
	}
}
