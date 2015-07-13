using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Imarda.Lib
{
	public static class DisplayUtils
	{
		private static readonly Regex _rxCamelCase = new Regex(@"(?<=\p{Ll})(\p{Lu})", RegexOptions.Compiled);

		/// <summary>
		/// Deprecated.
		/// </summary>
		/// <param name="ifp"></param>
		/// <param name="s"></param>
		/// <param name="dateFmt"></param>
		/// <param name="numFmt"></param>
		/// <returns></returns>
		public static string GetDisplayString(ImardaFormatProvider ifp, string s, string dateFmt, string numFmt)
		{
			if (string.IsNullOrEmpty(s)) return string.Empty;
			string display = s;

			if (s.Length == Formats.DateFmtLength && s[10] == 'T')
			{
				DateTime dt;
				if (DateTime.TryParseExact(s, "s", null, DateTimeStyles.None, out dt))
				{
					dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
					DateTime? local = ImardaFormatProvider.DateTimeInUserZone(dt, ifp.TimeZoneInfo);
					if (local.HasValue)
					{
						display = string.Format(ifp, dateFmt ?? "{0}", local.Value);
						//if (dateFmt == null) dateFmt = ifp.GetDateFormat("~");
						//display = dateFmt != null ? local.Value.ToString(dateFmt, ifp) : local.Value.ToString(ifp);
						return display;
					}
					return "";
				}
			}

			decimal number;
			if (decimal.TryParse(s, NumberStyles.Number, ifp, out number))
			{
				display = numFmt != null ? number.ToString(numFmt, ifp) : number.ToString(ifp);
			}
			else
			{
				int p = s.IndexOf(':');
				if (p != -1)
				{
					if (s.Length > p + 1)
					{
						char c = s[p + 1];
						if (c == '-' || char.IsDigit(c))
						{
							try
							{
								display = Measurement.Format(s, ifp.MeasurementFormatProvider);
							}
							catch
							{
							}
						}
					}
				}
			}
			return display;
		}

		/// <summary>
		/// Inserts spaces into a camelcase string. E.g. "VehicleDisplayName" => "Vehicle Display Name", "TrackID" => "Track ID".
		/// Space is inserted between lowercase and following uppercase character.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string SpaceCamelCase(this string s)
		{
			return _rxCamelCase.Replace(s, " $1");
		}
	}
}