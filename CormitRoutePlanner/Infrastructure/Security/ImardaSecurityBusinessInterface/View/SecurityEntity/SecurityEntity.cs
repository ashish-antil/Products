using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness {
	partial interface IImardaSecurity 
	{

		[OperationContract]
		GetItemResponse<SecurityEntity> GetSecurityEntity(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveSecurityEntity(SaveRequest<SecurityEntity> request);

		[OperationContract]
		GetListResponse<SecurityEntity> GetSecurityEntityList();

		[OperationContract]
		BusinessMessageResponse SaveSecurityEntityList(SaveListRequest<SecurityEntity> request);

		[OperationContract]
		BusinessMessageResponse DeleteSecurityEntity(IDRequest request);

		[OperationContract]
		GetListResponse<SecurityEntity> GetSecurityEntityListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetUpdateCountResponse GetSecurityEntityUpdateCount(GetUpdateCountRequest request);
	}
}
