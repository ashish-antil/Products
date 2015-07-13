using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Data.SqlClient;

namespace ImardaLoggingBusiness
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class ImardaLogging : IImardaLogging
	{
	    private const string LoggingDbConnectionString = "ImardaLoggingBusinessInterface";

        /// <summary>
        /// Saves logs to the database
        /// </summary>
        /// <param name="request"></param>
	    bool IImardaLogging.SaveLogToDatabase(Logging request)
        {
            int maxLogLevel = 0;
            bool canSaveLogToDatabase = false;

            if (ConfigurationManager.AppSettings["MaxLogLevel"] != null)
                maxLogLevel = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLogLevel"]);

            if (request.LogLevel <= maxLogLevel)
            {
                canSaveLogToDatabase = IsLoggingDatabaseOnline();
                if (canSaveLogToDatabase)
                {
                    var logEntry = new LogEntry();
                    logEntry.Priority = request.Priority;
                    logEntry.Severity = request.Severity;
                    logEntry.Title = request.Title;
                    logEntry.TimeStamp = DateTime.UtcNow;

                    var ipAddress =
                        Dns.GetHostEntry(Environment.MachineName).AddressList.Select(addr => addr.ToString()).ToArray();
                    if (ipAddress.Length > 0)
                    {
                        logEntry.MachineName = ipAddress[1];
                    }

                    logEntry.Message = request.Message;
                    logEntry.Categories.Add(request.Category);

                    int logId = ExecuteWriteLogStoredProcedure(logEntry, request.Category);
                    canSaveLogToDatabase = logId > 0;
                }
            }

            return canSaveLogToDatabase;
        }
       
        /// <summary>
        /// Write Log to the database (Stored proc is called manually, otherwise log message gets truncated at default 1500 chars)
        /// </summary>
        /// <param name="logEntry"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        private int ExecuteWriteLogStoredProcedure(LogEntry logEntry, string categoryName)
        {
            Database db = DatabaseFactory.CreateDatabase(LoggingDbConnectionString);
            DbCommand cmd = db.GetStoredProcCommand("WriteLog");

            db.AddInParameter(cmd, "EventID", DbType.Int32, 0);
            db.AddInParameter(cmd, "Priority", DbType.Int32, logEntry.Priority);
            db.AddParameter(cmd, "Severity", DbType.String, 32, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.Severity);
            db.AddParameter(cmd, "Title", DbType.String, 256, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.Title);
            db.AddInParameter(cmd, "Timestamp", DbType.DateTime, DateTime.UtcNow);
            db.AddParameter(cmd, "MachineName", DbType.String, 32, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.MachineName);
            db.AddParameter(cmd, "AppDomainName", DbType.String, 512, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.AppDomainName);
            db.AddParameter(cmd, "ProcessID", DbType.String, 256, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.ProcessId);
            db.AddParameter(cmd, "ProcessName", DbType.String, 512, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.ProcessName);
            db.AddParameter(cmd, "ThreadName", DbType.String, 512, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.ManagedThreadName);
            db.AddParameter(cmd, "Win32ThreadId", DbType.String, 128, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.Win32ThreadId);
            db.AddParameter(cmd, "Message", DbType.String, 8000, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, logEntry.Message);
            db.AddInParameter(cmd, "FormattedMessage", DbType.String, String.Empty);
            db.AddOutParameter(cmd, "LogId", DbType.Int32, 4);
            db.ExecuteNonQuery(cmd);

            int logId = Convert.ToInt32(cmd.Parameters[cmd.Parameters.Count - 1].Value, CultureInfo.InvariantCulture);
            AddCategory(categoryName, logId);

            return logId;
        }

        /// <summary>
        /// Add Category
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="logId"></param>
        private void AddCategory(string categoryName, int logId)
        {
            Database db = DatabaseFactory.CreateDatabase(LoggingDbConnectionString);
            DbCommand cmd = db.GetStoredProcCommand("AddCategory");

            db.AddParameter(cmd, "CategoryName", DbType.String, 64, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Default, categoryName);
            db.AddInParameter(cmd, "LogID", DbType.Int32, logId);
            db.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Checks if Imarda360.Logging database is online or not
        /// </summary>
        /// <returns></returns>
	    private bool IsLoggingDatabaseOnline()
        {
            bool isLoggingDbOnline = false;
           
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(Convert.ToString(ConfigurationManager.ConnectionStrings[LoggingDbConnectionString])))
                {
                    string sql = "SELECT COUNT(1) FROM master.dbo.sysdatabases WHERE [Name] = '" + sqlCon.Database + "'";

                    SqlCommand comm = new SqlCommand(sql, sqlCon);
                    sqlCon.Open();

                    isLoggingDbOnline = Convert.ToInt32(comm.ExecuteScalar()) > 0;
                }
            }
            catch (Exception)
            {
                isLoggingDbOnline = false;
            }

            return isLoggingDbOnline;
        }
    }
}

