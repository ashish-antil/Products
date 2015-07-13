using System;
using System.Collections.Generic;
using System.Configuration;

namespace FernBusinessBase
{
	public static class Util
	{
		private static readonly object _Sync = new object();

		/// <summary>
		/// Caches the mapping between a type and the assembly it is
		/// declared in. Hopefully this should save _some_ of the cost of this.
		/// </summary>
		private static readonly Dictionary<Type, string> _TypeAssemblyCache = new Dictionary<Type, string>();
		
		/// <summary>
		/// This method is a quick attempt at giving slight abstraction
		/// between a business layer and it's connection string.
		/// </summary>
		/// <returns></returns>
		public static string GetConnName<T>() where T : BaseEntity, new()
		{
			return GetConnName(typeof(T));
		}

		internal static string GetConnName(Type t)
		{
			ConnectionStringSettingsCollection constrings = ConfigurationManager.ConnectionStrings;

			// if there is only one connection string, there is only one 
			// option so just return it without doing anything more complicated
			// nb - 2 connection strings since one is injected by .net for whatever reason
			if (constrings.Count == 2)
			{
				return constrings[1].Name;
			}

			lock (_Sync)
			{
				string assemblyName;
				if (!_TypeAssemblyCache.TryGetValue(t, out assemblyName))
				{
					assemblyName = t.Assembly.GetName().Name;
					_TypeAssemblyCache.Add(t, assemblyName);
				}
				// the assembly name is the name of the connection string
				return assemblyName;
			}
		}
	}
}
