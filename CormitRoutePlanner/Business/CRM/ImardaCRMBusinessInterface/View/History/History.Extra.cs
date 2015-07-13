//& gs-104
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{

		#region Extra Contracts for History

		[OperationContract]
		GetListResponse<History> GetHistoryExtent(GetFilteredExtentRequest request);

		[OperationContract]
		GetListResponse<History> GetHistoryListByContactID(IDRequest request);

		[OperationContract]
		GetListResponse<History> GetHistoryListByJobID(IDRequest request);

		[OperationContract]
		GetListResponse<History> GetHistoryListByTaskID(IDRequest request);

		[OperationContract]
		GetListResponse<History> GetHistoryListByUserID(IDRequest request);

		#endregion

	}
}