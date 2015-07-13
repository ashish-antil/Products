using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		public GetItemResponse<ContactMap> GetContactMap(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ContactMap>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ContactMap>>(ex);
			}
		}

		public GetUpdateCountResponse GetContactMapUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<ContactMap>("ContactMap", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ContactMap> GetContactMapListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ContactMap>("ContactMap", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ContactMap>>(ex);
			}
		}

		public GetListResponse<ContactMap> GetContactMapList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<ContactMap>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ContactMap>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveContactMap(SaveRequest<ContactMap> request)
		{
			try
			{
				ContactMap entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.ContactId
						,entity.ContactPersonId
						,entity.DefaultPerson

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<ContactMap>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveContactMapList(SaveListRequest<ContactMap> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (ContactMap entity in request.List)
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
						,entity.ContactId
						,entity.ContactPersonId
						,entity.DefaultPerson

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<ContactMap>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteContactMap(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ContactMap>("ContactMap", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}