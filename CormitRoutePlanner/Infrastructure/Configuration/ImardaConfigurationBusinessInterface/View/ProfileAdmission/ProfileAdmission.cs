using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaConfigurationBusiness 
{
	partial interface IImardaConfiguration 
	{

		#region Operation Contracts for ProfileAdmission
		
		[OperationContract]
		GetListResponse<ProfileAdmission> GetProfileAdmissionListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<ProfileAdmission> GetProfileAdmissionList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveProfileAdmissionList(SaveListRequest<ProfileAdmission> request);

		[OperationContract]
		BusinessMessageResponse SaveProfileAdmission(SaveRequest<ProfileAdmission> request);

		[OperationContract]
		BusinessMessageResponse DeleteProfileAdmission(IDRequest request);

		[OperationContract]
		GetItemResponse<ProfileAdmission> GetProfileAdmission(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetProfileAdmissionUpdateCount(GetUpdateCountRequest request);

		[OperationContract]
		GetItemResponse<ProfileAdmission> GetProfileAdmissionByCompanyID(IDRequest request);
		
		#endregion

	}
}