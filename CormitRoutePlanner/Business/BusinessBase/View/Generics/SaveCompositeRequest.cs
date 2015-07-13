using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	[MessageContract]
    public class SaveCompositeRequest : ParameterMessageBase
	{
		private Dictionary<string, object> _Items;

		/// <summary>
		/// Check if any items are set.
		/// </summary>
		public bool HasItems
		{
			get { return _Items != null && _Items.Keys.Count > 0; }
		}

		/// <summary>
		/// Prefered way of getting an item if it is not certain that
		/// the parameter exists.
		/// </summary>
		/// <param name="key">identifies the entity</param>
		/// <returns>value of parameter or null if does not exist</returns>
		/// <remarks>Efficient, does not create empty dictionary</remarks>
		public object GetItem(string key)
		{
			object item;
			if (HasItems && _Items.TryGetValue(key, out item)) return item;
			else return null;
		}

		/// <summary>
		/// Get the value and convert it to the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public new bool Get<T>(string key, out T item)
		{
			object x = GetItem(key);
			if (x == null || ! (x is T))
			{
				item = default(T);
				return false;
			}
			else
			{
				item = (T)x;
				return true;
			}
		}

		/// <summary>
		/// Get/set the entities. When doing a 'get', an empty Dictionary will be created
		/// when none exists.
		/// </summary>
		[MessageBodyMember]
		public Dictionary<string, object> Items
		{
			get
			{
				if (_Items == null) _Items = new Dictionary<string, object>();
				return _Items;
			}
			set
			{
				_Items = value;
				foreach (BaseEntity e in _Items.Values) e.PrepareForSave();
			}
		}


	}
}
