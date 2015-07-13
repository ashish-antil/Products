using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes;
using FernBusinessBase.Control;

namespace Imarda360.Infrastructure.ConfigurationService
{
	public class ConfigTemplate
	{
		private const string SepStr = "\u001F";
		private static readonly char[] SepChar = SepStr.ToCharArray();
		private static readonly Regex _PlaceHolder = new Regex(@"\$\(([-\w]+)(\|([^\)]*))?\)", RegexOptions.Compiled);

		private string[] _FixedText;
		private string[] _Keys;
		private string[] _Defaults;

		public string DefaultValue { get; set; }


		public ConfigTemplate(string template)
			: this(template, string.Empty)
		{
		}

		/// <summary>
		/// Create a new template based on the given string, which contains formal parameters
		/// like: $(name)
		/// </summary>
		/// <param name="template">string with formal parameters $(...)</param>
		/// <param name="dflt">default if no individual parameter default given</param>
		/// <remarks>The template gets stored in format that is more efficient for instantiation</remarks>
		public ConfigTemplate(string template, string dflt)
		{
			DefaultValue = dflt;
			var keys = new List<string>();
			var defaults = new List<string>();
			var s = _PlaceHolder.Replace(template, 
				delegate(Match m)
				{
					string key = m.Groups[1].Value;
					if (key.IndexOf('-') != -1) key = key.ToUpper(); // to handle GUID comparison
					keys.Add(key);
					defaults.Add(m.Groups[2].Value == string.Empty ? DefaultValue : m.Groups[3].Value);
					return SepStr;
				});
			_FixedText = s.Split(SepChar);
			_Keys = keys.ToArray();
			_Defaults = defaults.ToArray();
		}

		/// <summary>
		/// Get a copy of the formal parameter names.
		/// </summary>
		public string[] Keys
		{
			get { return (string[])_Keys.Clone(); }
		}

		/// <summary>
		/// Get a copy of the array with the text in between the formal parameters
		/// </summary>
		public string[] Parts
		{
			get { return (string[])_FixedText.Clone(); }
		}

		/// <summary>
		/// Instantiate the template using the values from the fields marked by the ConfigAssign attributes.
		/// The template should have placeholders like $(04FBD457-C5C7-4618-A46C-FC5B8A05E0A8)
		/// </summary>
		/// <param name="group">object with public fields</param>
		/// <returns>filled in template</returns>
		public string Instantiate(ConfigGroup group)
		{
			var map = new HybridDictionary();

			FieldInfo[] fields = group.GetType().GetFields();
			foreach (FieldInfo info in fields)
			{
				var attr = Attribute.GetCustomAttribute(info, typeof(ConfigAssignAttribute));
				if (attr != null)
				{
					string id = ((ConfigAssignAttribute)attr).ItemID.ToString().ToUpper();
					object val = info.GetValue(group);
					map[id] = (val == null) ? "" : val.ToString();
				}
			}
			return Instantiate(map);
		}

		/// <summary>
		/// Instantiate the template and fill in the values for the keys.
		/// </summary>
		/// <param name="keys">names of formal parameters that were enclosed in $(...) in the original template</param>
		/// <param name="values">actual parameters</param>
		/// <returns>filled in template</returns>
		public string Instantiate(string[] keys, string[] values)
		{
			var map = new HybridDictionary();
			for (int i=0; i<keys.Length; i++) map[keys[i]] = values[i];
			return Instantiate(map);
		}


		/// <summary>
		/// Instantiate the template for an object that has properties with certain attributes.
		/// </summary>
		/// <param name="objectWithProperties">the object containing the actual parameters in properties</param>
		/// <param name="attrType">attribute type used to mark the property that has to be used as parameter value</param>
		/// <returns>filled in template</returns>
		/// <remarks>The property names correspond to the names enclosed in $(...) in the original template string</remarks>
		public string Instantiate(object objectWithProperties, Type attrType)
		{
			PropertyInfo[] properties = objectWithProperties.GetType().GetProperties();
			var map = new HybridDictionary();
			foreach (PropertyInfo prop in properties)
			{
				if (attrType == null || Attribute.GetCustomAttribute(prop, attrType) != null)
				{
					object val = prop.GetValue(objectWithProperties, null);
					map[prop.Name] = (val == null) ? "" : val.ToString();
				}
			}
			return Instantiate(map);
		}

		/// <summary>
		/// Use a special format parameter list as a string to fill in the parameters in the template
		/// </summary>
		/// <param name="paramList">key1|value1||key2|value2||key3|value3</param>
		/// <returns>filled in template</returns>
		public string Instantiate(string paramList)
		{
			IDictionary map = new CfgParams { VersionValue = paramList }.Map;
			return Instantiate(map);
		}



		/// <summary>
		/// Instantiate the template using the key/value map provided.
		/// </summary>
		/// <param name="map">any dictionary that contains the formal parameter -> value mapping</param>
		/// <returns>filled in template</returns>
		public string Instantiate(IDictionary map)
		{
			var sb = new StringBuilder();
			int n = _Keys.Length;
			for (int i = 0; i < n; i++)
			{
				string key = _Keys[i];
				object obj = map[key];
				string val = obj == null || "".Equals(obj) ? _Defaults[i] : (string)obj;
				sb.Append(_FixedText[i]).Append(val);
			}
			sb.Append(_FixedText[n]);
			return sb.ToString();
		}

	}
}
