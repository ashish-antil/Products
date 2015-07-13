
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{

		#region Operation Contracts for Asset
		
		[OperationContract]
		GetListResponse<Asset> GetAssetListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<Asset> GetAssetList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveAssetList(SaveListRequest<Asset> request);

		[OperationContract]
		BusinessMessageResponse SaveAsset(SaveRequest<Asset> request);

		[OperationContract]
		BusinessMessageResponse DeleteAsset(IDRequest request);

		[OperationContract]
		GetItemResponse<Asset> GetAsset(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetAssetUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}
