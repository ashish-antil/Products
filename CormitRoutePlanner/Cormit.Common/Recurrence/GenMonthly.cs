using System;

namespace Imarda.Lib
{
	/// <summary>
	/// E.g. every 24th of every 4th month
	/// </summary>
	public class MonthlyAbsoluteGenerator : DateGenerator
	{
		private readonly int _Day;
		private readonly int _Months;

		public MonthlyAbsoluteGenerator(int day, int months)
		{
			_Day = day;
			_Months = months;
		}

		public int DayOfMonth
		{
			get { return _Day; }
		}

		public int Cycle
		{
			get { return _Months; }
		}

		protected override DateTime First(DateTime date)
		{
			int n = _Day - date.Day;
			if (n >= 0) date = date.AddDays(n);
			else
			{
				date = date.AddMonths(1);
				date = new DateTime(date.Year, date.Month, _Day);
			}
			return date;
		}

		protected override DateTime Next(DateTime date)
		{
			return date.AddMonths(_Months);
		}

		public override DateTime Seek(DateTime start)
		{
			return _Months == 1 ? First(start) : base.Seek(start);
		}
	}

	/// <summary>
	/// E.g. the Monday of the last week of every 2nd month.
	/// </summary>
	public class MonthlyRelativeGenerator : DateGenerator
	{
		protected DayOfWeek _DayOfWeek;
		protected DayType _DayType;
		protected int _M;
		protected int _WeekInMonth;

		/// <summary>
		/// Create n-monthly repeating date generator.
		/// </summary>
		/// <param name="wday">Day of week</param>
		/// <param name="week">1=first, 2=second, 3=third, 4=fourth, 5=last</param>
		/// <param name="nMonths">how many months between events</param>
		public MonthlyRelativeGenerator(WDay wday, int week, int nMonths)
		{
			DayId = wday;
			switch (wday)
			{
				case WDay.Any:
					_DayType = DayType.EveryDay;
					break;
				case WDay.WeekDay:
					_DayType = DayType.WeekDay;
					break;
				case WDay.WeekendDay:
					_DayType = DayType.WeekendDay;
					break;
				default:
					_DayType = DayType.Specific;
					_DayOfWeek = (wday == WDay.Sun) ? DayOfWeek.Sunday : (DayOfWeek) (wday - 2);
					break;
			}
			_WeekInMonth = week;
			_M = nMonths;
		}

		public WDay DayId { get; private set; }

		public int WeekInMonth
		{
			get { return _WeekInMonth; }
		}

		public int Cycle
		{
			get { return _M; }
		}

		protected override DateTime First(DateTime date)
		{
			DateTime d = date.AddMonths(-_M);
			d = Next(d);
			if (_WeekInMonth < 5 && d < date) d = Next(date);
			return d;
		}

		protected override DateTime Next(DateTime date)
		{
			date = date.AddMonths(_M);
			int n = _WeekInMonth;
			if (n < 5)
			{
				date = new DateTime(date.Year, date.Month, 1).AddDays(-1);
				while (n-- > 0)
				{
					do
					{
						date = date.AddDays(1);
					} while (!DayTypeOK(date));
				}
			}
			else
			{
				date = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
				while (!DayTypeOK(date)) date = date.AddDays(-1);
			}
			return date;
		}

		public override DateTime Seek(DateTime start)
		{
			return _M == 1 ? First(start) : base.Seek(start);
		}


		private bool DayTypeOK(DateTime date)
		{
			bool ok;
			bool weekday = (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday);
			switch (_DayType)
			{
				case DayType.WeekDay:
					ok = weekday;
					break;
				case DayType.WeekendDay:
					ok = !weekday;
					break;
				case DayType.Specific:
					ok = (date.DayOfWeek == _DayOfWeek);
					break;
				default:
					ok = true;
					break;
			}
			return ok;
		}

		#region Nested type: DayType

		protected enum DayType
		{
			EveryDay,
			WeekDay,
			WeekendDay,
			Specific
		}

		#endregion
	}
}