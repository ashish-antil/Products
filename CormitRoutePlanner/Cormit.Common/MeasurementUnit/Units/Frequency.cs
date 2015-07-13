using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	/// <summary>
	/// Frequency is similar to AngularVelocity, but lacks the radians unit,
	/// so its unit is 1/seconds. Use frequency to express e.g. events per time unit
	/// rather than cycles per time unit.
	/// </summary>
	[DataContract]
	public struct Frequency : IMeasurement
	{
		public static readonly MUnit Unit = new MUnit(0, 0, -1, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public Frequency(double hertz)
		{
			_SI = hertz;
		}

		[Unit("Hz")]
		[Unit("kHz", Factor = 1000)]
		[Unit("MHz", Factor = 1e6)]
		[Unit("ps", Display = "/s")]
		public double InHertz
		{
			get { return _SI; }
		}

		[Unit("RPM")]
		public double InRpm
		{
			get { return _SI/60; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("8e6a3ddb-e189-4198-bc95-25bdfd8891c6"); }
		}

		public string MetricSymbol
		{
			get { return "Hz"; }
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

		[Parse("Hz")]
		public static Frequency Hertz(double hertz)
		{
			return new Frequency(hertz);
		}

		[Parse("rpm")]
		[Parse("RPM")]
		public static Frequency Rpm(double rpm)
		{
			return new Frequency(rpm*60);
		}

		public static Frequency operator +(Frequency n1, Frequency n2)
		{
			return new Frequency(n1._SI + n2._SI);
		}

		public static Frequency operator -(Frequency n1, Frequency n2)
		{
			return new Frequency(n1._SI - n2._SI);
		}


		public static implicit operator Measurement(Frequency x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Frequency(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Frequency(x.Value);
		}

		public static implicit operator Frequency(AngularVelocity x)
		{
			return new Frequency(x.InRadPerSec/Angle.RadiansPerCircle);
		}


		
		public static bool operator >(Frequency x1, Frequency x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Frequency x1, Frequency x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Frequency x1, Frequency x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Frequency x1, Frequency x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Frequency x1, Frequency x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Frequency x1, Frequency x2)
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
			if (obj.GetType() != typeof(Frequency)) return false;
			return ((Frequency)obj)._SI == _SI;
		}

		//EndOfBody Frequency


	}
}