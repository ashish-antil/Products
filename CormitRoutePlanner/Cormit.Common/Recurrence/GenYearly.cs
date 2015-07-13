using System;

namespace Imarda.Lib
{
	/// <summary>
	/// Generates anniversaries.
	/// </summary>
	public class AnniversaryGenerator : DateGenerator
	{
		private readonly int _Day;
		private readonly int _Month;

		/// <summary>
		/// Generates annual fixed dates.
		/// </summary>
		/// <param name="month">1=Jan, 2=Feb, ..., 12=Dec</param>
		/// <param name="day">1=first, 2=second, 3=third, 4=fourth, 5=last</param>
		public AnniversaryGenerator(int month, int day)
		{
			_Month = month;
			_Day = day;
		}


		public int Month
		{
			get { return _Month; }
		}

		public int Day
		{
			get { return _Day; }
		}


		protected override DateTime First(DateTime date)
		{
			var d = new DateTime(date.Year, _Month, _Day);
			if (d < date) d = Next(d);
			return d;
		}

		protected override DateTime Next(DateTime date)
		{
			var d = new DateTime(date.Year, _Month, _Day);
			d = d.AddYears(1);
			return d;
		}

		public override DateTime Seek(DateTime start)
		{
			return First(start);
		}
	}

	/// <summary>
	/// E.g. "every 2nd Sunday of May"
	/// </summary>
	public class YearlyRelativeGenerator : MonthlyRelativeGenerator
	{
		private readonly int _monthNumber;

		/// <summary>
		/// Create yearly repeating date generator.
		/// </summary>
		/// <param name="wday">Day of week</param>
		/// <param name="week">1=first, 2=second, 3=third, 4=fourth, 5=last</param>
		/// <param name="monthNumber">1=Jan, 2=Feb, ..., 12=Dec</param>
		public YearlyRelativeGenerator(WDay wday, int week, int monthNumber)
			: base(wday, week, 12)
		{
			_monthNumber = monthNumber;
		}

		public int MonthNumber
		{
			get { return _monthNumber; }
		}

		protected override DateTime First(DateTime date)
		{
			var d1 = new DateTime(date.Year, _monthNumber, 1);
			DateTime df = base.First(d1);
			if (df >= date) return df;
			d1 = d1.AddYears(1);
			return base.First(d1);
		}

		public override DateTime Seek(DateTime start)
		{
			return First(start);
		}
	}
}