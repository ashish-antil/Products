using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaConfigurationBusiness
{
	partial class ImardaConfiguration
	{
		public GetItemResponse<ProfileAssignment> GetProfileAssignment(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ProfileAssignment>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ProfileAssignment>>(ex);
			}
		}

		public GetUpdateCountResponse GetProfileAssignmentUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<ProfileAssignment>("ProfileAssignment", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<ProfileAssignment> GetProfileAssignmentListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ProfileAssignment>("ProfileAssignment", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ProfileAssignment>>(ex);
			}
		}

		public GetListResponse<ProfileAssignment> GetProfileAssignmentList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<ProfileAssignment>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ProfileAssignment>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveProfileAssignment(SaveRequest<ProfileAssignment> request)
		{
			try
			{
				ProfileAssignment entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.ProfileID
						,entity.AssignedToID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<ProfileAssignment>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveProfileAssignmentList(SaveListRequest<ProfileAssignment> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (ProfileAssignment entity in request.List)
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
						,entity.ProfileID
						,entity.AssignedToID

#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<ProfileAssignment>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteProfileAssignment(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ProfileAssignment>("ProfileAssignment", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}


		public BusinessMessageResponse UpdateProfileAssignmentList(SaveListRequest<ProfileAssignment> request)
		{
			var companyID = request.CompanyID;
			var areaType = request["AreaType"];
			var assignedToID = new Guid(request["AssignedToID"]);
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ProfileAssignment>());
				db.ExecuteNonQuery("SPClearProfileAssignment", assignedToID, areaType);
				return SaveProfileAssignmentList(request); ;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

	}
}