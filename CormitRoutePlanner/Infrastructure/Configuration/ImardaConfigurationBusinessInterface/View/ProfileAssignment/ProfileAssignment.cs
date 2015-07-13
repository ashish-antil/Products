using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaConfigurationBusiness 
{
	partial interface IImardaConfiguration 
	{

		#region Operation Contracts for ProfileAssignment
		
		[OperationContract]
		GetListResponse<ProfileAssignment> GetProfileAssignmentListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<ProfileAssignment> GetProfileAssignmentList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveProfileAssignmentList(SaveListRequest<ProfileAssignment> request);

		[OperationContract]
		BusinessMessageResponse SaveProfileAssignment(SaveRequest<ProfileAssignment> request);

		[OperationContract]
		BusinessMessageResponse DeleteProfileAssignment(IDRequest request);

		[OperationContract]
		GetItemResponse<ProfileAssignment> GetProfileAssignment(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetProfileAssignmentUpdateCount(GetUpdateCountRequest request);

		[OperationContract]
		BusinessMessageResponse UpdateProfileAssignmentList(SaveListRequest<ProfileAssignment> request);
		
		#endregion

	}
}