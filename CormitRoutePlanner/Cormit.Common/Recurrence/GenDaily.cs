using System;

namespace Imarda.Lib
{
	public class WeekDaysGenerator : DateGenerator
	{
		protected override DateTime First(DateTime date)
		{
			return Next(date.AddDays(-1));
		}

		protected override DateTime Next(DateTime date)
		{
			DayOfWeek dow = date.DayOfWeek;
			int days;
			switch (dow)
			{
				case DayOfWeek.Friday:
					days = 3;
					break;
				case DayOfWeek.Saturday:
					days = 2;
					break;
				default:
					days = 1;
					break;
			}
			return date.AddDays(days);
		}

		public override DateTime Seek(DateTime start)
		{
			return First(start);
		}
	}

	public class FixedIntervalGenerator : DateGenerator
	{
		private readonly int _Interval;

		public FixedIntervalGenerator(int numberOfDays)
		{
			_Interval = numberOfDays;
		}

		public int Cycle
		{
			get { return _Interval; }
		}

		protected override DateTime First(DateTime date)
		{
			return date;
		}

		protected override DateTime Next(DateTime date)
		{
			return date.AddDays(_Interval);
		}

		public override DateTime Seek(DateTime start)
		{
			int days = Convert.ToInt32((start.Date - _Start.Date).TotalDays);
			if (days <= 0) return _Start;
			int actual = ((days - 1)/_Interval + 1)*_Interval;
			return _Start + TimeSpan.FromDays(actual);
		}
	}
}