
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{

		#region Operation Contracts for RelationShipType
		
		[OperationContract]
		GetListResponse<RelationShipType> GetRelationShipTypeListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<RelationShipType> GetRelationShipTypeList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveRelationShipTypeList(SaveListRequest<RelationShipType> request);

		[OperationContract]
		BusinessMessageResponse SaveRelationShipType(SaveRequest<RelationShipType> request);

		[OperationContract]
		BusinessMessageResponse DeleteRelationShipType(IDRequest request);

		[OperationContract]
		GetItemResponse<RelationShipType> GetRelationShipType(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetRelationShipTypeUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}