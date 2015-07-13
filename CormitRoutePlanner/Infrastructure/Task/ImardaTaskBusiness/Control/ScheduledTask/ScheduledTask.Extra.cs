using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using System.Data;
using System.Globalization;
using Imarda.Logging;
using ConfigUtils = Imarda.Lib.ConfigUtils;
using Execute = Imarda.Lib.Execute;


namespace ImardaTaskBusiness
{
	partial class ImardaTask
	{
		public GetListResponse<ScheduledTask> GetScheduledTaskListForProcessing(GenericRequest request)
		{
			try
			{
				byte managerID = (byte)request[0];
				int topN = (int)request[1];
				return ImardaDatabase.GetList<ScheduledTask>("SPGetScheduledTaskListForProcessing", managerID, topN);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ScheduledTask>>(ex);
			}
		}


		public BusinessMessageResponse RequeuePendingMessages(GenericRequest request)
		{
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ScheduledTask>());
				byte managerID = (byte)request[0];
				int n = db.ExecuteNonQuery("SPRequeuePendingMessages", managerID);
				Execute.Later(ConfigUtils.GetTimeSpan("SystemTaskDelay", TimeSpan.FromMinutes(1.0)), () => db.ExecuteNonQuery("SPReinstallSystemTasks"));
				return new BusinessMessageResponse { Status = true, StatusMessage = n.ToString(CultureInfo.InvariantCulture) };
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}

		}

		public BusinessMessageResponse SetScheduledTaskStatus(GenericRequest request)
		{
			var count = 1;
			int retry = ConfigUtils.GetInt("retry", 1); ;
			try
			{

				var brResponse = SetScheduledTaskStatus_mth(request);
				return brResponse;
			}
			catch (SqlException e)
			{
				while (count <= retry)
				{
					try
					{
						DebugLog.Write("Retry for scheduled task");
						var brResponse = SetScheduledTaskStatus_mth(request);
						return brResponse;
					}
					catch (Exception)
					{
						if (e.Number == -2 || e.Number == 1205)
							// if (e.Message == "SQL Timeout")
							count++;
						else
						{
							return ErrorHandler.Handle(e);
						}
					}
				}
				return ErrorHandler.Handle(e);
			}
		}

		public BusinessMessageResponse SetScheduledTaskStatus_mth(GenericRequest request)
		{
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ScheduledTask>());
				byte status = (byte)request[0];
				string queueID = request.Parameters.Length > 1 ? (string)request[1] : null;
				db.ExecuteNonQuery("SPSetScheduledTaskStatus", request.ID, status, queueID);

				//   throw new Exception("SQL Timeout");
				return new BusinessMessageResponse();

			}
			catch (SqlException)
			{
				throw;
			}
		}



		public GetListResponse<ScheduledTask> GetScheduledTaskListFiltered(ScheduledTaskListRequest request)
		{
			try
			{
				return ImardaDatabase.GetList<ScheduledTask>(
					"SPGetScheduledTaskListFiltered",
					request.IncludeInactive,
					request.CompanyID,
					request.OwnerID,
					request.TopN,
					request.StartTime,
					request.ManagerID,
					request.ProgramID,
					request.AnyStatus,
					request.IncludeNewAndQueued,
					request.IncludeSuccessful,
					request.IncludeFailed,
					request.IncludeCancelled
				);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ScheduledTask>>(ex);
			}
		}

		#region SQL Build Helpers
		//request.AnyStatus = false;
		//request.IncludeNewAndQueued = true;
		//request.IncludeSuccessful = false;
		//request.IncludeFailed = true;		
		//request.IncludeCancelled = false;		
		//request.IncludeInactive = false;
		private string GetScheduledReportTaskInnerSelectSQL(int scope, Guid companyID)
		{ 
			var innerSelect = string.Format(
				"SELECT s.*, p.FullName AS Owner, CASE WHEN p.Active = 1 AND p.Deleted = 0 THEN 1 ELSE 0 END AS OwnerEnabled, c.Name AS Company, " +
				"(SELECT CASE WHEN CHARINDEX('<Desc>', s.Arguments) > 0 THEN SUBSTRING(s.Arguments, CHARINDEX('<Desc>', s.Arguments)+6, (CHARINDEX('<', s.Arguments, CHARINDEX('<Desc>', s.Arguments)+6)) - (CHARINDEX('<Desc>', s.Arguments)+6)) ELSE '' END ) AS Description, " +
				"(SELECT CASE WHEN CHARINDEX('<Recipients>', s.Arguments) > 0 THEN SUBSTRING(s.Arguments, CHARINDEX('<Recipients>', s.Arguments)+12, (CHARINDEX('<', s.Arguments, CHARINDEX('<Recipients>', s.Arguments)+12)) - (CHARINDEX('<Recipients>', s.Arguments)+12)) ELSE '' END ) AS Recipients " +
				"FROM [ScheduledTask] s WITH (nolock) " +
				"LEFT JOIN [Imarda360.CRM].dbo.Person p ON p.ID = s.OwnerID " +
				"LEFT JOIN [Imarda360.CRM].dbo.Company c ON c.ID = s.CompanyID " +
				"WHERE " +
				"( ({0} = 0 AND s.CompanyID = '{1}') " +
					"OR " +
					"({0} = 1 ) " + // to consider if any further restriction is required here
					"OR " +
					"({0} = 2 ) " + // to consider if any further restriction is required here
				") " +
				"AND s.CompanyID <> '11111111-1111-1111-1111-111111111111' " +
				"AND ManagerID = 1 " +  //TaskManagerParameters.ManagerID
				"AND ProgramID = 1 " +  //Programs.ReportHandler,
				"AND s.Deleted = 0 " +
				"AND s.Active = 1 " +
				"AND (Status = 0 OR Status=1 OR Status=4) " //queued and failed   
			, scope, companyID.ToString());

			//AND OwnerID=@ownerid  
			//AND StartTime >= @starttime  
 
			return innerSelect;
		}

		private string GetScheduledReportTaskCountInnerSelectSQL(int scope, Guid companyID)
		{
			var innerSelect = string.Format(
				"SELECT s.*, " +
				"(SELECT CASE WHEN CHARINDEX('<Desc>', s.Arguments) > 0 THEN SUBSTRING(s.Arguments, CHARINDEX('<Desc>', s.Arguments)+6, (CHARINDEX('<', s.Arguments, CHARINDEX('<Desc>', s.Arguments)+6)) - (CHARINDEX('<Desc>', s.Arguments)+6)) ELSE '' END ) AS Description " +
				"FROM [ScheduledTask] s WITH (nolock) " +
				"WHERE " +
				"( ({0} = 0 AND s.CompanyID = '{1}') " +
					"OR " +
					"({0} = 1 ) " + // to consider if any further restriction is required here
					"OR " +
					"({0} = 2 ) " + // to consider if any further restriction is required here
				") " +
				"AND s.CompanyID <> '11111111-1111-1111-1111-111111111111' " +
				"AND ManagerID = 1 " +  //TaskManagerParameters.ManagerID
				"AND ProgramID = 1 " +  //Programs.ReportHandler,
				"AND s.Deleted = 0 " +
				"AND s.Active = 1 " +				
				"AND (Status = 0 OR Status=1 OR Status=4) " //queued and failed   
			, scope, companyID.ToString());

			//AND OwnerID=@ownerid  
			//AND StartTime >= @starttime  

			return innerSelect;
		}

		#endregion

		public GetListResponse<ScheduledTask> GetScheduledReportTaskExtent(GetFilteredExtentRequest request)
		{
			var scope = 0;
			if (request.Scope.HasValue)
				scope = request.Scope.Value;

			//for reports from group - not used currently
			//var groupID = request.OwnerID.HasValue ? request.OwnerID.Value : Guid.Empty;

			var innerSelect = GetScheduledReportTaskInnerSelectSQL(scope, request.CompanyID);

			try
			{
				var response = GenericGetExtentWithCustomSelect<ScheduledTask>(
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
				return ErrorHandler.Handle<GetListResponse<ScheduledTask>>(ex);
			}
		}

		public SimpleResponse<int> GetScheduledReportTaskExtentCount(GetFilteredExtentRequest request)
		{
			var scope = 0;
			if (request.Scope.HasValue)
				scope = request.Scope.Value;

			//for reports from group - not used currently
			//var groupID = request.OwnerID.HasValue ? request.OwnerID.Value : Guid.Empty;

			var innerSelect = GetScheduledReportTaskCountInnerSelectSQL(scope, request.CompanyID);

			try
			{
				var response = GenericGetExtentWithCustomSelectCount<ScheduledTask>(
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

		public BusinessMessageResponse UpdateScheduledTaskOwnerID(SaveRequest<ScheduledTask> request)
		{
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ScheduledTask>());
				db.ExecuteNonQuery("SPUpdateScheduledTaskOwnerID", request.Item.ID, request.Item.OwnerID);
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}