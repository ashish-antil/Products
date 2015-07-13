using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{

		#region Extra Contracts for ActivityLog

		[OperationContract]
		GetListResponse<ActivityLogEntry> GetActivityLogExtent(GetFilteredExtentRequest request);

		[OperationContract]
		GetListResponse<ActivityLogEntry> GetActivityLogListByUserID(IDRequest request);

		#endregion

	}
}