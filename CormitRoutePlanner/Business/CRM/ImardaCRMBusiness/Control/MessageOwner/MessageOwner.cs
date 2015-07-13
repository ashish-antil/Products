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
        public GetItemResponse<MessageOwner> GetMessageOwner(IDRequest request)
        {
            try
            {
                return GenericGetEntity<MessageOwner>(request);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<MessageOwner>>(ex);
            }
        }


        public GetListResponse<MessageOwner> GetMessageOwnerListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                return GenericGetEntityListByTimestamp<MessageOwner>("MessageOwner", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<MessageOwner>>(ex);
            }
        }

        public GetListResponse<MessageOwner> GetMessageOwnerList(IDRequest request)
        {
            try
            {
                return GenericGetEntityList<MessageOwner>(request);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<MessageOwner>>(ex);
            }
        }

        public BusinessMessageResponse SaveMessageOwner(SaveRequest<MessageOwner> request)
        {
            try
            {
                MessageOwner entity = request.Item;
                var properties = new object[]
				{
						entity.ID,
						entity.MessageID,
						entity.CRMID,
                        entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
                        entity.Active,
                        entity.Deleted,
                        entity.Viewed

					};
                var response = GenericSaveEntity<MessageOwner>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveMessageOwnerList(SaveListRequest<MessageOwner> request)
        {
            var response = new BusinessMessageResponse();
            try
            {
                foreach (MessageOwner entity in request.List)
                {
                    var properties = new object[]
					{
						entity.ID,
						entity.MessageID,
						entity.CRMID,
                        entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
                        entity.Active,
                        entity.Deleted,
                        entity.Viewed

#if EntityProperty_NoDate
						,entity.`field`
#endif

						
						

#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<MessageOwner>(entity.CompanyID, entity.Attributes, properties);  //Review IM-3747
                }
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse DeleteMessageOwner(IDRequest request)
        {
            try
            {
                return GenericDeleteEntity<MessageOwner>("MessageOwner", request.ID);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
    }
}