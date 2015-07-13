using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Imarda.Lib
{
	public static class TimeUtils
	{
		public static readonly DateTime MaxValue = new DateTime(3000, 1, 1);
		public static readonly DateTime MinValue = SqlDateTime.MinValue.Value;
		public static readonly DateTime SmallDateTimeMinValue = new DateTime(1900, 1, 1, 0, 0, 0);

		/// <summary>
		/// TimeZoneIDs array cache.
		/// </summary>
		private static Dictionary<string, string> _TimeZoneIDs;

		/// <summary>
		/// Get the list of TimeZoneIDs. 
		/// These can be used as parameter in TimeZoneInfo.FindSystemTimeZoneById(string).
		/// </summary>
		/// <returns></returns>
		public static string GetTimeZoneID(string displayName)
		{
			if (_TimeZoneIDs == null)
			{
				_TimeZoneIDs = new Dictionary<string, string>();
				foreach (TimeZoneInfo tzi in TimeZoneInfo.GetSystemTimeZones())
				{
					_TimeZoneIDs[tzi.DisplayName.Replace("GMT", "UTC")] = tzi.Id;
				}
				_TimeZoneIDs["(UTC) Coordinated Universal Time"] = "UTC";
			}
			string id;
			return _TimeZoneIDs.TryGetValue(displayName, out id) ? id : displayName;
		}

		public static string ExpandTimeZoneName(string tz)
		{
			if (tz == "UTC 12") tz = "UTC+12";
			string tz1 = tz == null || tz == "undefined" ? "UTC" : tz.Replace("#", "Standard Time");
			return tz1;
		}

		public static string AbbreviateTimeZoneName(string tz)
		{
			if (tz == "UTC 12") tz = "UTC+12";
			string tz1 = string.IsNullOrEmpty(tz) || tz == "undefined" ? "UTC" : tz.Replace("Standard Time", "#");
			return tz1;
		}

		public static DateTime ToUtc(DateTime dt, TimeZoneInfo tzi)
		{
			if (dt.Kind == DateTimeKind.Local) throw new ArgumentException("DateTime kind must not be local");
			return dt.Kind == DateTimeKind.Utc ? dt : TimeZoneInfo.ConvertTimeToUtc(dt, tzi);
		}

		public static DateTime FromUtc(DateTime dt, TimeZoneInfo tzi)
		{
			if (dt.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime kind must be UTC");
			return TimeZoneInfo.ConvertTimeFromUtc(dt, tzi);
		}

		/// <summary>
		/// Converts Iso8601 date strings in the format 2014-04-03T19:55:48+00:00
		/// </summary>
		/// <param name="dateString"></param>
		/// <returns></returns>
		public static DateTime ParseIso8601(string dateString)
		{
			if (string.IsNullOrEmpty(dateString))
			{
				return default(DateTime);
			}

			return DateTime.Parse(dateString);

		}

		/// <summary>
		/// Converts Iso8601 date string in the format 2014-04-03T19:55:48Z or 2014-04-03T23:12:22.000000Z
		/// </summary>
		/// <param name="dateString"></param>
		/// <returns></returns>
		public static DateTime ParseIso8601UtcZ(string dateString)
		{
			if (string.IsNullOrEmpty(dateString))
			{
				return default(DateTime);
			}
			//dateString = dateString.Substring(0,dateString.Length-1); //remove Z

			var strWithNoMs = dateString.Substring(0, "2014-04-03T23:12:22".Length);
			var fullDateString = dateString;

			dateString = strWithNoMs; //remove anything after last .ss e.g.: 2014-04-03T23:12:22.000000 -> 2014-04-03T23:12:22

			var dt = DateTime.ParseExact(dateString, Formats.Iso8601DateTimeFmt, CultureInfo.InvariantCulture, DateTimeStyles.None);

			if (fullDateString.Length > strWithNoMs.Length)
			{
				var msString = fullDateString.Substring(strWithNoMs.Length+1);
				msString = msString.TrimEnd(new char[] {'Z'}); //remove Z if present
				msString = msString.PadRight(6, '0'); //make sure the string length is 6
				msString = string.Format("{0}.{1}", msString.Substring(0, 3), msString.Substring(3, 3));
				double ms;
				if (double.TryParse(msString, out ms))
				{
					dt = dt.AddMilliseconds(ms);
				}
			}

			return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
		}

		public static DateTimeOffset DateTimeOffsetFromIso8601(string dateString)
		{
			//const string pattern = "yyyy-MM-dd'T'HH:mm:ss.FFFK";
			var dto = DateTimeOffset.Parse/*Exact*/ (dateString/*, pattern, CultureInfo.InvariantCulture*/);
			return dto;
		}


		/// <summary>
		/// Interpret the date string as UTC. See also TimeUtil.ParseUtc() which is not accessible from the Biz/Infra layers
		/// This method throws an exception if the datestring is invalid.
		/// </summary>
		/// <param name="datestring">string in our standard date string format</param>
		/// <returns>UTC date time, or default(DateTime) if datestring null or empty</returns>
		public static DateTime ParseUtc(string datestring)
		{
			if (string.IsNullOrEmpty(datestring)) return default(DateTime);
			DateTime dt = DateTime.ParseExact(datestring, Formats.JsDateTimeFmt, CultureInfo.InvariantCulture, DateTimeStyles.None);
			return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
		}

		public static DateTime Parse(string datestring)
		{
			DateTime dt = DateTime.ParseExact(datestring, Formats.JsDateTimeFmt, CultureInfo.InvariantCulture, DateTimeStyles.None);
			return dt;
		}

		/// <summary>
		/// Interpret the date string as UTC. 
		/// This method throws an exception if the datestring is invalid.
		/// </summary>
		/// <param name="datestring">string sent from unit</param>
		/// <returns>UTC date time, or default(DateTime) if datestring null or empty</returns>
		public static DateTime ParseUnitUtc(string datestring)
		{
			if (string.IsNullOrEmpty(datestring)) return default(DateTime);
			DateTime dt = DateTime.ParseExact(datestring, Formats.UnitDateTimeFmt, CultureInfo.InvariantCulture,
																				DateTimeStyles.None);
			return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
		}

		public static double CalcTimeZoneOffset(TimeZoneInfo tzi, DateTime userNow, out string timezoneName)
		{
			double offset = tzi.GetUtcOffset(userNow).TotalHours;
			var hour = (int)Math.Truncate(offset);
			string oldname = tzi.DisplayName;
			oldname = oldname.Remove(0, oldname.IndexOf(')') + 1);
			timezoneName = "(UTC" + hour.ToString("+00;-00") + ":" + ((int)((offset - hour) * 60)).ToString("00") + ") " + oldname;

			return offset;
		}

		public static DateTime SecondsSince1970ToDateTime(int secondsSince1970)
		{
			DateTime value = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return value.AddSeconds(secondsSince1970);
		}

		public static DateTime SecondsSince31Dec1989ToDateTime(int secondsSince31Dec1989)
		{
			DateTime value = new DateTime(1989, 12, 31);
			return value.AddSeconds(secondsSince31Dec1989);
		}

		public static int DateTimeToSecondsSince31Dec1989(DateTime dateTime)
		{
			DateTime _31Dec1989 = new DateTime(1989, 12, 31, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan diffInTime = dateTime - _31Dec1989;
			return (int)diffInTime.TotalSeconds;
		}

		public static string DateTimeDiffToFormatted(DateTime dtStart, DateTime dtEnd, string format)
		{
			TimeSpan diff = dtEnd - dtStart;
			return new DateTime(diff.Ticks).ToString(format);
		}

		public static string DateTimeToStringWithMsec(DateTime dateTime)
		{
			string fullPattern = DateTimeFormatInfo.CurrentInfo.FullDateTimePattern;
			fullPattern = Regex.Replace(fullPattern, "(:ss|:s)", "$1.fff");
			return dateTime.ToString(fullPattern);
		}

		#region sequential time

		private static DateTime _LastTimestamp;
		private static readonly object LastTimestampSync = new object();

		/// <summary>
		/// Consecutive calls to this method will result in DateTime objects that are increasing with
		/// at least 7ms difference. Use this if you have to make sure a DateTime column in SQL server
		/// has to have unique times for an ORDER BY datetime. The resolution of a SQL server date time
		/// is 3.33ms.
		/// </summary>
		public static DateTime UtcNowSeq
		{
			get
			{
				lock (LastTimestampSync)
				{
					DateTime now = DateTime.UtcNow;
					if (now <= _LastTimestamp)
					{
						now = _LastTimestamp.AddMilliseconds(7); // > resolution in SQL (3.33ms)
					}
					_LastTimestamp = now;
					return now;
				}
			}
		}

		#endregion
	}
}