using System;
using System.Collections.Generic;
using System.Text;
using Imarda360.Infrastructure.ConfigurationService;

namespace ImardaConfigurationBusiness
{
	/// <summary>
	/// Caches calculated configuration values. 
	/// </summary>
	public class ConfigCache
	{
		#region Static
		private static ConfigCache _Instance = new ConfigCache();

		public static ConfigCache Instance
		{
			get { return _Instance; }
		}

		#endregion 

		private Dictionary<ConfigKey, ConfigValue> _Cache;

		public ConfigCache()
		{
			_Cache = new Dictionary<ConfigKey, ConfigValue>();
		}

		public ConfigValue this[ConfigKey key]
		{
			get
			{
				lock (_Cache)
				{
					ConfigValue cv;
					return _Cache.TryGetValue(key, out cv) ? cv : null;
				}
			}
			set
			{
				lock (_Cache)
				{
					_Cache[key] = value;
				}
			}
		}

		public void Clear()
		{
			lock (_Cache) _Cache.Clear();
		}

		/// <summary>
		/// Remove any key that matches the wildcard spec.
		/// If the spec has Guid.Empty as ID, then only match the levels. If ID is given then ID must match.
		/// If the spec has less levels than an entry, but the existing levels match, then remove the entry.
		/// </summary>
		/// <param name="spec"></param>
		/// <returns></returns>
		public int RemoveMatch(ConfigKey spec)
		{
			lock (_Cache)
			{
				var matchingKeys = new List<ConfigKey>();
				// loop thru entire cache
				foreach (ConfigKey key in _Cache.Keys)
				{
					if (key.ID == spec.ID || spec.ID == Guid.Empty)
					{
						Guid[] specLevels = spec.Context.Levels;
						Guid[] actualLevels = key.Context.Levels;
						if (specLevels.Length > actualLevels.Length)
						{
							continue;
						}
						bool match = true;
						for (int i = 0; i < specLevels.Length; i++)
						{
							if (specLevels[i] != actualLevels[i])
							{
								match = false;
								break;
							}
						}
						if (match) matchingKeys.Add(key);
					}
				}

				// remove all the matching keys
				int count = 0;
				foreach (ConfigKey key in matchingKeys) 
				{
					_Cache.Remove(key);
					count++;
				}
				return count;
			}
		}

		public void RemoveUID(Guid uid)
		{
			if (uid == Guid.Empty) return;
			lock (_Cache)
			{
				ConfigKey found = null;
				foreach (ConfigKey key in _Cache.Keys)
				{
					if (this[key].UID == uid)
					{
						found = key;
						break;
					}
				}

				if (found != null)
				{
					_Cache.Remove(found);
				}
			}
		}

		public int Count { get { lock (_Cache) return _Cache.Count; } }

	}
}
