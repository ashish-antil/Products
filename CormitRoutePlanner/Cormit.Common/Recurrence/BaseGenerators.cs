using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imarda.Lib
{
	public enum ShiftMode
	{
		None, // no date shifting
		Nearest = 1, // shift to nearest specified day
		OnOrBefore = 2, // if calculated date not on specified date, then take first specified day before
		OnOrAfter = 3, // if calculated date not on specified date, then take first specified day after
		Before = 4, // shift to first specified day before the calculated date
		After = 5, // shift to first specified day after the calculated date
	}

	public enum WDay
	{
		Invalid = -1,
		Any = 0,
		WeekDay = 1,
		WeekendDay = 2,
		Mon = 3,
		Tue = 4,
		Wed = 5,
		Thu = 6,
		Fri = 7,
		Sat = 8,
		Sun = 9
	}


	/// <summary>
	/// Base class for all DateTime generators that share the same event name.
	/// </summary>
	public abstract class DateGenerator : Generator
	{
		protected DateGenerator()
		{
		}

		protected DateGenerator(ShiftMode direction, WDay wday)
		{
			Mode = direction;
			RequiredDay = wday;
		}

		/// <summary>
		/// Specify a correction for the calculated date, e.g. ShiftMode.Nearest will
		/// change the calculated date to the nearest specified day (see RequiredDay).
		/// </summary>
		public ShiftMode Mode { get; set; }

		/// <summary>
		/// Day of week to shift date to when Direction != ShiftMode.None
		/// </summary>
		public WDay RequiredDay { get; set; }


		internal IEnumerator<DateTime> GetEnumerator(DateTime start, bool seek)
		{
			DateTime date = seek ? Seek(start) : First(start);
			if (date > default(DateTime))
			{
				int number = 1;
				while (number++ <= _Occurrences && date <= _End)
				{
					DateTime date1 = ShiftDate(date);
					if (date1 >= start && date1 <= _End) yield return date1;
					date = Next(date);
				}
			}
		}

		public DateTime OnOrAfter(DateTime dt)
		{
			return new Iterator(this, dt).FirstOrDefault();
		}

		public IEnumerable<DateTime> Series(DateTime seek)
		{
			return new Iterator(this, seek);
		}

		public IEnumerable<DateTime> Series()
		{
			return new Iterator(this, _Start);
		}

		/// <summary>
		/// Get the first date on of after the given start date valid for the
		/// series defined by this Generator.
		/// </summary>
		/// <param name="start"></param>
		/// <returns></returns>
		public virtual DateTime Seek(DateTime start)
		{
			foreach (DateTime date in Series().Where(date => date >= start))
			{
				return date;
			}
			return default(DateTime);
		}


		private DateTime ShiftDate(DateTime dt)
		{
			switch (Mode)
			{
				case ShiftMode.None: // most often the case
					break;

				case ShiftMode.Nearest:
					DateTime before = ShiftDay(dt, 0, 7, -1);
					DateTime after = ShiftDay(dt, 0, 7, 1);
					return Math.Abs(dt.Ticks - before.Ticks) < Math.Abs(dt.Ticks - after.Ticks) ? before : after;

				case ShiftMode.OnOrAfter:
					return ShiftDay(dt, 0, 7, 1);
				case ShiftMode.After:
					return ShiftDay(dt, 1, 8, 1);

				case ShiftMode.OnOrBefore:
					return ShiftDay(dt, 0, 7, -1);
				case ShiftMode.Before:
					return ShiftDay(dt, 1, 8, -1);
			}
			return dt;
		}

		private DateTime ShiftDay(DateTime dt, int from, int to, int step)
		{
			DateTime dt1;
			for (int i = from; i < to; i++)
			{
				dt1 = dt.AddDays(step*i);
				if (IsDayOK(dt1.DayOfWeek)) return dt1;
			}
			return dt;
		}

		private bool IsDayOK(DayOfWeek dow)
		{
			switch (RequiredDay)
			{
				case WDay.Any:
					return true;
				case WDay.WeekDay:
					return dow != DayOfWeek.Saturday && dow != DayOfWeek.Sunday;
				case WDay.WeekendDay:
					return dow == DayOfWeek.Saturday || dow == DayOfWeek.Sunday;
				default:
					if (dow == DayOfWeek.Sunday) return RequiredDay == WDay.Sun;
					else return (WDay) ((int) dow + 2) == RequiredDay;
			}
		}


		protected abstract DateTime First(DateTime date);

		protected abstract DateTime Next(DateTime date);

		#region Nested type: Iterator

		private class Iterator : IEnumerable<DateTime>
		{
			private readonly DateGenerator _Gen;
			private readonly DateTime _Seek;

			internal Iterator(DateGenerator gen, DateTime start)
			{
				_Gen = gen;
				_Seek = start;
			}

			#region IEnumerable<DateTime> Members

			public IEnumerator<DateTime> GetEnumerator()
			{
				return _Gen.GetEnumerator(_Seek, _Seek != _Gen.Start);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			#endregion
		}

		#endregion
	}


	/// <summary>
	/// Base class for all event generators.
	/// </summary>
	public abstract class EventGenerator : Generator, IEnumerable<ICalEvent>
	{
		#region IEnumerable<ICalEvent> Members

		public abstract IEnumerator<ICalEvent> GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}


	/// <summary>
	/// This generator will generate all events for each of the
	/// event generators passed to its constructor, in the order
	/// of the parameters.
	/// </summary>
	public class MultiEventGenerator : EventGenerator
	{
		private readonly EventGenerator[] _Generators;

		public MultiEventGenerator(params EventGenerator[] args)
		{
			_Generators = args;
		}

		public override IEnumerator<ICalEvent> GetEnumerator()
		{
			return _Generators.SelectMany(gen => gen).GetEnumerator();
		}

		/// <summary>
		/// Make start and end known to subgenerators.
		/// </summary>
		protected override void AfterSetRangeEnd()
		{
			foreach (EventGenerator gen in _Generators) gen.SetRange(_Start, _End);
		}

		/// <summary>
		/// Make start and number of occurrences known to subgenerators.
		/// </summary>
		protected override void AfterSetRangeOcc()
		{
			foreach (EventGenerator gen in _Generators) gen.SetRange(_Start, _Occurrences);
		}
	}


	/// <summary>
	/// Recurring events, based on a date pattern, but with the same name for each event.
	/// Handy for public holiday calendars, anniversaries, birth days...
	/// </summary>
	public class PatternEventGenerator<T> : EventGenerator
		where T : ICalEvent, new()
	{
		private readonly DateGenerator _dateGen;
		private readonly string _eventName;

		public PatternEventGenerator(string eventName, DateGenerator dateGen)
		{
			_eventName = eventName;
			_dateGen = dateGen;
		}

		public override IEnumerator<ICalEvent> GetEnumerator()
		{
			foreach (DateTime dt in _dateGen.Series()) yield return new T {Date = dt, Name = _eventName};
		}

		protected override void AfterSetRangeEnd()
		{
			_dateGen.SetRange(_Start, _End);
		}

		protected override void AfterSetRangeOcc()
		{
			_dateGen.SetRange(_Start, _Occurrences);
		}
	}


	/// <summary>
	/// Generates events simply by iterating over the given dates.
	/// </summary>
	public class EnumGenerator<T> : PatternEventGenerator<T>
		where T : ICalEvent, new()
	{
		public EnumGenerator(string eventName, IEnumerable<DateTime> dates)
			: base(eventName, new EnumDateGenerator(dates))
		{
		}
	}
}