using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		#region GetPersonExtent
		public GetListResponse<Person> OldGetPersonExtent(GetFilteredExtentRequest req)
		{
			try
			{
				var response = GenericGetExtent<Person>(
					req.CompanyID,
					req.CreatedAfter, req.CreatedBefore,
					req.ModifiedAfter, req.ModifiedBefore,
					req.Deleted, req.Active, req.Template, req.Path,
					req.Limit, req.Offset, req.SortColumns, req.OwnerID, req.OwnerType);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Person>>(ex);
			}
		}

		#region SQL Build Helpers
		private string OldGetUserInnerSelectSQL(int scope, Guid companyID, Guid fleetID)
		{
			//fleetJoin is used for getting fleet related users
			var fleetJoin = "";
			if (fleetID != Guid.Empty)
				fleetJoin = string.Format("INNER JOIN [Imarda360.VehicleManagement].[dbo].[UserFleet] uf WITH (nolock) ON uf.PersonID=p.ID AND uf.FleetID='{0}' ", fleetID);

			var innerSelect = string.Format(
				"SELECT  p.*, s.LoginUsername, s.LastLogonDate " +
				"FROM [Person] p " +
				"INNER JOIN [Imarda360.Security].[dbo].[SecurityEntity] s with (nolock) ON s.CRMId = p.ID " +
				"{2}" +
				"WHERE " +
				"( ({0} = 0 AND p.CompanyID = '{1}') " +
					"OR " +
					"({0} = 1 AND CHARINDEX('{1}', p.Path) = LEN(p.Path) - 72) " +
					"OR " +
					"({0} = 2 AND (p.Path LIKE '%{1}%' OR p.Path = '' OR p.Path IS NULL)) " +
				") " +
				"AND p.CompanyID <> '11111111-1111-1111-1111-111111111111' " +
				"AND p.Deleted = 0 " +
				"AND s.Deleted = 0 ", scope, companyID.ToString(), fleetJoin);

			return innerSelect;
		}

		private string GetUserInnerSelectSQL(int scope, Guid companyID, Guid fleetID)
		{
			//fleetJoin is used for getting fleet related users
			var fleetJoin = "";
			if (fleetID != Guid.Empty)
				fleetJoin = string.Format("INNER JOIN [Imarda360.VehicleManagement].[dbo].[UserFleet] uf WITH (nolock) ON uf.PersonID=p.ID AND uf.FleetID='{0}' ", fleetID);

			var innerSelect = string.Format(
				"SELECT  p.*, s.LoginUsername, s.LastLogonDate " +   //, s.IsAdmin, s.TimeZone, s.Culture " +
				"FROM [Person] p " +
				"INNER JOIN [Imarda360.Security].[dbo].[SecurityEntity] s with (nolock) ON s.CRMId = p.ID " +
				"{2}" +
				"WHERE " +
				"( ({0} = 0 AND p.CompanyID = '{1}') " +
					"OR " +
					"({0} = 1 AND CHARINDEX('{1}', p.Path) = LEN(p.Path) - 72) " +
					"OR " +
					"({0} = 2 AND (p.Path LIKE '%{1}%' OR p.Path = '' OR p.Path IS NULL)) " +
				") " +
				"AND p.CompanyID <> '11111111-1111-1111-1111-111111111111' " +
				"AND p.Deleted = 0 " +
				"AND s.Deleted = 0 ", scope, companyID.ToString(), fleetJoin);

			return innerSelect;
		}

		#endregion

		public GetListResponse<Person> GetPersonExtent(GetFilteredExtentRequest request)
		{
			var scope = 0;
			if (request.Scope.HasValue)
				scope = request.Scope.Value;

			//for users from fleet
			var fleetID = request.OwnerID.HasValue ? request.OwnerID.Value : Guid.Empty;

			var innerSelect = GetUserInnerSelectSQL(scope, request.CompanyID, fleetID);

			try
			{
				var response = GenericGetExtentWithCustomSelect<Person>(
							request.CompanyID,
							request.CreatedAfter, request.CreatedBefore,
							request.ModifiedAfter, request.ModifiedBefore,
							request.Deleted, request.Active, request.Template, request.Path,
							request.Limit, request.Offset, request.SortColumns,
							null, null,
							innerSelect, request.Conditions, request.LogicalOperator);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Person>>(ex);
			}
		}
		#endregion

		public GetListResponse<Person> GetPersonListByCompanyID(IDRequest request)
		{
			try
			{
				return GenericGetRelated<Person>("CompanyID", request.ID, false);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Person>>(ex);
			}
		}

		//& gs-102
		#region GetActiveUserCount and GetInactiveUserCount
		//counts persons that are active AND have a security entity
		public GetUpdateCountResponse GetActiveUserCount(IDRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();

			try
			{
				response = GenericGetEntityCount<Person>("ActiveUser", true, request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}

			return response;
		}

		//counts users that are inactive OR have an inactive security entity
		public GetUpdateCountResponse GetInactiveUserCount(IDRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();

			try
			{
				response = GenericGetEntityCount<Person>("InactiveUser", true, request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}

			return response;
		}
		#endregion
		//. gs-102

	}
}