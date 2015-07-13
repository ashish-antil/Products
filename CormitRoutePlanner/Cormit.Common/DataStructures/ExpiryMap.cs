using System;
using System.Collections.Generic;


namespace Imarda.Lib
{
	/// <summary>
	/// Maps a string to a T with an expiry date.
	/// </summary>
	public class ExpiryMap<T>
	{
		private class ExpiringItem
		{
			public ExpiringItem(T value, DateTime expiry)
			{
				Value = value;
				Expiry = expiry;
			}

			public DateTime Expiry;
			public readonly T Value;
		}

		private readonly object _Sync = new object();
		private readonly Dictionary<string, ExpiringItem> _Map;
		private readonly TimeSpan _DefaultExpiry;

		public ExpiryMap(TimeSpan defaultExpiry)
		{
			_Map = new Dictionary<string, ExpiringItem>();
			_DefaultExpiry = defaultExpiry;
		}

		public void Put(string key, T value, DateTime expiry)
		{
			lock (_Sync) _Map[key] = new ExpiringItem(value, expiry);
		}

		public void Put(string key, T value, TimeSpan expiry)
		{
			lock (_Sync) _Map[key] = new ExpiringItem(value, DateTime.UtcNow + expiry);
		}

		public void Put(string key, T value)
		{
			lock (_Sync) _Map[key] = new ExpiringItem(value, DateTime.UtcNow + _DefaultExpiry);
		}

		public bool Has(string key)
		{
			lock (_Sync) return _Map.ContainsKey(key);
		}

		/// <summary>
		/// Get the item for the key. 
		/// create = false: return given value if key not found or expired
		/// create = true: add given value if not exists, or extend life of expired one, return existing item or given value
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="create"></param>
		/// <returns></returns>
		public T Get(string key, T value, bool create)
		{
			lock (_Sync)
			{
				ExpiringItem tid;
				if (_Map.TryGetValue(key, out tid))
				{
					if (DateTime.UtcNow > tid.Expiry)
					{
						if (create)
						{
							tid.Expiry += _DefaultExpiry;
							return tid.Value;
						}
						_Map.Remove(key);
						return value;
					}
					return tid.Value;
				}
				
				if (create) _Map[key] = new ExpiringItem(value, DateTime.UtcNow + _DefaultExpiry);
				return value;
			}
		}

		/// <summary>
		/// Remove the object with the given key and return that object for further processing
		/// </summary>
		/// <param name="key"></param>
		/// <returns>the object, or default(T) if not found or expired</returns>
		public T Take(string key)
		{
			lock (_Sync)
			{
				T id = Get(key, default(T), false);
				Remove(key);
				return id;
			}
		}

		/// <summary>
		/// Remove the object with the given key
		/// </summary>
		/// <param name="key"></param>
		public void Remove(string key)
		{
			lock (_Sync) _Map.Remove(key);
		}

		/// <summary>
		/// Remove all expired objects
		/// </summary>
		public void Scavenge()
		{
			lock (_Sync)
			{
				var now = DateTime.UtcNow;
				var list = new List<string>();
				foreach (string key in _Map.Keys)
				{
					ExpiringItem tid = _Map[key];
					if (now > tid.Expiry) list.Add(key);
				}
				foreach (var key in list) _Map.Remove(key);
			}
		}
	}
}
