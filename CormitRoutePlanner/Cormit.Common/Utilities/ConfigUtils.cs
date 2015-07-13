using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Collections.Generic; 
using System.Linq;
using Imarda.Lib.MVVM.Extensions;

namespace Imarda.Lib
{
	/// <summary>
	/// This class is intended to expose AppSettings by having symbolic names for the features.
	/// </summary>
	public static class ConfigUtils
	{
		/// <summary>
		/// Get the bool value of the appSetting. The appSetting has to be "true" (ignore case) or "1"
		/// to return true, otherwise false is returned.
		/// </summary>
		/// <param name="name">appSettings key</param>
		/// <param name="fallback"></param>
		/// <returns>true if "true" or "1"</returns>
		public static bool GetFlag(string name, params string[] fallback)
		{
			string s = GetString(name, fallback);
			return s == "1" || string.Equals(s, "true", StringComparison.OrdinalIgnoreCase);
		}

		//& SPII-23
		public static string GetStringElseEmpty(string name, params string[] fallback)
		{
			string s = GetString(name, fallback);
			return s ?? "";
		}
		//. SPII-23

		public static string GetString(string name, params string[] fallback)
		{
			string s = ConfigurationManager.AppSettings[name];
			for (int i = 0; s == null && i < fallback.Length; i++)
			{
				s = ConfigurationManager.AppSettings[fallback[i]];
			}
			return s;
		}

		public static string[] GetAllKeys(Predicate<string> condition )
		{
			var settings = ConfigurationManager.AppSettings;
			return settings.AllKeys.Where(k=>condition(k)).ToArray();
		}
		
		public static int GetInt(string name, int dflt)
		{
			string s = ConfigurationManager.AppSettings[name];
			int value;
			return int.TryParse(s, out value) ? value : dflt;
		}

		public static int GetInt(string name, params string[] fallback)
		{
			string s = GetString(name, fallback);
			int value;
			return int.TryParse(s, out value) ? value : 0;
		}

		public static double GetDouble(string name, double dflt)
		{
			string s = ConfigurationManager.AppSettings[name];
			double value;
			return double.TryParse(s, out value) ? value : dflt;
		}

		public static double GetDouble(string name, params string[] fallback)
		{
			string s = GetString(name, fallback);
			double value;
			return double.TryParse(s, out value) ? value : 0.0;
		}

		public static Guid GetGuid(string name)
		{
			return GetGuid(name, Guid.Empty);
		}

		public static Guid GetGuid(string name, Guid dflt)
		{
			try
			{
				string s = GetString(name);
				return s == null ? dflt : new Guid(s);
			}
			catch
			{
				return Guid.Empty;
			}
		}

		public static Guid[] GetGuidArray(string name)
		{
			try
			{
				string s = ConfigurationManager.AppSettings[name];
				if (s == null) return new Guid[0];
				string[] ss = s.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
				return ss.Select(x => new Guid(x)).ToArray();
			}
			catch
			{
				return new Guid[0];
			}
		}

		/// <summary>
		/// Parse a TimeSpan string, e.g. "1.0:15:0.7" -> 1 day, 15 minutes, 0.7 seconds.
		/// Fallback to other key names if first key name not found.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="fallback"></param>
		/// <see>http://msdn.microsoft.com/en-us/library/se73z7b9(v=vs.90).aspx</see>
		/// <returns></returns>
		public static TimeSpan GetTimeSpan(string name, params string[] fallback)
		{
			string s = GetString(name, fallback);
			TimeSpan value;
			return TimeSpan.TryParse(s, out value) ? value : TimeSpan.Zero;
		}

		/// <summary>
		/// Parse a TimeSpan string, e.g. "1.0:15:0.7" -> 1 day, 15 minutes, 0.7 seconds.
		/// Use given TimeSpan as default if key not found in appSetings.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dflt"></param>
		/// <see>http://msdn.microsoft.com/en-us/library/se73z7b9(v=vs.90).aspx</see>
		/// <returns></returns>
		public static TimeSpan GetTimeSpan(string name, TimeSpan dflt)
		{
			string s = ConfigurationManager.AppSettings[name];
			TimeSpan value;
			return TimeSpan.TryParse(s, out value) ? value : dflt;
		
		}

		/// <summary>
		/// Get array of values from the appSettings entry with the given key. Convert to the specified type.
		/// </summary>
		/// <typeparam name="T">type, must implement IConvertible, e.g. string, int, double, bool, DateTime</typeparam>
		/// <param name="name">appSettings/add key</param>
		/// <param name="dflt">default value if conversion fails</param>
		/// <returns>null if key not found, array of the given type if found</returns>
		public static T[] GetArray<T>(string name, T dflt) where T : IConvertible
		{
			string s = ConfigurationManager.AppSettings[name];
			if (s == null) return null;
			string[] ss = s.Split(new[] {',', ';'}, StringSplitOptions.RemoveEmptyEntries);
			var tt = new T[ss.Length];
			for (int i = 0; i < ss.Length; i++)
			{
				try
				{
					tt[i] = (T) Convert.ChangeType(ss[i], typeof (T));
				}
				catch (Exception)
				{
					tt[i] = dflt;
				}
			}
			return tt;
		}

		/// <summary>
		/// Get array of integers from configuration file item. The array should be have
		/// numbers separated by comman, semicolon or space. A range can be defined by using ".."
		/// between two numbers (no spacing), e.g. 1,5..10,18 = 1,5,6,7,8,9,10,18
		/// </summary>
		/// <param name="name">appSettings key</param>
		/// <param name="sort">sort</param>
		/// <returns>int array, ranges expanded</returns>
		public static int[] GetIntArray(string name, bool sort)
		{
			string s = ConfigurationManager.AppSettings[name];
			return NumberUtils.ToIntArray(s, sort);
		}

		//? IM-2371
		public static IDictionary<string, string> GetKeyValueMap(string name)
		{
			var s = GetString(name);
			return string.IsNullOrEmpty(s) ? null : s.KeyStringMap(true);
		}
		//. IM-2371


		public static void RefreshAppSettings()
		{
			ConfigurationManager.RefreshSection("appSettings");
		}
	}


	/// <summary>
	/// This class handles configuration sections in a flexible way.
	/// </summary>
	public sealed class XmlConfigurator : IConfigurationSectionHandler
	{
		// In the app.config, put the following line in the standard <configSections> section for each section you want to create:
		//   <section name="MySettings" type="Imarda.Lib.XmlConfigurator, Imarda.Common" />
		// Then you can use under the root element:
		// <MySettings type="MyNamespace.MySettings, MyAssembly">
		//   <Things>
		//     <Thing Foo="Hello">
		//       <Bar>12</Bar>
		//     </Thing>
		//     <Thing Foo="World"/>
		//   </Things>
		// </MySettings>
		//
		// Then make sure Things appear in MySettings like:
		//
		//  public class MySettings
		//  {
		//    private List<Thing> _Things = new List<Thing>();
		//    public List<Thing> Things { get { return _Things; } }
		//  }
		//  public class Thing
		//  {
		//     [XmlAttribute] public string Foo { get; set; }
		//     public int Bar { get; set; } // you can optionally precede it by [XmlElement]
		//  }

		#region IConfigurationSectionHandler Members

		public object Create(object parent, object configContext, XmlNode section)
		{
			if (section == null) return null;

			XPathNavigator navigator = section.CreateNavigator();
			var typeName = (string) navigator.Evaluate("string(@type)");
			Type sectionType = Type.GetType(typeName);

			var xs = new XmlSerializer(sectionType);
			var reader = new XmlNodeReader(section);

			object settings = null;
			try
			{
				settings = xs.Deserialize(reader);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			return settings;
		}

		#endregion
	}
}