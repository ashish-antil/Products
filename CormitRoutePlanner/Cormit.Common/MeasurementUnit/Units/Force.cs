using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	[DataContract]
	public struct Force : IMeasurement
	{
		public const double NewtonPerPoundForce = 4.4482216152605;
		public static readonly MUnit Unit = new MUnit(1, 1, -2, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public Force(double newton)
		{
			_SI = newton;
		}


		[Unit("N")]
		public double InNewton
		{
			get { return _SI; }
		}

		[Unit("lbf")]
		public double InPoundForce
		{
			get { return _SI/NewtonPerPoundForce; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("0f77a2c7-e733-4c50-8cea-79a9506e4db8"); }
		}

		public string MetricSymbol
		{
			get { return "N"; }
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

		[Parse("N")]
		public static Force Newton(double newton)
		{
			return new Force(newton);
		}

		[Parse("lbf")]
		public static Force PoundForce(double lbf)
		{
			return new Force(lbf*NewtonPerPoundForce);
		}

		public static Force operator +(Force n1, Force n2)
		{
			return new Force(n1._SI + n2._SI);
		}

		public static Force operator -(Force n1, Force n2)
		{
			return new Force(n1._SI - n2._SI);
		}



		public static implicit operator Measurement(Force x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Force(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Force(x.Value);
		}


		
		public static bool operator >(Force x1, Force x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Force x1, Force x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Force x1, Force x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Force x1, Force x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Force x1, Force x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Force x1, Force x2)
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
			if (obj.GetType() != typeof(Force)) return false;
			return ((Force)obj)._SI == _SI;
		}

		//EndOfBody Force


	}
}