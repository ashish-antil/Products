//& gs-104
using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		public GetItemResponse<NewsItem> GetNewsItem(IDRequest request)
		{
			try
			{
				return GenericGetEntity<NewsItem>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<NewsItem>>(ex);
			}
		}

		public GetUpdateCountResponse GetNewsItemUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<NewsItem>("NewsItem", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<NewsItem> GetNewsItemListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<NewsItem>("NewsItem", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NewsItem>>(ex);
			}
		}

		public GetListResponse<NewsItem> GetNewsItemList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<NewsItem>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<NewsItem>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveNewsItem(SaveRequest<NewsItem> request)
		{
			try
			{
				NewsItem entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted
						,BusinessBase.ReadyDateForStorage(entity.Date)
						,entity.Subject
						,entity.Body
						,entity.Type

#if EntityProperty_NoDate
						,entity.`field`
#endif



#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<NewsItem>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveNewsItemList(SaveListRequest<NewsItem> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (NewsItem entity in request.List)
				{
					var properties = new object[]
					{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted
						,BusinessBase.ReadyDateForStorage(entity.Date)
						,entity.Subject
						,entity.Body
						,entity.Type

#if EntityProperty_NoDate
						,entity.`field`
#endif



#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<NewsItem>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteNewsItem(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<NewsItem>("NewsItem", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}