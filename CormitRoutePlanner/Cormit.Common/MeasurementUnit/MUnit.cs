using System;
using System.Runtime.Serialization;
using System.Text;

namespace Imarda.Lib
{
	/// <summary>
	/// Represents an SI base unit or derived unit. In addition a currency unit 
	/// has been added. Which currency is unspecified, it is only included for 
	/// calculations within a known context, and for specifying e.g. a unit price, a cost etc.
	/// Currency conversions have to be done outside the unit system.
	/// </summary>
	[DataContract]
	public struct MUnit
	{
		private const int NumberOfBaseUnits = 7;

		public static readonly MUnit NoUnit = new MUnit(0, 0, 0, 0, 0, 0, 0);

		private static readonly string[] Symbols = {"m", "kg", "s", "K", "A", "\u00A4", "rad"};

		[DataMember] private sbyte[] _Exponents;


		public MUnit(
			sbyte metre,
			sbyte kilogram,
			sbyte second,
			sbyte kelvin,
			sbyte ampere,
			sbyte currency,
			sbyte radians)
		{
			_Exponents = new[] {metre, kilogram, second, kelvin, ampere, currency, radians};
		}

		private MUnit(sbyte[] exponents)
		{
			_Exponents = exponents;
		}

		/// <summary>
		/// Adding the unit exponents. This is normally done during a multiplication of
		/// measurements.
		/// </summary>
		/// <param name="unit1"></param>
		/// <param name="unit2"></param>
		/// <returns></returns>
		public static MUnit operator +(MUnit unit1, MUnit unit2)
		{
			var result = new sbyte[NumberOfBaseUnits];
			for (int i = 0; i < NumberOfBaseUnits; i++)
			{
				result[i] = (sbyte) (unit1._Exponents[i] + unit2._Exponents[i]);
			}
			return new MUnit(result);
		}

		/// <summary>
		/// Multiply the unit with a number. E.g. when squaring the measurement,
		/// use 2, for getting the reciprocal of a measurement, use -1.
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public static MUnit operator *(MUnit unit, int n)
		{
			var result = new sbyte[NumberOfBaseUnits];
			for (int i = 0; i < NumberOfBaseUnits; i++)
			{
				result[i] = (sbyte) (unit._Exponents[i]*n);
			}
			return new MUnit(result);
		}

		/// <summary>
		/// Subtracting the unit exponents. This is normally done during a division of
		/// measurements.
		/// </summary>
		/// <param name="unit1"></param>
		/// <param name="unit2"></param>
		/// <returns></returns>
		public static MUnit operator -(MUnit unit1, MUnit unit2)
		{
			return unit1 + (unit2*-1);
		}

		/// <summary>
		/// Two units are equal if all the exponents are equal.
		/// </summary>
		/// <param name="unit1"></param>
		/// <param name="unit2"></param>
		/// <returns></returns>
		public static bool operator ==(MUnit unit1, MUnit unit2)
		{
			for (int i = 0; i < NumberOfBaseUnits; i++)
			{
				if (unit1._Exponents[i] != unit2._Exponents[i]) return false;
			}
			return true;
		}

		/// <summary>
		/// Two units are unequal if at least one of their respective
		/// exponents are unequal.
		/// </summary>
		/// <param name="unit1"></param>
		/// <param name="unit2"></param>
		/// <returns></returns>
		public static bool operator !=(MUnit unit1, MUnit unit2)
		{
			return !(unit1 == unit2);
		}

		public override int GetHashCode()
		{
			return _Exponents[0] + _Exponents[1]*4 + _Exponents[2]*8;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() != typeof (MUnit)) return false;
			var u = (MUnit) obj;
			return u == this;
		}


		/// <summary>
		/// Create a base unit symbol string, e.g. "kg.m2.s-3"
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var sb = new StringBuilder();

			for (int i = 0; i < NumberOfBaseUnits; i++)
			{
				int exp = _Exponents[i];
				if (exp != 0)
				{
					sb.Append(Symbols[i]);
					if (exp != 1) sb.Append(exp);
					sb.Append('.');
				}
			}
			if (sb.Length > 0) sb.Length--; // remove trailing separator
			return sb.ToString();
		}

		public string ToSuperscriptString()
		{
			var sb = new StringBuilder();

			for (int i = 0; i < NumberOfBaseUnits; i++)
			{
				int exp = _Exponents[i];
				if (exp != 0)
				{
					sb.Append(Symbols[i]);
					if (exp != 1)
					{
						int x = Math.Abs(exp);
						if (exp < 0) sb.Append('-');
						switch (x)
						{
							case 1:
								sb.Append('\u00B9');
								break;
							case 2:
								sb.Append('\u00B2');
								break;
							case 3:
								sb.Append('\u00B3');
								break;
							default:
								sb.Append(x);
								break;
						}
					}
				}
			}
			return sb.ToString();
		}


		public static string DisplayUnit(string easyUnit)
		{
			char[] arr = easyUnit.ToCharArray();
			for (int i = 0; i < arr.Length; i++)
			{
				char c = arr[i];
				switch (c)
				{
					case '2':
						c = '\u00B2'; // superscript 2
						break;
					case '3':
						c = '\u00B3'; // superscript 3
						break;
					case '@':
						c = '\u00A4'; // generic currency symbol
						break;
				}
				arr[i] = c;
			}
			return new string(arr);
		}


		public static MUnit Parse(string expstring)
		{
			try
			{
				string[] exparr = expstring.Split(',');
				var arr = new sbyte[7];
				for (int i = 0; i < exparr.Length; i++) arr[i] = sbyte.Parse(exparr[i]);
				return new MUnit(arr);
			}
			catch (Exception ex)
			{
				throw new FormatException(string.Format("MUnit cannot parse {0}. Reason: {1}", expstring, ex.Message));
			}
		}

		public string PString()
		{
			var sb = new StringBuilder();
			foreach (sbyte t in _Exponents)
			{
				sb.Append(t).Append(',');
			}
			return sb.ToString(0, sb.Length - 1);
		}
	}
}