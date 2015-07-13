using System;
using System.Collections.Generic;
using System.Text;
using Imarda.Lib;
using System.Data;
using FernBusinessBase;
using FernBusinessBase.Errors;
//using FernCommon.Model;

namespace ImardaSecurityBusiness {
	partial class ImardaSecurity {

		#region Get SecurityEntity
		

		public GetItemResponse<SecurityEntity> GetSecurityEntity(IDRequest request)
		{

			var response = new GetItemResponse<SecurityEntity>();
			try
			{
				response = GenericGetEntity<SecurityEntity>(request);
				if (response != null && response.Item != null)
				{
					SecurityEntity entity = response.Item;

					Guid applicationID;
					request.Get<Guid>("appid", out applicationID);

					//fill in permission list
					entity.PermissionList = GetSecurityPermissionList(applicationID, entity);

					// fill in the parent id list
					entity.ImmediateParentsIds = GetEntityRelationships(entity.ID);

					response.Item = entity;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<SecurityEntity>>(ex);
			}
			return response;
		}
		#endregion

		#region Save securityEntity


		//
		//
		//    THIS METHOD IS NOT USED, SEE SaveUserSecurityEntity
		//
		//
		//
		public BusinessMessageResponse SaveSecurityEntity(SaveRequest<SecurityEntity> request)
		{

			var response = new BusinessMessageResponse();

			try
			{
				if (request.Item != null)
				{
					SecurityEntity it = request.Item;

					object date = it.LastLogonDate;
					if (it.LastLogonDate == default(DateTime)) date = null;

					//
					//
					//    THIS METHOD IS NOT USED, SEE SaveUserSecurityEntity
					//
					//
					//


					response = GenericSaveEntity<SecurityEntity>("SecurityEntity",
						it.ID,
						it.EntityName,
						it.EntityType,
						it.LoginEnabled,
						it.LoginUsername,
						it.LoginPassword,
						it.CompanyID,
						it.Path,
						it.UserID,
						it.CRMId,
						it.Description,
						it.BranchID,
						date,
						it.Active,
						it.Deleted,
						string.IsNullOrEmpty(it.TimeZone) || it.TimeZone.StartsWith("(") ? "UTC" : it.TimeZone,
						it.Locale ?? "en",
						it.EnableTimeZoneSelect,
						it.IsAdmin,
						it.Salt,
						true
						);

					//save permission list
					response = SaveSecurityPermissionList(it);

					// save the entities parents
					//SaveSecurityEntityParents(it.ID, it.ImmediateParentsIds);
					/*
					response = GenericDeleteEntityList<SecurityEntityParent>("SecurityEntityParent", true, it.ID);

					if (it.ImmediateParentsIds != null && it.ImmediateParentsIds.Count > 0)
					{
						SaveListRequest<SecurityEntityParent> parentRequest = new SaveListRequest<SecurityEntityParent>();
						parentRequest.List = new List<SecurityEntityParent>();

						foreach (Guid parentID in it.ImmediateParentsIds)
						{
							SecurityEntityParent newParent = new SecurityEntityParent();

							newParent.RelationId = Guid.NewGuid();
							newParent.EntityId = it.ID;
							newParent.ParentId = parentID;
							parentRequest.List.Add(newParent);
						}
						response = SaveSecurityEntityParentList(parentRequest);
					}
					*/
				}
				response.Status = true;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}

			return response;
		}
		#endregion

		#region Get SecurityEntity List
		

		public GetListResponse<SecurityEntity> GetSecurityEntityList()
		{

			throw new NotImplementedException();

			//var response = new GetListResponse<SecurityEntity>();
			//try
			//{
			//  response.List = new List<SecurityEntity>();
			//  var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityEntity>());
			//  using (IDataReader dr = db.ExecuteDataReader("SPGetSecurityEntityList", new object[] { });
			//  // load into entity objects
			//  SecurityEntity secEntity;
			//  while (dr.Read())
			//  {
			//	secEntity = new SecurityEntity();
			//	secEntity.AssignData(dr);
			//	//fill in permission list
			//		Guid applicationID;
			//		request.Get<Guid>("appid", out applicationID);
			//	secEntity.PermissionList = GetSecurityPermissionList(applicationID, secEntity);
			//	// fill in the parent id list
			//	secEntity.ImmediateParentsIds = GetEntityRelationships(secEntity.ID);
			//	// add to the response
			//	response.List.Add(secEntity);
			//  }
			//  response.Status = true;
			//}
			//catch (Exception ex)
			//{
			//  return ErrorHandler.Handle<GetListResponse<SecurityEntity>>(ex);
			//}
			//return response;
		} 
		#endregion

		#region Save securityEntity List
		

		public BusinessMessageResponse SaveSecurityEntityList(SaveListRequest<SecurityEntity> request)
		{

			var response = new BusinessMessageResponse();

			try
			{
				foreach (SecurityEntity it in request.List)
				{
					response = GenericSaveEntity<SecurityEntity>("SecurityEntity", it.ID, it.EntityName, it.EntityType,
						it.LoginEnabled, it.LoginUsername, it.LoginPassword, it.CompanyID, it.Path, it.UserID,
						it.CRMId, it.Description, it.BranchID, it.LastLogonDate, it.Active,
						it.Deleted, it.TimeZone, it.Locale, it.EnableTimeZoneSelect, it.IsAdmin, it.Salt);

					//save permission list
					response = SaveSecurityPermissionList(it);

					// save the entities parents
					//SaveSecurityEntityParents(it.ID, it.ImmediateParentsIds);
					response = GenericDeleteEntityList<SecurityEntityParent>("SecurityEntityParent", it.ID);

					if (it.ImmediateParentsIds != null && it.ImmediateParentsIds.Count > 0)
					{ 				   
						SaveListRequest<SecurityEntityParent> parentRequest = new SaveListRequest<SecurityEntityParent>();
						parentRequest.List = new List<SecurityEntityParent>();

						foreach (Guid parentID in it.ImmediateParentsIds)
						{
							SecurityEntityParent newParent = new SecurityEntityParent();

							newParent.RelationId = SequentialGuid.NewDbGuid();
							newParent.EntityId = it.ID;
							newParent.ParentId = parentID;
							parentRequest.List.Add(newParent);
						}
						response = SaveSecurityEntityParentList(parentRequest);
					}
				}
				response.Status = true;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}

			return response;
		} 
		#endregion

		#region Delete SecurityEntity
		

		public BusinessMessageResponse DeleteSecurityEntity(IDRequest request)
		{
			try
			{
				//delete all related securityEntityParent as well
				BusinessMessageResponse response = GenericDeleteEntityList<SecurityEntityParent>("SecurityEntityParent", new object[] { request.ID });
				response = GenericDeleteEntity<SecurityEntity>("SecurityEntity", new object[] { request.ID });
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		} 
		#endregion

		#region GetSecurityEntityUpdateCount
		

		public GetUpdateCountResponse GetSecurityEntityUpdateCount(GetUpdateCountRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();
			try
			{
				response = GenericGetEntityUpdateCount<SecurityEntity>(request.TimeStamp, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
			return response;
		}
		#endregion

		#region GetSecurityEntityListByTimeStamp
		

		public GetListResponse<SecurityEntity> GetSecurityEntityListByTimeStamp(GetListByTimestampRequest request)
		{
			var response = new GetListResponse<SecurityEntity>();
			response.List = new List<SecurityEntity>();

			try
			{
				response = GenericGetEntityListByTimestamp<SecurityEntity>(request.TimeStamp, request.Cap, true, request.IncludeInactive, request.ID, request.LastRecordID);
				if (response != null && response.List != null)
				{
					Guid applicationID;
					request.Get<Guid>("appid", out applicationID);
					foreach (SecurityEntity entity in response.List)
					{
						//FILL in permission list
						entity.PermissionList = GetSecurityPermissionList(applicationID, entity);

						// fill in the parent id list
						entity.ImmediateParentsIds = GetEntityRelationships(entity.ID);
					}
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityEntity>>(ex);
			}

			return response;
		}
		#endregion

		#region private methods
		/// <summary>
		/// Get SecurityEntries based on entityID
		/// </summary>
		/// <param name="responseEntity"></param>
		private List<SecurityPermission> GetSecurityPermissionList(Guid applicationID, SecurityEntity entity)
		{
			List<SecurityPermission> permissions = new List<SecurityPermission>();

			if (entity != null)
			{
				GetPermissions(applicationID, entity.ID, ref permissions);

				if (entity.ImmediateParentsIds != null)
				{
					foreach (Guid parentID in entity.ImmediateParentsIds)
					{
						GetPermissions(applicationID, parentID, ref permissions);
					}
				}
			}

			return permissions;
		}

		private void GetPermissions(Guid applicationID, Guid entityID, ref List<SecurityPermission> permissions)
		{
			if (permissions != null)
			{
				// get the list of all SecurityEntries belong to this entity
				var response = new GetListResponse<SecurityEntry>();
				response.List = new List<SecurityEntry>();

				IDRequest entityReq = new IDRequest(entityID, "appid", applicationID);
				response = GetEntitySecurityEntryList(entityReq);

				if (response != null && response.List != null)
				{
					foreach (SecurityEntry se in response.List)
					{
						// get just the securityObjectID and corresponding permissionGranted
						SecurityPermission permission = new SecurityPermission();
						permission.AssignFromEntry(se);
						if (!permissions.Contains(permission))
						{
							permissions.Add(permission);
						}
					}
				}
			}
		}

		/// <summary>
		/// Save SecurityEntries based on entityID
		/// </summary>
		/// <param name="responseEntity"></param>
		private BusinessMessageResponse SaveSecurityPermissionList(SecurityEntity responseEntity)
		{
			var response = new BusinessMessageResponse();

			if (responseEntity.PermissionList != null && responseEntity.PermissionList.Count > 0)
			{
				try 
				{		 
					//delete all existing ones first
					response = this.DeleteEntitySecurityEntry(new IDRequest(responseEntity.ID));

					SaveListRequest<SecurityEntry> request = new SaveListRequest<SecurityEntry>();
					request.List = new List<SecurityEntry>();
					foreach (SecurityPermission sp in responseEntity.PermissionList)
					{
						SecurityEntry entry = sp.AssignToEntry();
						entry.EntityID = responseEntity.ID;
						request.List.Add(entry);
					}

					response = this.SaveSecurityEntryList(request);
				}
				catch (Exception ex)
				{
				return ErrorHandler.Handle<BusinessMessageResponse>(ex);
			}
				
			}
			return response;
		}


		/// <summary>
		/// Split off into another method for readability. This method, will get a list
		/// of the immediate parents of an entity.
		/// </summary>
		/// <param name="request">The id if the security entity to add the parent 
		/// information to.</param>
		protected List<Guid> GetEntityRelationships(Guid responseEntityID)
		{

			// get the list of all the parent ids
			var db = ImardaDatabase.CreateDatabase(Util.GetConnName<SecurityEntity>());

			using (IDataReader dr = db.ExecuteDataReader("SPGetSecurityEntityParentsList", responseEntityID))
			{
				List<Guid> ids = new List<Guid>();
				while (dr.Read())
				{
					// get just the entities ids
					ids.Add((Guid) dr["ParentId"]);
				}
				return ids;
			}
		}
		#endregion
	}
}
