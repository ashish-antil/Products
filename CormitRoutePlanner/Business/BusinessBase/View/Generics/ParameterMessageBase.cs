#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using Imarda.Lib;

#endregion

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
	[MessageContract]
	public abstract class ParameterMessageBase : IRequestBase
	{
		[MessageBodyMember]
		private Dictionary<string, string> _Parameters;

		public IEnumerable<string> Keys
		{
			get { return _Parameters == null ? (IEnumerable<string>)new string[0] : _Parameters.Keys; }
		}

		/// <summary>
		/// Check if any Parameters are set.
		/// </summary>
		/// <remarks>Calling idReq.Parameters will create an empty dict when none exists,
		/// but idReq.HasParameters will never create an empty dict.</remarks>
		public bool HasParameters
		{
			get { return _Parameters != null && _Parameters.Keys.Count > 0; }
		}

		/// <summary>
		/// Add parameters, even=key, odd=value
		/// </summary>
		/// <param name="args"></param>
		public void AddParameters(params object[] args)
		{
			for (int i = 0; i < args.Length; i += 2)
			{
				var key = ((string) args[i]).ToLowerInvariant();
				object o = args[i + 1];
				if (o == null) Parameters.Add(key, null);
				else if (o is DateTime) Parameters.Add(key, ((DateTime) o).ToString("s"));
				else Parameters.Add(key, o.ToString());
			}
		}


		public bool ContainsKey(string s)
		{
			return Parameters.ContainsKey(s.ToLowerInvariant());
		}

		/// <summary>
		/// Prefered way of getting a parameter if it is not certain that
		/// the parameter exists.
		/// </summary>
		/// <param name="key"></param>
		/// <returns>value of parameter or null if does not exist</returns>
		/// <remarks>Efficient, does not create empty dictionary</remarks>
		public string GetString(string key)
		{
			string val;
			return HasParameters && _Parameters.TryGetValue(key.ToLowerInvariant(), out val) ? val : null;
		}

		/// <summary>
		/// Get the value and convert it to the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public bool Get<T>(string key, out T result)
		{
			string value = GetString(key);
			if (value == null)
			{
				result = default(T);
				return false;
			}

			if (typeof(T) == typeof(Guid))
			{
				try
				{
					result = (T) (object) (new Guid(value));
				}
				catch
				{
					result = (T) (object) Guid.Empty;
				}
			}
			else if (typeof(T) == typeof(TimeSpan))
			{
				TimeSpan ts;
				TimeSpan.TryParse(value, out ts);
				result = (T)(object)ts;
			}
			else if (typeof(T) == typeof(DateTime))
			{
				DateTime dt;
				if (DateTime.TryParseExact(value, "s", null, DateTimeStyles.None, out dt))
				{
					result = (T)(object) dt;
				}
				else if (DateTime.TryParseExact(value, Formats.JsDateTimeFmt, null, DateTimeStyles.None, out dt))
				{
					result = (T) (object) dt;
				}
				else
				{
					result = (T) (object) TimeUtils.MinValue;
				}
			}
			else if (typeof(T) == typeof(bool))
			{
				result = (T)(object)(value == "1" || value == "true" || value == "True");
			}
			else 
			{
				result = (T)Convert.ChangeType(value, typeof(T));
			}
			return true;
		}

		public T Get<T>(string key, T dflt)
		{
			T val;
			return Get(key, out val) ? val : dflt;
		}

		/// <summary>
		/// Check if a flag with the given key has been set. Do not confuse with HasSome/HasAll.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool HasFlagSet(string key)
		{
			string s = GetString(key);
			if (s == null) return false;
			return s == "1" || s.ToLowerInvariant() == "true";
		}

		/// <summary>
		/// Set flag true or false, do this by setting the key to "1" or removing the key.
		/// </summary>
		/// <param name="key">flag name</param>
		/// <param name="value"></param>
		public void SetFlag(string key, bool value)
		{
			if (value) {Put(key, "1");}
			else if (HasParameters) Remove(key);
		}

	    /// <summary>
	    ///     Get/set session ID. This is for smart clients that use a service directly, i.e. that do not use
	    ///     other means of identifying the session object.
	    /// </summary>
	    public Guid SessionID
		{
			get
			{
				string s;
			    if (Parameters.TryGetValue("sessionid", out s))
			    {
			        return new Guid(s);
			    }

				return Guid.Empty;
			}
			set { Parameters["sessionid"] = value.ToString(); }
		}

		public object SID
		{
			set { Parameters["sessionid"] = value.ToString(); }
		}

		/// <summary>
		/// Local use only, not transported across services. 
		/// </summary>
		public object DebugInfo { get; set; }

        // ReSharper disable once InconsistentNaming
		public Guid CompanyID
		{
			get
			{
				string s;
			    if (Parameters.TryGetValue("companyid", out s))
			    {
			        return new Guid(s);
			    }

				return Guid.Empty;
			}
			set { Parameters["companyid"] = value.ToString(); }
		}

        // ReSharper disable once InconsistentNaming
		public Guid UserID
		{
			get
			{
				string s;
			    if (Parameters.TryGetValue("userid", out s))
			    {
			        return new Guid(s);
			    }

				return Guid.Empty;
			}
			set { Parameters["userid"] = value.ToString(); }
		}

	    /// <summary>
	    ///     Get/set the parameters. When doing a get, an empty Dictionary will be created when none exists.
	    /// </summary>
	    protected Dictionary<string, string> Parameters
		{
			get { return _Parameters ?? (_Parameters = new Dictionary<string, string>()); }
		}

		public string this[string key]
		{
			get { return GetString(key); }
			set { Put(key, value);}
		}


		public void Put(string key, object o)
		{
			if (key == null) throw new ArgumentNullException("key");
			key = key.ToLowerInvariant();
			if (o == null) Parameters[key] = null ;
			else if (o is DateTime) Parameters[key] = ((DateTime)o).ToString("s");
			else Parameters[key] = o.ToString();
		}

		public void Remove(string key)
		{
			if (key == null) throw new ArgumentNullException("key");
			Parameters.Remove(key.ToLowerInvariant());
		}

		public override string ToString()
		{
			return string.Format("{0}({1} params, CompID={2})",
				GetType().Name, (HasParameters ? Parameters.Count : 0), CompanyID.ShortString());
		}
	}
}
