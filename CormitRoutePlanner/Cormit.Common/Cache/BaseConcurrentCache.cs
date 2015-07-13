using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
//using Imarda.Lib.MVVM.Extensions;
using MoreLinq;

// ReSharper disable once CheckNamespace
namespace Imarda.Common.Cache
{
	public abstract class BaseConcurrentCache<TKey,TEntry> :IDisposable
		where TEntry : class
	{
		private ConcurrentDictionary<TKey, TEntry> _dictionary;

		protected ConcurrentDictionary<TKey, TEntry> Dictionary
		{
			get { return _dictionary ?? (_dictionary = new ConcurrentDictionary<TKey, TEntry>()); }

			set { _dictionary = value; }
		}

		public int Count
		{
			get { return Dictionary.Count; }
		}

		public void Clear()
		{
			Dictionary.Clear();
		}

		public void Purge(int howManyKeeps)
		{
			if(Dictionary.Count <= howManyKeeps){return;}
			var removables = Dictionary.Take(howManyKeeps).Select(p=>p.Key);
			removables.ForEach(k=>Remove(k));
		}

		public bool HasEntries
		{
			get { return Dictionary.Count > 0; }
		}

		public TEntry GetEntry(TKey key)
		{
			TEntry entry;
			if (Dictionary.TryGetValue(key, out entry))
			{
				return entry;
			}
			return null;
		}

        public TEntry GetFirstEntry()
        {
            return Dictionary.FirstOrDefault().Value;
        }

		public IEnumerable<TEntry> GetEntries(Func<TEntry, bool> predicate)
		{
            return Dictionary.Where(entry => predicate(entry.Value)).Select(entry => entry.Value);
		}

       

        //public IEnumerable<TEntry> GetEntryViaParams()
        //{
        //    foreach (var dict in Dictionary)
        //    {
        //        dict.Value as Att
        //    }
        //}

		public void UpdateEntry(TKey key, TEntry entry)
		{
			if (Dictionary.ContainsKey(key))
			{
				Dictionary[key] = entry;
			}
		}

		public abstract bool AddEntry(TEntry val);

		public bool Remove(TKey key)
		{
			TEntry val;
			return Dictionary.TryRemove(key, out val);
		}

		public void Dispose()
		{
			_dictionary.Clear();
			_dictionary = null;
		}
	}

}
