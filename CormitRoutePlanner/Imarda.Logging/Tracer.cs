using System.Text;
using Imarda.Lib;
using System;
using System.Collections;
using System.Diagnostics;

namespace Imarda.Logging
{
	/*
	 * How to Use the Tracer
	 * 
	 * Add a custom value to TraceLogs enum if you want to generate a specific log file
	 * 
	 * Set the config keys: 
	 * TraceEnabled = 1 enables tracing
	 * TraceOpenLogs = 1 will automatically open the newly created log in default text app
	 * 
	 * In the code:
	 * 
	 * 			#region Trace
	 * #if DEBUG
	 * 			ErrorLogger.TraceFormat(TraceLogs.MyTrace, "Some stuff: {0}", tracedObject);
	 * 			ErrorLogger.Trace(TraceLogs.MyTrace, tracedObject);
	 * #endif
	 * 			#endregion Trace
	 * 			
	 * The call to Trace is expected to be within #if DEBUG/#endif
	 * 
	 * tracedObject if not a value will use DumpT extension, ToString otherwise
	 * 
	*/

	public enum TraceLogs
	{
		BaseTrace,
		ApnTrace01,
		TrlLnkg01,
	}

	public class Tracer
	{
		private const string TraceOpenLogs = "TraceOpenLogs";
		private const string TraceEnabled = "TraceEnabled";

		private readonly Hashtable _traceLogs;
		private bool? _isEnabled;

		public Tracer()
		{
			_traceLogs = new Hashtable();
		}

		public bool IsEnabled
		{
			get
			{
				if (null == _isEnabled)
				{
					_isEnabled = ConfigUtils.GetFlag(TraceEnabled);
				}
				return _isEnabled.Value;
			}
		}

		public ErrorLogger GetLog(TraceLogs logId)
		{
			var log = _traceLogs[logId];
			if (null == log)
			{
				log = ErrorLogger.GetLogger("_trace_" + logId);
				_traceLogs.Add(logId, log);

				var logger = (ErrorLogger)log;
				logger.InfoFormat("Trace Logs Enabled: {0}", IsEnabled);

				var openTraceLogs = ConfigUtils.GetFlag(TraceOpenLogs);
				logger.InfoFormat("Trace Logs Automatically Opened: {0}", openTraceLogs);

				logger.InfoFormat("Current Trace: {0}", Enum.GetName(typeof(TraceLogs), logId));

				if (openTraceLogs)
				{
					Process.Start(logger.FilePath);
				}
			}
			return log as ErrorLogger;
		}

		public void Trace(object traced)
		{
			_Trace(0, traced, null);
		}

		public void Trace(TraceLogs logId, object traced)
		{
			_Trace(logId, traced, null);
		}

		public void TraceFormat(string s, object traced)
		{
			_Trace(0, traced, s);
		}

		public void TraceFormat(TraceLogs logId, string s, object traced)
		{
			_Trace(logId, traced,s);
		}

		public static string GetCaller(bool shortName = true)
		{
			var frame = new StackFrame(2); //must take into account intermediate callers to get to calling frame
			var method = frame.GetMethod();
			var type = method.DeclaringType;
			var name = method.Name;
			return shortName ? name : string.Format("{0}.{1}", type, name);
		}


		public static string GetCallerExt(bool shortName = true)
		{
			var frame = new StackFrame(2,true); //must take into account intermediate callers to get to calling frame
			var method = frame.GetMethod();
			var type = method.DeclaringType;
			var name = method.Name;
			var resultName = shortName ? name : string.Format("{0}.{1}", type, name);
			return string.Format("{0} at line {1}", resultName, frame.GetFileLineNumber());
		}

		private void _Trace(TraceLogs logId, object traced, string s)
		{
			if(!IsEnabled)
			{
				return;
			}

			var log = GetLog(logId);

			if (log.MaxLevel < 4)
			{
				return;
			}

			var frame = new StackFrame(3); //must take into account intermediate callers to get to calling frame
			var method = frame.GetMethod();
			var type = method.DeclaringType;
			var name = method.Name;

#if !DEBUG
			log.WarnFormat("{0}.{1} # {2}", type, name, "Trace Call outside of DEBUG block!");
#endif

			var sTraced = (traced is ValueType || traced is string) ? traced.ToString():traced.DumpT();

			if (null == s)
			{
				var s1 = string.Format("{0}.{1} # {2}", type, name, sTraced);
				log.Debug(s1);
				return;
			}

			var s2 = string.Format("{0}.{1} # {2}", type, name, s);
			log.Log(4, s2, sTraced);

		}
	}
}