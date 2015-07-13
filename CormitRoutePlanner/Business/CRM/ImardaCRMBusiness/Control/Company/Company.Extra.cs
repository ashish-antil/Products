using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{

		#region GetCompanyExtent + GetCompanyExtentCount
		private string GetCompanyInnerSelectSQL(int scope, Guid companyID)
		{
			//[16/1/2014] "the number in the main page must match the sum of active and inactive vehicles in the company.vehicle page"
			var innerSelect = string.Format(
				"SELECT  c.*, " +				
				"(SELECT COUNT(*) " +
				"FROM  [Imarda360.VehicleManagement].dbo.Vehicle v WITH (nolock) " +
				"WHERE v.CompanyID = c.ID AND v.Deleted = 0 " + //AND v.Active = 1
				") AS NrVehicles, " +				
				"(SELECT COUNT(*) " +
				"FROM  [Imarda360.VehicleManagement].dbo.Fleet f WITH (nolock) " +
				"WHERE f.CompanyID = c.ID AND f.Active = 1 AND f.Deleted = 0 " +
				") AS NrFleets, " +				
				"(SELECT  COUNT(*) " +
				"FROM Person p WITH (nolock) " +
				"INNER JOIN [Imarda360.Security].[dbo].[SecurityEntity] se WITH (nolock) ON se.CRMId=p.ID " +
				"WHERE p.CompanyID= c.ID AND p.Active = 1 AND p.Deleted = 0 AND se.Deleted = 0 " +
				"AND (se.IsTemplate = 0 OR se.IsTemplate IS NULL) " +
				"AND p.CompanyID <> '11111111-1111-1111-1111-111111111111' " + 
				") AS NrUsers, " +				
				"(SELECT Name " +
				"FROM    [Imarda360.CRM].dbo.Company o WITH (nolock)" +
				"WHERE   o.ID = c.CompanyID " + 
				") AS Owner " +
				"FROM [Company] c " +
				"WHERE " +
				"(  ({0} = 0 AND (c.CompanyID = '{1}' OR SUBSTRING(c.Path, LEN(c.Path)-35, 36) = '{1}') ) " + // owned or self
					"OR " +
					"({0} = 1 AND (c.CompanyID = '{1}' OR SUBSTRING(c.Path, LEN(c.Path)-35, 36) = '{1}') ) " + // owned or self
					"OR " +
					"({0} = 2 AND (c.Path LIKE '%{1}%')) " +
				") " +
				"AND c.CompanyID <> '11111111-1111-1111-1111-111111111111' " +
				"AND c.Deleted = 0 ", scope, companyID.ToString());
			return innerSelect;
		}

		private string GetCompanyCountInnerSelectSQL(int scope, Guid companyID)
		{
			//[16/1/2014] "the number in the main page must match the sum of active and inactive vehicles in the company.vehicle page"
			var innerSelect = string.Format(
				"SELECT  c.*, " +
				"(SELECT Name " +
				"FROM    [Imarda360.CRM].dbo.Company o WITH (nolock)" +
				"WHERE   o.ID = c.CompanyID " +
				") AS Owner " +
				"FROM [Company] c " +
				"WHERE " +
				"(  ({0} = 0 AND (c.CompanyID = '{1}' OR SUBSTRING(c.Path, LEN(c.Path)-35, 36) = '{1}') ) " + // owned or self
					"OR " +
					"({0} = 1 AND (c.CompanyID = '{1}' OR SUBSTRING(c.Path, LEN(c.Path)-35, 36) = '{1}') ) " + // owned or self
					"OR " +
					"({0} = 2 AND (c.Path LIKE '%{1}%')) " +
				") " +
				"AND c.CompanyID <> '11111111-1111-1111-1111-111111111111' " +
				"AND c.Deleted = 0 ", scope, companyID.ToString());
			return innerSelect;
		}

		public GetListResponse<Company> GetCompanyExtent(GetFilteredExtentRequest request)
		{
			try
			{
				var scope = 0;
				if (request.Scope.HasValue)
					scope = request.Scope.Value;

				var innerSelect = GetCompanyInnerSelectSQL(scope, request.CompanyID);
				try
				{
					var response = GenericGetExtentWithCustomSelect<Company>(
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
					return ErrorHandler.Handle<GetListResponse<Company>>(ex);
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Company>>(ex);
			}
		}

		public SimpleResponse<int> GetCompanyExtentCount(GetFilteredExtentRequest request)
		{
			try
			{
				var scope = 0;
				if (request.Scope.HasValue)
					scope = request.Scope.Value;

				var innerSelect = GetCompanyCountInnerSelectSQL(scope, request.CompanyID);
				try
				{
					var response = GenericGetExtentWithCustomSelectCount<Company>(
								request.CompanyID,
								request.CreatedAfter, request.CreatedBefore,
								request.ModifiedAfter, request.ModifiedBefore,
								request.Deleted, request.Active, request.Template, request.Path,
								null, null,
								innerSelect, request.Conditions, request.LogicalOperator);

					return response;
				}
				catch (Exception ex)
				{
					return ErrorHandler.Handle<SimpleResponse<int>>(ex);
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SimpleResponse<int>>(ex);
			}
		}
		#endregion

        public GetListResponse<Company> GetCompanyListManagedBy(IDRequest request)
		{
			try
			{
                var response = new GetListResponse<Company>();
                var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Company>());

                bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
                //int topn = request.Get<int>("TopN", int.MaxValue);
                using (IDataReader dr = db.ExecuteDataReader("SPGetCompaniesManagedBy", includeInactive, request.ID))
                {
                    response.List = new List<Company>();
                    while (dr.Read())
                    {
                        response.List.Add(GetFromData<Company>(dr));
                    }

                    return response;
                }

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Company>>(ex);
			}
		}

		public GetListResponse<CustomerInfo> GetCustomerInfoList(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<CustomerInfo>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Company>());

				//bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				//int topn = request.Get<int>("TopN", int.MaxValue);
				using (IDataReader dr = db.ExecuteDataReader("SPGetCustomerInfo")) //, includeInactive, request.ID))
				{
					response.List = new List<CustomerInfo>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<CustomerInfo>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<CustomerInfo>>(ex);
			}
		}

		public GetUpdateCountResponse GetScopedCompanyCount(IDRequest request)
		{
			var response = new GetUpdateCountResponse();
			try
			{
				var scope = request["Scope"];
				if (string.IsNullOrEmpty(scope))
					scope = "0";
				try
				{
					var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Company>());
					using (IDataReader dr = db.ExecuteDataReader("SPGetScopedCompanyCount", request.ID, scope))
					{
						if (dr.Read()) response.Count = Convert.ToInt32(dr[0]);
					}
				}
				catch (Exception ex)
				{
					ErrorHandler.Handle<GetUpdateCountResponse>(ex);
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public SimpleResponse<bool> CheckIfUniqueForCompany(IDRequest request)
		{
			var response = new SimpleResponse<bool>();
			try
			{
				var fieldName =	request["FieldName"];
				var value = request["Value"];
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Company>());
				string query = string.Format("SELECT COUNT(*) FROM Company WHERE {0} = '{1}' AND ID <> '{2}'", fieldName, value, request.ID);
				using (IDataReader dr = db.ExecuteDataReader(CommandType.Text, query))
				{
					if (dr.Read() && Convert.ToInt32(dr[0]) > 0)
						response = new SimpleResponse<bool>(false);
					else
						response = new SimpleResponse<bool>(true);
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SimpleResponse<bool>>(ex);
			}
		}

	}
}