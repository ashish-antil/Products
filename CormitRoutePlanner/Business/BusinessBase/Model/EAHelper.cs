using System;
using Imarda.Lib;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;
using System.Collections.Specialized;
using System.Globalization;
using Imarda.Logging;
using NumberUtils = Imarda.Lib.NumberUtils;
using StringUtils = Imarda.Lib.StringUtils;
using ValueFormat = Imarda.Lib.ValueFormat;


namespace FernBusinessBase
{
	public static class EAHelper
	{
		private static readonly Regex _rxKeyAnalyzer = new Regex(@"^(\w[\w\d_#-]*)(.)(\w*)(\[\d*\])? ?(.*)?$", RegexOptions.Compiled);
		public const string Pilcrow = "\u00B6";		//& IM-3424


		public static string SerType(Type type, int vari, string format)
		{
			Type etype = type.HasElementType ? type.GetElementType() : type;
			string s = "$"; // default is string
			if (etype == typeof(Guid)) s = "*";
			else if (etype == typeof(bool)) s = "?";
			else if (etype == typeof(DateTime)) s = "@";
			else if (NumberUtils.IsNumberType(etype)) s = "!";
			else if (typeof(IMeasurement).IsAssignableFrom(etype)) s = '!' + etype.Name;
			else if (typeof(Enum).IsAssignableFrom(etype)) s = "&";
			if (type.HasElementType) s += "[" + (vari == 0 ? "" : vari.ToString()) + "]";
			if (format != null) s += ' ' + format;
			return s; // e.g. "Mileages!Length[] ~dist"   "PowerOn? Off;On"   "Name$"  "EventDates@[3] g"
		}


		internal static string SerValue(object singleValue)
		{
			string s;
			if (singleValue is string)
			{
				s = ((string)singleValue).Replace('|', StringUtils.BrokenBar);
			}
			else if (singleValue is DateTime)
			{
				s = ((DateTime)singleValue).ToString("s");
			}
			else if (singleValue is bool)
			{
				s = (bool)singleValue ? "1" : "0";
			}
			else if (singleValue is IMeasurement)
			{
				var m = (IMeasurement)singleValue;
				s = Measurement.VString(m);
			}
			else if (singleValue is Enum)
			{
				s = (Convert.ToInt32(singleValue)).ToString();
			}
			else if (singleValue == null)
			{
				s = String.Empty;
			}
			else
			{
				s = singleValue.ToString();
			}
			return s;
		}

		public static StringBuilder AppendKV(this StringBuilder sb, string key, object singleValue)
		{
			if (singleValue == null) throw new ArgumentException(string.Format("Null value for key [{0}]", key), "singleValue");
			return sb.AppendKV(key, singleValue.GetType(), singleValue, null);
		}

		public static StringBuilder AppendKV(this StringBuilder sb, string key, object singleValue, string format)
		{
			if (singleValue == null) throw new ArgumentException(string.Format("Null value for key [{0}] format [{1}]", key, format), "singleValue");
			return sb.AppendKV(key, singleValue.GetType(), singleValue, format);
		}

        public static StringBuilder AppendKV_IfNew(this StringBuilder sb, string key, object singleValue, string format)
        {
            if (singleValue == null) throw new ArgumentException(string.Format("Null value for key [{0}] format [{1}]", key, format), "singleValue");
            return sb.AppendKV_IfNew(key, singleValue.GetType(), singleValue, format);
        }

		public static StringBuilder AppendKV(this StringBuilder sb, string key, Type type, object singleValue, string format)
		{
			if (key != null)
			{
				PrepareKV(sb);
				sb.Append(key).Append(SerType(type, 0, format));
				if (typeof(IMeasurement).IsAssignableFrom(type) && !(singleValue is IMeasurement))
				{
					ConstructorInfo cons = type.GetConstructor(new Type[] { typeof(double) });
					singleValue = cons.Invoke(new object[] { Convert.ToDouble(singleValue) });
				}
				string sval = SerValue(singleValue);
				if (!String.IsNullOrEmpty(sval)) sb.Append('|').Append(sval);
			}
			return sb;
		}

        public static StringBuilder AppendKV_IfNew(this StringBuilder sb, string key, Type type, object singleValue, string format)
        {
            if (key != null)
            {
                string theKey = key + SerType(type, 0, format);
                string theKeyWithoutFormat = key + SerType(type, 0, null);

                if (!sb.ToString().Contains(theKey.Trim()) && !sb.ToString().Contains(theKeyWithoutFormat.Trim()))
                {
                    PrepareKV(sb);
                    sb.Append(key).Append(SerType(type, 0, format));
                    if (typeof (IMeasurement).IsAssignableFrom(type) && !(singleValue is IMeasurement))
                    {
                        ConstructorInfo cons = type.GetConstructor(new Type[] {typeof (double)});
                        singleValue = cons.Invoke(new object[] {Convert.ToDouble(singleValue)});
                    }
                    string sval = SerValue(singleValue);
                    if (!String.IsNullOrEmpty(sval)) sb.Append('|').Append(sval);
                }
            }
            return sb;
        }


		/// <summary>
		/// Make sure string builder ending is ready for appending more k|v pairs
		/// </summary>
		/// <param name="sb"></param>
		public static void PrepareKV(StringBuilder sb)
		{
			if (sb.Length > 0)
			{
				if (sb[sb.Length - 1] == '|')
				{
					if (sb.Length <= 2 || sb[sb.Length - 2] != '|') sb.Append('|');
				}
				else sb.Append("||");
			}
		}


		public static IDictionary MakeFormattedValues(string s, ImardaFormatProvider ifp)
		{
			var map = new HybridDictionary();
			Deserialize(s, map, ifp);
			return map;
		}

		public static void Deserialize(string s, Dictionary<string, object> dict)
		{
			Deserialize(s, dict, null);
		}


		/// <summary>
		/// Deserialize the typed value string into a dictionary.
		/// </summary>
		/// <param name="s"></param>
		/// <param name="map"></param>
		/// <param name="ifp"></param>
		public static void Deserialize(string s, IDictionary map, ImardaFormatProvider ifp)
		{
			IDictionary hmap = StringUtils.KeyValueMap(s, ValueFormat.Mix, true);
			foreach (string key in hmap.Keys)
			{
				string name;
				Type type;
				int vari;
				string format;

				AnalyzeKey(key, out name, out type, out vari, out format);

				object val = hmap[key];
				if (vari >= 0)
				{
					bool placeholderInKey = name.Contains("_");
					string[] arr = (val is string) ? new[] { (string)val } : (string[])val;
					int n = Math.Max(arr.Length, vari);
					var target = ifp != null ? new string[n] : Array.CreateInstance(type, n);
					for (int i = 0; i < n; i++)
					{
						object v0 = (i >= arr.Length) ? GetDefaultValue(type) : Parse(type, arr[i]);
						if (ifp != null) v0 = Format(format, v0, ifp);
						target.SetValue(v0, i);
						if (vari > i)
						{
							string num = (i + 1).ToString();
							string keyi = placeholderInKey ? name.Replace("_", num) : name + "_" + num;
							map[keyi] = v0;
						}
					}
					map[name] = target;
				}
				else
				{
					object v0 = Parse(type, (string)val);
					if (ifp != null) v0 = Format(format, v0, ifp);
					map[name] = v0;
				}
			}
		}


		internal static string Format(string format, object val, ImardaFormatProvider ifp)
		{
			if (val is DateTime)
			{
				//DateTime dt = (DateTime)val;
				//if (dt.Kind != DateTimeKind.Utc) throw new Exception();
				string s;
				if (ifp.ForceDefaultDateFormat)
				{
					DateTime? dtl = ImardaFormatProvider.DateTimeInUserZone((DateTime)val, ifp.TimeZoneInfo);
					if (dtl.HasValue)
					{
						string contextFormat = ifp.GetDateFormat("") ?? "g";
						s = dtl.Value.ToString(contextFormat, ifp.DefaultCulture);
					}
					else s = String.Empty;
				}
				else
				{
					string fmt = format != null ? "{0:" + format + "}" : "{0:z} {0:t}";
					s = String.Format(ifp, fmt, val);
				}
				return s;
			}
			return ifp.Format(format, val, null);
		}

		private static object Parse(Type type, string val)
		{
			if (typeof(IMeasurement).IsAssignableFrom(type))
			{
				ConstructorInfo cons = type.GetConstructor(new Type[] { typeof(double) });
				return cons.Invoke(new object[] { Convert.ToDouble(val) });
			}
			if (type == typeof(string)) return val;
			if (type == typeof(bool)) return val == "1";
			if (type == typeof(DateTime))
			{
				if (val == "\u00B6d") return DateTime.MinValue;
				DateTime dt = DateTime.ParseExact(val, "s", null); // strangely, DateTimeStyles.AssumeUniversal makes the time local
				dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
				return dt;
			}
			if (type == typeof(double)) return Convert.ToDouble(val);
			if (type == typeof(Guid)) return String.IsNullOrEmpty(val) || val == "0" ? Guid.Empty : new Guid(val);
			if (type == typeof(Enum)) return (AnyEnum)Convert.ToInt32(val);
			throw new Exception("Unknown type: " + type);
		}

		/// <summary>
		/// Interpret the data as given type. Special cases: bool allow "1" to represent true, 
		/// IMeasurement: expect PString, DateTime: read as UTC.
		/// </summary>
		/// <param name="data">the data to be parsed</param>
		/// <param name="type">output type of data</param>
		/// <param name="value">the parsed data as the requested type</param>
		/// <returns>true if successful, false if cannot parse</returns>
		public static bool TryParse(string data, Type type, out object value)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = type.GetGenericArguments()[0];
			}
			value = null;
			try
			{
				if (type == typeof(string))
				{
					value = data;
				}
				else if (typeof(IMeasurement).IsAssignableFrom(type))
				{
					value = Measurement.Parse(type, data);
				}
				else if (type == typeof(bool))
				{
					value = data == "1" || data.ToLowerInvariant() == "true";
				}
				else if (typeof(IConvertible).IsAssignableFrom(type))
				{
					if (data.Trim() == "")
					{
						//handle defaults, in particular for uninitialized Custom Attributes this could cause trouble before
						if (type == typeof(double)) value = 0.0;
						else if (type == typeof(DateTime)) value = DateTime.MinValue;
						// bool and string are dealt with above.
					}
					else if (type == typeof(DateTime))
					{
						value = DateTime.Parse(data, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
						value = DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
					}
					else
					{
						value = Convert.ChangeType(data, type);
					}
				}
				else if (type == typeof(Guid))
				{
					value = string.IsNullOrEmpty(data) || data == "0" || data == "0.0" ? Guid.Empty : new Guid(data);
				}
				else return false;
				return true;
			}
			catch (Exception ex)
			{
				DebugLog.Write("TryParse: {0} ({1}): {2}", data, type, ex);
				return false;
			}
		}


		public enum AnyEnum { Null = 0 }

		/// <summary>
		/// Analyze the information in the key.
		/// </summary>
		/// <param name="key">contains the name, type and format information, e.g. "Dimensions!Length[3] ~eng"</param>
		/// <param name="name">variable name, e.g. "Dimensions!Length[3] ~eng" => Dimensions</param>
		/// <param name="type">type, e.g. "Dimensions!Length[3] ~eng" => Length</param>
		/// <param name="vari">how many variables to create from indices, e.g. "Dimensions!Length[3] ~eng" => 3, Dimensions_1, Dimensions_2, Dimensions_3</param>
		/// <param name="format"></param>
		public static void AnalyzeKey(string key, out string name, out Type type, out int vari, out string format)
		{
			Match m = _rxKeyAnalyzer.Match(key);
			name = m.Groups[1].Value.Replace('#', '_');
			try
			{
				type = TypeFromSymbol(m.Groups[2].Value, m.Groups[3].Value);
			}
			catch (Exception ex)
			{
				throw new Exception(
					"key:" + key + Environment.NewLine + "symbol:" + m.Groups[2].Value + ";structName:" + m.Groups[3].Value, ex);
			}
			string grp4 = m.Groups[4].Value;
			if (grp4 == "") vari = -1;
			else if (grp4 == "[]") vari = 0;
			else vari = Int32.Parse(grp4.Trim('[', ']'));

			format = String.IsNullOrEmpty(m.Groups[5].Value) ? null : m.Groups[5].Value;
		}

		//& IM-3424
		public static string GetDefaultValueString(Type type)
		{
			if (type == typeof(String)) return Pilcrow;
			if (type == typeof(double)) return "0.0";
			if (type == typeof(DateTime)) return Pilcrow + "d";
			if (type == typeof(bool)) return "0";
			return "0.0";
		}
		//. IM-3424

        //& IM-3747
        public static string GetDefaultValueString(Char theType)
        {
            switch (theType)
            {
                case '!': return "0.0";
                case '$': return "";    // I was using "Pilcrow" here, but HistoryWeek processing was falling over when empty strings get Pilcrow (Dave - IM-3747)
                case '?': return "0";
                case '@': return Pilcrow + "d";
                default: return "0.0";
            }
        }
        //. IM-3747

		public static Type TypeFromSymbol(string symbol, string structName)
		{
			switch (symbol[0])
			{
				case '*': return typeof(Guid);
				case '!': return string.IsNullOrEmpty(structName) ? typeof(double) : Measurement.GetType(structName);
				case '?': return typeof(bool);
				case '$': return typeof(string);
				case '@': return typeof(DateTime);
				case '&': return typeof(Enum);
				default: throw new ArgumentException("Unknown type symbol: " + symbol);
			}
		}

		private static object GetDefaultValue(Type type)
		{
			if (type == typeof(string)) return null;
			if (type is IMeasurement)
			{
				ConstructorInfo c = type.GetConstructor(new[] { typeof(double) });
				return c.Invoke(new object[] { 0.0 });
			}
			return Activator.CreateInstance(type);
		}

		/// <summary>
		/// Get name of simple variable (non-array)
		/// </summary>
		/// <param name="key">E.g. "ODO!Length ~dist"</param>
		/// <returns>first identifier without _ and # </returns>
		public static string GetSimpleVarName(string key)
		{
			var sb = new StringBuilder();
			foreach (char c in key)
			{
				if (char.IsLetterOrDigit(c)) sb.Append(c);
				else
				{
					if (c == '#' || c == '_') sb.Length = 0; // do not return names of arrays
					break;
				}
			}
			return sb.ToString();
		}

	}
}