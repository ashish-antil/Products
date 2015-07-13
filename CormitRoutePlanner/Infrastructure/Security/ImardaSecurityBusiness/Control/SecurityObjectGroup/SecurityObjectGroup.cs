using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		

		public GetListResponse<SecurityObjectGroup> GetSecurityObjectGroupList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<SecurityObjectGroup>("SecurityObjectGroup", true, request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityObjectGroup>>(ex);
			}
		}

		

		public BusinessMessageResponse SaveSecurityObjectGroupList(SaveListRequest<SecurityObjectGroup> request)
		{
			try
			{
				BusinessMessageResponse bmr = new BusinessMessageResponse();

				foreach (SecurityObjectGroup it in request.List)
				{
					bmr = GenericSaveEntity<SecurityObjectGroup>("SecurityObjectGroup", it.ID, 
						it.Name, it.Description, it.Visibility,
						it.CompanyID, it.UserID, it.Active, it.Deleted);
				}

				return bmr;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		

		public BusinessMessageResponse DeleteSecurityObjectGroup(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<SecurityObjectGroup>("SecurityObjectGroup", true, new object[] { request.ID });
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}
