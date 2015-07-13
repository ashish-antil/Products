
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{

		#region Operation Contracts for RelationShip
		
		[OperationContract]
		GetListResponse<RelationShip> GetRelationShipListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<RelationShip> GetRelationShipList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveRelationShipList(SaveListRequest<RelationShip> request);

		[OperationContract]
		BusinessMessageResponse SaveRelationShip(SaveRequest<RelationShip> request);

		[OperationContract]
		BusinessMessageResponse DeleteRelationShip(IDRequest request);

		[OperationContract]
		GetItemResponse<RelationShip> GetRelationShip(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetRelationShipUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}