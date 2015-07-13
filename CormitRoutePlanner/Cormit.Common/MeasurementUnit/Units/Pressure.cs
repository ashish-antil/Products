using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	[DataContract]
	public struct Pressure : IMeasurement
	{
		public const double PascalPerPsi = 6894.75729;
		public const double PascalPerBar = 1e5;
		public static readonly MUnit Unit = new MUnit(-1, 1, -2, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public Pressure(double pascal)
		{
			_SI = pascal;
		}

		[Unit("Pa")]
		[Unit("kPa", Factor = 1000)]
		public double InPascal
		{
			get { return _SI; }
		}

		[Unit("psi")]
		public double InPoundPerSquareInch
		{
			get { return _SI/PascalPerPsi; }
		}

		[Unit("bar")]
		public double InBar
		{
			get { return _SI/PascalPerBar; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("804d05a2-6a5b-49fe-b7e9-0c0a43a4ecf9"); }
		}

		public string MetricSymbol
		{
			get { return "Pa"; }
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

		[Parse("kPa")]
		public static Pressure KiloPascal(double kPa)
		{
			return new Pressure(kPa*1000);
		}

		[Parse("N/m2")]
		[Parse("Pa")]
		public static Pressure Pascal(double pascal)
		{
			return new Pressure(pascal);
		}

		[Parse("bar")]
		public static Pressure Bar(double bar)
		{
			return new Pressure(bar*PascalPerBar);
		}

		[Parse("psi")]
		public static Pressure PoundPerSquareInch(double psi)
		{
			return new Pressure(psi*PascalPerPsi);
		}

		public double InSI(double prefix)
		{
			return _SI/prefix;
		}

		public static Pressure operator +(Pressure n1, Pressure n2)
		{
			return new Pressure(n1._SI + n2._SI);
		}

		public static Pressure operator -(Pressure n1, Pressure n2)
		{
			return new Pressure(n1._SI - n2._SI);
		}


		public static implicit operator Measurement(Pressure x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Pressure(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Pressure(x.Value);
		}


		
		public static bool operator >(Pressure x1, Pressure x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Pressure x1, Pressure x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Pressure x1, Pressure x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Pressure x1, Pressure x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Pressure x1, Pressure x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Pressure x1, Pressure x2)
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
			if (obj.GetType() != typeof(Pressure)) return false;
			return ((Pressure)obj)._SI == _SI;
		}

		//EndOfBody Pressure


	}
}