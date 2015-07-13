using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

// See http://en.wikipedia.org/wiki/SI_derived_unit

namespace Imarda.Lib
{
	/// <summary>
	/// General measurement value. This consists of a real value and a unit.
	/// </summary>
	[DataContract]
	public struct Measurement : IComparable, IMeasurement
	{
		public static Measurement Zero;
		public static Measurement One = new Measurement(1.0, MUnit.NoUnit);

		[DataMember] private MUnit _Unit;
		[DataMember] private double _Value;

		/// <summary>
		/// Create a scalar measurement.
		/// </summary>
		/// <param name="value"></param>
		public Measurement(double value)
		{
			_Value = value;
			_Unit = MUnit.NoUnit;
		}

		/// <summary>
		/// Create a measurement with the given unit.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		public Measurement(double value, MUnit unit)
		{
			_Value = value;
			_Unit = unit;
		}

		/// <summary>
		/// Create a measurement with the given unit but multiply the measurement
		/// with the prefix. E.g. new Measurement(SIPrefix.centi, 20.0, Length.Unit)
		/// </summary>
		/// <param name="prefix">multiplier</param>
		/// <param name="value">base value</param>
		/// <param name="unit">unit of measurement</param>
		public Measurement(double prefix, double value, MUnit unit)
		{
			_Value = prefix*value;
			_Unit = unit;
		}

		/// <summary>
		/// The value part, in SI units.
		/// </summary>
		public double Value
		{
			get { return _Value; }
		}

		/// <summary>
		/// The unit.
		/// </summary>
		public MUnit Unit
		{
			get { return _Unit; }
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			var other = (Measurement) obj;
			if (Unit != other.Unit) throw new ArgumentException();
			if (other.Value > Value) return -1;
			return other.Value < Value ? 1 : 0;
		}

		#endregion

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("f1d256dc-26bd-4889-9845-54a75f72782e"); }
		}

		public string MetricSymbol
		{
			get { return string.Empty; }
		}


		public string ToString(string format, IFormatProvider formatProvider)
		{
			var mfi = formatProvider as MeasurementFormatInfo;
			return mfi != null ? mfi.Format(format, this, null) : AsMeasurement().ToString();
		}

		public Measurement AsMeasurement()
		{
			return this;
		}

		#endregion

		/// <summary>
		/// Pass in a prefix, e.g. m.InSI(SIPrefix.Centi)
		/// </summary>
		/// <param name="prefix"></param>
		/// <returns></returns>
		public double InSI(double prefix)
		{
			return _Value/prefix;
		}

		public static Measurement Add(Measurement x1, Measurement x2)
		{
			if (x1.Unit != x2.Unit) throw new ArgumentException();
			return new Measurement(x1.Value + x2.Value, x1.Unit);
		}

		public static Measurement Subtract(Measurement x1, Measurement x2)
		{
			if (x1.Unit != x2.Unit) throw new ArgumentException();
			return new Measurement(x1.Value - x2.Value, x1.Unit);
		}

		public static Measurement operator -(Measurement x)
		{
			return new Measurement(-x.Value, x.Unit);
		}

		public static Measurement operator *(Measurement x1, Measurement x2)
		{
			return new Measurement(x1.Value*x2.Value, x1.Unit + x2.Unit);
		}

		public static Measurement operator /(Measurement x1, Measurement x2)
		{
			return new Measurement(x1.Value/x2.Value, x1.Unit - x2.Unit);
		}

		public static Measurement Multiply(Measurement x1, Measurement x2)
		{
			return new Measurement(x1.Value*x2.Value, x1.Unit + x2.Unit);
		}

		public static Measurement Divide(Measurement x1, Measurement x2)
		{
			return new Measurement(x1.Value/x2.Value, x1.Unit - x2.Unit);
		}

		public static Measurement operator *(double scalar, Measurement x)
		{
			return new Measurement(scalar*x.Value, x.Unit);
		}

		public static Measurement operator *(Measurement x, double scalar)
		{
			return new Measurement(scalar*x.Value, x.Unit);
		}

		public static Measurement operator /(Measurement x, double scalar)
		{
			return new Measurement(x.Value/scalar, x.Unit);
		}

		private static void CheckUnits(Measurement x1, Measurement x2)
		{
			if (x1.Unit != x2.Unit) throw new ArgumentException("Unit mismatch: [" + x1.Unit + "] <> [" + x2.Unit + "]");
		}

		public static bool operator >(Measurement x1, Measurement x2)
		{
			CheckUnits(x1, x2);
			return x1.Value > x2.Value;
		}

		public static bool operator <(Measurement x1, Measurement x2)
		{
			CheckUnits(x1, x2);
			return x1.Value < x2.Value;
		}

		public static bool operator >=(Measurement x1, Measurement x2)
		{
			CheckUnits(x1, x2);
			return x1.Value >= x2.Value;
		}

		public static bool operator <=(Measurement x1, Measurement x2)
		{
			CheckUnits(x1, x2);
			return x1.Value <= x2.Value;
		}


		public static bool operator ==(Measurement x1, Measurement x2)
		{
			CheckUnits(x1, x2);
			return x1.Value == x2.Value;
		}

		public static bool operator !=(Measurement x1, Measurement x2)
		{
			CheckUnits(x1, x2);
			return x1.Value != x2.Value;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() != typeof (Measurement)) return false;
			return this == (Measurement) obj;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", _Value, _Unit).TrimEnd();
		}


		public static Type GetType(string structName)
		{
			return Assembly.GetExecutingAssembly().GetType(typeof (Measurement).Namespace + '.' + structName);
		}

		/// <summary>
		/// Return a string after reinterpreting the Measurement as being of the given type, using
		/// the given formatter.
		/// </summary>
		/// <param name="name">helper struct name, e.g. Time, Speed, Temperature</param>
		/// <param name="format">interpreted by the MeasurementFormatInfo</param>
		/// <returns>measurement formatted as indicated</returns>
		public string ToString(string name, string format)
		{
			Type type = GetType(name);
			ConstructorInfo c = type.GetConstructor(new[] {typeof (double)});
			var obj = (IFormattable) c.Invoke(new object[] {Value});
			return obj.ToString(format, MeasurementFormatInfo.Default);
		}

		/// <summary>
		/// Returns a string that can be parsed back into a Measurement or helper struct.
		/// This format can be stored in a database varchar.
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public static string PString(IMeasurement m)
		{
			string name;
			string unit;
			if (m is Measurement)
			{
				name = "M";
				unit = ';' + ((Measurement) m).Unit.PString();
			}
			else
			{
				name = m.GetType().Name;
				unit = string.Empty;
			}
			return name + ":" + m.AsMeasurement()._Value.ToString("R") + unit;
		}

		/// <summary>
		/// Returns a string that contains only the value in SI units but not the unit.
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public static string VString(IMeasurement m)
		{
			return m.AsMeasurement()._Value.ToString("R");
		}


		public static string DefaultFormatHandler(string format, IMeasurement arg, IFormatProvider formatProvider)
		{
			MeasurementFormatInfo mfi;
			if (formatProvider is ImardaFormatProvider)
			{
				mfi = ((ImardaFormatProvider) formatProvider).MeasurementFormatProvider;
			}
			else
			{
				mfi = formatProvider as MeasurementFormatInfo;
			}

			if (format != null && format.Length > 1 && format[0] == '>')
			{
				// specific formatting
				return arg.SpecificFormat(format.Substring(1), mfi);
			}
			else
			{
				// standard formatting
				if (mfi != null) return mfi.Format(format, arg, null);
			}
			return arg.AsMeasurement().ToString();
		}

		#region parse methods

		private static readonly Regex _PStringRegex = new Regex(@"M:([^;]+);([\d,-]+)", RegexOptions.Compiled);

		public string SpecificFormat(string format, IFormatProvider formatProvider)
		{
			return AsMeasurement().ToString();
		}

		/// <summary>
		/// Parse a string representing the value of a measurment helper struct and return the struct.
		/// </summary>
		/// <typeparam name="T">Length, Volume, Mass etc.</typeparam>
		/// <param name="pstring">"Length:30" or "Volume:10.93" etc</param>
		/// <returns>the struct value</returns>
		public static T Parse<T>(string pstring)
			where T : struct, IMeasurement
		{
			int p = pstring.IndexOf(':');
			int q = pstring.IndexOf(' ', p + 1);
			if (q == -1) q = pstring.Length;
			string typename = pstring.Substring(0, p);
			if (typeof (T).Name != typename)
			{
				throw new ArgumentException(pstring + " is not " + typeof (T));
			}
			string valstring = pstring.Substring(p + 1, q - p - 1);
			double val = double.Parse(valstring);
			ConstructorInfo c = typeof (T).GetConstructor(new[] {typeof (double)});
			var m = (T) c.Invoke(new object[] {val});
			return m;
		}

		/// <summary>
		/// Parse any measurement string and return an IMeasurement value.
		/// </summary>
		/// <param name="pstring">"Length:23 ~dist", "Speed:36.8", "M:239;1,0,-1,0,0,0,0", etc.</param>
		/// <returns>Measurement if pstring starts with "M:", or specific IMeasurement struct otherwise</returns>
		public static IMeasurement Parse(string pstring)
		{
			try
			{
				if (pstring.StartsWith("M:"))
				{
					Match match = _PStringRegex.Match(pstring);
					string valstring = match.Groups[1].Value;
					string expstring = match.Groups[2].Value;
					var measurement = new Measurement(double.Parse(valstring), MUnit.Parse(expstring));
					return measurement;
				}
				else return ParseSpecific(pstring);
			}
			catch
			{
				throw new FormatException(string.Format("\"{0}\" invalid pstring format", pstring));
			}
		}

		public static IMeasurement Parse(Type type, string pstring)
		{
			if (pstring.StartsWith(type.Name + ':')) 
                return Parse(pstring);
			else
			{
				double x;
				if (double.TryParse(pstring, out x))
				{
					return (IMeasurement) type.GetConstructor(new[] {typeof (double)}).Invoke(new object[] {x});
				}
				else throw new FormatException(string.Format("Cannot parse: {0} as {1}", pstring, type));
			}
		}

		/// <summary>
		/// Format a measurement string with format.
		/// </summary>
		/// <param name="fstring">"Length:23 km;0.000", "Angle:1.9893949 ~brg"</param>
		/// <returns>formatted value</returns>
		public static string Format(string fstring, MeasurementFormatInfo mfi)
		{
			if (mfi == null) return Parse(fstring).ToString(null, null);

			string[] parts = fstring.Split(new[] {' '}, 2);
			string pstring = parts[0];
			string fmt = parts.Length > 1 ? parts[1] : null;
			return Parse(pstring).ToString(fmt, mfi);
		}


		/// <summary>
		/// Creates an string representation of the measurement with an indication 
		/// of the default formatting. If Format() is called on the return value
		/// of this method then we get the formatted string.
		/// </summary>
		/// <param name="m"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static string FString(IMeasurement m, string fmt)
		{
			return PString(m) + ' ' + fmt;
		}


		public static string ChopUnit(string measurementWithUnit)
		{
			return measurementWithUnit.Split(' ')[0]; //TODO change to work for prefix units or no-space units as well.
		}

		public static void SplitUnit(string measurementWithUnit, out string val, out string unit)
		{
			//TODO handle prefix units like "$ 123" and also handle cases where the space is missing like "123.5m/s", or where multiple spaces exist
			string[] split = measurementWithUnit.Split(' ');
			val = split[0];
			unit = (split.Length > 1) ? split[1] : string.Empty;
		}

		/// <summary>
		/// Parse a measurement type other then the generic Measurement.
		/// </summary>
		/// <param name="pstring">PString or FString "Area:39", "Speed:39.9 ~avg", etc.</param>
		/// <returns>an value of an IMeasurement struct such as Speed, Area, etc., but not a Measurement struct</returns>
		private static IMeasurement ParseSpecific(string pstring)
		{
			int p = pstring.IndexOf(':');
			int q = pstring.IndexOf(' ', p + 1);
			if (q == -1) q = pstring.Length;
			string typename = pstring.Substring(0, p);
			string valstring = pstring.Substring(p + 1, q - p - 1);
			double val = double.Parse(valstring);
			Type type = GetType(typename);
			ConstructorInfo cons = type.GetConstructor(new[] {typeof (double)});
			return (IMeasurement) cons.Invoke(new object[] {val});
		}

		#endregion

		
		public static T ParseInput<T>(string input)
		{
			string val;
			string mUnit;
			SplitUnit(input, out val, out mUnit);
			return (T)UnitParser.Instance.Create(Convert.ToDouble(val), mUnit);
		}
		

		//EndOfBody
	}
}