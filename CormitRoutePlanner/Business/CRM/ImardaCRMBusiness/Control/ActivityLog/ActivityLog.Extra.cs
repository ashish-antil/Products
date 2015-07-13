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

		#region SQL Build Helpers
		private string GetFilteredActivityLogExtentSQL(int scope, int startRow, int endRow, string whereClause, string sortClause)
		{
			var database = "CRM";
			var innerSelect = string.Format(
				"SELECT  h.*, p.FullName as UserName " +
				"FROM [ActivityLog] h " +
				"LEFT JOIN [Imarda360.CRM].dbo.Person p on p.ID = h.UserID " +
				"WHERE h.Deleted = 0 ");

			if (!string.IsNullOrEmpty(whereClause))
				innerSelect = string.Format("SELECT * FROM ({0}) AS AllData WHERE ({1}) ", innerSelect, whereClause);

			var sb = new StringBuilder(string.Format("USE [Imarda360.{0}]; ", database));
			sb.Append("SELECT numberedData.* FROM (");
			sb.Append(string.Format("SELECT InnerSelect.*, ROW_NUMBER() OVER (ORDER BY {0} ) AS RowNum FROM ( {1} ) AS InnerSelect ", sortClause, innerSelect));
			sb.Append(") AS numberedData");
			sb.Append(string.Format(" WHERE numberedData.RowNum BETWEEN {0} AND {1}", startRow, endRow));

			return sb.ToString();
		}


		#endregion

		public GetListResponse<ActivityLogEntry> GetActivityLogExtent(GetFilteredExtentRequest request)
		{
			try
			{
				var response = new GetListResponse<ActivityLogEntry>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ActivityLogEntry>());

				Guid? companyID = request.CompanyID;
				DateTime? createdAfter = request.ModifiedAfter;
				DateTime? createdBefore = request.ModifiedBefore;
				bool? deleted = request.Deleted;
				bool? active = request.Active;
				Guid? ownerID = request.OwnerID;
				int? ownerType = request.OwnerType;

				var scope = 0;
				if (request.Scope.HasValue)
					scope = request.Scope.Value;

				int startNum = 1;
				int endNum = Int32.MaxValue;
				if (request.Offset.HasValue)
				{
					startNum = request.Offset.Value;
					if (startNum < 1) startNum = 1;
					if (request.Limit.HasValue && request.Limit.Value > 0)
						endNum = startNum + request.Limit.Value - 1;
				}

				var whereClause = "";
				var con = "";
				//these are 'AND'd together
				string[] filters =
				{
					companyID.HasValue && companyID.Value != Guid.Empty ? string.Format(" [CompanyID] = '{0}' ", companyID.Value.ToString()) : null,
					createdAfter.HasValue ? string.Format(" [DateCreated] >= '{0}' ", createdAfter.Value.ToString("s")) : null,
					createdBefore.HasValue ? string.Format(" [DateCreated] <= '{0}' ", createdBefore.Value.ToString("s")) : null,
					deleted.HasValue ? " [Deleted] = " + (deleted.Value ? '1' : '0') : null,
					active.HasValue ? " [Active] = " + (active.Value ? '1' : '0') : null,
					ownerID.HasValue ? string.Format(" [OwnerID] = '{0}' ", ownerID.ToString()) : null,
					ownerType.HasValue ? string.Format(" [OwnerType] = '{0}' ", ownerType.ToString()) : null,
				};
				var filterClause = new StringBuilder();
				con = "";
				foreach (var filter in filters)
				{
					if (filter != null)
					{
						filterClause.Append(con).Append(filter);
						con = " AND ";
					}
				}
				var conditionClause = new StringBuilder();
				con = "";
				//add any further conditions these ar OR'd together	currently
				foreach (Condition condition in request.Conditions)
				{
					var part = condition.GetSQL();
					conditionClause.Append(con).Append(part);
					con = " OR ";
				}
				if (conditionClause.Length > 0)
					filterClause.Append(string.Format(" OR ( {0} )", conditionClause.ToString()));
				whereClause = filterClause.ToString();

				var sortClause = "";
				con = "";
				foreach (SortColumn sortColumn in request.SortColumns)
				{
					var part = sortColumn.FieldName;
					sortClause += con + part;
					if (sortColumn.SortDescending) sortClause += " DESC";
					con = ", ";
				}
				if (string.IsNullOrEmpty(sortClause))
					sortClause = "ID"; //we need some column for the OVER part


				var query = GetFilteredActivityLogExtentSQL(scope, startNum, endNum, whereClause, sortClause);
				using (IDataReader dr = db.ExecuteDataReader(CommandType.Text, query))
				{
					response.List = new List<ActivityLogEntry>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<ActivityLogEntry>(dr));
					}
					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ActivityLogEntry>>(ex);
			}
		}

		public GetListResponse<ActivityLogEntry> GetActivityLogListByUserID(IDRequest request)
		{
			var response = new GetListResponse<ActivityLogEntry>();
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ActivityLogEntry>());
				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				var spName = "SPGetActivityLogListByUserID";
				using (IDataReader dr = db.ExecuteDataReader(spName, includeInactive, request.CompanyID, request.ID))
				{
					response.List = new List<ActivityLogEntry>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<ActivityLogEntry>(dr));
					}
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ActivityLogEntry>>(ex);
			}
		}

	}
}