using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	[DataContract]
	public struct Density : IMeasurement
	{
		private const double Conversion1 = 0.0624279606;
		private const double Conversion2 = 1000.0;

		public static readonly MUnit Unit = new MUnit(-3, 1, 0, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public Density(double si)
		{
			_SI = si;
		}

		[Unit("kg/m\u00B3")]
		public double InKgPerCubicMetre
		{
			get { return _SI; }
		}

		[Unit("kg/L")]
		public double InKgPerLitre
		{
			get { return _SI/Conversion2; }
		}

		[Unit("lb/cu ft")]
		[Unit("lb/ft\u00B3")]
		public double InPoundPerCubicFoot
		{
			get { return _SI*Conversion1; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("fee84f2b-a91b-4988-a175-0aa5b1bf3f0b"); }
		}

		public string MetricSymbol
		{
			get { return "kg/m3"; }
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

		[Parse("kg/m3")]
		public static Density KgPerCubicMetre(double kg_p_m3)
		{
			return new Density(kg_p_m3);
		}

		[Parse("kg/L")]
		public static Density KgPerLitre(double kg_p_L)
		{
			return new Density(kg_p_L*Conversion2);
		}

		[Parse("lb/ft3")]
		public static Density LbPerCuFt(double lb_p_cuft)
		{
			return new Density(lb_p_cuft/Conversion1);
		}

		public static Density operator +(Density n1, Density n2)
		{
			return new Density(n1._SI + n2._SI);
		}

		public static Density operator -(Density n1, Density n2)
		{
			return new Density(n1._SI - n2._SI);
		}


		public static implicit operator Measurement(Density x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Density(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Density(x.Value);
		}


		
		public static bool operator >(Density x1, Density x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Density x1, Density x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Density x1, Density x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Density x1, Density x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Density x1, Density x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Density x1, Density x2)
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
			if (obj.GetType() != typeof(Density)) return false;
			return ((Density)obj)._SI == _SI;
		}

		//EndOfBody Density


	}
}