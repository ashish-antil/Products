#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

#endregion

namespace FernServiceBase
{
	public interface IRestartableHost
	{
		ServiceHost Host { get; }
		void Start();
		void Stop();
		Type ServiceType { get; }
	}

	/// <summary>
	/// A base for Windows Services that require database connectivity,
	/// and operate a Service Contract.
	/// On start, it will check event log and database connectivity.
	/// If either of these fail, the service will stop.
	/// Next it will run updates on the Database.
	/// Finally, it will start the <see cref="ServiceHost"/> that hosts
	/// the Service Contract.
	/// </summary>
	public abstract class FernServiceBase : System.ServiceProcess.ServiceBase
	{
		protected readonly Dictionary<Type, IRestartableHost> _HostList;

	    protected FernServiceBase(string windowsServiceName)
		{
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			ServiceName = windowsServiceName;
			_HostList = new Dictionary<Type, IRestartableHost>();
        }

	    protected override void Dispose(bool disposing)
	    {
            base.Dispose(disposing);
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var logText = new StringBuilder();
                logText.AppendFormat("{0} | Unhandled Exception in AppDomain" + Environment.NewLine, ServiceName);
                if (e.IsTerminating)
                {
                    logText.Append("IsTerminating: true" + Environment.NewLine);
                }
                var ex = e.ExceptionObject as Exception;
                if (ex != null)
                {
                    logText.AppendFormat("e.ExceptionObject: {0}" + Environment.NewLine, ex);
                }
                EventLog.WriteEntry(logText.ToString(), EventLogEntryType.Error);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // don't throw an exception in the exception handler
            }
        }

	    /// <summary>
	    ///     Called when the Windows Service is started.
	    /// </summary>
	    /// <param name="args"></param>
	    protected override void OnStart(string[] args)
		{
			StartHosts();
		}

		/// <summary>
		/// Called when the Windows Service is stopped
		/// </summary>
		protected override void OnStop()
		{
			StopHosts();
		}

		public void RegisterHost(IRestartableHost safeHost)
		{
			try 
			{
				if (_HostList.ContainsKey(safeHost.ServiceType))
				{
					throw new ArgumentException("Host for " + safeHost.ServiceType + " already registered");
				}
				_HostList.Add(safeHost.ServiceType, safeHost);
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry(ex.ToString());
				ExitCode = 8408; // Internal error
				Stop();
			}
		}

		public void StartHosts()
		{
			try
			{
				//if (!TestEventLog()) return; // not necessary

				foreach (IRestartableHost serviceHost in _HostList.Values)
				{
					serviceHost.Start();
					EventLog.WriteEntry(ServiceName + ": started");
				}
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry(ex.ToString());
			}
		}

		public void StopHosts()
		{
			try
			{
				foreach (IRestartableHost serviceHost in _HostList.Values)
				{
					serviceHost.Stop();
					EventLog.WriteEntry(ServiceName + ": stopped");
				}
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry(ex.ToString());
			}
		}


		/// <summary>
		/// Checks that the event log can be written to. A failure here should be
		/// a fatal error.
		/// </summary>
		private bool TestEventLog()
		{
			try
			{
				EventLog.WriteEntry(ServiceName + ": can write to event log");
				return true;
			}
			catch (Exception)
			{
				// Code 1501 = "Log could not be opened"
				ExitCode = 1501;
				Stop();
				return false;
			}
		}

	}

	/// <summary>
	/// Windows Service with database connectivity.
	/// </summary>
	public abstract class FernBusinessServiceBase : FernServiceBase
	{
		/// <summary>
		/// Constructs a new FernServiceBase object.
		/// </summary>
		protected FernBusinessServiceBase(string windowsServiceName) 
            : base(windowsServiceName)
		{
		}

		/// <summary>
		/// Updates the Database through the given <see cref="SqlConnection"/>.
		/// If unsuccessful, should throw some kind of Exception.
		/// </summary>
		/// <param name="conn">
		/// The Connection to use. Doesn't need to be opened, it will already be when
		/// this method is called.
		/// </param>
		protected abstract void InternalUpdateDatabase(SqlConnection conn);


		/// <summary>
		/// Will parse the given SQL Script into blocks, separated by "GO"
		/// statements. Will execute each block.
		/// </summary>
		/// <param name="conn">
		/// The Connection to use. Should already be opened when passed in here.
		/// </param>
		/// <param name="allSql">
		/// The entire series of SQL statements to execute, each one separated by
		/// a "GO" statement.
		/// </param>
		protected void ExecuteScript(SqlConnection conn, string allSql)
		{
			string[] sqlCommands = allSql.Split(new [] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandType = CommandType.Text;
			foreach (string sql in sqlCommands)
			{
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery();
			}
		}
	}
}