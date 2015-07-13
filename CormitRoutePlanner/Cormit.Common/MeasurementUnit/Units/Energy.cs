using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	/// <summary>
	/// Energy, Work (Joule = m2 kg s-2)
	/// </summary>
	[DataContract]
	public struct Energy : IMeasurement
	{
		public const double JoulePerKWh = 3600000.0;
		public const double JoulePerHph = 2684519.54;
		public static readonly MUnit Unit = new MUnit(2, 1, -2, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public Energy(double joule)
		{
			_SI = joule;
		}

		[Unit("J")]
		public double InJoule
		{
			get { return _SI; }
		}

		[Unit("kWh")]
		public double InKiloWattHour
		{
			get { return _SI/JoulePerKWh; }
		}

		[Unit("hph")]
		public double InHorsePowerHour
		{
			get { return _SI/JoulePerHph; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("bc28b734-7284-450a-a02b-1a60c2538b97"); }
		}

		public string MetricSymbol
		{
			get { return "J"; }
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

		[Parse("J")]
		public static Energy Joule(double joule)
		{
			return new Energy(joule);
		}

		[Parse("kWh")]
		public static Energy KiloWattHour(double kWh)
		{
			return new Energy(kWh*JoulePerKWh);
		}

		[Parse("hph")]
		public static Energy HorsePowerHour(double hph)
		{
			return new Energy(hph*JoulePerHph);
		}

		public double InSI(double prefix)
		{
			return _SI/prefix;
		}

		public static Energy operator +(Energy n1, Energy n2)
		{
			return new Energy(n1._SI + n2._SI);
		}

		public static Energy operator -(Energy n1, Energy n2)
		{
			return new Energy(n1._SI - n2._SI);
		}


		public static implicit operator Measurement(Energy x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Energy(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Energy(x.Value);
		}


		
		public static bool operator >(Energy x1, Energy x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Energy x1, Energy x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Energy x1, Energy x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Energy x1, Energy x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Energy x1, Energy x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Energy x1, Energy x2)
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
			if (obj.GetType() != typeof(Energy)) return false;
			return ((Energy)obj)._SI == _SI;
		}

		//EndOfBody Energy


	}
}