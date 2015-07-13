using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using System.ServiceModel;

namespace ImardaReportBusiness 
{
	partial interface IImardaReport
	{
		[OperationContract]
		GetListResponse<Report> GetReportListByReportTypeID(IDRequest request);


		/// <summary>
		/// Delete linked report by ID or all linked reports that match the name.
		/// </summary>
		/// <param name="req">req.ID=Report ID or alternatively req["name"] contains 
		/// name pattern, using SQL pattern syntax, e.g. "Travel%" </param>
		/// <returns>StatusMessage = number of reports deleted</returns>
		[OperationContract]
		BusinessMessageResponse DeleteLinkedReports(IDRequest req);
	}

}
