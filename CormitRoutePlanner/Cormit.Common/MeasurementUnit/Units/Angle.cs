using System;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace Imarda.Lib
{
	/// <summary>
	/// An angle. This struct has some extra methods and constants for common calculations.
	/// </summary>
	[DataContract]
	public struct Angle : IMeasurement
	{
		public const double RadiansPerDegree = Math.PI/180.0; // = 0.0174532925...
		public const double RadiansPerCircle = Math.PI*2;
		public static readonly MUnit Unit = new MUnit(0, 0, 0, 0, 0, 0, 1);
		public static readonly Angle Zero = new Angle(0);
		public static readonly Angle FullCircle = new Angle(RadiansPerCircle);

		private static readonly Regex DigitGroups =
			new Regex(@"(?<C>[NE ])|(?<D>[-0#]+(?:\.[0#]+)?)(?<T>[ DdCc'\u2032""\u2033]?)", RegexOptions.Compiled);


		[DataMember] private double _SI;

		static Angle()
		{
			// Default bearing labels in English
			BearingLabels = new[] {"N", "NE", "E", "SE", "S", "SW", "W", "NW"};
		}

		public Angle(double radians)
		{
			_SI = radians;
		}

		public static string[] BearingLabels { get; set; }

		[Unit("rad")]
		public double InRadians
		{
			get { return _SI; }
		}

		[Unit("\u00B0")]
		[Unit("deg", Display = "\u00B0")]
		public double InDegrees
		{
			get { return _SI/RadiansPerDegree; }
		}

		public void InDegMinSec(out int deg, out int min, out double sec)
		{
			double a = Math.Abs(InDegrees);
			deg = (int)a;
			a -= deg;
			min = (int)(a *= 60);
			sec = (a - min) * 60;
			if (InDegrees < 0) deg = -deg;
		}

		/// <summary>
		/// 1 cycle is 2pi radians or 360 degrees
		/// </summary>
		[Unit("cycles")]
		[Unit("%", Factor = 0.01)]
		public double InCycles
		{
			get { return _SI/RadiansPerCircle; }
		}


		/// <summary>
		/// Gets the Bearing label (N,NE,E,etc.) representing this angle.
		/// </summary>
		public string Bearing8
		{
			get { return BearingLabels[IndexBearing8]; }
		}

		public string Bearing4
		{
			get { return BearingLabels[IndexBearing4 << 1]; }
		}

		public int IndexBearing8
		{
			get { return 7 & (int)((16 * WithinFull().InCycles + 1) / 2); }
		}
	
		/// <summary>
		/// Gets the Bearing quadrant label (N,E,S,W) representing this angle.
		/// </summary>
		public int IndexBearing4
		{
			get { return 3 & (int) ((8 * WithinFull().InCycles + 1) / 2); }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("d1f0c48f-a870-4983-9ec5-2cf431c16c32"); }
		}

		public string MetricSymbol
		{
			get { return "rad"; }
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return Measurement.DefaultFormatHandler(format, this, formatProvider);
		}

		/// <summary>
		/// Custom formatting for Angle values: degrees
		/// </summary>
		/// <param name="format">Should start with '>'</param>
		/// <param name="formatProvider"></param>
		/// <returns></returns>
		public string SpecificFormat(string format, IFormatProvider formatProvider)
		{
			// X = bearing label quadrants N S E W
			// x = bearing label quadrants n s e w
			// B = bearing label, e.g. NE for north-east.
			// b = bearing label lower case, e.g. sw for south-west.
			//Degrees:
			// + as first character: make sure degrees in range 0..360.
			// - as first character: make sure degrees in range -180..+180
			// d = positive or negative degrees
			// D = postive degrees (combine with N or E)
			// c = postive or negative degrees, good for links in google. Always uses decimal point, not comma.
			// C = positive degrees (combine with N or E). Always uses decimal point, not comma.
			// ' or \u2032 = arcminutes
			// " or \u2033 = arcseconds
			// N = north / south indicator: N for positive degrees, S for negative degrees, space for 0
			// E = east / west indicator: E for positive degrees, W for negative degrees, space for 0
			//Examples:
			//0d00'00.00"
			//0DN 00.000'
			//0.00000d
			//0d 0.000'
			//0.0000d
			//0DE 0\u2032 0.0\u2033	  using prime and double prime character for seconds (similar to ' and " but a better glyph)
			//0 #.000'

			switch (format)
			{
				case "B":
					return Bearing8;
				case "b":
					return Bearing8.ToLower();
				case "X":
					return Bearing4;
				case "x":
					return Bearing4.ToLower();
			}
			double deg;
			var sb = new StringBuilder();
			if (format.StartsWith("+"))
			{
				deg = WithinFull().InDegrees;
				format = format.Substring(1);
			}
			else if (format.StartsWith("-"))
			{
				deg = WithinHalf().InDegrees;
				format = format.Substring(1);
				if (deg == 0) sb.Append(' ');
				else if (deg > 0)
				{
					var mfi = formatProvider as MeasurementFormatInfo;
					if (mfi != null) sb.Append(mfi.NumberFormat.PositiveSign);
				}
			}
			else deg = InDegrees;

			MatchCollection matches = DigitGroups.Matches(format);
			double degAbs = Math.Abs(deg);
			double min = (degAbs*60.0 + 1e-10)%60.0; // add 1e-10 to avoid rounding problem
			foreach (Match m in matches)
			{
				string digits = m.Groups["D"].Value;
				string type = m.Groups["T"].Value;
				string other = m.Groups["C"].Value;
				if (!string.IsNullOrEmpty(other)) type = other;
				switch (type)
				{
					case "D":
						sb.Append(NumberToString(formatProvider, degAbs, digits));
						sb.Append('\u00B0');
						break;
					case "d":
						sb.Append(NumberToString(formatProvider, deg, digits));
						sb.Append('\u00B0');
						break;
					case "C":
						sb.Append(degAbs.ToString(digits));
						break;
					case "c":
						sb.Append(deg.ToString(digits));
						break;
					case "'":
					case "\u2032":
						sb.Append(NumberToString(formatProvider, min, digits));
						sb.Append("\u2032");
						break;
					case "\"":
					case "\u2033":
						double sec = (min * 60.0) % 60.0;
						sb.Append(NumberToString(formatProvider, sec, digits));
						sb.Append("\u2033");
						break;
					case " ":
						sb.Append(' ');
						break;
					case "":
						sb.Append(NumberToString(formatProvider, degAbs, digits));
						break;
					case "N":
						sb.Append(deg == 0.0 ? " " : deg > 0 ? BearingLabels[0] : BearingLabels[4]);
						break;
					case "E":
						sb.Append(deg == 0.0 ? " " : deg > 0 ? BearingLabels[2] : BearingLabels[6]);
						break;
				}
			}
			return sb.ToString();
		}

		
		private static string NumberToString(IFormatProvider formatProvider, double number, string digits)
		{
			// make sure decimal comma is possible.
			return new Unitless(number).ToString("*;" + digits, formatProvider);
		}

		public Measurement AsMeasurement()
		{
			return this;
		}

		#endregion

		[Parse("rad")]
		public static Angle Radians(double rad)
		{
			return new Angle(rad);
		}

		/// <summary>
		/// 360 degrees in a circle.
		/// </summary>
		/// <param name="degrees"></param>
		/// <returns></returns>
		[Parse("deg")]
		public static Angle Degrees(double degrees)
		{
			return new Angle(degrees*RadiansPerDegree);
		}

		public static Angle Degrees(int deg, int min, double sec)
		{
			double d = Math.Abs(deg) + Math.Abs(min) / 60.0 + Math.Abs(sec) / 3600.0;
			return Degrees(deg < 0 ? -d : d);
		}

		/// <summary>
		/// Fraction of a full circle: 0.5 is 180 degrees, 1 is 360 degrees.
		/// </summary>
		/// <param name="cycle"></param>
		/// <returns></returns>
		[Parse("cycle")]
		public static Angle Cycle(double cycle)
		{
			return new Angle(cycle*RadiansPerCircle);
		}

		/// <summary>
		/// Construct the angle based on the line through (0,0) and (x,y).
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Angle Coordinates(double x, double y)
		{
			return new Angle(Math.Atan2(y, x));
		}

		/// <summary>
		/// Get the bearing label of an angle in degrees, greater than or equal 0.
		/// </summary>
		/// <param name="positiveDegrees"></param>
		/// <returns></returns>
		public static string GetBearing(double positiveDegrees)
		{
			int sector = 7 & (int) ((16*(positiveDegrees/360.0) + 1)/2);
			return BearingLabels[sector];
		}

		public static Angle operator +(Angle n1, Angle n2)
		{
			return new Angle(n1._SI + n2._SI);
		}

		public static Angle operator -(Angle n1, Angle n2)
		{
			return new Angle(n1._SI - n2._SI);
		}

		public static implicit operator Measurement(Angle x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Angle(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Angle(x.Value);
		}

		/// <summary>
		/// Normalize the angle to a [0..2pi) range.
		/// </summary>
		/// <returns></returns>
		public Angle WithinFull()
		{
			double x = _SI%RadiansPerCircle;
			return new Angle(x < 0 ? x + RadiansPerCircle : x);
		}

		/// <summary>
		/// Normalize the angle to a (-pi,+pi] range.
		/// </summary>
		/// <returns></returns>
		public Angle WithinHalf()
		{
			double x = (_SI + Math.PI)%RadiansPerCircle;
			return new Angle(x < 0 ? x + Math.PI : x - Math.PI);
		}

		/// <summary>
		/// Find the opposite angle (i.e. rotate by 180degrees).
		/// </summary>
		/// <returns></returns>
		public Angle Opposite()
		{
			return new Angle(_SI + Math.PI);
		}

		/// <summary>
		/// Get this angle as an offset angle from the reference.
		/// </summary>
		/// <param name="reference"></param>
		/// <returns></returns>
		public Angle RelativeTo(Angle reference)
		{
			return new Angle(_SI - reference._SI).WithinHalf();
		}

		public double Cos()
		{
			return Math.Cos(_SI);
		}

		public double Sin()
		{
			return Math.Sin(_SI);
		}

		public double Tan()
		{
			return Math.Tan(_SI);
		}

		/// <summary>
		/// Get full degrees.
		/// </summary>
		/// <returns></returns>
		public int RoundDegrees()
		{
			return Convert.ToInt32(InDegrees);
		}


		
		public static bool operator >(Angle x1, Angle x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Angle x1, Angle x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Angle x1, Angle x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Angle x1, Angle x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Angle x1, Angle x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Angle x1, Angle x2)
		{
			return x1._SI != x2._SI;
		}

		public override int GetHashCode()
		{
			return _SI.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() != typeof(Angle)) return false;
			return ((Angle)obj)._SI == _SI;
		}

		//EndOfBody Angle


	}
}