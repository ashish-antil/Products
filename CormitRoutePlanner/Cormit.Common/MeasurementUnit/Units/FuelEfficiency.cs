using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	[DataContract]
	public struct FuelEfficiency : IMeasurement
	{
		public static readonly MUnit Unit = new MUnit(2, 0, 0, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public FuelEfficiency(double m2)
		{
			_SI = m2;
		}


		[Unit("sqm")]
		[Unit("m\u00B2")]
		public double InSquareMetre
		{
			get { return _SI; }
		}

		[Unit("L/100km")]
		public double InLitrePer100km
		{
			get { return _SI*1e8; }
		}

		[Unit("gal/100mi")]
		public double InGallonPer100Mile
		{
			get { return (_SI*1e8)/(1e6*Volume.CubicMetrePerUSLiquidGallon/Length.MetrePerMile); }
		}

		[Unit("km/L")]
		public double InKmPerLitre
		{
			get { return 1e-6/_SI; }
		}

		[Unit("mpg(US)", Display = "mpg")]
		public double InMilePerUSGallon
		{
			get { return (1e-6/_SI)*(1e6*Volume.CubicMetrePerUSLiquidGallon/Length.MetrePerMile); }
		}

		[Unit("mpg(Imp)", Display = "mpg")]
		public double InMilePerImperialGallon
		{
			get { return (1e-6/_SI)*(1e6*Volume.CubicMetrePerImperialGallon/Length.MetrePerMile); }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("28bc1811-f8fb-4de0-a85b-d91ed61c0e02"); }
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

		/// <summary>
		/// Cubic metres of fuel per metres covered distance = square metres.
		/// In fact this is the area of a cross section of a tube filled with fuel along
		/// the path travelled. This makes the expression of fuel efficiency independent
		/// of a standard distance, such as 100km, 100miles etc, or a standard volume unit 
		/// as litre, gallon etc.
		/// </summary>
		/// <param name="m2"></param>
		/// <returns></returns>
		public static FuelEfficiency SquareMetre(double m2)
		{
			return new FuelEfficiency(m2);
		}

		[Parse("L/100km")]
		public static FuelEfficiency LitrePer100km(double lp100km)
		{
			return new FuelEfficiency(lp100km/1e8);
		}

		[Parse("gal/100mi")]
		[Parse("gal-us/100mi")]
		public static FuelEfficiency USGallonPer100Mile(double gp100mi)
		{
			double lp100km = gp100mi*(1e6*Volume.CubicMetrePerUSLiquidGallon/Length.MetrePerMile);
			return new FuelEfficiency(lp100km/1e8);
		}

		[Parse("gal-i/100mi")]
		public static FuelEfficiency ImpGallonPer100Mile(double gp100mi)
		{
			double lp100km = gp100mi*(1e6*Volume.CubicMetrePerImperialGallon/Length.MetrePerMile);
			return new FuelEfficiency(lp100km/1e8);
		}

		[Parse("km/L")]
		public static FuelEfficiency KmPerLitre(double kmpl)
		{
			return new FuelEfficiency(1e-6/kmpl);
		}

		[Parse("mpg")]
		[Parse("mpg-us")]
		public static FuelEfficiency MilePerUSGallon(double mpg)
		{
			double kmpl = mpg/(1e6*Volume.CubicMetrePerUSLiquidGallon/Length.MetrePerMile);
			return new FuelEfficiency(1e-6/kmpl);
		}

		[Parse("mpg-i")]
		public static FuelEfficiency MilePerImpGallon(double mpg)
		{
			double kmpl = mpg/(1e6*Volume.CubicMetrePerImperialGallon/Length.MetrePerMile);
			return new FuelEfficiency(1e-6/kmpl);
		}

		public static FuelEfficiency operator +(FuelEfficiency n1, FuelEfficiency n2)
		{
			return new FuelEfficiency(n1._SI + n2._SI);
		}

		public static FuelEfficiency operator -(FuelEfficiency n1, FuelEfficiency n2)
		{
			return new FuelEfficiency(n1._SI - n2._SI);
		}


		public static implicit operator Measurement(FuelEfficiency x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator FuelEfficiency(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new FuelEfficiency(x.Value);
		}


		
		public static bool operator >(FuelEfficiency x1, FuelEfficiency x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(FuelEfficiency x1, FuelEfficiency x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(FuelEfficiency x1, FuelEfficiency x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(FuelEfficiency x1, FuelEfficiency x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(FuelEfficiency x1, FuelEfficiency x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(FuelEfficiency x1, FuelEfficiency x2)
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
			if (obj.GetType() != typeof(FuelEfficiency)) return false;
			return ((FuelEfficiency)obj)._SI == _SI;
		}

		//EndOfBody FuelEfficiency


	}
}