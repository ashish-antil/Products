using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	[DataContract]
	public struct Mass : IMeasurement
	{
		public const double KgPerLb = 0.45359237;
		public const double KgPerTonne = 1000.0; // metric tonne
		public const double KgPerShortTon = 907.18474;
		public const double KgPerLongTon = 1016.04691;
		public static readonly MUnit Unit = new MUnit(0, 1, 0, 0, 0, 0, 0);

		[DataMember] private double _SI;

		public Mass(double kg)
		{
			_SI = kg;
		}

		[Unit("kg")]
		public double InKg
		{
			get { return _SI; }
		}

		[Unit("lb")]
		public double InLb
		{
			get { return _SI/KgPerLb; }
		}

		[Unit("t")]
		public double InTonne
		{
			get { return _SI/KgPerTonne; }
		}

		[Unit("t(Imp)", Display = "long ton")]
		public double InLongTon
		{
			get { return _SI/KgPerLongTon; }
		}

		[Unit("t(US)", Display = "t")]
		public double InShortTon
		{
			get { return _SI/KgPerShortTon; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("e82ff33c-7438-4be4-a70e-8606a5bdad97"); }
		}

		public string MetricSymbol
		{
			get { return "kg"; }
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

		[Parse("kg")]
		public static Mass Kg(double kg)
		{
			return new Mass(kg);
		}

		/// <summary>
		/// US Pound
		/// </summary>
		/// <param name="lb"></param>
		/// <returns></returns>
		[Parse("lb")]
		public static Mass Lb(double lb)
		{
			return new Mass(lb*KgPerLb);
		}

		/// <summary>
		/// The metric tonne is 1000 kg.
		/// </summary>
		/// <param name="tonne"></param>
		/// <returns></returns>
		[Parse("t")]
		public static Mass Tonne(double tonne)
		{
			return new Mass(tonne*KgPerTonne);
		}

		/// <summary>
		/// The normal US ton.
		/// </summary>
		/// <param name="ton"></param>
		/// <returns></returns>
		[Parse("st")]
		[Parse("shortton")]
		public static Mass ShortTon(double ton)
		{
			return new Mass(ton*KgPerShortTon);
		}

		/// <summary>
		/// The imperial long ton.
		/// </summary>
		/// <param name="ton"></param>
		/// <returns></returns>
		[Parse("lt")]
		[Parse("longton")]
		public static Mass LongTon(double ton)
		{
			return new Mass(ton*KgPerLongTon);
		}

		public static Mass operator +(Mass n1, Mass n2)
		{
			return new Mass(n1._SI + n2._SI);
		}

		public static Mass operator -(Mass n1, Mass n2)
		{
			return new Mass(n1._SI - n2._SI);
		}

		public static implicit operator Measurement(Mass x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Mass(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Mass(x.Value);
		}


		
		public static bool operator >(Mass x1, Mass x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Mass x1, Mass x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Mass x1, Mass x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Mass x1, Mass x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Mass x1, Mass x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Mass x1, Mass x2)
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
			if (obj.GetType() != typeof(Mass)) return false;
			return ((Mass)obj)._SI == _SI;
		}

		//EndOfBody Mass


	}
}