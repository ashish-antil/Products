using System;

namespace Imarda.Lib
{
	/// <summary>
	/// Contains two time windows: a 48h and a 7d one. The latter gets populated with
	/// each entry representing a whole day total taken from the last 24h of the former.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ValueHistory<T>
		where T : class, new()
	{
		private readonly Func<T, T, T> _Add;
		private readonly Func<double, T, T> _Multiply;
		private readonly TimeWindow<T> _Window;


		public ValueHistory(Func<double, T, T> multiply, Func<T, T, T> add)
		{
			_Window = new TimeWindow<T>(TimeSpan.FromDays(8));
			_Multiply = multiply;
			_Add = add;
		}

		public TimeWindow<T> Window
		{
			get { return _Window; }
		}

		public void Add(DateTime dt, T value)
		{
			TimeWindow<T>.TimedValue? last = _Window.Last;
			_Window.Add(dt, value);
			if (last != null && last.Value.Timestamp.Date != dt.Date)
			{
				_Window.ReplaceBySum(last.Value.Timestamp.Date, TimeSpan.FromDays(1), _Add);
			}
		}

		public T Day(DateTime since)
		{
			return _Window.Slots(since, TimeSpan.FromDays(1), 1, false, _Multiply, _Add)[0];
		}

		public T Week(DateTime since)
		{
			return _Window.Slots(since, TimeSpan.FromDays(7), 1, false, _Multiply, _Add)[0];
		}
	}
}