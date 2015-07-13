using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imarda.Lib
{
	/// <summary>
	/// Holds a list of timestamped values. Each new record must have a later timestamp than the other values.
	/// </summary>
	/// <remarks>
	/// Values may be removed from the beginning of the list to maintain the timespan of the window.
	/// At any time an array of time slots can be requested, each time slot holds the cumulative of the
	/// values that were recorded in the slot's time interval. Fractions of values can be assigned to 
	/// multiple slots. E.g. if a value was recorded on 20:00 Monday and another value, say 100, is
	/// recorded on 06:00 Tuesday, then the time interval covered by this value is (24-20)+6=10 hours.
	/// 4/10 of 100 = 40 will be allocated to the Monday slot and 6/10 of 100 = 60 will go to the Tuesday slot.
	/// This of course only if the slots are chosen with 24h intervals, with midnight boundaries.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public class TimeWindow<T> : IEnumerable<TimeWindow<T>.TimedValue>
		where T : class, new()
	{
		private readonly List<TimedValue> _Sequence;
		private readonly TimeSpan _Span;

		public TimeWindow(TimeSpan span)
		{
			_Sequence = new List<TimedValue>();
			_Span = span;
		}


		public TimedValue[] Sequence
		{
			get { return _Sequence.ToArray(); }
		}

		public TimedValue? Last
		{
			get
			{
				if (_Sequence.Count == 0) return null;
				else return _Sequence.Last();
			}
		}

		#region IEnumerable<TimeWindow<T>.TimedValue> Members

		public IEnumerator<TimedValue> GetEnumerator()
		{
			return _Sequence.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		/// <summary>
		/// Record a new value. If the time between the first value and the new value is
		/// larger than the timespan of this TimeWindow, then drop any older values 
		/// at the beginning of the list until the time span fits the window.
		/// </summary>
		/// <param name="time">the time must be later than the last recorded one</param>
		/// <param name="value">the value to be recorded</param>
		public void Add(DateTime time, T value)
		{
			if (time > _Sequence.LastOrDefault().Timestamp)
			{
				while (_Sequence.Count > 0 && time - _Sequence[0].Timestamp > _Span)
				{
					_Sequence.RemoveAt(0);
				}
				_Sequence.Add(new TimedValue(time, value));
			}
		}

		/// <summary>
		/// Finds the slice of the window between start and start+interval and 
		/// replaces all the values by the sum of the values, as defined by the 'add' delegate
		/// </summary>
		/// <param name="start">defines the start of the series to add up</param>
		/// <param name="interval">the timespan since start</param>
		/// <param name="add">defines how to add two values</param>
		public void ReplaceBySum(DateTime start, TimeSpan interval, Func<T, T, T> add)
		{
			int k = 0;
			DateTime end = start + interval;
			while (k < _Sequence.Count && _Sequence[k].Timestamp < start) k++;
			k++;
			DateTime? lastTime = null;
			var total = new T();
			while (k < _Sequence.Count && _Sequence[k].Timestamp <= end)
			{
				total = add(total, _Sequence[k].Value);
				lastTime = _Sequence[k].Timestamp;
				//Console.WriteLine("Remove [{0}] {1}  val {2}", k, _Sequence[k].Timestamp, _Sequence[k].Value);
				_Sequence.RemoveAt(k);
			}
			if (lastTime.HasValue)
			{
				_Sequence.Insert(k, new TimedValue(lastTime.Value, total)); // insert the sum.
				//Console.WriteLine("Total {0} insert at [{1}] @ {2}", total, k, lastTime);
			}
		}

		/// <summary>
		/// Create an array of intervals (slots) that aggregate the recorded values that fall inside
		/// each interval. 
		/// </summary>
		/// <param name="start">Start of window</param>
		/// <param name="interval">length of interval (width of time slot)</param>
		/// <param name="n">number of intervals</param>
		/// <param name="allowGaps">false to disallow gaps (slots that would be empty) using the next value, true to put 0 in empty slots</param>
		/// <param name="mult">function to multiple a factor 0..1 with a value, returning a fraction of the value</param>
		/// <param name="add">function to add 2 values</param>
		/// <returns></returns>
		public T[] Slots(DateTime start, TimeSpan interval, int n, bool allowGaps, Func<double, T, T> mult, Func<T, T, T> add)
		{
			var slots = new T[n];
			DateTime d0 = start;
			int k = 0;
			while (k < _Sequence.Count && _Sequence[k].Timestamp <= d0) k++;
			if (k < _Sequence.Count)
			{
				for (int i = 0; i < n; i++)
				{
					DateTime d1 = d0 + interval;
					DateTime p = d0;
					while (k < _Sequence.Count && _Sequence[k].Timestamp <= d1)
					{
						TimedValue seqk = _Sequence[k];
						double multiplier = GetMultiplier(d0, d1, k);
						T part = mult(multiplier, seqk.Value);
						if (slots[i] == null) slots[i] = part;
						else slots[i] = add(slots[i], part);
						p = seqk.Timestamp;
						k++;
					}
					if (k < _Sequence.Count && (!allowGaps || p != d0))
					{
						double remMultiplier = GetMultiplier(d0, d1, k);
						T remPart = mult(remMultiplier, _Sequence[k].Value);
						if (slots[i] == default(T)) slots[i] = remPart;
						else slots[i] = add(slots[i], remPart);
					}
					d0 = d1;
				}
			}
			return slots;
		}


		/// <summary>
		/// Get a multiplier for the value.
		/// </summary>
		/// <param name="d0">lower slot boundary</param>
		/// <param name="d1">upper slot boundary</param>
		/// <param name="k">index of record</param>
		/// <returns>value 0..1</returns>
		private double GetMultiplier(DateTime d0, DateTime d1, int k)
		{
			if (k == 0) return 0;
			DateTime p0 = _Sequence[k - 1].Timestamp; // previous measurement
			DateTime p1 = _Sequence[k].Timestamp; // current measurement
			if (p1 >= d1)
			{
				if (p0 < d0)
					return (d1 - d0).TotalHours/(p1 - p0).TotalHours;
						// p0..[d0]--[d1]..p1 value is split between previous, this and next slot
				else
					return (d1 - p0).TotalHours/(p1 - p0).TotalHours;
						// [d0]..p0--[d1]..p1 value is split between this slot and next slot
			}
			else if (p0 >= d0) return 1; // [d0]..p0--p1..[d1] value is entirely in this slot
			else
				return (p1 - d0).TotalHours/(p1 - p0).TotalHours;
					// p0..[d0]--p1..[d1] value is split between previous slot and this slot
		}

		#region Nested type: TimedValue

		/// <summary>
		/// Holds one timestamped value.
		/// </summary>
		public struct TimedValue
		{
			private readonly DateTime _Time;
			private readonly T _Value;

			internal TimedValue(DateTime time, T value)
			{
				_Time = time;
				_Value = value;
			}

			public T Value
			{
				get { return _Value; }
			}

			public DateTime Timestamp
			{
				get { return _Time; }
			}
		}

		#endregion
	}

	/// <summary>
	/// Helper methods for the TimeWindow, independent of the template parameter of TimeWindow
	/// </summary>
	public static class TimeWindowHelper
	{
		/// <summary>
		/// Given the user's current time, this method will calculate the user's start of the week (midnight Monday) and
		/// the user's midnight today as UTC values. These dates can be used in the Slots()
		/// as start dates.
		/// </summary>
		/// <param name="utc">current time</param>
		/// <param name="tzi">user's time zone</param>
		/// <param name="utcMonday">User's Monday midnight expressed in UTC</param>
		/// <param name="utcMidnightToday">User's midnight expressed in UTC</param>
		public static void TimeInfo(DateTime utc, TimeZoneInfo tzi, out DateTime utcMonday, out DateTime utcMidnightToday)
		{
			if (utc.Kind != DateTimeKind.Utc) throw new ArgumentException("need utc");
			DateTime dtUser = TimeZoneInfo.ConvertTimeFromUtc(utc, tzi);

			DateTime midnight = dtUser.Date;
			int daysSinceMonday = (int) dtUser.DayOfWeek - 1;
			if (daysSinceMonday == -1) daysSinceMonday = 6;
			DateTime monday = midnight.AddDays(-daysSinceMonday);
			utcMonday = TimeZoneInfo.ConvertTimeToUtc(monday, tzi);
			utcMidnightToday = TimeZoneInfo.ConvertTimeToUtc(midnight, tzi);
		}
	}
}