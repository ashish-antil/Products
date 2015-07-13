using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	[DataContract]
	public struct Volume : IMeasurement
	{
		public const double CubicMetrePerLitre = 0.001;
		public const double CubicMetrePerCC = 1e-6;
		public const double CubicMetrePerUSLiquidGallon = 0.003785411784;
		//public const double CubicMetrePerUSDryGallon = 0.00440488377086;
		public const double CubicMetrePerImperialGallon = 0.00454609188;
		public static readonly MUnit Unit = new MUnit(3, 0, 0, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public Volume(double m3)
		{
			_SI = m3;
		}

		[Unit("m\u00B3")]
		public double InCubicMetre
		{
			get { return _SI; }
		}

		[Unit("L")]
		public double InLitre
		{
			get { return _SI/CubicMetrePerLitre; }
		}

		[Unit("gal(US)", Display = "gal")]
		public double InUSGallon
		{
			get { return _SI/CubicMetrePerUSLiquidGallon; }
		}

		[Unit("gal(Imp)", Display = "gal")]
		public double InImpGallon
		{
			get { return _SI/CubicMetrePerImperialGallon; }
		}

		[Unit("cc")]
		public double InCC
		{
			get { return _SI/CubicMetrePerCC; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("3a96abed-943e-4cd5-aa3d-9e513741c623"); }
		}

		public string MetricSymbol
		{
			get { return "m\u00B3"; }
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

		[Parse("m3")]
		public static Volume CubicMetre(double m3)
		{
			return new Volume(m3);
		}

		[Parse("L")]
		public static Volume Litre(double litre)
		{
			return new Volume(litre*CubicMetrePerLitre);
		}

		/// <summary>
		/// Cubic centimetres (e.g. engine cylinder)
		/// </summary>
		/// <param name="cc"></param>
		/// <returns></returns>
		[Parse("cc")]
		public static Volume CC(double cc)
		{
			return new Volume(cc*CubicMetrePerCC);
		}

		/// <summary>
		/// Create volume in US Gallons
		/// </summary>
		[Parse("gal")]
		[Parse("gal-us")]
		public static Volume USGallon(double gal)
		{
			return new Volume(gal*CubicMetrePerUSLiquidGallon);
		}

		/// <summary>
		/// Create volume in Imperial Gallons
		/// </summary>
		[Parse("gal-i")]
		public static Volume ImpGallon(double gal)
		{
			return new Volume(gal*CubicMetrePerImperialGallon);
		}

		public static Volume operator+(Volume v1, Volume v2)
		{
			return new Volume(v1._SI + v2._SI);
		}

		public static Volume operator -(Volume v1, Volume v2)
		{
			return new Volume(v1._SI - v2._SI);
		}


		public static implicit operator Measurement(Volume x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Volume(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Volume(x.Value);
		}


		
		public static bool operator >(Volume x1, Volume x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Volume x1, Volume x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Volume x1, Volume x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Volume x1, Volume x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Volume x1, Volume x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Volume x1, Volume x2)
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
			if (obj.GetType() != typeof(Volume)) return false;
			return ((Volume)obj)._SI == _SI;
		}

		//EndOfBody Volume


	}
}