using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	[DataContract]
	public struct Unitless : IMeasurement
	{
		public static readonly MUnit Unit = MUnit.NoUnit;

		private readonly double _SI;

		public Unitless(double value)
		{
			_SI = value;
		}

		public double Value
		{
			get { return _SI; }
		}

		
		public static Unitless Parse(string s)
		{
			double number;
			return new Unitless(double.TryParse(s, out number) ? number : 0.0);
		}
		

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("E196AD12-3B63-4193-ACBC-BA44EC2FA092"); }
		}

		public string MetricSymbol
		{
			get { return ""; }
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

		public static Unitless operator +(Unitless n1, Unitless n2)
		{
			return new Unitless(n1._SI + n2._SI);
		}

		public static Unitless operator -(Unitless n1, Unitless n2)
		{
			return new Unitless(n1._SI - n2._SI);
		}

		public static implicit operator Measurement(Unitless x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Unitless(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Unitless(x.Value);
		}


		
		public static bool operator >(Unitless x1, Unitless x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Unitless x1, Unitless x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Unitless x1, Unitless x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Unitless x1, Unitless x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Unitless x1, Unitless x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Unitless x1, Unitless x2)
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
			if (obj.GetType() != typeof(Unitless)) return false;
			return ((Unitless)obj)._SI == _SI;
		}

		//EndOfBody Unitless


	}
}