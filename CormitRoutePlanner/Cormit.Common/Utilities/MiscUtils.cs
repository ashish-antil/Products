using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Imarda.Lib
{
	/// <summary>
	/// Lazy evaluation of no-arg functions.
	/// </summary>
	/// <typeparam name="T">return type of Value</typeparam>
	public sealed class Lazy<T>
	{
		// E.g. 
		// var x = new Lazy<double>(() => ..some expensive calc..);
		// double d = x.Value; // first time use will evaluate, then cache result
		// double d2 = x.Value; // use cached result.
		private readonly Func<T> _Func;
		private bool _HasValue;
		private T _Value;

		/// <summary>
		/// Pass a lambda expression with empty parentheses, such as:  ()=>1+1
		/// </summary>
		/// <param name="func"></param>
		public Lazy(Func<T> func)
		{
			_Func = func;
		}

		public T Value
		{
			get
			{
				if (!_HasValue)
				{
					_Value = _Func();
					_HasValue = true;
				}
				return _Value;
			}
		}
	}

	/// <summary>
	/// Class to for conditional execution of delegates.
	/// </summary>
	/// <example>
	/// Execute.Once("Init1", () => MyInit());
	/// Execute.Twice("InOut", () => Swap());
	/// Execute.Max("Check", () => Print(), 5);
	/// </example>
	public static class Execute
	{
		#region execution control

		private static Dictionary<string, int> _TallyMap;

		private static Dictionary<string, int> TallyMap
		{
			get { return _TallyMap ?? (_TallyMap = new Dictionary<string, int>()); }
		}

		/// <summary>
		/// Execute the named action only once. If executed before, do nothing.
		/// This method is threadsafe.
		/// </summary>
		/// <param name="s">name of action</param>
		/// <param name="action">action to execute</param>
		/// <returns>true if executed now, false if executed once before</returns>
		public static bool Once(string s, Action action)
		{
			return Max(1, s, action);
		}

		public static bool Twice(string s, Action action)
		{
			return Max(2, s, action);
		}

		public static bool Max(int n, string s, Action action)
		{
			bool todo;
			lock (TallyMap)
			{
				int remaining;
				if (!TallyMap.TryGetValue(s, out remaining)) remaining = TallyMap[s] = n;
				todo = remaining > 0;
				TallyMap[s]--;
			}
			if (todo) action();
			return todo;
		}


		/// <summary>
		/// Forget that an action was ever executed.
		/// </summary>
		/// <param name="s"></param>
		/// <return>true if was executed before, false if was not executed</return>
		public static bool Forget(string s)
		{
			lock (TallyMap) return TallyMap.Remove(s);
		}

		/// <summary>
		/// Forget all that was executed before
		/// </summary>
		public static void ForgetAll()
		{
			lock (TallyMap) _TallyMap = null;
		}

		#endregion

		public static void Async(Action action)
		{
			ThreadPool.QueueUserWorkItem(x => action());
		}

		private static Exception TrySync(int n, TimeSpan pause, Action action)
		{
			while (n-- > 0)
			{
				try
				{
					action();
					return null;
				}
				catch (Exception ex)
				{
					if (n == 0) return ex; // don't sleep anymore after last try.
					Thread.Sleep(pause);
				}
			}
			return null; // if n<=0
		}

		/// <summary>
		/// Try to execute something until it succeeds (no exception), or for a max number of times.
		/// </summary>
		/// <param name="async">true to run in separate thread, false in same thread.</param>
		/// <param name="n">how often to try, 0=do not try, 1=try once, 2=try twice etc.</param>
		/// <param name="pause">how long to wait between tries</param>
		/// <param name="action">delegate to execute</param>
		/// <returns>exception or null</returns>
		public static Exception Try(bool async, int n, TimeSpan pause, Action action)
		{
			if (async)
			{
				ThreadPool.QueueUserWorkItem(x => TrySync(n, pause, action));
				return null;
			}
			return TrySync(n, pause, action);
		}

		public static void Later(TimeSpan pause, Action action)
		{
			ThreadPool.QueueUserWorkItem(
				delegate
				{
					Thread.Sleep(pause);
					action();
				});
		}
	}



	/// <summary>
	/// Access methods for a hitcounter, e.g. for cache hits.
	/// Also manages a collection of hitcounters.
	/// </summary>
	public class HitCounter
	{
		#region static:

		private static readonly Dictionary<string, HitCounter> _Map = new Dictionary<string, HitCounter>();

		public static HitCounter Create(string name)
		{
			var counter = new HitCounter(name, 0L, DateTime.MinValue);
			lock (_Map) _Map.Add(name, counter);
			return counter;
		}

		public static void Delete(string name)
		{
			lock (_Map) _Map.Remove(name);
		}

		public static HitCounter Get(string name)
		{
			lock (_Map) return _Map[name];
		}

		public static long Sum(params string[] names)
		{
			lock (_Map)
			{
				return names.Sum(name => _Map[name].Hits);
			}
		}

		public static long Sum(params HitCounter[] counters)
		{
			return counters.Sum(counter => counter.Hits);
		}

		#endregion static.

		private long _Hits;
		private long _LastHit;

		private HitCounter(string name, long hits, DateTime lastHit)
		{
			Name = name;
			_Hits = hits;
			_LastHit = lastHit.Ticks;
		}

		public string Name { get; private set; }

		/// <summary>
		/// Get or set the counter. When setting: the LastHit is not updated, unless value == 0 then it is reset
		/// </summary>
		public long Hits
		{
			get { return Interlocked.Read(ref _Hits); }
			set
			{
				Interlocked.Exchange(ref _Hits, value);
				if (value == 0L) Interlocked.Exchange(ref _Hits, DateTime.MinValue.Ticks);
			}
		}

		public DateTime LastHit
		{
			get { return new DateTime(Interlocked.Read(ref _LastHit), DateTimeKind.Utc); }
		}

		public void Increment()
		{
			Interlocked.Increment(ref _Hits);
			Interlocked.Exchange(ref _LastHit, DateTime.UtcNow.Ticks);
		}
	}

	/// <summary>
	/// An object of this class wraps an action and will execute that action
	/// every Nth time Notify() is called. N is a constructor parameter but can
	/// be changed with Set(). Class is not threadsafe.
	/// </summary>
	public sealed class Cycle
	{
		private int _Cycles;
		private int _Counter;
		private readonly Action _Action;

		public Cycle(int cycles, int counter, Action action)
		{
			_Cycles = cycles;
			_Counter = counter;
			_Action = action;
		}

		public Cycle(int cycles, int counter)
			: this(cycles, counter, null)
		{
		}

		public Cycle(int cycles, Action action)
			: this(cycles, 0, action)
		{
		}

		/// <summary>
		/// The Nth time calling this will execute the action, if any
		/// </summary>
		/// <returns>true if the cycle has been reached, false if not yet</returns>
		public bool Notify()
		{
			if (++_Counter >= _Cycles)
			{
				if (_Action != null) _Action();
				_Counter = 0;
				return true;
			}
			return false;
		}

		public void Set(int cycles, int counter)
		{
			_Cycles = cycles;
			_Counter = counter;
		}
	}


	/// <summary>
	/// Execute a given action after a delay since the last execution.
	/// Call Notify() to test if it is time to execute the action, and execute if it is the case.
	/// Not threadsafe. 
	/// </summary>
	public sealed class DelayCycle
	{
		private TimeSpan _Delay;
		private DateTime _Next;
		private readonly Action _Action;

		public DelayCycle(TimeSpan delay, Action action, bool wait)
		{
			_Delay = delay;
			_Next = DateTime.UtcNow;
			if (wait) _Next += _Delay;
			_Action = action;
		}

		public DelayCycle(TimeSpan delay, Action action)
			: this(delay, action, false)
		{
		}

		public DelayCycle(TimeSpan delay)
			: this(delay, null)
		{
		}

		/// <summary>
		/// Execute the action when the time has come.
		/// </summary>
		/// <returns>true if the time has come, false if not yet</returns>
		public bool Notify()
		{
			if (DateTime.UtcNow >= _Next)
			{
				if (_Action != null) _Action();
				_Next = DateTime.UtcNow + _Delay;
				return true;
			}
			return false;
		}

		public void Set(TimeSpan delay)
		{
			_Delay = delay;
		}
	}
}