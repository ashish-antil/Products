using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	/// <summary>
	/// Used for both temperature scales and temperature differences.
	/// Note that the client must use In*Scale properties if the temperature is on a 
	/// Celsius or Fahrenheit scale, and In*Delta if the struct contains a temperature difference.
	/// For the absolute scale Kelvin this is irrelevant.
	/// </summary>
	[DataContract]
	public struct Temperature : IMeasurement
	{
		public const string Deg = "\u00B0";
		public static readonly MUnit Unit = new MUnit(0, 0, 0, 1, 0, 0, 0);

		[DataMember] private double _SI;

		public Temperature(double kelvin)
		{
			_SI = kelvin;
		}

		/// <summary>
		/// Get a reading of the temperature on the Celsius scale.
		/// </summary>
		[Unit("\u00B0C")]
		[Unit("degC", Display = "\u00B0C")]
		public double InCelsiusScale
		{
			get { return _SI - 273.15; }
		}

		/// <summary>
		/// The degrees temperature difference or range using
		/// Celsius degree intervals.
		/// </summary>
		public double InCelsiusDelta
		{
			get { return _SI; }
		}

		[Unit("K")]
		public double InKelvin
		{
			get { return _SI; }
		}

		/// <summary>
		/// Get a reading of the temperature on the Fahrenheit scale
		/// </summary>
		[Unit("\u00B0F")]
		[Unit("degF", Display = "\u00B0F")]
		public double InFahrenheitScale
		{
			get { return (_SI - 273.15)*1.8 + 32.0; }
		}

		/// <summary>
		/// The degrees temperature difference or range using
		/// Fahrenheit degree intervals.
		/// </summary>
		public double InFahrenheitDelta
		{
			get { return _SI*1.8; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("cd4b2f20-3551-4f36-bfb2-6d95bfc3e1b7"); }
		}

		public string MetricSymbol
		{
			get { return "K"; }
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

		[Parse("K")]
		public static Temperature Kelvin(double kelvin)
		{
			return new Temperature(kelvin);
		}

		/// <summary>
		/// Create a temperature measured on the Celsius scale.
		/// </summary>
		/// <param name="celsius"></param>
		/// <returns></returns>
		[Parse("*C")]
		[Parse("degC")]
		public static Temperature CelsiusScale(double celsius)
		{
			return new Temperature(celsius + 273.15);
		}

		/// <summary>
		/// Create a temperature measured on the Fahrenheit scale.
		/// </summary>
		/// <param name="fahrenheit"></param>
		/// <returns></returns>
		[Parse("*F")]
		[Parse("degF")]
		public static Temperature FahrenheitScale(double fahrenheit)
		{
			return new Temperature((fahrenheit - 32.0)/1.8 + 273.15);
		}



		public static implicit operator Measurement(Temperature x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Temperature(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Temperature(x.Value);
		}


		
		public static bool operator >(Temperature x1, Temperature x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Temperature x1, Temperature x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Temperature x1, Temperature x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Temperature x1, Temperature x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Temperature x1, Temperature x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Temperature x1, Temperature x2)
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
			if (obj.GetType() != typeof(Temperature)) return false;
			return ((Temperature)obj)._SI == _SI;
		}

		//EndOfBody Temperature


	}
}