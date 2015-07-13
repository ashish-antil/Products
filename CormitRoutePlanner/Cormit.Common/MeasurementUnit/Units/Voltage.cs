using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	[DataContract]
	public struct Voltage : IMeasurement
	{
		public static readonly MUnit Unit = new MUnit(2, 1, -3, 0, -1, 0, 0); //  m2 kg s-3 A-1

		[DataMember] private double _SI;

		public Voltage(double si)
		{
			_SI = si;
		}

		[Unit("V")]
		public double InVolt
		{
			get { return _SI; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("f7257538-0aa5-4398-b6d6-c77b898a867e"); }
		}

		public string MetricSymbol
		{
			get { return "V"; }
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

		[Parse("V")]
		public static Voltage Volt(double x)
		{
			return new Voltage(x);
		}

		public static Voltage operator +(Voltage n1, Voltage n2)
		{
			return new Voltage(n1._SI + n2._SI);
		}

		public static Voltage operator -(Voltage n1, Voltage n2)
		{
			return new Voltage(n1._SI - n2._SI);
		}


		public static implicit operator Measurement(Voltage x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Voltage(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Voltage(x.Value);
		}


		
		public static bool operator >(Voltage x1, Voltage x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Voltage x1, Voltage x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Voltage x1, Voltage x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Voltage x1, Voltage x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Voltage x1, Voltage x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Voltage x1, Voltage x2)
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
			if (obj.GetType() != typeof(Voltage)) return false;
			return ((Voltage)obj)._SI == _SI;
		}

		//EndOfBody Voltage


	}
}