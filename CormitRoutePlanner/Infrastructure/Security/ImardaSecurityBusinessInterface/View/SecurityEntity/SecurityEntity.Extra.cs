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
		GetListResponse<SecurityEntity> GetSecurityEntityExtent(GetFilteredExtentRequest req);

		[OperationContract]
		GetItemResponse<SecurityEntity> GetSecurityEntityByCrmID(IDRequest request);

		[OperationContract]
		GetItemResponse<SecurityEntity> GetSecurityEntityByLoginUserName(IDRequest request);

		[OperationContract]
		GetListResponse<SecurityEntity> GetSecurityEntityListByCompanyID(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveUserSecurityEntity(SaveRequest<SecurityEntity> request);

		#region SecurityObjects For Entity
		[OperationContract]
		GetListResponse<SecurityObject> GetUnassignedSecurityObjects(IDRequest request);

		[OperationContract]
		GetListResponse<SecurityObject> GetAssignedSecurityObjects(IDRequest request);

		[OperationContract]
		GetListResponse<SecurityObject> GetKnownSecurityObjects(IDRequest request);
		#endregion

		[OperationContract]
		BusinessMessageResponse SetDeletedSecurityEntityByCRMID(IDRequest request);

		[OperationContract]
		SimpleResponse<bool> CheckIfUniqueForUser(IDRequest request);

		#region By ApplicationID (for IAC and other new operations)
		[OperationContract]
		GetListResponse<SecurityObject> GetSecurityObjectsByApplicationID(IDRequest request);

		[OperationContract]
		GetListResponse<SecurityObject> GetSecurityObjectsAssignedByApplicationID(IDRequest request);
		#endregion
	}
}
