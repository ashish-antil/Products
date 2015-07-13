#region

using System.Globalization;

#endregion

namespace Imarda.Lib.MVVM.Extensions
{
    public static class ValueTypeExtensions
    {
        public static string ToInvariantString(this char me)
        {
            return me.ToString(CultureInfo.InvariantCulture);
        }

        public static string ToInvariantString(this int me)
        {
            return me.ToString(CultureInfo.InvariantCulture);
        }

		public static string ToInvariantString(this byte me)
		{
			return me.ToString(CultureInfo.InvariantCulture);
		}

        public static string ToInvariantString(this uint me)
        {
            return me.ToString(CultureInfo.InvariantCulture);
        }

        public static string ToInvariantString(this double me)
        {
            return me.ToString(CultureInfo.InvariantCulture);
        }

        public static string ToInvariantString(this decimal me)
        {
            return me.ToString(CultureInfo.InvariantCulture);
        }

        public static string ToStringWithNoTrailingZeros(this decimal me, CultureInfo culture)
        {
            string numberDecimalSeparator = culture.NumberFormat.NumberDecimalSeparator;

            var decimalAsString = me.ToString(culture);
            if (decimalAsString.Contains(numberDecimalSeparator))
            {
                decimalAsString = decimalAsString.TrimEnd('0');
                if (decimalAsString.EndsWith(numberDecimalSeparator))
                {
                    int decimalSeparatorLength = numberDecimalSeparator.Length;
                    decimalAsString = decimalAsString.Remove(decimalAsString.Length - decimalSeparatorLength, decimalSeparatorLength);
                }
            }

            return decimalAsString;
        }
    }
}