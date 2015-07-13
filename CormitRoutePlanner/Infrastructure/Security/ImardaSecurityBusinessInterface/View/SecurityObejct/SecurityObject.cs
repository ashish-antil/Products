using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness {
	partial interface IImardaSecurity {
		[OperationContract]
		GetUpdateCountResponse GetSecurityObjectUpdateCount(GetUpdateCountRequest request);

		[OperationContract]
		GetListResponse<SecurityObject> GetSecurityObjectList(IDRequest request);

		[OperationContract]
		GetListResponse<SecurityObject> GetSecurityObjectListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		BusinessMessageResponse SaveSecurityObjectList(SaveListRequest<SecurityObject> request);

		[OperationContract]
		BusinessMessageResponse DeleteSecurityObject(IDRequest request);

		[OperationContract]
		BusinessMessageResponse AddSecurityObjectList(SaveListRequest<SecurityObjectInfo> permissionInfo);
	}
}
