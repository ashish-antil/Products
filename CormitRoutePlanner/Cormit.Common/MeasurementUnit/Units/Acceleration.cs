using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	/// <summary>
	/// Acceleration measurement.
	/// </summary>
	[DataContract]
	public struct Acceleration : IMeasurement
	{
		public const double Mps2PerGforce = 9.80665;

		public static readonly MUnit Unit = new MUnit(1, 0, -2, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public Acceleration(double mps2)
		{
			_SI = mps2;
		}

		[Unit("m/s\u00B2")]
		[Unit("mps2")]
		public double InMetrePerSec2
		{
			get { return _SI; }
		}

		[Unit("G")]
		public double InGForce
		{
			get { return _SI/Mps2PerGforce; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("812e62fe-2c41-45d5-ad53-c261000b5c2e"); }
		}

		public string MetricSymbol
		{
			get { return "m/s\u00B2"; }
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

		[Parse("m/s2")]
		public static Acceleration MetrePerSec2(double mps2)
		{
			return new Acceleration(mps2);
		}

		[Parse("G")]
		public static Acceleration GForce(double gforce)
		{
			return new Acceleration(gforce*Mps2PerGforce);
		}

		public static Acceleration operator +(Acceleration n1, Acceleration n2)
		{
			return new Acceleration(n1._SI + n2._SI);
		}

		public static Acceleration operator -(Acceleration n1, Acceleration n2)
		{
			return new Acceleration(n1._SI - n2._SI);
		}

		public static implicit operator Measurement(Acceleration x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static explicit operator Acceleration(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Acceleration(x.Value);
		}


		
		public static bool operator >(Acceleration x1, Acceleration x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Acceleration x1, Acceleration x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Acceleration x1, Acceleration x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Acceleration x1, Acceleration x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Acceleration x1, Acceleration x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Acceleration x1, Acceleration x2)
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
			if (obj.GetType() != typeof(Acceleration)) return false;
			return ((Acceleration)obj)._SI == _SI;
		}

		//EndOfBody Acceleration


	}
}