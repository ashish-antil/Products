using System;

namespace ImardaConfigurationBusiness
{
	public static class ConfigExtensions
	{
		/// <summary>
		/// Get the right type value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="cv"></param>
		/// <returns></returns>
		public static T As<T>(this ConfigValue cv)
		{
			object obj = cv.Value;
			if (obj.GetType() != typeof(T)) return (T)Convert.ChangeType(obj, typeof(T));
			return (T)obj;
		}

		public static Guid AsGuid(this ConfigValue cv)
		{
			return cv.Value is string ? new Guid((string) cv.Value) : Guid.Empty;
		}

		/// <summary>
		/// Get the right type value. Return a default if ConfigValue.Value is null.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="cv"></param>
		/// <param name="defaultValue"> </param>
		/// <returns></returns>
		public static T As<T>(this ConfigValue cv, T defaultValue)
		{
			if (cv == null) return defaultValue; // legal to call extension methods on null values!
			object obj = cv.Value;
			if (obj == null) return defaultValue;
			if (obj.GetType() != typeof(T)) return (T)Convert.ChangeType(obj, typeof(T));
			else return (T)obj;
		}

	}
}
