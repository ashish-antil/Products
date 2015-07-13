using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
        public GetItemResponse<MessageItem> GetMessageItem(IDRequest request)
        {
            try
            {
                return GenericGetEntity<MessageItem>(request);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<MessageItem>>(ex);
            }
        }


        public GetListResponse<MessageItem> GetMessageItemListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                return GenericGetEntityListByTimestamp<MessageItem>("MessageItem", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<MessageItem>>(ex);
            }
        }

        public GetListResponse<MessageItem> GetMessageItemList(IDRequest request)
        {
            try
            {
                return GenericGetEntityList<MessageItem>(request);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<MessageItem>>(ex);
            }
        }

        public BusinessMessageResponse SaveMessageItem(SaveRequest<MessageItem> request)
        {
            try
            {
                MessageItem entity = request.Item;
                var properties = new object[]
				{
						entity.ID,
						entity.MessageTitle,
						entity.MessageContent,
						entity.MessageType,
						entity.UserID,
                        entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.MessageDate,
                        entity.Active,
                        entity.Deleted
					};
                var response = GenericSaveEntity<MessageItem>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveMessageItemList(SaveListRequest<MessageItem> request)
        {
            var response = new BusinessMessageResponse();
            try
            {
                foreach (MessageItem entity in request.List)
                {
                    var properties = new object[]
					{
						entity.ID,
						entity.MessageTitle,
						entity.MessageContent,
						entity.MessageType,
						entity.UserID,
                        entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.MessageDate,
                        entity.Active,
                        entity.Deleted

#if EntityProperty_NoDate
						,entity.`field`
#endif

						
						

#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<MessageItem>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
                }
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse DeleteMessageItem(IDRequest request)
        {
            try
            {
                return GenericDeleteEntity<MessageItem>("MessageItem", request.ID);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
	}
}