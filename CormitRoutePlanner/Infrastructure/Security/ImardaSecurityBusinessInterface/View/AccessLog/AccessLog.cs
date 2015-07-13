using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness
{
	partial interface IImardaSecurity
	{
		[OperationContract]
		GetListResponse<AccessLog> GetAccessLogList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveAccessLog(SaveRequest<AccessLog> request);

		[OperationContract]
		BusinessMessageResponse SaveAccessLogList(SaveListRequest<AccessLog> request);

		[OperationContract]
		BusinessMessageResponse DeleteAccessLog(IDRequest request);

		[OperationContract]
		GetListResponse<AccessLog> GetAccessLogListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetUpdateCountResponse GetAccessLogUpdateCount(GetUpdateCountRequest request);
	}
}
