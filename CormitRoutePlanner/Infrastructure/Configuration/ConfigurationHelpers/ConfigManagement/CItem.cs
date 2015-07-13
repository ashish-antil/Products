using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace Imarda360.Infrastructure.ConfigurationService
{

	/// <summary>
	/// Generic wrapper for configuration items.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class CItem
	{
		/// <summary>
		/// Get the config value for the given item ID. Use only within active ConfigProxyContext.
		/// Will cast to given type if necessary.
		/// </summary>
		/// <typeparam name="T">type of the value to be returned</typeparam>
		/// <param name="id">item ID</param>
		/// <returns>config value or the default of the data type if not found.</returns>
		public static T Get<T>(Guid id)
		{
			ConfigItem ci = ConfigServiceContext.Get().DataProvider.FindItem(id);
			if (ci != null)
			{
				object obj = ci.CalcValue().VersionValue;
				if (obj.GetType() != typeof(T)) return (T)Convert.ChangeType(obj, typeof(T));
				else return (T)obj;
			}
			else return default(T);
		}

		/// <summary>
		/// Get the config value. Use this method if the type is a value type, the nullable
		/// result lets you test for the case where the config item is not found. 
		/// </summary>
		/// <typeparam name="T">underlying type of nullable value to be returned</typeparam>
		/// <param name="id">item ID</param>
		/// <returns>config value or null if not found</returns>
		/// <remarks>Note that the VersionValue column in the database is declared NOT NULL, 
		/// so if a null is returned it can only mean the item ID does not exist.
		/// </remarks>
		public static T? GetNullable<T>(Guid id) where T : struct
		{
			ConfigItem ci = ConfigServiceContext.Get().DataProvider.FindItem(id);
			if (ci != null)
			{
				object obj = ci.CalcValue().VersionValue;
				if (obj.GetType() != typeof(T)) return (T?)Convert.ChangeType(obj, typeof(T));
				else return (T?)obj;
			}
			else return null;
		}

		public static string GetString(Guid id, params object[] args)
		{
			ConfigItem ci = ConfigServiceContext.Get().DataProvider.FindItem(id);
			if (ci == null) return null;
			else return string.Format((string)ci.CalcValue().VersionValue, args);
		}

		public static ConfigTemplate GetTemplate(Guid id)
		{
			return new ConfigTemplate(CItem.Get<string>(id));
		}
	}

}
