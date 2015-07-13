using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	/// <summary>
	/// Power (Watt = m2 kg s-3)
	/// </summary>
	[DataContract]
	public struct Power : IMeasurement
	{
		public const double WattsPerHorsepower = 745.699872;
		public static readonly MUnit Unit = new MUnit(2, 1, -3, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public Power(double watt)
		{
			_SI = watt;
		}

		[Unit("W")]
		[Unit("kW", Factor = 1000)]
		public double InWatt
		{
			get { return _SI; }
		}


		[Unit("hp")]
		public double InMechanicalHorsepower
		{
			get { return _SI/WattsPerHorsepower; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("9bf7e022-d86f-472d-9501-b49139ec5398"); }
		}

		public string MetricSymbol
		{
			get { return "W"; }
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

		[Parse("kW")]
		public static Power KiloWatt(double kW)
		{
			return new Power(kW*1000);
		}

		[Parse("W")]
		public static Power Watt(double watt)
		{
			return new Power(watt);
		}

		[Parse("hp")]
		public static Power MechanicalHorsepower(double hp)
		{
			return new Power(hp*745.699872);
		}

		public double InSI(double prefix)
		{
			return _SI/prefix;
		}
		public static Power operator +(Power n1, Power n2)
		{
			return new Power(n1._SI + n2._SI);
		}

		public static Power operator -(Power n1, Power n2)
		{
			return new Power(n1._SI - n2._SI);
		}


		public static implicit operator Measurement(Power x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Power(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Power(x.Value);
		}


		
		public static bool operator >(Power x1, Power x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Power x1, Power x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Power x1, Power x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Power x1, Power x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Power x1, Power x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Power x1, Power x2)
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
			if (obj.GetType() != typeof(Power)) return false;
			return ((Power)obj)._SI == _SI;
		}

		//EndOfBody Power


	}
}