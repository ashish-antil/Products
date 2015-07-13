/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\adamson.delacruz
Date Generated : 27/04/2009 1:27 PM
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{

		#region Operation Contracts for RoleType
		[OperationContract]
		GetListResponse<RoleType> GetRoleTypeListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<RoleType> GetRoleTypeList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveRoleTypeList(SaveListRequest<RoleType> request);

		[OperationContract]
		BusinessMessageResponse SaveRoleType(SaveRequest<RoleType> request);

		[OperationContract]
		BusinessMessageResponse DeleteRoleType(IDRequest request);

		[OperationContract]
		GetItemResponse<RoleType> GetRoleType(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetRoleTypeUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}

