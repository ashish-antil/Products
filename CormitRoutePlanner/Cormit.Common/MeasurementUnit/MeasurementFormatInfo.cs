using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Imarda.Lib
{
	public class MeasurementFormatInfo : IFormatProvider, ICustomFormatter
	{
		public static readonly MeasurementFormatInfo Default
			= new MeasurementFormatInfo(new CultureInfo("en-NZ").NumberFormat);

		internal static readonly char[] Sep = { ';' };
		private IDictionary<string, double> _ExchangeRates;

		private NumberFormatInfo _NumberFormat;
		private IDictionary<string, string> _Preferences = new Dictionary<string, string>();

		public MeasurementFormatInfo(NumberFormatInfo nfi)
		{
			NumberFormat = nfi;
			ValueUnitSeparator = " ";
		}

		public NumberFormatInfo NumberFormat
		{
			get { return _NumberFormat; }
			set
			{
				if (!value.IsReadOnly)
				{
					value.PositiveInfinitySymbol = "---";
					value.NegativeInfinitySymbol = "---";
					value.NaNSymbol = "?";
				}
				_NumberFormat = value;
			}
		}

		public string ValueUnitSeparator { get; set; }

		public IDictionary<string, double> ExchangeRates
		{
			get { return _ExchangeRates ?? (_ExchangeRates = new Dictionary<string, double>()); }
		}

		#region ICustomFormatter Members

		public string Format(string format, object arg, IFormatProvider provider)
		{
			if (format != null && format.StartsWith("~"))
			{
				// We have a context-specific unit request here. It means that multiple preferences
				// for the same measurement can exist, but they have a 'suffix' to indicate the context
				// of the measurement, e.g. Volume can be fuel[L], engine[cc], cargo[m3] and the format
				// now indicates what the suffix is. The measurement name with suffix becomes key to 
				// look up the format in the preferences table. The format may not be another suffix, 
				// but it can have a number format string part, which is default but can be overridden
				// by the format string part of the 'format' parameter as passed into this method.
				// E.g. pref table: "Volume~fuel"->"L;0", format param: "~fuel;0.0" then the result
				// format is: "L;0.0". This will be processed further as usual.
				string[] parts = format.Split(Sep, 2); //e.g. "~fuel;0.0" => "~fuel", "0.0"
				string context = parts[0];
				if (context == "~")
					context = string.Empty; // e.g. a fmt like "~;0.00" will lookup the preferred unit but overrides its number format
				string key = arg.GetType().Name + context; // e.g. "Volume~fuel"
				if (_Preferences.TryGetValue(key, out format))
				{
					if (!format.StartsWith("<") && parts.Length > 1)
					{
						string[] partsPref = format.Split(Sep, 2);
						format = partsPref[0] + ';' + parts[1];
					}
				}
				else
				{
					format = null;
				}
			}

			if (format != null && format.StartsWith("<"))
			{
				string compositeFormat = format.Substring(1).Replace("{", "{0:");
				return string.Format(this, compositeFormat, arg);
			}


			if (format == null)
			{
				string key = arg.GetType().Name;
				if (!_Preferences.TryGetValue(key, out format)) format = string.Empty;
			}

			var msr = arg as IMeasurement;
			if (format != string.Empty && msr != null)
			{
				char first = format[0];
				string[] parts = format.Split(Sep, 2);
				string nfmt = parts.Length > 1 ? parts[1] : "n";
				Measurement measure = msr.AsMeasurement();

				switch (first)
				{
					case '!': // e.g. kg.m2.s-1
						return string.Format(NumberFormat, "{0:" + nfmt + "} {1}", measure.Value, measure.Unit);

					case '^': // superscript 1,2,3, no dots between.
						return string.Format(NumberFormat, "{0:" + nfmt + "} {1}", measure.Value, measure.Unit.ToSuperscriptString());

					case '*': // unitless, multiply with a number
					case '/': // unitless, divide by a number
						double factor = parts[0].Length > 1 ? double.Parse(parts[0].Substring(1)) : 1.0;
						double result = (first == '*')
															? measure.Value * factor
															: measure.Value / factor;
						return result.ToString(nfmt, NumberFormat);

					case '>': // Type specific formatting
						return ((IFormattable)arg).ToString(format, this);

					default:
						string easyUnit = parts[0];
						string displayUnit = MUnit.DisplayUnit(easyUnit);
						string unit = displayUnit.TrimStart();
						if (unit == string.Empty)
						{
							unit = msr.MetricSymbol;
							displayUnit = easyUnit + unit;
						}

						UnitAttribute ua;
						PropertyInfo pinfo;
						if (UnitConverter.Instance.Get(arg.GetType(), unit, out ua, out pinfo))
						{
							var x = (double)pinfo.GetValue(arg, null);
							double val = x / ua.Factor;
							if (ua.UnitBeforeValue)
							{
								return unit + val.ToString(nfmt, NumberFormat);
							}
							if (ua.Display != null) displayUnit = displayUnit.Replace(ua.UnitSymbol, ua.Display);
							return val.ToString(nfmt, NumberFormat) + ValueUnitSeparator + displayUnit;
						}

						throw new FormatException("Invalid Measurement Format: " + format);
				}
			}

			if (arg is IFormattable)
			{
				return ((IFormattable)arg).ToString(format, provider);
			}
			else if (arg != null)
			{
				return arg.ToString();
			}
			else
			{
				return "(null)";
			}
		}

		#endregion

		#region IFormatProvider Members

		public object GetFormat(Type formatType)
		{
			return formatType == typeof(ICustomFormatter) ? this : null;
		}

		#endregion

		public void SetPreferences(IDictionary preferences)
		{
			_Preferences = new Dictionary<string, string>();
			foreach (string k in preferences.Keys) _Preferences.Add(k, (string)preferences[k]);
		}

		public string GetPreference(string key)
		{
			string format;
			return _Preferences.TryGetValue(key, out format) ? format : null;
		}
	}
}