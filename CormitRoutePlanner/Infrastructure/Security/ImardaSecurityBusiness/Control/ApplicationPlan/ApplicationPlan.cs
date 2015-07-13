using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		public GetItemResponse<ApplicationPlan> GetApplicationPlan(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ApplicationPlan>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ApplicationPlan>>(ex);
			}
		}

		public GetUpdateCountResponse GetApplicationPlanUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<ApplicationPlan>("ApplicationPlan", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ApplicationPlan> GetApplicationPlanListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ApplicationPlan>("ApplicationPlan", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationPlan>>(ex);
			}
		}

		public GetListResponse<ApplicationPlan> GetApplicationPlanList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<ApplicationPlan>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationPlan>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveApplicationPlan(SaveRequest<ApplicationPlan> request)
		{
			try
			{
				ApplicationPlan entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.ApplicationID
						,entity.Name
						,entity.Description

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<ApplicationPlan>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveApplicationPlanList(SaveListRequest<ApplicationPlan> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (ApplicationPlan entity in request.List)
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
						,entity.ApplicationID
						,entity.Name
						,entity.Description

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<ApplicationPlan>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteApplicationPlan(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ApplicationPlan>("ApplicationPlan", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}
