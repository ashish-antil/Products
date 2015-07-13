using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace FernBusinessBase
{
	/// <summary>
	/// DataContract-based NameValueList. Intended to pass small sets of values between WCF services.
	/// </summary>
	/// <remarks>
	/// Efficient for enumeration only. Adding/removing keys and looking up by this[key] is not efficient.
	/// </remarks>
	[DataContract]
	public class NameValueList : IDictionary
	{
		/// <summary>
		/// not serializable, intended for local lock().
		/// </summary>
		private readonly object _Sync;

		[DataMember]
		private object[] _Keys;

		[DataMember]
		private object[] _Values;

		/// <summary>
		/// Create an empty NameValueList.
		/// </summary>
		public NameValueList()
		{
			_Sync = new object();
			Clear();
		}

		/// <summary>
		/// Create a NameValueList and populate it with the given dictionary.
		/// </summary>
		/// <param name="dict"></param>
		public NameValueList(IDictionary dict)
		{
			int n = dict.Keys.Count;
			_Keys = new object[n];
			_Values = new object[n];
			dict.Keys.CopyTo(_Keys, 0);
			dict.Values.CopyTo(_Values, 0);
		}

		public NameValueList(NameValueCollection nvc)
		{
			_Keys = nvc.AllKeys;
			int n = _Keys.Length;
			_Values = new object[n];
			for (int i = 0; i < n; i++) _Values[i] = nvc.Get(i);
		}


		#region IDictionary Members

		/// <summary>
		/// Add a new key and name, the key must be a string even tho the IDictionary interface would allow any object.
		/// </summary>
		/// <param name="key">string</param>
		/// <param name="value"></param>
		public void Add(object key, object value)
		{
			if (key == null) throw new ArgumentNullException("key");
			if (Contains(key)) throw new ArgumentException("Duplicate key");
			if (!(key is string)) throw new ArgumentException("Key is not String");

			int n = _Keys.Length;
			var newKeys = new object[n + 1];
			var newValues = new object[n + 1];
			Array.Copy(_Keys, newKeys, n);
			Array.Copy(_Values, newValues, n);
			newKeys[n] = key;
			newValues[n] = value;
			_Keys = newKeys;
			_Values = newValues;
		}

		public void Clear()
		{
			_Keys = new object[0];
			_Values = new object[0];			
		}

		public bool Contains(object key)
		{
			foreach (object k in _Keys) if (k.Equals(key)) return true;
			return false;
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return new Hashtable(this).GetEnumerator();
		}

		public bool IsFixedSize
		{
			get { return false; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public ICollection Keys
		{
			get { return _Keys.ToArray(); }
		}

		public void Remove(object key)
		{
			int i = Array.IndexOf(_Keys, key);
			_Keys = Array.FindAll<object>(_Keys, o => !o.Equals(key));
			object[] v = new object[_Keys.Length];
			Array.Copy(_Values, v, i);
			Array.Copy(_Values, i + 1, v, i, v.Length - i);
			_Values = v;
		}

		public ICollection Values
		{
			get { return _Values.ToArray(); }
		}

		public object this[object key]
		{
			get
			{
				int i = Array.IndexOf(_Keys, key);
				return i == -1 ? null : _Values[i];
			}
			set
			{
				int i = Array.IndexOf(_Keys, key);
				if (i == -1) Add(key, value);
				else if (value == null) Remove(key);
				else _Values[i] = value;
			}
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// Copies System.Collections.Generic.KeyValuePair<string,object> objects into an array starting
		/// at given index.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(Array array, int index)
		{
			for (int i = 0; i < _Keys.Length; i++)
			{
				var kv = new KeyValuePair<string, object> { Key = (string)_Keys[i], Value = _Values[i] };
				array.SetValue(kv, index + i);
			}
		}

		public int Count
		{
			get { return _Keys.Length; }
		}

		public bool IsSynchronized
		{
			get { return false; }
		}

		public object SyncRoot
		{
			get { return _Sync; }
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
