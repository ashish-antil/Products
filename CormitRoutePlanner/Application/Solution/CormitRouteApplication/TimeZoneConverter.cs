using System;
using System.Collections.Generic;
using FernBusinessBase;
using ImardaSecurityBusiness;
using System.Reflection;
using Imarda360Application.Security;
using Imarda.Lib;
using System.Collections;
using System.Globalization;

namespace Imarda360Application
{
	public static class TimeZoneConverter
	{
		/// <summary>
		/// Change timestamp from unspecified to utc.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="so"></param>
		public static void Globalize(GetListByTimestampRequest request, SessionObject so)
		{
			var cso = (ConfiguredSessionObject)so;
			if (cso.RequiresTimeZoneConversion)
			{
				request.TimeStamp = cso.ToUtc(request.TimeStamp);
			}
		}

		/// <summary>
		/// Change all the DateTime fields in the object which are marked as DateTimeKind.Unspecified to UTC
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="so"></param>
		public static void GlobalizeObject(object obj, SessionObject so)
		{
			var cso = so as ConfiguredSessionObject;
			if (obj == null || cso == null) return;

			PropertyInfo[] properties = obj.GetType().GetProperties();
			foreach (PropertyInfo prop in properties)
			{
				if (prop.PropertyType == typeof(DateTime))
				{
					var dt = (DateTime)prop.GetValue(obj, null);
					if (dt != DateTime.MinValue && dt.Kind == DateTimeKind.Unspecified)
					{
						DateTime utc;
					    if (cso.RequiresTimeZoneConversion)
					    {
					        utc = cso.ToUtc(dt);
					    }
					    else
					    {
					        utc = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
					    }

					    if (utc < TimeUtils.MinValue)
					    {
					        utc = TimeUtils.MinValue;
					    }
						else if (utc > TimeUtils.MaxValue)
						{
						    utc = TimeUtils.MaxValue;
						}

						prop.SetValue(obj, utc, null);
					}
				}
				//if (prop.PropertyType == typeof(string) && prop.Name.Contains("TimeZone"))
				//{
				//  string s = (string)prop.GetValue(obj, null);
				//  if (s.StartsWith("("))
				//  {
				//    prop.SetValue(obj, TimeUtils.GetTimeZoneID(s), null);
				//  }
				//}
			}
			//if (obj is FullBusinessEntity)
			//{
			//  var entity = (FullBusinessEntity)obj;
			//  EntityAttributes ea = entity.Attributes;
			//  if (ea != null && ea.HasAttributes)
			//  {
			//    //TODO look into this
			//    //foreach (object key in ea.Map.Keys)
			//    //{
			//    //  ea.Map[key] = GlobalizeAttribute((string)ea.Map[key], cso);
			//    //}
			//    ea.UpdateAttributeString();
			//  }
			//}
		}

		private static string GlobalizeAttribute(string s, ConfiguredSessionObject cso)
		{
			//TODO look into this

			//if (s != null && s.Length == Formats.DateFmtLength && s[10] == 'T')
			//{
			//  // good chance of having a date time string
			//  try
			//  {
			//	return cso.ToUtc(EntityAttributes.ParseDateTime(s)).ToString("s");
			//  }
			//  catch { }
			//}
			return s;
		}

		/// <summary>
		/// Change all the DateTime fields in each object in the list which are marked as DateTimeKind.Unspecified to UTC
		/// </summary>
		/// <param name="list"></param>
		/// <param name="so"></param>
		public static void GlobalizeObjectList(IList list, SessionObject so)
		{
			var cso = so as ConfiguredSessionObject;
		    if (list == null || cso == null)
		    {
		        return;
		    }

			if (cso.RequiresTimeZoneConversion)
			{
			    foreach (object obj in list)
			    {
			        GlobalizeObject(obj, cso);
			    }
			}
		}

		/// <summary>
		/// Iterates thru the parameters of the request and converts every valid date in our standard date time format
		/// to UTC. It interprets the date time strings as being in the configured time zone.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="so"></param>
		public static void GlobalizeRequest(ParameterMessageBase request, SessionObject so)
		{
			var cso = so as ConfiguredSessionObject;
			if (cso == null) return;

			if (cso.RequiresTimeZoneConversion)
			{
				var list = new List<KeyValuePair<string, string>>();
				foreach (string key in request.Keys)
				{
					string val = request[key];
					DateTime dt;
					if (DateTime.TryParseExact(val, "s", null, DateTimeStyles.None, out dt) ||
						DateTime.TryParseExact(val, Formats.JsDateTimeFmt, null, DateTimeStyles.None, out dt))
					{
						DateTime utc = cso.ToUtc(dt);
						list.Add(new KeyValuePair<string, string> { Key = key, Value = utc.ToString("s") });
					}
				}
				foreach (var kv in list)
				{
					request[kv.Key] = kv.Value;
				}
			}
		}

		/// <summary>
		/// Change all the DateTime fields which are UTC to the preferred zone
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="so"></param>
		public static void LocalizeObject(object obj, SessionObject so)
		{
		    if (obj == null)
		    {
		        return;
		    }

			var cso = so as ConfiguredSessionObject;
			if (cso != null && cso.RequiresTimeZoneConversion)
			{
				PropertyInfo[] properties = obj.GetType().GetProperties();
				foreach (PropertyInfo prop in properties)
				{
					if (prop.PropertyType == typeof (DateTime))
					{
						var dt = (DateTime) prop.GetValue(obj, null);
						if (dt.Kind == DateTimeKind.Utc)
						{
							DateTime local = cso.FromUtc(dt);
							prop.SetValue(obj, local, null);
						}
					}
				}
			}
		}

		// Code below moved to Web Tier.

		//public static void LocalizeAttributes(EntityAttributes ea, SessionObject so)
		//{
		//  if (ea == null || !ea.HasAttributes) return;
		//  var cso = so as ConfiguredSessionObject;
		//  if (cso != null && cso.RequiresTimeZoneConversion)
		//  {
		//    var keyArray = new object[ea.Map.Keys.Count];
		//    ea.Map.Keys.CopyTo(keyArray, 0);
		//    foreach (object key in keyArray)
		//    {
		//      object val = ea.Map[key];
		//      if (val is string[])
		//      {
		//        var elems = (string[])val;
		//        for (int i = 0; i < elems.Length; i++)
		//        {
		//          elems[i] = LocalizeAttribute(elems[i], cso);
		//        }
		//      }
		//      else
		//      {
		//        ea.Map[key] = LocalizeAttribute((string)ea.Map[key], cso);
		//      }
		//    }
		//    ea.UpdateAttributeString();
		//  }
		//}

		//private static string LocalizeAttribute(string s, ConfiguredSessionObject cso)
		//{
		//  if (s != null && s.Length == Formats.DateFmtLength && s[10] == 'T')
		//  {
		//    DateTime? dt = EntityAttributes.ParseDateTime(s);
		//    return dt.HasValue ? cso.FromUtc(dt.Value).ToString("s") : "INVALID";
		//  }
		//  return s;
		//}

		/// <summary>
		/// Change all the DateTime fields in all the objects in the list if the are UTC, to preferred zone.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="so"></param>
		public static void LocalizeObjectList(IList list, SessionObject so)
		{
			if (list == null) return;
			var cso = so as ConfiguredSessionObject;
			if (cso != null && cso.RequiresTimeZoneConversion)
			{
				foreach (object obj in list) LocalizeObject(obj, cso);
			}
		}

		/// <summary>
		/// Interprets a standard format date string as being UTC
		/// </summary>
		/// <param name="datestring">our standard format date string</param>
		/// <returns>UTC DateTime, except when datestring null or empty, then it 
		/// is default(DateTime) which has DateTimeKine.Unspecified</returns>
		public static DateTime ParseUtc(string datestring)
		{
			return Parse(datestring, DateTimeKind.Utc);
		}

		/// <summary>
		/// Interprets a standard format date as being localized (configured time zone).
		/// </summary>
		/// <param name="datestring"></param>
		/// <returns></returns>
		public static DateTime ParseLocalized(string datestring)
		{
			return Parse(datestring, DateTimeKind.Unspecified);
		}

		private static DateTime Parse(string datestring, DateTimeKind kind)
		{
			if (string.IsNullOrEmpty(datestring)) return default(DateTime);
			DateTime dt = DateTime.ParseExact(datestring, Formats.JsDateTimeFmt, CultureInfo.InvariantCulture, DateTimeStyles.None);
			return DateTime.SpecifyKind(dt, kind);
		}

	}
}
