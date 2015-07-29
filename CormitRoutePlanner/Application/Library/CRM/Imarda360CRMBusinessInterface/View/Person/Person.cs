/***********************************************************************
Auto Generated Code

Generated by   : adam-Laptop\adam
Date Generated : 28/04/2009 1:32 PM
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using ImardaCRMBusiness;
namespace Imarda360Application.CRM
{
	partial interface IImardaCRM 
	{

		#region Operation Contracts for Person
		[OperationContract]
		GetListResponse<Person> GetPersonListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<Person> GetPersonList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SavePersonList(SaveListRequest<Person> request);

		[OperationContract]
		BusinessMessageResponse SavePerson(SaveRequest<Person> request);

		[OperationContract]
		BusinessMessageResponse DeletePerson(IDRequest request);

		[OperationContract]
		GetItemResponse<Person> GetPerson(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetPersonUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}


