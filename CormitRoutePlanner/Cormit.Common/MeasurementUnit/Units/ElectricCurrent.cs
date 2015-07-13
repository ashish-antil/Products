using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	[DataContract]
	public struct ElectricCurrent : IMeasurement
	{
		public static readonly MUnit Unit = new MUnit(0, 0, 0, 0, 1, 0, 0);

		[DataMember] private double _SI;

		public ElectricCurrent(double amp)
		{
			_SI = amp;
		}

		[Unit("A")]
		public double InAmpere
		{
			get { return _SI; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("e13795bb-2c56-4314-9065-439c8ccbb919"); }
		}

		public string MetricSymbol
		{
			get { return "A"; }
		}

		public static ElectricCurrent operator +(ElectricCurrent n1, ElectricCurrent n2)
		{
			return new ElectricCurrent(n1._SI + n2._SI);
		}

		public static ElectricCurrent operator -(ElectricCurrent n1, ElectricCurrent n2)
		{
			return new ElectricCurrent(n1._SI - n2._SI);
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

		[Parse("A")]
		public static ElectricCurrent Ampere(double amp)
		{
			return new ElectricCurrent(amp);
		}

		public static implicit operator Measurement(ElectricCurrent x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator ElectricCurrent(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new ElectricCurrent(x.Value);
		}


		
		public static bool operator >(ElectricCurrent x1, ElectricCurrent x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(ElectricCurrent x1, ElectricCurrent x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(ElectricCurrent x1, ElectricCurrent x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(ElectricCurrent x1, ElectricCurrent x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(ElectricCurrent x1, ElectricCurrent x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(ElectricCurrent x1, ElectricCurrent x2)
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
			if (obj.GetType() != typeof(ElectricCurrent)) return false;
			return ((ElectricCurrent)obj)._SI == _SI;
		}

		//EndOfBody ElectricCurrent


	}
}