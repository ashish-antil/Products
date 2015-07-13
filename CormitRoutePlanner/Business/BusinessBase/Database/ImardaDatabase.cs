using System;
using System.Data;
using System.Data.Common;
using Imarda.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Imarda.Lib;
using System.Diagnostics;
using FernBusinessBase.Errors;

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
	public sealed class ImardaDatabase
	{
		internal static readonly ErrorLogger Log;
		private static readonly bool _LogDuration;

		static ImardaDatabase()
		{
			if (Imarda.Lib.ConfigUtils.GetFlag("LogDbCalls")) Log = ErrorLogger.GetLogger("DbCalls");
            _LogDuration = ConfigUtils.GetFlag("LogDbCallDuration");
		}

		private readonly Database _Database;

		private ImardaDatabase(Database db)
		{
			_Database = db;
		}

		public static ImardaDatabase CreateDatabase(string name)
		{
			return new ImardaDatabase(DatabaseFactory.CreateDatabase(name));
		}

		public IDataReader ExecuteDataReader(string storedProcedureName, params object[] parameterValues)
		{
			FixDateTime(parameterValues);
			Stopwatch stopwatch = _LogDuration && Log != null ? Stopwatch.StartNew() : null;
			return new ImardaDataReader(_Database.ExecuteReader(storedProcedureName, parameterValues), storedProcedureName, stopwatch);
		}

		public IDataReader ExecuteDataReader(CommandType commandType, string commandText)
		{
			Stopwatch stopwatch = _LogDuration && Log != null ? Stopwatch.StartNew() : null;
			return new ImardaDataReader(_Database.ExecuteReader(commandType, commandText), commandText.Truncate(60), stopwatch);
		}

        public IDataReader ExecuteDataReader(DbCommand cmd)
        {
            var stopwatch = _LogDuration && Log != null ? Stopwatch.StartNew() : null;
            return new ImardaDataReader(_Database.ExecuteReader(cmd), cmd.CommandText.Truncate(60), stopwatch);
        }

		public DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues)
		{
			FixDateTime(parameterValues);
			return _Database.ExecuteDataSet(storedProcedureName, parameterValues);
		}

		public DataSet ExecuteDataSet(CommandType commandType, string commandText)
		{
			return _Database.ExecuteDataSet(commandType, commandText);
		}

		public int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues)
		{
			FixDateTime(parameterValues);
			return _Database.ExecuteNonQuery(storedProcedureName, parameterValues);
		}

		public int ExecuteNonQuery(CommandType commandType, string commandText)
		{
			return _Database.ExecuteNonQuery(commandType, commandText);
		}

		public object ExecuteScalar(string storedProcedureName, params object[] parameterValues)
		{
			FixDateTime(parameterValues);
			return _Database.ExecuteScalar(storedProcedureName, parameterValues);
		}

		public static SimpleResponse<TR> GetValue<TE, TR>(string storedProc, params object[] args)
			where TE : BusinessEntity, new()
		{
			try
			{
				FixDateTime(args);
				var db = CreateDatabase(Util.GetConnName<TE>());
				using (IDataReader dr = db.ExecuteDataReader(storedProc, args))
				{
					var response = new SimpleResponse<TR>();
					if (dr.Read())
					{
						object val = dr[0];
						response.Value = val != DBNull.Value ? (TR)val : default(TR);
					}
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SimpleResponse<TR>>(ex);
			}
		}

		public static GetItemResponse<T> GetItem<T>(string storedProc, params object[] args)
			where T : BusinessEntity, new()
		{
			try
			{
				FixDateTime(args);
				var db = CreateDatabase(Util.GetConnName<T>());
				using (IDataReader dr = db.ExecuteDataReader(storedProc, args))
				{
					var response = new GetItemResponse<T>();
					if (dr.Read())
					{
						var entity = new T();
						entity.AssignData(dr);
						response.Item = entity;
					}
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<T>>(ex);
			}
		}

		public static GetListResponse<T> GetList<T>(string storedProc, params object[] args)
			where T : BaseEntity, new()
		{
			try
			{
				FixDateTime(args);
				var db = CreateDatabase(Util.GetConnName<T>());
				using (IDataReader dr = db.ExecuteDataReader(storedProc, args))
				{
					var response = new GetListResponse<T>();
					while (dr.Read())
					{
						var entity = new T();
						entity.AssignData(dr);
						response.List.Add(entity);
					}
					return response;
				}
			}
			catch (Exception ex)
			{
                ErrorHandler.ErrorFormatStoredProc(storedProc, args, ex);
				return ErrorHandler.Handle<GetListResponse<T>>(ex);
			}
		}

	    public DbCommand GetDbCommand(string storedProc)
	    {
	        return _Database.GetStoredProcCommand(storedProc);
	    }

		private static void FixDateTime(object[] args)
		{
			if (args == null) return;
			for (var i = 0; i < args.Length; i++)
			{
				var arg = args[i];
				if (arg is DateTime)
				{
					args[i] = BusinessBase.ReadyDateForStorage((DateTime)arg); // may or may not get stored, but should be in SQL Server datetime range.
				}
			}
		}

		/*
		private static string FormatArgs(IEnumerable<object> args)
		{
			return string.Join(", ",
												 args.Select(
													o =>
													(o is string
														? '"' + (string)o + '"'
														: o is DateTime ? ((DateTime)o).ToString("s") : string.Format("{0}", o))).ToArray());
		}
		*/

	}
}
