using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Imarda.Lib
{
	/// <summary>
	/// User this format provider in Imarda 360 presentation tier everywhere.
	/// </summary>
	public class ImardaFormatProvider : IFormatProvider, ICustomFormatter
	{
		private readonly CultureInfo _Default;

		private readonly Dictionary<Type, string[]> _DiscreteUnitsMap;
		private readonly MeasurementFormatInfo _MeasurementFormatProvider;

		private readonly TimeZoneInfo _Zone;

		/// <summary>
		/// Create a format provider.
		/// </summary>
		/// <param name="dflt">the default format provider (e.g. CultureInfo) for dates and numbers</param>
		/// <param name="measurementFormatProvider">format provider for measurements and units</param>
		/// <param name="tzi"> </param>
		public ImardaFormatProvider(CultureInfo dflt, MeasurementFormatInfo measurementFormatProvider, TimeZoneInfo tzi)
		{
			_Default = dflt;
			_MeasurementFormatProvider = measurementFormatProvider;
			_Zone = tzi;
			NullValue = string.Empty;
			_DiscreteUnitsMap = new Dictionary<Type, string[]>();

			//TODO populate _DiscreteUnitsMap
		}

		public bool ForceDefaultDateFormat { get; set; }

		public CultureInfo DefaultCulture
		{
			get { return _Default; }
		}

		/// <summary>
		/// What to return from Format if arg is null.
		/// </summary>
		public string NullValue { get; set; }

		public TimeZoneInfo TimeZoneInfo
		{
			get { return _Zone; }
		}

		public MeasurementFormatInfo MeasurementFormatProvider
		{
			get { return _MeasurementFormatProvider; }
		}

		#region ICustomFormatter Members

		/// <summary>
		/// Format a single value using this format provider's settings.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="arg"></param>
		/// <param name="formatProvider"></param>
		/// <returns></returns>
		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			string s;
			if (arg == null) s = NullValue;
			else if (arg is string)
			{
				s = (string)arg;
				int p = s.IndexOf(':'); // either a meaurement or date time
				if (p != -1)
				{
					bool done = false;
					if (p == 13 && s.Length >= 19 && s[10] == 'T' && s[4] == '-')  // quick check: "2012-07-04T02:07:51" ':' on position [13], 'T' on [10], '-' on [4]
					{
						DateTime dt;
						if (DateTime.TryParseExact(s, "s", null, DateTimeStyles.None, out dt))
						{
							dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
							s = FormatDateTime(format, dt);
							done = true;
						}
					}

					if (!done && s.IndexOf(' ', 0, p) == -1)
					{
						try
						{
							s = Measurement.Format(s, _MeasurementFormatProvider);
						}
						catch
						{
							//just keep original value of 's'
						}
					}
				}
				else if (s.IndexOf(StringUtils.BrokenBar) != -1) s = s.Replace(StringUtils.BrokenBar, '|');
			}
			else if (arg is IMeasurement)
			{
				var m = (IMeasurement)arg;
				s = m.ToString(format, _MeasurementFormatProvider);
			}
			else if (arg is DateTime)
			{
				s = FormatDateTime(format, (DateTime)arg);
			}
			else if (arg is int || arg is short || arg is byte)
			{
				s = FormatInteger(format, arg);
			}
			else if (arg is double || arg is decimal || arg is float)
			{
				// if format contains a '*' in place of a '.' then make sure the output decimal sep is always a '.'
				string dsep = _MeasurementFormatProvider.NumberFormat.NumberDecimalSeparator;
				bool preserveDot = format != null && format.Contains("*");
				if (preserveDot)
				{
					format = format == "*" ? null : format.Replace('*', '.');
				}
				s = ((IFormattable)arg).ToString(format, _MeasurementFormatProvider.NumberFormat);
				if (preserveDot) s = s.Replace(dsep, ".");
			}
			else if (arg is bool)
			{
				if (string.IsNullOrEmpty(format)) s = arg.ToString().ToLowerInvariant();
				else
				{
					int p = format.IndexOf(';'); // e.g. "{0:no;yes}"  take "no" for false, "yes" for true.
					if (p == -1) s = arg.ToString();
					else s = (bool)arg ? format.Substring(p + 1) : format.Substring(0, p);
				}
			}
			else if (arg is Enum)
			{
				s = FormatEnum(arg);
			}
			else if (arg is MwF)
			{
				s = ((MwF)arg).ToString(_MeasurementFormatProvider);
			}
			else if (arg is IFormattable)
			{
				s = ((IFormattable)arg).ToString(format, _Default);
			}
			else
			{
				s = arg.ToString();
			}
			return s;
		}

		#endregion

		#region IFormatProvider Members

		/// <summary>
		/// Required by IFormatProvider interface.
		/// </summary>
		/// <param name="formatType"></param>
		/// <returns></returns>
		public object GetFormat(Type formatType)
		{
			return formatType == typeof(ICustomFormatter) ? this : null;
		}

		#endregion

		public string GetDateFormat(string key)
		{
			if (key == "~") key = string.Empty;
			return _MeasurementFormatProvider.GetPreference("DateFormat" + key);
		}

		/// <summary>
		/// Get the format string for a fixed set of enum values.
		/// </summary>
		/// <param name="key">e.g. "~switch" or "~colr" or "~address"</param>
		/// <returns>a string in the format like "`email`sms`fax", each entry corresponds to an enum value 0, 1, 2, ...</returns>
		public string GetEnumFormat(string key)
		{
			return _MeasurementFormatProvider.GetPreference("EnumFormat" + key);
		}

		/// <summary>
		/// Get the unit for the given type.
		/// </summary>
		/// <example>ifp.GetUnit(typeof(Length), "dist")</example>
		/// <param name="type">IMeasurement implementing struct</param>
		/// <param name="context">application context, e.g. "dist", "eng", or null if n/a; should yield simple unit, not complex '>' format</param>
		/// <returns>the user's preferred unit</returns>
		public string GetUnit(Type type, string context)
		{
			if (context == null) context = "";
			else context = ":~" + context;
			ConstructorInfo cons = type.GetConstructor(new[] { typeof(double) });
			var val = (IMeasurement)cons.Invoke(new object[] { 0.0 });
			string s = string.Format(this, "{0" + context + "}", val);
			int p = s.IndexOf(' ');
			s = s.Substring(p + 1);
			return s;
		}

		/// <summary>
		/// Create an IMeasurement value for the given value
		/// </summary>
		/// <param name="type"></param>
		/// <param name="context"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public IMeasurement Create(Type type, string context, object value)
		{
			if (!NumberUtils.IsNumber(value)) throw new ArgumentException("Cannot convert to number: " + (value ?? "(null)"));
			var mu = GetUnit(type, context);
			return UnitParser.Instance.Create(Convert.ToDouble(value), mu);
		}


		/// <summary>
		/// Gets a set of user defined discrete values for display, i.e. localized.
		/// </summary>
		/// <param name="enumType">identifies the list of names to be displayed for the numeric values</param>
		public string[] GetDiscreteUnit(Type enumType)
		{
			string[] values;
			return _DiscreteUnitsMap.TryGetValue(enumType, out values) ? values : null;
		}

		public void AddDiscreteUnit(Type type, string[] values)
		{
			_DiscreteUnitsMap[type] = values;
		}


		private static bool InWeek(DateTime now, DateTime dt)
		{
			int daysSinceMonday = (int)now.DayOfWeek - 1;
			if (daysSinceMonday == -1) daysSinceMonday = 6;
			DateTime monday = now.AddDays(-daysSinceMonday);
			return dt >= monday && dt <= monday + TimeSpan.FromDays(7);
		}

		public static DateTime? DateTimeInUserZone(DateTime dt, TimeZoneInfo userZone)
		{
			if (dt.Kind == DateTimeKind.Local) throw new ArgumentException("Invalid date kind (local");
			if (dt == default(DateTime)) return null; // we don't want to display 0001-01-01T00:00:00
			if (dt.Kind == DateTimeKind.Utc) dt = TimeZoneInfo.ConvertTimeFromUtc(dt, userZone);
			return dt;
		}

		private string FormatDateTime(string format, DateTime dt)
		{
			DateTime? dtl = DateTimeInUserZone(dt, _Zone);
			if (dtl == null) return string.Empty;
			dt = dtl.Value;

			string s;

			if (!string.IsNullOrEmpty(format))
			{
				switch (format[0])
				{
					case 'w':
					case 'z':
						// format is like "w;2010-02-11;g", "z:2010-02-11;d/M/yy", "z" "z;;d" etc. 
						// this will use the name of the week day if the passed in date is in the week of the given date (in the format spec, utcnow by default)
						// else it will use the format after the second semicolon (default 'd' = short date format)
						// format 'z' differs from 'w' in that it uses "Yesterday", "Today", "Tomorrow" 

						string[] parts = format.Split(';');
						DateTime realNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _Zone).Date;
						DateTime nominalNow = parts.Length == 1 || parts[1] == ""
																		? realNow
																		: DateTime.ParseExact(parts[1], "yyyy-MM-dd", null);
						if (InWeek(nominalNow, dt))
						{
							s = _Default.DateTimeFormat.DayNames[(int)dt.DayOfWeek];
						}
						else
						{
							string format1 = parts.Length > 2 ? parts[2] : "d";
							s = dt.ToString(format1, _Default);
						}

						if (format[0] == 'z')
						{
							var dt0 = dt.Date;
							var today = nominalNow.Date;
							if (dt0 == today.AddDays(-1)) s = "Yesterday"; //TODO translated in config table 
							else if (dt0 == today) s = "Today"; //TODO translated in config table 
							else if (dt0 == today.AddDays(1)) s = "Tomorrow"; //TODO translated in config table 
						}
						break;

					case '~':
						string contextFormat = GetDateFormat(format) ?? "g";
						s = dt.ToString(contextFormat, _Default);
						break;

					default:
						s = dt.ToString(format, _Default);
						break;
				}
			}
			else s = dt.ToString(format, _Default);
			return s;
		}

		private string FormatInteger(string format, object arg)
		{
			const char sep = '`'; // separator for enum literals
			if (format == null) return ((IFormattable)arg).ToString(null, _Default);
			if (format.StartsWith("~")) return FormatInteger(GetEnumFormat(format), arg);

			if (format.Length > 1 && format[0] == sep) // "`zero`one`two`three"  => int or enum selects item, start 0
			{
				int n = Convert.ToInt32(arg);
				int p = 1;
				while (n-- > 0 && p > 0)
				{
					p = format.IndexOf(sep, p) + 1;
				}
				if (p < 1) p = format.LastIndexOf(sep) + 1;
				int q = format.IndexOf(sep, p);
				if (q < 0) q = format.Length;
				return format.Substring(p, q - p);
			}
			return ((IFormattable)arg).ToString(format, _Default);
		}

		private string FormatEnum(object arg)
		{
			Type type = arg.GetType();
			string[] values = GetDiscreteUnit(type);
			int i = Convert.ToInt32(arg);
			if (values != null && i >= 0 && i < values.Length) return values[i];
			string valname = Enum.GetName(type, i);
			return string.Format("{0}-{1}", type.Name, valname);
		}
	}
}