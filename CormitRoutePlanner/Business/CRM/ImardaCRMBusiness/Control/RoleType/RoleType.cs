/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\adamson.delacruz
Date Generated : 27/04/2009 1:27 PM
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{

		#region Get RoleType
		public GetItemResponse<RoleType> GetRoleType(IDRequest request)
		{
			try
			{
				return GenericGetEntity<RoleType>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<RoleType>>(ex);
			}
		}
		#endregion
		#region GetRoleTypeUpdateCount
		

		public GetUpdateCountResponse GetRoleTypeUpdateCount(GetUpdateCountRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();

			try
			{
				response = GenericGetEntityUpdateCount<RoleType>("RoleType", request.TimeStamp, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}

			return response;
		}
		#endregion
		#region GetRoleTypeListByTimeStamp
		

		public GetListResponse<RoleType> GetRoleTypeListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<RoleType>("RoleType", request.TimeStamp, request.Cap, true, request.IncludeInactive, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<RoleType>>(ex);
			}
		}
		#endregion
		#region GetRoleTypeList
		

		public GetListResponse<RoleType> GetRoleTypeList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<RoleType>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<RoleType>>(ex);
			}
		}
		#endregion
		#region Save RoleType
		public BusinessMessageResponse SaveRoleType(SaveRequest<RoleType> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				RoleType entity = request.Item;
				BaseEntity.ValidateThrow(entity);
				 			   
				object [] properties=new object[]{			entity.ID,
			entity.Name,
			entity.Description
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				response = GenericSaveEntity<RoleType>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
		#endregion
		#region SaveRoleTypeList
		

		public BusinessMessageResponse SaveRoleTypeList(SaveListRequest<RoleType> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (RoleType entity in request.List)
				{
					BaseEntity.ValidateThrow(entity);
					object [] properties=new object[]
					{
			entity.ID,
			entity.Name,
			entity.Description
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<RoleType>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747 				   
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
		#endregion
		#region Delete RoleType
		

		public BusinessMessageResponse DeleteRoleType(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<RoleType>("RoleType", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
	}
}


