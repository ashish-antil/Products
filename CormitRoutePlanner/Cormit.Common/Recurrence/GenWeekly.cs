using System;
using System.Linq;

namespace Imarda.Lib
{
	/// <summary>
	/// Generates dates of certain days of the week. 
	/// </summary>
	public class WeeklyGenerator : DateGenerator
	{
		private readonly bool[] _Days;
		private readonly int _Interval;

		/// <summary>
		/// Create a weekly pattern generator
		/// </summary>
		/// <param name="n">cycle: every n weeks</param>
		/// <param name="days">[0] = Sunday, [1] = Monday, ... [6] = Saturday</param>
		public WeeklyGenerator(int n, bool[] days)
		{
			_Interval = n - 1;
			_Days = days;
			if (!days.Contains(true))
			{
				throw new ArgumentException("At least one weekday required", "days");
			}
		}

		public bool[] Days
		{
			get { return _Days; }
		}

		public int Cycle
		{
			get { return _Interval + 1; }
		}

		protected override DateTime First(DateTime date)
		{
			while (!IsDayOK(date)) date = date.AddDays(1);
			return date;
		}

		protected override DateTime Next(DateTime date)
		{
			do
			{
				date = date.AddDays(1);
				if (date.DayOfWeek == DayOfWeek.Monday) date = date.AddDays(_Interval*7);
			} while (!IsDayOK(date));
			return date;
		}

		private bool IsDayOK(DateTime date)
		{
			return _Days[(int) date.DayOfWeek];
		}

		public override DateTime Seek(DateTime start)
		{
			if (start < _Start) start = _Start;
			if (_Interval == 0) return First(start);
			else
			{
				return base.Seek(start);

				//TODO optimize, code below close, but not correct.
				//int dow = (int)_Start.DayOfWeek;
				//if (dow == 0) dow = 7;
				//DateTime monday1 = _Start.AddDays(1 - dow);
				//if (start < monday1.AddDays(7)) return First(start);

				//DateTime monday2 = _Start.AddDays(7);
				//int days = Convert.ToInt32((start.Date - monday2).TotalDays);
				//int cycleDays = Cycle * 7;
				//int cycles = days / cycleDays + 1;
				//DateTime x = monday2.AddDays(cycles * cycleDays).AddDays(-7);

				//return First(x);
			}
		}
	}
}