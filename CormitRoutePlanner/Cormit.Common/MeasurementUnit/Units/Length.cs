using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	[DataContract]
	public struct Length : IMeasurement
	{
		public const double MetrePerMile = 1609.344;
		public const double MetrePerFoot = 0.3048;
		public const double MetrePerYard = 0.9144;
		public const double MetrePerInch = 0.0254;

		public static readonly MUnit Unit = new MUnit(1, 0, 0, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public Length(double m)
		{
			_SI = m;
		}

		[Unit("mm")]
		public double InMillimetre
		{
			get { return _SI*1000.0; }
		}

		[Unit("m")]
		public double InMetre
		{
			get { return _SI; }
		}

		[Unit("km")]
		public double InKilometre
		{
			get { return _SI/SIPrefix.Kilo; }
		}

		[Unit("mi")]
		public double InMile
		{
			get { return _SI/MetrePerMile; }
		}

		[Unit("ft")]
		[Unit("'")]
		[Unit("\u2032")] // prime (similar to ')
			public double InFoot
		{
			get { return _SI/MetrePerFoot; }
		}

		[Unit("yd")]
		public double InYard
		{
			get { return _SI/MetrePerYard; }
		}

		[Unit("in")]
		[Unit("\"")]
		[Unit("\u2033")] // double prime (similar to ")
			public double InInch
		{
			get { return _SI/MetrePerInch; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("bee9b6ab-f0d3-4fc5-ae50-fb0eee242c00"); }
		}

		public string MetricSymbol
		{
			get { return "m"; }
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return Measurement.DefaultFormatHandler(format, this, formatProvider);
		}

		public string SpecificFormat(string format, IFormatProvider formatProvider)
		{
			return AsMeasurement().ToString();
		}


		public Measurement AsMeasurement()
		{
			return this;
		}

		#endregion

		[Parse("m")]
		public static Length Metre(double m)
		{
			return new Length(m);
		}

		[Parse("km")]
		public static Length Kilometre(double km)
		{
			return new Length(km*SIPrefix.Kilo);
		}

		[Parse("mi")]
		public static Length Mile(double miles)
		{
			return new Length(miles*MetrePerMile);
		}

		[Parse("ft")]
		public static Length Foot(double foot)
		{
			return new Length(foot*MetrePerFoot);
		}

		[Parse("yd")]
		public static Length Yard(double yard)
		{
			return new Length(yard*MetrePerYard);
		}

		[Parse("in")]
		public static Length Inch(double inch)
		{
			return new Length(inch*MetrePerInch);
		}

		public static Length operator +(Length n1, Length n2)
		{
			return new Length(n1._SI + n2._SI);
		}

		public static Length operator -(Length n1, Length n2)
		{
			return new Length(n1._SI - n2._SI);
		}

		public double InSI(double prefix)
		{
			return _SI/prefix;
		}

		public static implicit operator Measurement(Length x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Length(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Length(x.Value);
		}


		
		public static bool operator >(Length x1, Length x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Length x1, Length x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Length x1, Length x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Length x1, Length x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Length x1, Length x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Length x1, Length x2)
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
			if (obj.GetType() != typeof(Length)) return false;
			return ((Length)obj)._SI == _SI;
		}

		//EndOfBody Length


	}
}