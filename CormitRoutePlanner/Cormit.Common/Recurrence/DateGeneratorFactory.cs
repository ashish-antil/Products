using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Imarda.Lib
{
	/// <summary>
	/// Creates DateGenerators based on defining strings.
	/// </summary>
	public static class DateGeneratorFactory
	{
		private const string DateFmt = "yyyy-MM-dd";

		//private Regex _rxHourly =
		//  new Regex(@"H(?<nhours>\d+)?", RegexOptions.Compiled);

		private const string SrxShift = @"(?:(?<shift><=|<|>|=>|<>)(?<req>ad|mf|ss|mo|tu|we|th|fr|sa|su))?";

		private static readonly Regex RxDaily =
			new Regex(@"D(?<ndays>\d+)?" + SrxShift, RegexOptions.Compiled);

		private static readonly Regex RxWeekly =
			new Regex(@"W(?<dow>mf|ss|ad|(?:su)?(?:mo)?(?:tu)?(?:we)?(?:th)?(?:fr)?(?:sa)?(?:su)?)(?:,(?<nweeks>\d+))?",
			          RegexOptions.Compiled);

		private static readonly Regex RxMonthly =
			new Regex(
				@"M(?:" +
				@"(?<wom>[1-5])(?<dow>ad|mf|ss|mo|tu|we|th|fr|sa|su)(?:,(?<nmonths>\d+))?" +
				@"|" +
				@"(?<dom>\d\d?)(?:,(?<nmonths>\d+))?" +
				@")" + SrxShift
				, RegexOptions.Compiled);

		private static readonly Regex RxYearly =
			new Regex(
				@"Y(?<month>\d\d?)," +
				@"(?:" +
				@"(?<wom>[1-5])(?<dow>ad|mf|ss|mo|tu|we|th|fr|sa|su)" +
				@"|" +
				@"(?<dom>\d\d?)" +
				@")" + SrxShift
				, RegexOptions.Compiled);

		private static readonly Regex RxEnum =
			new Regex(@"E(?:(?<date>\d{4}-\d\d-\d\d),?)+" + SrxShift, RegexOptions.Compiled);


		private static int GetInt(Match m, string name)
		{
			Group g = m.Groups[name];
			return !g.Success ? 1 : int.Parse(g.Value);
		}

		/// <summary>
		/// Create a DateGenerator from the spec string.
		/// </summary>
		/// <remarks>
		/// Hourly
		/// - every [n] hours:  Hn	or H for H1
		/// Daily
		/// - every [n] days:   Dn	or D for D1
		/// Weekly
		/// - every [n] weeks on (day,weekday,weekendday,Mon..Sun): Wss   Wmf   Wmf,n  Wtuth
		/// Monthly
		/// - every day n of every [m] month(s): Mn,m	or Mn for Mn,1
		/// - every (1st, 2nd, 3rd, 4th, last) (day,weekday,weekendday,Mon..Sun) of every [m] months: 
		///	M4,mf [every 4th weekday of the month]   
		///	M5,ad,3 [every last day of every 3 months]
		///	M1,su,3 [every first sunday of every 3 months]
		/// Yearly
		/// - every (Jan..Dec) day n:  Y12,31
		/// - the (1st..last) (day,weekday,weekkendday,Mon..Sun) of (Jan..Dec):   Y12,5,ss [last weekend day of Dec]
		/// Enumeration
		/// - E2009-12-31,2010-01-07,2010-02-28
		/// </remarks>
		/// <param name="spec">a string that defines the DateGenerator, e.g. "D12", "Wmotuwe,4", "Y5,2su"</param>
		/// <returns></returns>
		public static DateGenerator Create(string spec)
		{
			if (spec.Contains('|'))
			{
				string[] specs = spec.Split('|');
				return new MultiplexGenerator(specs.Select(childSpec => Create(childSpec)).ToArray());
			}

			string[] parts = spec.Split(';'); // "2010-01-07;2011-12-31;Wmowefr,3"   "2010-01-07;26;Wmo,2"
			string pattern;
			DateTime start;
			DateTime end;
			int occurrences = int.MaxValue;

			if (parts.Length == 1)
			{
				pattern = parts[0];
				start = DateTime.MinValue;
				end = DateTime.MinValue;
			}
			else
			{
				pattern = parts[2];
				if (DateTime.TryParseExact(parts[0], DateFmt, CultureInfo.InvariantCulture, DateTimeStyles.None,
				                           out start))
				{
					if (DateTime.TryParseExact(parts[1], DateFmt, CultureInfo.InvariantCulture, DateTimeStyles.None,
					                           out end))
					{
						occurrences = int.MaxValue;
					}
					else
					{
						if (int.TryParse(parts[1], out occurrences))
						{
							end = DateTime.MaxValue;
						}
					}
				}
				else end = DateTime.MinValue;
			}

			DateGenerator gen = null;

			char scale = pattern[0];
			Group g;
			Match m;
			int n;
			int dom; // day of month: 1..31
			int wom; // week of month: 1..4, 5 means "last week"
			string dow; // ad (any day), mf (mon-fri), ss (sat-sun), mo, tu, we, th, fr, sa, su
			switch (scale)
			{
					//case 'H': // hour pattern
					//  m = _rxHourly.Match(pattern);
					//  if (m.Success)
					//  {
					//	n = GetInt(m, "nhours");
					//	// gen = ... to do
					//  }
					//  break;
				case 'D': // day pattern
					m = RxDaily.Match(pattern);
					if (m.Success)
					{
						n = GetInt(m, "ndays");
						gen = new FixedIntervalGenerator(n);
						ApplyShiftPattern(gen, m.Groups);
					}
					break;
				case 'W': // week pattern
					m = RxWeekly.Match(pattern);
					if (m.Success)
					{
						n = GetInt(m, "nweeks");
						dow = m.Groups["dow"].Value;
						var dayset = new bool[7];

						switch (dow)
						{
							case "mf":
								dow = "motuwethfr";
								break;
							case "ss":
								dow = "sasu";
								break;
							case "ad":
								dow = "motuwethfrsasu";
								break;
						}

						for (int i = 0; i < dow.Length; i += 2)
						{
							int p = "su mo tu we th fr sa".IndexOf(dow.Substring(i, 2));
							if (p != -1) dayset[p/3] = true;
						}
						gen = new WeeklyGenerator(n, dayset);
					}
					break;
				case 'M': // month pattern
					m = RxMonthly.Match(pattern);
					if (m.Success)
					{
						n = GetInt(m, "nmonths");
						g = m.Groups["dom"];
						if (g.Success)
						{
							dom = GetInt(m, "dom");
							gen = new MonthlyAbsoluteGenerator(dom, n);
						}
						else
						{
							wom = GetInt(m, "wom");
							dow = m.Groups["dow"].Value;
							WDay wday = GetWDayFromAbbrev(dow);
							if (wday != WDay.Invalid) gen = new MonthlyRelativeGenerator(wday, wom, n);
						}
						ApplyShiftPattern(gen, m.Groups);
					}
					break;
				case 'Y': // year pattern
					m = RxYearly.Match(pattern);
					if (m.Success)
					{
						int month = GetInt(m, "month");
						g = m.Groups["dom"];
						if (g.Success)
						{
							dom = GetInt(m, "dom");
							gen = new AnniversaryGenerator(month, dom);
						}
						else
						{
							wom = GetInt(m, "wom");
							dow = m.Groups["dow"].Value;
							WDay wday = GetWDayFromAbbrev(dow);
							if (wday != WDay.Invalid) gen = new YearlyRelativeGenerator(wday, wom, month);
						}
						ApplyShiftPattern(gen, m.Groups);
					}
					break;
				case 'E': // enumeration of dates
					m = RxEnum.Match(pattern);
					if (m.Success)
					{
						g = m.Groups["date"];
						IEnumerable<DateTime> r = from Capture c in g.Captures
						                          select DateTime.ParseExact(c.Value, DateFmt, null);
						gen = new EnumDateGenerator(r);
						ApplyShiftPattern(gen, m.Groups);
					}
					break;
			}
			if (gen != null && start != DateTime.MinValue)
			{
				if (occurrences == int.MaxValue)
				{
					gen.SetRange(start, end);
				}
				else
				{
					gen.SetRange(start, occurrences);
				}
			}
			return gen;
		}

		private static WDay GetWDayFromAbbrev(string s)
		{
			int p = "ad mf ss mo tu we th fr sa su".IndexOf(s);
			if (p != -1) return (WDay) (p/3);
			return WDay.Invalid;
		}

		private static void ApplyShiftPattern(DateGenerator gen, GroupCollection groups)
		{
			Group gShift = groups["shift"];
			Group gReq = groups["req"];
			if (gShift.Success && gReq.Success)
			{
				ShiftMode mode = ShiftMode.None;
				switch (gShift.Value)
				{
					case "<=":
						mode = ShiftMode.OnOrBefore;
						break;
					case "<":
						mode = ShiftMode.Before;
						break;
					case "<>":
						mode = ShiftMode.Nearest;
						break;
					case ">":
						mode = ShiftMode.After;
						break;
					case "=>":
						mode = ShiftMode.OnOrAfter;
						break;
				}
				WDay wday = GetWDayFromAbbrev(gReq.Value);
				gen.Mode = mode;
				gen.RequiredDay = wday;
			}
		}


		/// <summary>
		/// Create a string representation of the date generator.
		/// </summary>
		public static string GetString(DateGenerator gen)
		{
			string s = null;
			if (gen is FixedIntervalGenerator)
			{
				s = "D" + ((FixedIntervalGenerator) gen).Cycle;
			}
			else if (gen is WeekDaysGenerator)
			{
				s = "Wmf,1";
			}
			else if (gen is WeeklyGenerator)
			{
				var weekly = (WeeklyGenerator) gen;
				var sb = new StringBuilder();
				bool[] days = weekly.Days;
				for (int i = 1; i < 7; i++)
				{
					if (days[i]) sb.Append("sumotuwethfrsa", i << 1, 2);
				}
				if (days[0]) sb.Append("su");
				string sDays = sb.ToString();
				switch (sDays)
				{
					case "motuwethfr":
						sDays = "mf";
						break;
					case "sasu":
						sDays = "ss";
						break;
					case "motuwethfrsasu":
						sDays = "ad";
						break;
				}
				s = "W" + sDays + "," + weekly.Cycle;
			}
			else if (gen is MonthlyAbsoluteGenerator)
			{
				var monthly = (MonthlyAbsoluteGenerator) gen;
				s = "M" + monthly.DayOfMonth + "," + monthly.Cycle;
			}
			else if (gen is MonthlyRelativeGenerator)
			{
				var monthly = (MonthlyRelativeGenerator) gen;
				string day = "admfssmotuwethfrsasu".Substring((int) monthly.DayId << 1, 2);
				if (gen is YearlyRelativeGenerator)
				{
					var yr = (YearlyRelativeGenerator) gen;
					s = "Y" + yr.MonthNumber + "," + yr.WeekInMonth + day;
				}
				else
				{
					s = "M" + monthly.WeekInMonth + day + "," + monthly.Cycle;
				}
			}
			else if (gen is AnniversaryGenerator)
			{
				var anniv = (AnniversaryGenerator) gen;
				s = "Y" + anniv.Month + "," + anniv.Day;
			}
			else if (gen is EnumDateGenerator)
			{
				var genEnum = (EnumDateGenerator) gen;
				s = "E" + string.Join(",", genEnum.Dates.Select(dt => dt.ToString(DateFmt)).ToArray());
			}
			else if (gen is MultiplexGenerator)
			{
				var genMerge = (MultiplexGenerator) gen;
				s = string.Join("|", genMerge.Generators.Select(g => GetString(g)).ToArray());
			}

			if (gen.HasRange)
			{
				s = string.Format("{0};{1};{2}",
				                  gen.Start.ToString(DateFmt),
				                  (gen.End == DateTime.MaxValue ? gen.Occurrences.ToString() : gen.End.ToString(DateFmt)),
				                  s);
			}

			if (gen.Mode != ShiftMode.None)
			{
				var symbols = new[] {"", "<>", "<=", "=>", "<", ">"};
				s += symbols[(int) gen.Mode] + "admfssmotuwethfrsasu".Substring((int) gen.RequiredDay << 1, 2);
			}
			return s;
		}
	}
}