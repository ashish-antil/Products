using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	[DataContract]
	public struct Area : IMeasurement
	{
		public const double SquareMetresPerAcre = 4046.8564224;
		public static readonly MUnit Unit = new MUnit(2, 0, 0, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public Area(double m2)
		{
			_SI = m2;
		}

		[Unit("sqm")]
		[Unit("m\u00B2")]
		public double InSquareMetre
		{
			get { return _SI; }
		}

		[Unit("ha")]
		public double InHectare
		{
			get { return _SI/10000.0; }
		}

		[Unit("acre")]
		public double InAcre
		{
			get { return _SI/SquareMetresPerAcre; }
		}

		[Unit("mi\u00B2")]
		[Unit("sqmi")]
		public double InSquareMile
		{
			get { return _SI/(Length.MetrePerMile*Length.MetrePerMile); }
		}

		[Unit("ft\u00B2")]
		[Unit("sqft")]
		public double InSquareFoot
		{
			get { return _SI/(Length.MetrePerFoot*Length.MetrePerFoot); }
		}

		[Unit("in\u00B2")]
		[Unit("sqin")]
		public double InSquareInch
		{
			get { return _SI/(Length.MetrePerInch*Length.MetrePerInch); }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("187caf1d-68d1-499a-855c-35107c9435c6"); }
		}

		public string MetricSymbol
		{
			get { return "m\u00B2"; }
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

		[Parse("sqm")]
		[Parse("m2")]
		public static Area SquareMetre(double m2)
		{
			return new Area(m2);
		}

		[Parse("ha")]
		public static Area Hectare(double hectare)
		{
			return new Area(hectare*10000.0);
		}

		[Parse("acre")]
		public static Area Acre(double acre)
		{
			return new Area(acre*SquareMetresPerAcre);
		}

		[Parse("sqmi")]
		public static Area SquareMile(double sqmi)
		{
			return new Area(sqmi*Length.MetrePerMile*Length.MetrePerMile);
		}

		[Parse("sqft")]
		public static Area SquareFoot(double sqft)
		{
			return new Area(sqft*Length.MetrePerFoot*Length.MetrePerFoot);
		}

		[Parse("sqin")]
		public static Area SquareInch(double sqin)
		{
			return new Area(sqin*Length.MetrePerInch*Length.MetrePerInch);
		}

		public static Area operator +(Area n1, Area n2)
		{
			return new Area(n1._SI + n2._SI);
		}

		public static Area operator -(Area n1, Area n2)
		{
			return new Area(n1._SI - n2._SI);
		}

		public static implicit operator Measurement(Area x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Area(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Area(x.Value);
		}

		
		public static bool operator >(Area x1, Area x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Area x1, Area x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Area x1, Area x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Area x1, Area x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Area x1, Area x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Area x1, Area x2)
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
			if (obj.GetType() != typeof(Area)) return false;
			return ((Area)obj)._SI == _SI;
		}

		//EndOfBody Area


	}
}