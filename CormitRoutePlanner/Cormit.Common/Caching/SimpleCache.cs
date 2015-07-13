using System;
using System.Collections.Generic;
using System.Linq;

namespace Imarda.Lib
{
	public class SimpleCache<K, T>
		where T: class
	{
		private object _Sync = new object();
		private Dictionary<K, T> _Cache;

		public SimpleCache()
		{
			_Cache = new Dictionary<K, T>();
		}

		public int CacheCount
		{
			get { lock (_Sync) return _Cache.Count; }
		}


		public T Get(K id)
		{
			lock (_Sync)
			{
				T target;

				if (_Cache.TryGetValue(id, out target)) return target;
				return null;
			}
		}

		public void Put(K id, T target)
		{
			lock (_Sync)
			{
				if (target == null) throw new ArgumentNullException();
				_Cache[id] = target;
			}
		}

		public bool Remove(K id)
		{
			lock (_Sync) return _Cache.Remove(id);
		}

		public string[] ToStrings()
		{
			lock (_Sync) return _Cache.Values.Select(item => item.ToString()).ToArray();
		}

		public T[] ToArray()
		{
			lock (_Sync) return _Cache.Values.ToArray();
		}

		public override string ToString()
		{
			return string.Format("SimpleCache<{0}>({1})", typeof(T).Name, _Cache.Count);
		}
	}
}
