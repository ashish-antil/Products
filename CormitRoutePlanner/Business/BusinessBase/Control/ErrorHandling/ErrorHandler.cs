using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Imarda.Lib;
using Imarda.Logging;

// ReSharper disable once CheckNamespace
namespace FernBusinessBase.Errors
{
	/// <summary>
	/// Error Handler Code
	/// </summary>
	public static class ErrorHandler
	{
		/// <summary>
		/// Error log for internal problems;
		/// </summary>
		private static readonly ErrorLogger _InternalLog;


		static ErrorHandler()
		{
			Process p = Process.GetCurrentProcess();
			string logname = p.ProcessName.Split('.')[0].Replace(' ', '_');
			_InternalLog = ErrorLogger.GetLogger(logname + "_Internal");
		}

		/// <summary>
		/// Handle exceptions and log them on ERROR level. Use this method in the catch 
		/// clause of top level OperationContract methods.
		/// </summary>
		/// <typeparam name="T">the return type of the method that caught the exception</typeparam>
		/// <param name="ex">the exception to be handled</param>
		/// <param name="args"></param>
		/// <returns>a service message response with the error information</returns>
		public static T Handle<T>(Exception ex, params object[] args) 
			where T : IServiceMessageResponse, new()
		{
			try
			{
				MethodBase methodBase = new StackFrame(1).GetMethod();
				return (T)GetResponse<T>(ex, methodBase, args);
			}
			catch (Exception ex2)
			{
				return (T)GetInternalError<T>(ex2);
			}
		}

		public static void HandleInternal(Exception ex)
		{
			try
			{
				_InternalLog.ErrorFormat("Imarda Framework Exception {0}", ex);
			}
			catch
			{
				// well, now what? can't even log the error
                // let's dance then...
			}
		}

	    public static void ErrorFormat(string fmt, params object[] args)
	    {
            _InternalLog.ErrorFormat(fmt, args);
	    }

	    public static void ErrorFormatStoredProc(string storeProcName, object[] args, Exception ex)
	    {
	        if (args == null || args.Length == 0)
	        {
	            return;
	        }

            var argsTextArray = args.Select(c => c != null ? c.ToString() : "null").ToArray();
            var argsText = string.Join(",", argsTextArray);
	        ErrorFormat("{0} {1}|ex:{2}", storeProcName ?? "[null]", argsText, ex);
        }

        public static void Warn(Exception ex)
        {
            _InternalLog.Warn(ex);
        }

        public static BusinessMessageResponse Handle(Exception ex, params object[] args)
		{
			try
			{
				MethodBase methodBase = new StackFrame(1).GetMethod();
				return (BusinessMessageResponse)GetResponse<BusinessMessageResponse>(ex, methodBase, args);
			}
			catch (Exception ex2)
			{
				return (BusinessMessageResponse)GetInternalError<BusinessMessageResponse>(ex2);
			}
		}

		private static IServiceMessageResponse GetInternalError<T>(Exception ex2)
			where T : IServiceMessageResponse, new()
		{
			return new T
			{
				Status = false,
				StatusMessage = ex2.ToString(), //TODO disable in production
				ErrorCode = "INxh|" + ErrorCodeManager.GetExceptionID(ex2) + "|ErrorCodeManager.Handle"
			};
		}

		private static IServiceMessageResponse GetResponse<T>(
			Exception ex, 
			MethodBase methodBase, 
			params object[] args
		)
			where T : IServiceMessageResponse, new()
		{
			IServiceMessageResponse response;
			string code = ErrorCodeManager.Instance.GetErrorCode(ex, methodBase, typeof (T));

			var mre = ex as MessageResponseException; // this exception gets thrown for Check() CheckItem() and Checklist()
			if (mre != null && mre.Response != null)
			{
				response = mre.Response;
				if (response.Status) //OK
				{
					response.Status = false; // turn into error
					response.ErrorCode = code;
					response.StatusMessage = mre.Message;
				}
			}
			else
			{
				response = new T
				{
					Status = false,
					StatusMessage = GetInfo(ex, args),
					ErrorCode = code
				};
			}
		    if (methodBase.DeclaringType != null)
		        ErrorLogger.GetLogger(methodBase.DeclaringType.ToString()).Error(ServiceMessageHelper.ToString(response));
		    return response;
		}

		/// <summary>
		/// Check if response is OK, if not then throw exception. Use to check each response
		/// from a service method.
		/// </summary>
		/// <param name="response"></param>
		public static void Check(IServiceMessageResponse response)
		{
            if (response == null || !response.Status && response.StatusMessage != "OK")//IM-5359
			{
				throw new MessageResponseException(response);
			}
		}

		/// <summary>
		/// Check if item returned is not null. If null, throw exception.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="response"></param>
		public static void CheckItem<T>(GetItemResponse<T> response)
			where T : BaseEntity, new()
		{
			Check(response);
			if (response.Item == null)
			{
				throw new MessageResponseException(response, typeof(T).Name + " is null");
			}
		}

		/// <summary>
		/// Check if list returned with at least 1 item. If not throw exception.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="response"></param>
		public static void Checklist<T>(GetListResponse<T> response)
			where T : BaseEntity, new()
		{
			Check(response);
			if (response.List == null || response.List.Count == 0)
			{
				throw new MessageResponseException(response, typeof(T).Name + " is null or empty");
			}
		}


		public static bool IsSecurityException(string code)
		{
			if (string.IsNullOrEmpty(code)) return false;
			string[] parts = code.Split('|');
			return (parts.Length > 1 && parts[1] == "Security");
		}

		/// <summary>
		/// Get information: the exception and the args.
		/// Include the string values of the args, if array part of args, include each of its elements.
		/// There is a max number of chars per element in the output. Also max number of parameters.
		/// An ellipsis shows string truncation and array truncation.
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private static string GetInfo(Exception ex, params object[] args)
		{
			try
			{
				var validationException = ex as ValidationException;
				if (validationException != null)
				{
					string[] errors = validationException.Errors;
					return errors != null ? string.Join("|", errors) : string.Empty;
				}
				const string sNull = "(null)";
				const string sep = "; ";
				const int maxstrlen = 12;
				const int maxelem = 4;
				if (args != null && args.Length > 0)
				{
					var sb = new StringBuilder();
					int m = Math.Min(args.Length, maxelem);
					for (int k = 0; k < m; k++)
					{
						object arg = args[k];
						if (arg == null)
						{
							sb.Append(sNull).Append(sep);
						}
						else if (arg.GetType().IsArray)
						{
							var arr = (Array)arg;
							sb.Append('[');
							int n = Math.Min(arr.Length, maxelem);
							for (int i = 0; i < n; i++)
							{
								object elem = arr.GetValue(i);
								string s = elem == null ? sNull : elem.ToString().Truncate(maxstrlen);
								sb.Append(s).Append(sep);
							}
							if (arr.Length > 0) sb.Length -= sep.Length;
							if (arr.Length > maxelem) sb.Append(";+");
							sb.Append(']').Append(sep);
						}
						else
						{
							string s = arg.ToString();
							sb.Append(k == 0 ? s : s.Truncate(maxstrlen)).Append(sep);
						}

					}
					if (args.Length > 0) sb.Length -= sep.Length;
					if (args.Length > maxelem) sb.Append(";+");
					sb.AppendLine().Append(ex.ToString());
					return sb.ToString();
				}
				return ex.ToString();
			}
			catch (Exception ex0)
			{
				return ex0.ToString();
			}
		}

	}

}
