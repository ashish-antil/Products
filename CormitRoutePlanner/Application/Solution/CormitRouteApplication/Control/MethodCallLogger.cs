
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FernBusinessBase;
using Imarda.Lib;
using System.Timers;
using Imarda.Logging;

namespace Imarda360Application
{
	internal class MethodCallLogger
	{
		/// <summary>
		/// -1 = no method entry or result logging at all, 0 = no method entry logging, but allow method result logging, 1..4 log entry info
		/// </summary>
		private static int _LogServiceCalls = -1;

		/// <summary>
		/// 0 = log method name and optionally duration (depends on LogCallDuration), 1..4 
		/// </summary>
		private static int _LogServiceResults;

		/// <summary>
		/// 0 = no call duration measured and printed,  1 = measure and print call duration if LogServiceResults > 0 and LogServiceCalls >= 0
		/// </summary>
		private static bool _LogCallDuration;


		private readonly Timer _Timer = new Timer(60 * 1000);

		private readonly ErrorLogger _Log;


		internal MethodCallLogger(ErrorLogger log)
		{
			_Log = log;
			_Timer.Elapsed += TimerOnElapsed;
			LoadSettings();
			_Timer.Enabled = true;
		}

		private static void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			LoadSettings();
		}

		private static void LoadSettings()
		{
            ConfigUtils.RefreshAppSettings();
            _LogServiceCalls = ConfigUtils.GetInt("LogServiceCalls", -1);
            _LogServiceResults = ConfigUtils.GetInt("LogServiceResults", 0);
            _LogCallDuration = ConfigUtils.GetFlag("LogCallDuration");
		}

		private class CallInfo
		{
			internal readonly string MethodName;
			internal readonly Stopwatch Stopwatch;

			internal CallInfo(string name)
			{
				MethodName = name;
				if (_LogCallDuration) Stopwatch = Stopwatch.StartNew();
			}
		}

		internal void LogCall(string name, IRequestBase request)
		{
			// unfortunately we cannot return the CallInfo object
			// the [Conditional] of this Log method requires void return type.
			// therefore we store it in the request;
			// the LogResponse method can then retrieve it from the request that is passed in.

			if (_LogServiceCalls == -1) return;

			if (request != null) request.DebugInfo = new CallInfo(name);

			switch (_LogServiceCalls)
			{
				case 0:
					return;
				case 1:
					_Log.Info("+" + name);
					return;
			}

			if (request != null)
			{
				string session = request.SessionID.ShortString();
				switch (_LogServiceCalls)
				{
					case 2:
						_Log.InfoFormat("{0}|+{1}", session, name);
						break;
					case 3:
						_Log.InfoFormat("{0}|+{1}: {2}", session, name, request);
						break;
					case 4:
						string detail = RequestDetail(request);
						_Log.InfoFormat("{0}|+{1}: {2}", session, name, detail);
						break;
				}
			}
			else
			{
				_Log.InfoFormat("----|+{0}", name);
			}
		}

		private string RequestDetail(IRequestBase request)
		{
			var sb = new StringBuilder();
			sb.Append(request);
			if (request is ParameterMessageBase)
			{
				sb.Append('+');
				var r = (ParameterMessageBase)request;
				if (r.HasParameters)
				{
					foreach (var k in r.Keys)
					{
						sb.AppendFormat("{0}={1},", k, r[k]);
					}
				}
			}
			if (sb[sb.Length - 1] == ',')
			{
				sb.Length--;
			}
			return sb.ToString();
		}

		internal void LogResult(IRequestBase request, object response)
		{
			if (_LogServiceCalls == -1 || request == null) return;
			var info = request.DebugInfo as CallInfo;
			if (_LogServiceResults == 0 || info == null) return;

			string duration = info.Stopwatch != null
													? string.Format(" ({0} ms)", info.Stopwatch.ElapsedMilliseconds)
													: string.Empty;

			string session = request.SessionID.ShortString();
			string name = info.MethodName;
			switch (_LogServiceResults)
			{
				case 1:
					_Log.Info("-" + name + duration);
					break;
				case 2:
					_Log.InfoFormat("{0}|-{1}{2}", session, name, duration);
					break;
				case 3:
					_Log.InfoFormat("{0}|-{1}{2}: {3}", session, name, duration, response);
					break;
				case 4:
					string detail = ResponseDetail(response);
					_Log.InfoFormat("{0}|-{1}{2}: {3}", session, name, duration, detail);
					break;
			}
		}

		private string ResponseDetail(object response)
		{
			if (response == null) return "INVALID: NULL RESPONSE";
			var sb = new StringBuilder();
			var smr = response as IServiceMessageResponse;
			if (smr != null)
			{
				sb.AppendFormat("{0} {1} {2}: ", smr.Status, smr.StatusMessage, smr.ErrorCode);
				sb.Append(smr.Payload != null ? smr.ToString() : "null");
			}
			else
			{
				sb.AppendFormat(" INVALID RESPONSE TYPE {0}", response.GetType());
			}
			return sb.ToString();
		}


	}
}
