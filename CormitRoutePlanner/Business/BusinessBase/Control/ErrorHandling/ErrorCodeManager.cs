using System;
using System.Collections;
using System.Reflection;
using Imarda.Lib;

namespace FernBusinessBase.Errors
{
	public class ErrorCodeManager
	{
		#region Singleton
		private static ErrorCodeManager _Instance = new ErrorCodeManager();
		public static ErrorCodeManager Instance { get { return _Instance; } }
		#endregion

		private IDictionary _Services;

		private ErrorCodeManager()
		{
			string s = 
				"CRM|cr||GIS|gs||Tracking|tr||VehicleManagement|vm||" +
				"Configuration|cf*||Reporting|rp*||Task|tk*||Security|se*||Notification|no*";
			_Services = s.KeyValueMap(ValueFormat.Strings, true);
		}

		public string GetErrorCode(Exception ex, MethodBase method, Type returnType)
		{
			try
			{
				string namespaceName = method.DeclaringType.Namespace;
				string className = method.DeclaringType.Name;
				string methodName = method.Name;
				string entityName = returnType.IsGenericType ? returnType.GetGenericArguments()[0].Name : string.Empty;

				string ll, ss;

				const string sImarda360Application = "Imarda360Application";


				if (namespaceName.StartsWith(sImarda360Application))
				{
					// Imarda360Application.{service}, Imarda{service}
					ll = "AP"; // application service
					string key = namespaceName.Substring(sImarda360Application.Length).Trim('.');
					string raw = (string)_Services[key] ?? "xx";
					ss = raw.Substring(0, 2);
				}
				else if (namespaceName == "ImardaProvisioningBusiness")
				{
					ll = "AP";
					ss = "pr";
				}
				else if (namespaceName.EndsWith("Communicator"))
				{
					ll = "GW";
					ss = namespaceName.Substring(0, 2).ToLowerInvariant(); // V300 -> v3, SMDP -> sm
				}
				else if (namespaceName == "ImardaGateway")
				{
					ll = "GW";
					ss = "00";
				}
				else
				{
					// Imarda{service}Business, Imarda{service}
					string key = className.Substring("Imarda".Length);
					string raw = (string)_Services[key] ?? "xx";
					ll = (raw.Length > 2) ? "IN" : "BZ";
					ss = raw.Substring(0, 2);
				}
				string id = className + '.' + methodName + ':' + entityName; // e.g. "ImardaCRM.GetCompany:Company"
				string code = ll + ss + '|' + ErrorCodeManager.GetExceptionID(ex) + '|' + id;
				return code;
			}
			catch (Exception ex2)
			{
				return "INxc|" + GetExceptionID(ex2) + "|ErrorCodeManager.GetErrorCode:" + ex2.ToString();
			}
		}

		public static string GetExceptionID(Exception ex)
		{
			return ex.GetType().Name.Replace("Exception", string.Empty);
		}

	}
}
