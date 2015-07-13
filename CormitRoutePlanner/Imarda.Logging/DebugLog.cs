using Imarda.Lib;
using System;
using System.Threading;

namespace Imarda.Logging
{
	public static class DebugLog
	{
		private static readonly ErrorLogger _Log;
		private static readonly Timer _Timer;
		private static volatile bool _Enabled;

		static DebugLog()
		{
			_Timer = new Timer(Init, null, TimeSpan.FromMinutes(3), TimeSpan.FromSeconds(90));
			var path = ConfigUtils.GetString("DebugLogFolder");
			if (path != null)
			{
				Init(null);
				_Log = ErrorLogger.GetLogger(path, "DebugLog");
				Filter = ConfigUtils.GetString("DebugLogFilter");
			}
		}

		static void Init(object arg)
		{
			bool enabled = ConfigUtils.GetFlag("DebugLogEnabled");
			if (enabled != _Enabled)
			{
				if (enabled)
				{
					_Enabled = true;
					Write("DebugLog Enabled");
				}
				else
				{
					Write("DebugLog Disabled");
					_Enabled = false;
				}
				
			}
		}


		public static string Filter;

		public static void Write(object arg)
		{
			if (!_Enabled || _Log == null) return;
			if (arg == null) _Log.Debug("null");
			else
			{
				try
				{
					string s = arg.ToString();
					if (Filter == null || s.Contains(Filter))
					{
						_Log.Debug(s);
					}
				}
				catch (Exception ex)
				{
					_Log.DebugFormat("Error {0} {1}", arg, ex.Message);
				}
			}
		}

		public static void Write(string fmt, params object[] args)
		{
			if (!_Enabled || _Log == null) return;
			try
			{
				string s = string.Format(fmt, args);
				if (Filter == null || s.Contains(Filter))
				{
					_Log.Debug(s);
				}
			}
			catch (Exception ex)
			{
				_Log.DebugFormat("Error {0} {1}", fmt, ex.Message);
			}
		}

		/// <summary>
		/// Write to log if the first arg evaluates to true. 
		/// </summary>
		/// <remarks>This lazy-evaluation is
		/// better for Conditional compilation, making sure we don't have to
		/// write application code that would not be optimized away, e.g. 
		/// if (cond) Log.Write(...) would still compile the condition even if the
		/// compiler would not generate Log.Write().
		/// </remarks>
		/// <param name="guard">lambda containing the condition</param>
		/// <param name="fmt"></param>
		/// <param name="args"></param>
		public static void Write(Func<bool> guard, string fmt, params object[] args)
		{
			if (_Enabled && _Log != null && guard()) Write(fmt, args);
		}
	}
}