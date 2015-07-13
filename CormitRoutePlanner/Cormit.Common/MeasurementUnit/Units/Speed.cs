#region

using System;
using System.Runtime.Serialization;

#endregion

namespace Imarda.Lib
{
	[DataContract]
	public struct Speed : IMeasurement
	{
		public static readonly MUnit Unit = new MUnit(1, 0, -1, 0, 0, 0, 0);

		[DataMember] private double _SI;

	    /// <summary>
	    ///     Instantiate a new Speed object.
	    /// </summary>
	    /// <param name="mps">The value in metres per second</param>
	    public Speed(double mps)
		{
			_SI = mps;
		}

		[Unit("m/s")]
		public double InMetrePerSec
		{
			get { return _SI; }
		}

		[Unit("km/h")]
		[Unit("kph")]
		public double InKph
		{
			get { return _SI/(SIPrefix.Kilo/Duration.SecondsPerHour); }
		}

		[Unit("mph")]
		public double InMph
		{
			get { return _SI/(Length.MetrePerMile/Duration.SecondsPerHour); }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("2885377a-fd90-44b2-a6f7-f682f7da7180"); }
		}

		public string MetricSymbol
		{
			get { return "m/s"; }
		}

	    public override string ToString()
	    {
	        return string.Concat(InKph, " kph.");
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

		[Parse("m/s")]
		public static Speed MetrePerSec(double mps)
		{
			return new Speed(mps);
		}

		[Parse("kph")]
		[Parse("km/h")]
		public static Speed Kph(double kph)
		{
			return new Speed(kph*SIPrefix.Kilo/Duration.SecondsPerHour);
		}

		[Parse("mph")]
		[Parse("mi/h")]
		public static Speed Mph(double mph)
		{
			return new Speed(mph*Length.MetrePerMile/Duration.SecondsPerHour);
		}

		public static Speed operator +(Speed n1, Speed n2)
		{
			return new Speed(n1._SI + n2._SI);
		}

		public static Speed operator -(Speed n1, Speed n2)
		{
			return new Speed(n1._SI - n2._SI);
		}

		public static implicit operator Measurement(Speed x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Speed(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Speed(x.Value);
		}


		
		public static bool operator >(Speed x1, Speed x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Speed x1, Speed x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Speed x1, Speed x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Speed x1, Speed x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Speed x1, Speed x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Speed x1, Speed x2)
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
			if (obj.GetType() != typeof(Speed)) return false;
			return ((Speed)obj)._SI == _SI;
		}

		//EndOfBody Speed
	}
}