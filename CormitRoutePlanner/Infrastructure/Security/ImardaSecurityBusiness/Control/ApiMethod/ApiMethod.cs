using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		public GetItemResponse<ApiMethod> GetApiMethod(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ApiMethod>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ApiMethod>>(ex);
			}
		}

		public GetItemResponse<ApiMethod> GetApiMethodByName(IDRequest request)
		{
			try
			{
				var methodName = request["MethodName"];
				return ImardaDatabase.GetItem<ApiMethod>("SPGetApiMethodByName", methodName);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ApiMethod>>(ex);
			}
		}

		public GetUpdateCountResponse GetApiMethodUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<ApiMethod>("ApiMethod", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ApiMethod> GetApiMethodListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ApiMethod>("ApiMethod", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApiMethod>>(ex);
			}
		}

		public GetListResponse<ApiMethod> GetApiMethodList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<ApiMethod>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApiMethod>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveApiMethod(SaveRequest<ApiMethod> request)
		{
			try
			{
				ApiMethod entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted
						,entity.Name
						,entity.Description
						,entity.ApiID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<ApiMethod>(entity.CompanyID, entity.Attributes, properties);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveApiMethodList(SaveListRequest<ApiMethod> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (ApiMethod entity in request.List)
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
						,entity.Name
						,entity.Description
						,entity.ApiID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<ApiMethod>(entity.CompanyID, entity.Attributes, properties);
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteApiMethod(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ApiMethod>("ApiMethod", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}