using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ServiceStack.Text;

namespace Imarda.Logging
{

	public static class Utilities
	{
		public static string Dump(IEnumerable<object> objects)
		{
			var sb = new StringBuilder();
			foreach (var obj in objects)
			{
				sb.AppendLine(obj.DumpT());
			}
			return sb.ToString();
		}

		/// <summary>
		/// Dumps an object to the log (if not null) - Debug.Write otherwise
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="log"></param>
		public static void DumpTo(object obj, ErrorLogger log = null)
		{
			var toLog = null != log;
			var dump = new Action<object>(o =>
			{
				var a = (o is object[]) ? o as object[] : new[] { o };
				if (toLog)
				{
					//var lvl = log.MaxLevel;
					//log.MaxLevel = 5;
					log.Debug(Dump(a));
					//log.MaxLevel = lvl;
				}
				else
				{
                    Debug.Write(Imarda.Logging.Utilities.Dump(a));
				}
			});
			dump(obj);
		}

	}


	public static class Extensions
	{
		public static string DumpT<T>(this T me)
		{
			if (ReferenceEquals(me, null))
			{
				return string.Empty;
			}
			var sb = new StringBuilder();
			sb.AppendLine("Type: " + me.GetType().Name);
			sb.AppendLine(me.Dump());
			return sb.ToString();
		}
	}
}
