using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using FernBusinessBase;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace FernBusinessBase
{
	public class BusinessObjectCache<T> where T : BusinessEntity
	{
		private SlidingTime _Expiration;


		public TimeSpan Expiration
		{
			get
			{
				lock (_Sync) return _Expiration.ItemSlidingExpiration;
			}
			set
			{
				lock (_Sync) _Expiration = new SlidingTime(value);
			}
		}

		private readonly ICacheManager _Cache;
		private volatile int _Count;
		private readonly object _Sync = new object(); // can be used by wrapper classes to sync their code

		#region Singleton
		static BusinessObjectCache()  // see http://www.yoda.arachsys.com/csharp/singleton.html
		{
		}

		private static readonly BusinessObjectCache<T> _FernCache = new BusinessObjectCache<T>();

		public static BusinessObjectCache<T> Instance { get { return _FernCache; } }

		private BusinessObjectCache()
		{
			_Expiration = new SlidingTime(TimeSpan.FromMinutes(30)); // default
			_Cache = CacheFactory.GetCacheManager();
		}
		#endregion

		public int Count
		{
			get { lock (_Sync) return _Count;  }
		}

		#region indexer
		public T this[string itemKey]
		{
			get
			{
				lock (_Sync)
				{
					if (!string.IsNullOrEmpty(itemKey) && _Cache.Contains(itemKey))
					{
						return (T)_Cache[itemKey];
					}
					return null;
				}
			}
			set
			{
				lock (_Sync)
				{
					if (string.IsNullOrEmpty(itemKey)) return;
					if (!_Cache.Contains(itemKey) && value != null)
					{
						_Cache.Add(itemKey, value, CacheItemPriority.Normal, null, _Expiration);
						_Count++;
					}
					else
					{
						_Cache.Remove(itemKey);
						if (value != null)
						{
							_Cache.Add(itemKey, value, CacheItemPriority.Normal, null, _Expiration);
						}
					}
				}
			}
		}
		#endregion

		public bool Remove(string key)
		{
			lock (_Sync)
			{
				if (_Cache.Contains(key))
				{
					_Cache.Remove(key);
					_Count--;
					return true;
				}
				return false;
			}
		}

		public void RemoveAll()
		{
			lock (_Sync)
			{
				_Cache.Flush();
				_Count = 0;
			}
		}
	}
}
