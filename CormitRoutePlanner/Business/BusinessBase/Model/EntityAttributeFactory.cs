using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using Imarda.Lib;
using Imarda.Logging;

namespace FernBusinessBase
{
	/// <summary>
	/// Factory for objects based on EntityAttributes. Threadsafe.
	/// The factory stores PropertyInfo objects and other information
	/// which is used to quickly create objects rather than having
	/// to go thru the whole reflection cycle.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EntityAttributesFactory<T> where T : new()
	{
		/// <summary>
		/// Property information.
		/// </summary>
		private readonly EAInternal[] _Properties;

		/// <summary>
		/// Create a factory object.
		/// </summary>
		public EntityAttributesFactory()
		{
			var dfltAttr = new BusinessInfoAttribute();
			bool allBusinessInfo = typeof(T).GetCustomAttributes(typeof(BusinessInfoAttribute), false).Length > 0;
			var list = new List<EAInternal>();
			PropertyInfo[] props = typeof(T).GetProperties();
			try
			{
				foreach (PropertyInfo prop in props)
				{
					var attr = (BusinessInfoAttribute[])prop.GetCustomAttributes(typeof(BusinessInfoAttribute), false);
					string key = null;
					if (attr.Length > 0) key = attr[0].Key ?? prop.Name;
					if (key == null && allBusinessInfo) key = prop.Name;

					if (key != null)
					{
						if (!prop.CanRead || !prop.CanWrite)
						{
							throw new Exception("BusinessInfoAttribute - no read/write property: " + key);
						}
						BusinessInfoAttribute bizAttr = attr.Length == 0 ? dfltAttr : attr[0];
						var info = new EAInternal(key, prop, bizAttr.Count, bizAttr.Var, bizAttr.Store, bizAttr.Serialize);
						var fmt = (DisplayFormatAttribute[])prop.GetCustomAttributes(typeof(DisplayFormatAttribute), false);
						if (fmt.Length > 0)
						{
							info.Format = fmt[0].Format;
						}
						list.Add(info);
					}
				}
				_Properties = list.ToArray();
				Array.Sort(_Properties, (p1, p2) => string.Compare(p1.Key, p2.Key));
			}
			catch (Exception ex)
			{
				DebugLog.Write(ex);
			}
		}

		/// <summary>
		/// Create a target object based on information found in the EntityAttributes.
		/// </summary>
		/// <remarks>Create the entity attributes by iterating thru the target object properties
		/// and fetching and lookup the property names or keys in the entity attributes. Skip
		/// attributes that </remarks>
		/// <param name="entityAttr">contains property information</param>
		/// <returns>an initialized object</returns>
		public T Create(EntityAttributes entityAttr)
		{
			var target = new T();
			Populate(target, entityAttr);
			return target;
		}

		/// <summary>
		/// Populate an existing object with entity attributes
		/// </summary>
		/// <param name="target">object to be populated</param>
		/// <param name="entityAttr">attribute data</param>
		public void Populate(T target, EntityAttributes entityAttr)
		{
			foreach (EAInternal prop in _Properties)
			{
				try
				{
					PropertyInfo pinfo = prop.Property;
					Type t = pinfo.PropertyType;
					object eaVal = entityAttr.Map[prop.Key];

					if (eaVal == null)
					{
						if (t.IsArray && prop.Count > 0)
						{
							var tArr = Array.CreateInstance(t.GetElementType(), prop.Count);
							pinfo.SetValue(target, tArr, null);
						}
					}
					else
					{
						if (t.IsArray)
						{
							if (eaVal is string) eaVal = new [] { (string)eaVal };
							t = t.GetElementType();
						}

						object value;
						if (eaVal is string[])
						{
							string[] sArr = (string[])eaVal;
							var tArr = Array.CreateInstance(t, Math.Max(sArr.Length, prop.Count));
							for (int i = 0; i < sArr.Length; i++)
							{
								if (EAHelper.TryParse(sArr[i], t, out value))
								{
									tArr.SetValue(value, i);
								}
							}
							pinfo.SetValue(target, tArr, null);
						}
						else if (typeof(Enum).IsAssignableFrom(t))
						{
							object v = Enum.Parse(t, (string)eaVal);
							pinfo.SetValue(target, v, null);
						}
						else
						{
							if (EAHelper.TryParse((string)eaVal, t, out value))
							{
								try
								{
									pinfo.SetValue(target, value, null);
								}
								catch { }
							}
						}
					}
				}
				catch (Exception ex)
				{
					DebugLog.Write(ex);
				}
			}
		}

		/// <summary>
		/// Store the fields of the 'source' object into the entity attributes object, but only if their 'store' attribute is true
		/// </summary>
		/// <param name="ea"></param>
		/// <param name="source"></param>
		public void Store(EntityAttributes ea, object source)
		{
			IDictionary map = ea.Map;
			foreach (EAInternal prop in _Properties)
			{
				if (prop.Store)
				{
					object val = prop.Property.GetValue(source, null);
					map[prop.Key] = EntityAttributes.GetValue(val, prop.Format);
				}
			}
		}

		/// <summary>
		/// Store the GIVEN fields of the 'source' object into the entity attributes object.
		/// </summary>
		/// <param name="ea"></param>
		/// <param name="source"></param>
		/// <param name="fields">comma separated string of attribute keys</param>
		public void Store(EntityAttributes ea, object source, HashSet<string> fields)
		{
			IDictionary map = ea.Map;
			foreach (EAInternal prop in _Properties)
			{
				if (fields.Contains(prop.Key))
				{
					object val = prop.Property.GetValue(source, null);
					map[prop.Key] = EntityAttributes.GetValue(val, prop.Format);
				}
			}
		}

		///// <summary>
		///// Get type and format info of a given property.
		///// </summary>
		///// <param name="key">property name, or key as specified in BusinessInfoAttribute</param>
		///// <param name="type">property type</param>
		///// <param name="format">optional format info as specified in MeasurementFormatAttribute, maybe null</param>
		///// <returns>true if found, false if not found</returns>
		//public bool GetInfo(string key, out Type type, out string format)
		//{
		//  int index = Array.BinarySearch<EAInternal>(_Properties, new EAInternal(key, null, 0, 0, false, false));
		//  if (index < 0)
		//  {
		//    type = null;
		//    format = null;
		//    return false;
		//  }
		//  else
		//  {
		//    EAInternal prop = _Properties[index];
		//    type = prop.Property.PropertyType;
		//    format = prop.Format;
		//    return true;
		//  }
		//}

		///// <summary>
		///// Format a property value into a displayable text.
		///// </summary>
		///// <param name="source">object containing the property</param>
		///// <param name="key">property name of BusinessInfoAttribute.Key</param>
		///// <param name="i">index in case property is an array; ignored when not an array</param>
		///// <param name="mfi">measurement formatter</param>
		///// <returns>display string</returns>
		//public string Format(object source, string key, int valueIndex, ImardaFormatProvider ifp)
		//{
		//  int index = Array.BinarySearch<EAInternal>(_Properties, new EAInternal(key));
		//  if (index != -1)
		//  {
		//    EAInternal prop = _Properties[index];
		//    object val = prop.Property.GetValue(source, null);
		//    if (val.GetType().IsArray) val = ((Array)val).GetValue(valueIndex);
		//    return EAHelper.Format(prop.Format, val, ifp);
		//  }
		//  return string.Empty;
		//}

		/// <summary>
		/// Format all properties from the source object into a k|v-string that
		/// can be used to instantiate a template.
		/// </summary>
		/// <param name="source">object containing properties</param>
		/// <param name="ifp">format provider</param>
		/// <returns>dictionary with formatted values.</returns>
		public IDictionary FormatAll(object source, ImardaFormatProvider ifp)
		{
			var map = new HybridDictionary();

			foreach (EAInternal prop in _Properties)
			{
				object val = prop.Property.GetValue(source, null);
				if (val != null)
				{
					if (val.GetType().IsArray)
					{
						var arr = (Array)val;
						int n = arr.GetLength(0);
						var formattedValues = new string[n];
						for (int i = 0; i < n; i++)
						{
							var elem = arr.GetValue(i);
							formattedValues[i] = EAHelper.Format(prop.Format, elem, ifp);
						}
						map[prop.Key] = formattedValues;
					}
					else
					{
						map[prop.Key] = EAHelper.Format(prop.Format, val, ifp);
					}
				}
			}
			return map;
		}

		public void ToJSON(object source, ImardaFormatProvider ifp, StringBuilder sb)
		{
			sb.Append('{');
			foreach (EAInternal prop in _Properties)
			{
				object val = prop.Property.GetValue(source, null);
				if (val != null)
				{
					if (val.GetType().IsArray)
					{
						var arr = (Array)val;
						int n = arr.GetLength(0);
						sb.AppendFormat("\"{0}\":[", prop.Key.ToLowerInvariant());
						for (int i = 0; i < n; i++)
						{
							var elem = arr.GetValue(i);
							sb.AppendFormat("\"{0}\",", EAHelper.Format(prop.Format, elem, ifp));
						}
						if (sb[sb.Length - 1] == ',') sb.Length--;
						sb.Append(']');
					}
					else
					{
						string key = prop.Key.ToLowerInvariant();
						if (val is DateTime)
						{
							sb.AppendFormat("\"{0}_s\":\"{1:s}\",", key, val); // sortable format (ISO 8601) e.g. "SomeField_s":"2013-06-13T22:14:21",
						}
						sb.AppendFormat("\"{0}\":\"{1}\",", key, EAHelper.Format(prop.Format, val, ifp));
					}
				}
			} 
			if (sb[sb.Length - 1] == ',') sb.Length--;
			sb.Append('}');
		}

		/// <summary>
		/// Write the object into a compact string containing keys, values and type info.
		/// Type codes: !number, $string, %bool, *Guid, @DateTime, !'measurement'
		/// For measurements, the unqualified struct name is used, eg. topspeed#Speed
		/// </summary>
		/// <param name="source">source object containing the properties</param>
		/// <param name="sb">append to this string builder</param>
		public void Serialize(object source, StringBuilder sb)
		{
			bool added = false;
			EAHelper.PrepareKV(sb);
	
			foreach (EAInternal prop in _Properties)
			{
				if (!prop.Serialize) continue;
				object val = prop.Property.GetValue(source, null);
				added = false;
				if (val != null)
				{
					sb.Append(prop.Key).Append(EAHelper.SerType(prop.Property.PropertyType, prop.Var, prop.Format));
					if (val.GetType().IsArray)
					{
						var arr = (Array)val;
						int n = arr.GetLength(0);
						for (int i = 0; i < n; i++)
						{
							var elem = arr.GetValue(i) ?? "";
							string x = EAHelper.SerValue(elem);
							if (x == "0001-01-01T00:00:00") x = "\u00B6d";
							else if (x == "") x = "\u00B6";
							sb.Append('|').Append(x);
							added = true;
						}
					}
					else
					{
						string x = EAHelper.SerValue(val);
						if (!string.IsNullOrEmpty(x))
						{
							sb.Append('|').Append(x);
						}
						added = true; // ensure followed by ||
					}
				}
				if (added) sb.Append("||");
			}
			if (added) sb.Length -= 2;
		}
		// Deserialize() -> see EAHelper.cs, because is a static method that produces a Dictionary, not an entity

		/// <summary>
		/// Get an enumeration of the keys
		/// </summary>
		public IEnumerable<string> Keys
		{
			get
			{
				return _Properties.Select(prop => prop.Key);
			}
		}
	}

	/// <summary>
	/// Holds extended property info.
	/// </summary>
	internal class EAInternal : IComparable<EAInternal>
	{
		private readonly string _Key;
		private readonly PropertyInfo _Property;
		private readonly int _Capacity;
		private readonly int _Var;
		private readonly bool _Store;
		private readonly bool _Serialize;

		internal EAInternal(string key)
		{
			_Key = key;
		}

		internal EAInternal(string key, PropertyInfo prop, int capacity, int vari, bool store, bool serialize)
		{
			_Key = key;
			_Property = prop;
			_Capacity = capacity;
			_Var = vari;
			_Store = store;
			_Serialize = serialize;
		}

		internal string Key { get { return _Key; } }
		internal PropertyInfo Property { get { return _Property; } }
		internal int Count { get { return _Capacity; } }
		internal int Var { get { return _Var; } }
		internal bool Store { get { return _Store; } }
		internal bool Serialize { get { return _Serialize; } }

		internal string Format { get; set; }

		public int CompareTo(EAInternal other)
		{
			return string.Compare(_Key, other._Key);
		}
	}

	/// <summary>
	/// Mark up all properties in the target class with [BusinessInfo]. If the property
	/// name is different from the key in the EntityAttributes, then set named property Key
	/// as the lookup key. Capacity applies to initial size of arrays.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class BusinessInfoAttribute : Attribute
	{
		public BusinessInfoAttribute()
		{
			Store = true;
			Serialize = true;
		}

		/// <summary>
		/// Override method name with this name.
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Initial array size. Only for array types.
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// Number of array items to be made into variables, e.g. A# -> A1, A2, ...
		/// </summary>
		public int Var { get; set; }

		/// <summary>
		/// Whether to persist in database. Default true. False = transient, calculated.
		/// </summary>
		public bool Store { get; set; }

		/// <summary>
		/// Whether or not to put in dict with Serialize(). Default = true
		/// </summary>
		public bool Serialize { get; set; }
	}


	/// <summary>
	/// Optionally add this to IMeasurement type business properties to generate
	/// formatting information into the EntityAttributes string.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class DisplayFormatAttribute : Attribute
	{
		private readonly string _Format;

		public DisplayFormatAttribute(string format)
		{
			_Format = format;
		}

		public string Format
		{
			get { return _Format; }
		}
	}

}
