using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	/// <summary>
	/// Amount of mass flowing per second.
	/// </summary>
	[DataContract]
	public struct MassFlow : IMeasurement
	{
		public static readonly MUnit Unit = new MUnit(0, 1, -1, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public MassFlow(double val)
		{
			_SI = val;
		}

		[Unit("kg/s")]
		public double InKgPerSec
		{
			get { return _SI; }
		}

		[Unit("t/d")]
		public double InTonnePerDay
		{
			get { return _SI/Mass.KgPerTonne*Duration.SecondsPerDay; }
		}

		[Unit("t(US)/d", Display = "t/d")]
		public double InShortTonPerDay
		{
			get { return _SI/Mass.KgPerShortTon*Duration.SecondsPerDay; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("b0f54bb1-ff22-4f1a-b311-c97d8759074c"); }
		}

		public string MetricSymbol
		{
			get { return "kg/s"; }
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
		/// Kilogram per second.
		/// </summary>
		/// <param name="kgps"></param>
		/// <returns></returns>
		public static MassFlow KgPerSec(double kgps)
		{
			return new MassFlow(kgps);
		}

		public static MassFlow TonnePerDay(double tpd)
		{
			return new MassFlow(tpd*Mass.KgPerTonne/Duration.SecondsPerDay);
		}

		public static MassFlow ShortTonPerDay(double tpd)
		{
			return new MassFlow(tpd*Mass.KgPerShortTon/Duration.SecondsPerDay);
		}

		public static MassFlow operator +(MassFlow n1, MassFlow n2)
		{
			return new MassFlow(n1._SI + n2._SI);
		}

		public static MassFlow operator -(MassFlow n1, MassFlow n2)
		{
			return new MassFlow(n1._SI - n2._SI);
		}


		public static implicit operator Measurement(MassFlow x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator MassFlow(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new MassFlow(x.Value);
		}


		
		public static bool operator >(MassFlow x1, MassFlow x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(MassFlow x1, MassFlow x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(MassFlow x1, MassFlow x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(MassFlow x1, MassFlow x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(MassFlow x1, MassFlow x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(MassFlow x1, MassFlow x2)
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
			if (obj.GetType() != typeof(MassFlow)) return false;
			return ((MassFlow)obj)._SI == _SI;
		}

		//EndOfBody MassFlow


	}
}