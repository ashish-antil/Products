using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	/// <summary>
	/// Volume (of e.g. fluid) flowing per second.
	/// </summary>
	[DataContract]
	public struct VolumeFlow : IMeasurement
	{
		public static readonly MUnit Unit = new MUnit(3, 0, -1, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public VolumeFlow(double m3ps)
		{
			_SI = m3ps;
		}

		[Unit("L/s")]
		public double InLitrePerSecond
		{
			get { return _SI*1000; }
		}

		[Unit("m\u00B3/s")]
		public double InCubicMetrePerSecond
		{
			get { return _SI; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("0d96b3ce-ea5f-460b-a7fe-aa68335bcd32"); }
		}

		public string MetricSymbol
		{
			get { return "m\u00B3/s"; }
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

		[Parse("L/s")]
		public static VolumeFlow LitrePerSecond(double lps)
		{
			return new VolumeFlow(lps/1000);
		}

		[Parse("m3/s")]
		public static VolumeFlow CubicMetrePerSecond(double m3ps)
		{
			return new VolumeFlow(m3ps);
		}

		public static VolumeFlow operator +(VolumeFlow n1, VolumeFlow n2)
		{
			return new VolumeFlow(n1._SI + n2._SI);
		}

		public static VolumeFlow operator -(VolumeFlow n1, VolumeFlow n2)
		{
			return new VolumeFlow(n1._SI - n2._SI);
		}

		public static implicit operator Measurement(VolumeFlow x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator VolumeFlow(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new VolumeFlow(x.Value);
		}


		
		public static bool operator >(VolumeFlow x1, VolumeFlow x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(VolumeFlow x1, VolumeFlow x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(VolumeFlow x1, VolumeFlow x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(VolumeFlow x1, VolumeFlow x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(VolumeFlow x1, VolumeFlow x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(VolumeFlow x1, VolumeFlow x2)
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
			if (obj.GetType() != typeof(VolumeFlow)) return false;
			return ((VolumeFlow)obj)._SI == _SI;
		}

		//EndOfBody VolumeFlow


	}
}