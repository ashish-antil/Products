//using ImardaAttributingBusiness;

#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Imarda.Lib;

#endregion

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
    /// <summary>
    ///     Stores the optional attributes for a BusinessEntityExtended.
    ///     Also has accessors, to get the full string or to get the
    ///     dictionary. Caller is responsible for converting the attribute
    ///     value from string to the proper type.
    /// </summary>
	[DataContract]
    public class EntityAttributes
    {
        [DataMember] private string _AttributeString;
        private object _MapLock = new object();
        private IDictionary _map;

        public EntityAttributes()
            : this(string.Empty)
        {
        }

        public EntityAttributes(string attrString)
        {
            _AttributeString = attrString ?? string.Empty;
        }

        /// <summary>
        ///     Initialize using a dictionary.
        /// </summary>
        /// <param name="map">keys are strings, values are string or string[]</param>
        public EntityAttributes(IDictionary map)
        {
            _map = map;
            _AttributeString = _map.KeyValueString();
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext streamingCtx)
        {
            _MapLock = new object();
        }

        public bool HasAttributes
        {
            get { return Map == null ? !string.IsNullOrEmpty(_AttributeString) : Map.Count != 0; }
        }

        public IDictionary Map
        {
            get
            {
                if (_map == null)
                {
                    if (_AttributeString == null)
                    {
                        _AttributeString = string.Empty;
                    }

                    _map = _AttributeString.Replace("\u00B6d", "0001-01-01T00:00:00").KeyValueMap(ValueFormat.Mix, true);
                }

                return _map;
            }
        }

        /// <summary>
        ///     Get the values from the attributeString and use them to update this EntityAttributes object.
		///     (use unit.UpdateAttributes when updating unit attributes as this does not seem to work as expected)
        /// </summary>
        /// <param name="attributeString"></param>
        public void Update(string attributeString)
        {
            var newMap = attributeString.KeyValueMap(ValueFormat.Mix, true);
            Update(newMap);
        }

        /// <summary>
        ///     Update this EntityAttributes object with values from the parameter EntityAttributes.
		///     (use unit.UpdateAttributes when updating unit attributes as this does not seem to work as expected)
        /// </summary>
        /// <param name="eaNew">new values</param>
        public void Update(EntityAttributes eaNew)
        {
            Update(eaNew.Map);
        }

        /// <summary>
        ///     Update this EntityAttributes with the values provided in the given map.
		///     (use unit.UpdateAttributes when updating unit attributes as this does not seem to work as expected)
        /// </summary>
        /// <param name="newMap"></param>
        public void Update(IDictionary newMap)
        {
            lock (_MapLock)
            {
                if (newMap != Map)
                {
                    foreach (string key in newMap.Keys)
                    {
                        Map[key] = newMap[key];
                    }
                    _AttributeString = _map.KeyValueString();
                }                
            }
        }

        public string GetAttributes()
        {
            return _AttributeString;
        }

        //& IM-3515
        public bool Exists(string name)
        {
            return Map.Contains(name);
        }

        //. IM-3515

        public T Get<T>(string name)
        {
            if (_AttributeString == null)
            {
                return default(T);
            }
            object val = Map[name];
            if (val is T)
            {
                return (T) val;
            }
            if (val == null || string.Empty.Equals(val))
            {
                return default(T);
            }
            if (typeof (T) == typeof (DateTime))
            {
                var dateTime = ParseDateTime((string) val);
                if (dateTime != null) val = dateTime.Value;
            }
            else if (typeof (T) == typeof (bool))
            {
                if ("0".Equals(val))
                {
                    val = false;
                }
                else if ("1".Equals(val))
                {
                    val = true;
                }
            }
            if (val is IConvertible)
            {
                return (T) Convert.ChangeType(val, typeof (T));
            }
            return default(T);
        }

        /// <summary>
        ///     Get a new array of the given type for the given name, filled with T values.
        /// </summary>
        /// <typeparam name="T">primitive type</typeparam>
        /// <param name="name"></param>
        /// <returns>new array with type T elements, note: when T=string, still get a COPY of the internal array</returns>
        public T[] GetArray<T>(string name)
        {
            object val = Map[name];
            if (val == null || string.Empty.Equals(val))
            {
                return new T[0];
            }
            if (!(val is Array))
            {
                val = new[] {(string) val};
            }
            if (typeof (T) == typeof (DateTime))
            {
                return Array.ConvertAll((string[]) val, s =>
                {
                    var dateTime = ParseDateTime(s);
                    return dateTime.HasValue ? (T) Convert.ChangeType(dateTime.Value, typeof (T)) : default(T);
                });
            }
            if (typeof (T) == typeof (bool))
            {
                return Array.ConvertAll((string[]) val, s => (T) Convert.ChangeType(s == "1", typeof (T)));
            }
            return Array.ConvertAll((string[]) val, s => (T) Convert.ChangeType(s, typeof (T)));
            // This can also convert 0 and 1 to false and true
        }

        /// <summary>
        ///     Get the string array as it is stored in the Map. Any operations on this array will affect the
        ///     EntityAttributes without any need to call Set(name, array).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string[] GetInternalStringArray(string name)
        {
            object obj = Map[name];
            return obj == null ? null : (string[]) obj;
        }


        public static DateTime? ParseDateTime(string s)
        {
            DateTime dt;
            return DateTime.TryParseExact(s, "s", null, DateTimeStyles.None, out dt)
                       ? (DateTime?) DateTime.SpecifyKind(dt, DateTimeKind.Utc)
                       : null;
        }

        public T GetMeasurement<T>(string name)
            where T : struct, IMeasurement
        {
            object val = Map[name];
            if (val is string)
            {
                return Measurement.Parse<T>((string) val);
            }
            return default(T);
        }

        public T[] GetMeasurementArray<T>(string name)
            where T : struct, IMeasurement
        {
            object val = Map[name];
            if (val == null || string.Empty.Equals(val))
            {
                return new T[0];
            }
            if (!(val is Array))
            {
                val = new[] {(string) val};
            }
            return Array.ConvertAll((string[]) val, s => Measurement.Parse<T>(s));
        }

        /*

		/// <summary>
		/// Set the value in the map using the given name (key). The Map will always contain string 
		/// representations and/or string arrays, although one can pass non-string values to this method.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="val">value to be set, can be single value or 1-dim array of mixed or same types</param>
		/// <example>
		/// <code>
		/// Set("car#1", new object[] { dt, true, false, SequentialGuid.NewDbGuid(), "hello" });
		/// </code></example>
		public void Set(string name, object val)
		{
			if (_AttributeString == null) return;
			object internalValue = EntityAttributes.GetValue(val);
			if (internalValue != null) Map[name] = internalValue;
		}
		
		*/

        /// <summary>
        ///     Delete the attribute from the list.
        /// </summary>
        /// <param name="attributeKey">identifies the attribute</param>
        public void Delete(string attributeKey)
        {
            if (_AttributeString == null || attributeKey == null)
            {
                return;
            }
            Map.Remove(attributeKey);
        }

        /// <summary>
        ///     Get the value to store in the dictionary.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object GetValue(object val)
        {
            return GetValue(val, null);
        }

        /// <summary>
        ///     Get the value to store in the dictionary. Specify optional format string
        ///     for IMeasurement values.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="fmt">optional format string for measurements</param>
        /// <returns></returns>
        public static object GetValue(object val, string fmt, bool stringBoolValue = false)
        {
            if (val is Array)
            {
                if (val is string[])
                {
                    return val;
                }
                var tarr = (Array) val;
                int len = tarr.GetLength(0);
                var sarr = new string[len];
                for (int i = 0; i < len; i++)
                {
                    object source = tarr.GetValue(i);
                    string target;
                    if (source is DateTime)
                    {
                        target = ((DateTime) source).ToString("s");
                    }
                    else if (source is bool)
                    {
                        if (stringBoolValue)
                        {
                            target = (bool) source ? "true" : "false";
                        }
                        else
                        {
                            target = (bool) source ? "1" : "0";
                        }
                    }
                    else if (source is IMeasurement)
                    {
                        var m = (IMeasurement) source;
                        target = fmt == null ? Measurement.PString(m) : Measurement.FString(m, fmt);
                    }
                    else
                    {
                        target = Convert.ToString(source);
                    }
                    sarr[i] = target;
                }
                return sarr;
            }

            if (val == null)
            {
                return null;
            }
            Type t = val.GetType();
            string s;
            if (t == typeof (Guid))
            {
                s = ((Guid) val).ToString("D");
            }
            else if (t == typeof (bool))
            {
                if (stringBoolValue)
                {
                    s = true.Equals(val) ? "true" : "false";
                }
                else
                {
                    s = true.Equals(val) ? "1" : "0";
                }
            }
            else if (t == typeof (DateTime))
            {
                s = ((DateTime) val).ToString("s");
            }
            else if (val is IMeasurement)
            {
                var m = (IMeasurement) val;
                s = fmt == null ? Measurement.PString(m) : Measurement.FString(m, fmt);
            }
            else
            {
                s = val.ToString();
            }
            return s;
        }

        /// <summary>
        ///     Updates the string with modifications in the map.
        /// </summary>
        /// <returns></returns>
        public string UpdateAttributeString()
        {
            if (_map != null)
            {
                _AttributeString = _map.KeyValueString().Replace("0001-01-01T00:00:00", "\u00B6d");
            }

            return _AttributeString;
        }

        public Dictionary<string, string> GetFormattedValues(ImardaFormatProvider ifp)
        {
            string fmt = "{0:" + ifp.GetDateFormat("~edit") + "}";
            var map = Map;
            var formatted = new Dictionary<string, string>();
            foreach (string key in map.Keys)
            {
                object item = map[key];
                var arr = item as string[];
                if (arr != null)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string elem = arr[i];
                        string s = DisplayUtils.GetDisplayString(ifp, elem, fmt, null);
                        string key1 = key.Replace("#", (i + 1).ToString());
                        formatted[key1] = s;
                    }
                }
                else if (item is string)
                {
                    string s = string.Empty;

                    // This is an exception, every attribute in duration is formatted as hours:minutes, but this needs to be shown in hours only.
                    if (key == "VEH5")
                    {
                        item = item + " h";
                        s = DisplayUtils.GetDisplayString(ifp, (string) item, fmt, null).Replace(".00", "");
                    }
                    else
                    {
                        s = DisplayUtils.GetDisplayString(ifp, (string) item, fmt, null);
                    }
                    formatted[key] = s;
                }
            }
            return formatted;
        }
    }
}