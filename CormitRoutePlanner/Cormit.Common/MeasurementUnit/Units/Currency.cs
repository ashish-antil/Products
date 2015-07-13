using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	/// <summary>
	/// Some 'generic' currency which is known to the user of this struct.
	/// Could be USD, AUD, etc. Conversions have to be done outside the measurment system.
	/// Mostly intended for calculations. For storage use a 'decimal' based type.
	/// </summary>
	[DataContract]
	public struct Currency : IMeasurement
	{
		public static readonly MUnit Unit = new MUnit(0, 0, 0, 0, 0, 1, 0);

		[DataMember] private double _SI;

		public Currency(double value)
		{
			_SI = value;
		}

		public Currency(double value, string symbol, MeasurementFormatInfo mfi)
		{
			double rate;
			if (mfi.ExchangeRates.TryGetValue(symbol, out rate))
			{
				_SI = value/rate;
			}
			else throw new Exception("Unknown currency: " + symbol);
		}

		[Unit("$", UnitBeforeValue = true)]
		[Unit("\u00A4", UnitBeforeValue = true)]
		public double Value
		{
			get { return _SI; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("58df15fa-c398-4b13-ae3f-73232ab69531"); }
		}

		public string MetricSymbol
		{
			get { return "\u00A4"; }
		}


		public string ToString(string format, IFormatProvider formatProvider)
		{
			return Measurement.DefaultFormatHandler(format, this, formatProvider);
		}


		public string SpecificFormat(string format, IFormatProvider formatProvider)
		{
			var mfi = formatProvider as MeasurementFormatInfo;
			if (mfi == null) return AsMeasurement().ToString();
			// ">USD;0.00", ">NZD;0.00", ">NZD;@", ">EUR "
			string[] parts = format.Split(MeasurementFormatInfo.Sep, 2);
			string cur = parts[0]; // "NZD", "NZD "
			
			string key = cur.Trim();
			double rate;
			string result;
			if (mfi.ExchangeRates.TryGetValue(key, out rate))
			{
				double val = rate*Value;
				if (parts.Length > 1)
				{
					result = val.ToString(parts[1]).Replace("@", cur);
				}
				else
				{
					string nfmt = "0." + new string('0', mfi.NumberFormat.CurrencyDecimalDigits);
					result = cur + val.ToString(nfmt);
				}
			}
			else result = key + "?";
			return result;
		}

		public Measurement AsMeasurement()
		{
			return this;
		}

		#endregion

		public double GetValue(string symbol, MeasurementFormatInfo mfi)
		{
			double rate;
			if (mfi.ExchangeRates.TryGetValue(symbol, out rate))
			{
				return _SI*rate;
			}
			throw new Exception("Unknown currency: " + symbol);
		}

		public static Currency operator +(Currency n1, Currency n2)
		{
			return new Currency(n1._SI + n2._SI);
		}

		public static Currency operator -(Currency n1, Currency n2)
		{
			return new Currency(n1._SI - n2._SI);
		}

		public static implicit operator Measurement(Currency x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Currency(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Currency(x.Value);
		}


		
		public static bool operator >(Currency x1, Currency x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Currency x1, Currency x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Currency x1, Currency x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Currency x1, Currency x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Currency x1, Currency x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Currency x1, Currency x2)
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
			if (obj.GetType() != typeof(Currency)) return false;
			return ((Currency)obj)._SI == _SI;
		}

		//EndOfBody Currency


	}
}