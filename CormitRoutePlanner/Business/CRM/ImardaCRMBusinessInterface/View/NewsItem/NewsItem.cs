//& gs-104
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{

		#region Operation Contracts for NewsItem
		
		[OperationContract]
		GetListResponse<NewsItem> GetNewsItemListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<NewsItem> GetNewsItemList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveNewsItemList(SaveListRequest<NewsItem> request);

		[OperationContract]
		BusinessMessageResponse SaveNewsItem(SaveRequest<NewsItem> request);

		[OperationContract]
		BusinessMessageResponse DeleteNewsItem(IDRequest request);

		[OperationContract]
		GetItemResponse<NewsItem> GetNewsItem(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetNewsItemUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}