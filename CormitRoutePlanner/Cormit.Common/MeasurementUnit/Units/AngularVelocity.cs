using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	/// <summary>
	/// Like Frequency but is expressed in radians rather than revolutions.
	/// Use in applications where working with angles is important.
	/// </summary>
	[DataContract]
	public struct AngularVelocity : IMeasurement
	{
		public static readonly MUnit Unit = new MUnit(0, 0, -1, 0, 0, 0, 1);

		[DataMember] private double _SI;

		public AngularVelocity(double val)
		{
			_SI = val;
		}

		[Unit("rad/s")]
		public double InRadPerSec
		{
			get { return _SI; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("31988e7e-0be6-4950-8eea-12ae051d6ab6"); }
		}

		public string MetricSymbol
		{
			get { return "rad/s"; }
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

		[Parse("rad/s")]
		public static AngularVelocity RadPerSec(double radps)
		{
			return new AngularVelocity(radps);
		}

		public static implicit operator Measurement(AngularVelocity x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static AngularVelocity operator +(AngularVelocity n1, AngularVelocity n2)
		{
			return new AngularVelocity(n1._SI + n2._SI);
		}

		public static AngularVelocity operator -(AngularVelocity n1, AngularVelocity n2)
		{
			return new AngularVelocity(n1._SI - n2._SI);
		}


		public static implicit operator AngularVelocity(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new AngularVelocity(x.Value);
		}

		public static implicit operator AngularVelocity(Frequency x)
		{
			return new AngularVelocity(x.InHertz*Angle.RadiansPerCircle);
		}


		
		public static bool operator >(AngularVelocity x1, AngularVelocity x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(AngularVelocity x1, AngularVelocity x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(AngularVelocity x1, AngularVelocity x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(AngularVelocity x1, AngularVelocity x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(AngularVelocity x1, AngularVelocity x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(AngularVelocity x1, AngularVelocity x2)
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
			if (obj.GetType() != typeof(AngularVelocity)) return false;
			return ((AngularVelocity)obj)._SI == _SI;
		}

		//EndOfBody AngularVelocity


	}
}