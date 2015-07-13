using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness {
	partial class ImardaSecurity {

		#region GetSecurityObjectUpdateCount
		

		public GetUpdateCountResponse GetSecurityObjectUpdateCount(GetUpdateCountRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();
			try
			{
				response = GenericGetEntityUpdateCount<SecurityObject>(request.TimeStamp, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
			return response;
		}
		#endregion

		

		public GetListResponse<SecurityObject> GetSecurityObjectListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<SecurityObject>("SecurityObject", request.TimeStamp, request.Cap, true, request.IncludeInactive, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
			}
		}

		

		public GetListResponse<SecurityObject> GetSecurityObjectList(IDRequest request) {
			try {
				return GenericGetEntityList<SecurityObject>("SecurityObject", true, request.ID);
			}
			catch (Exception ex) {
				return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
			}
		}

		public GetListResponse<SecurityObject> GetAllSecurityObjectList(bool rethrow)
		{
			try
			{
				return GenericGetEntityList<SecurityObject>("AllSecurityObject", true, new Guid[0]);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
			}
		}

		

		public BusinessMessageResponse SaveSecurityObjectList(SaveListRequest<SecurityObject> request) {
			try {
				BusinessMessageResponse bmr = new BusinessMessageResponse();

				foreach (SecurityObject it in request.List) {
					bmr = GenericSaveEntity<SecurityObject>("SecurityObject", it.ID, it.SolutionID, it.ApplicationID,
						it.DisplayName, it.ParentID, it.ObjectGroupID, it.Description, it.PermissionsConfigurable,
						it.ObjectType, it.UserID , it.Active, it.Deleted, it.FeatureID);
				}

				return bmr;
			}
			catch (Exception ex) {
				return ErrorHandler.Handle(ex);
			}
		}

		

		public BusinessMessageResponse DeleteSecurityObject(IDRequest request) {
			try {
				return GenericDeleteEntity<SecurityObject>("SecurityObject", true, new object[] { request.ID });
			}
			catch (Exception ex) {
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse AddSecurityObjectList(SaveListRequest<SecurityObjectInfo> SecurityObjectInfo) {

			var response = new BusinessMessageResponse();

			try {

				List<SecurityObject> allSecurityObjects = GetAllSecurityObjectList(true).List;

				SaveListRequest<SecurityObject> saveRq = new SaveListRequest<SecurityObject>();
				saveRq.List = new List<SecurityObject>();

				foreach (SecurityObjectInfo pti in SecurityObjectInfo.List) {

					Predicate<SecurityObject> find = new Predicate<SecurityObject>(
						delegate(SecurityObject match) {
							return match.DisplayName == pti.Name;
						});

					SecurityObject found = allSecurityObjects.Find(find);

					if (found == null) {
						// this Security Object doesn't exist, add it

						SecurityObject newObject = new SecurityObject();

						newObject.DisplayName = pti.Name;
						newObject.ParentID = findSecurityObjectParentId(pti.Parent, ref allSecurityObjects);

						saveRq.List.Add(newObject);
					}
					else {
						// this Security Object does exist
					}
				}

				response = SaveSecurityObjectList(saveRq);
			}
			catch (Exception ex) {
				return ErrorHandler.Handle(ex);
			}


			return response;
		}

		private Guid findSecurityObjectParentId(string parentName, ref List<SecurityObject> permTypes) {

			Predicate<SecurityObject> find = new Predicate<SecurityObject>(
				delegate(SecurityObject match) {
					return match.DisplayName == parentName;
				});

			SecurityObject matched = permTypes.Find(find);

			return matched == null ? Guid.Empty : matched.ID;
		}
	}
}
