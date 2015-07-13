using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		public GetItemResponse<ClientScope> GetClientScope(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ClientScope>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ClientScope>>(ex);
			}
		}

		public GetUpdateCountResponse GetClientScopeUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<ClientScope>("ClientScope", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ClientScope> GetClientScopeListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ClientScope>("ClientScope", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ClientScope>>(ex);
			}
		}

		public GetListResponse<ClientScope> GetClientScopeList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<ClientScope>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ClientScope>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveClientScope(SaveRequest<ClientScope> request)
		{
			try
			{
				ClientScope entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.ClientID
						,entity.ScopeID
						,entity.ConsentType

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<ClientScope>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveClientScopeList(SaveListRequest<ClientScope> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (ClientScope entity in request.List)
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
						,entity.ClientID
						,entity.ScopeID
						,entity.ConsentType

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<ClientScope>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteClientScope(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ClientScope>("ClientScope", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}