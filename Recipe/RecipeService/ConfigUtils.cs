using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Common
{
	/// <summary>
	/// This class is intended to expose AppSettings by having symbolic names for the features.
	/// </summary>
	public static class ConfigUtils
	{
		public static bool GetFlag(string name)
		{
			string s = GetString(name, null);
			return s == "1" || string.Equals(s, "true", StringComparison.OrdinalIgnoreCase);
		}

		public static string GetString(string name, string dflt)
		{
			return ConfigurationManager.AppSettings[name] ?? dflt;
		}

		public static T GetValue<T>(string name, T dflt)
			where T : IConvertible
		{
			string s = ConfigurationManager.AppSettings[name];
			if (s == null) return dflt;
			return (T)Convert.ChangeType(s, typeof(T));
		}

		public static TimeSpan GetTimeSpan(string name, TimeSpan dflt)
		{
			string s = ConfigurationManager.AppSettings[name];
			if (s == null) return dflt;
			return TimeSpan.Parse(s);
		}

		public static T GetEnum<T>(string name, T dflt) where T : struct
		{
			string s = ConfigurationManager.AppSettings[name];
			if (s == null) return dflt;
			return (T)Enum.Parse(typeof(T), s);
		}

		public static T[] GetArray<T>(string name, T dflt) where T : IConvertible
		{
			string s = ConfigurationManager.AppSettings[name];
			if (s == null) return null;
			string[] ss = s.Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			T[] tt = new T[ss.Length];
			for (int i = 0; i < ss.Length; i++)
			{
				try
				{
					tt[i] = (T)Convert.ChangeType(ss[i], typeof(T));
				}
				catch (Exception)
				{
					tt[i] = dflt;
				}
			}
			return tt;
		}

		public static void RefreshAppSettings()
		{
			ConfigurationManager.RefreshSection("appSettings");
		}
	}
}
