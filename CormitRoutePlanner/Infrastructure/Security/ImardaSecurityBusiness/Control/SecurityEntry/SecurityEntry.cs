using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness {
	partial class ImardaSecurity {

		#region Get Entity scurityEntries
		public GetListResponse<SecurityEntry> GetEntitySecurityEntryList(IDRequest request)
		{
			try
			{
				Guid applicationID;
				request.Get<Guid>("appid", out applicationID);
				return GenericGetEntityList<SecurityEntry>("EntitySecurityEntry", true, applicationID, request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityEntry>>(ex);
			}
		}

		public GetListResponse<SecurityEntry> GetEntitySecurityEntryListForIac(IDRequest request)
		{
			try
			{
				Guid applicationID;
				request.Get<Guid>("appid", out applicationID);
				return GenericGetEntityList<SecurityEntry>("EntitySecurityEntryForIAC", true, applicationID, request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityEntry>>(ex);
			}
		}

		public GetListResponse<SecurityEntry> GetEntitySecurityEntryListForI360(IDRequest request)
		{
			try
			{
				Guid applicationID;
				request.Get<Guid>("appid", out applicationID);
				return GenericGetEntityList<SecurityEntry>("EntitySecurityEntryForI360", true, applicationID, request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityEntry>>(ex);
			}
		}
		#endregion

		#region Get All scurityEntries
		

		public GetListResponse<SecurityEntry> GetSecurityEntryList()
		{
			try
			{
				return GenericGetEntityList<SecurityEntry>(new Guid[0]);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityEntry>>(ex);
			}
		} 
		#endregion

		#region Save Security Entries
		

		public BusinessMessageResponse SaveSecurityEntryList(SaveListRequest<SecurityEntry> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (SecurityEntry it in request.List)
				{
					object[] properties = new object[]
					{
						it.ID,
						it.ApplicationID,
						it.EntityID,
						it.SecurityObjectID,
						it.PermissionsGranted,
						it.PermissionsDenied,
						it.EntryType,
						it.Active,
						it.Deleted
					};

					response = GenericSaveEntity<SecurityEntry>(properties);

				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}

			return response;
		} 
		#endregion

		#region Save Entity Security Entries


		////[MethodMarker(SaveSecurityEntryList)]
		//public BusinessMessageResponse SaveEntitySecurityEntryList(SetMapsForEntityRequest<SecurityEntry> request)
		//{
		//	//[EntryMarker(SaveSecurityEntryList)]
		//	var response = new BusinessMessageResponse();
		//	try
		//	{
		//		//remove all existing entries for this entity first
		//		IDRequest entityID = new IDRequest(request.OwnerID);
		//		response = DeleteEntitySecurityEntry(entityID);

		//		//save all new one
		//		foreach (SecurityEntry it in request.Maps)
		//		{
		//			object[] properties = new object[]
		//			{
		//				it.ID,
		//				it.ApplicationID,
		//				it.EntityID,
		//				it.SecurityObjectID,
		//				it.PermissionsGranted,
		//				it.PermissionsDenied,
		//				it.EntryType,
		//				it.Active,
		//				it.Deleted
		//			};
		//			response = GenericSaveEntity<SecurityEntry>(it.Attributes, properties);
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		return ErrorHandler.Handle(ex);
		//	}

		//	return response;
		//}
		#endregion

		#region Delete Security Entry

		public BusinessMessageResponse DeleteSecurityEntry(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<SecurityEntry>("SecurityEntry", new object[] { request.ID });
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		} 
		#endregion

		#region Delete Entity Security Entry
		

		public BusinessMessageResponse DeleteEntitySecurityEntry(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<SecurityEntry>("EntitySecurityEntry", new object[] { request.ID });
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
	}
}
