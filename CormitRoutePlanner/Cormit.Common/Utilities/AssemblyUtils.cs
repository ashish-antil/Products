using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Imarda.Lib
{
	public static class AssemblyUtils
	{
		private static readonly Dictionary<string, Type> _Loaded = new Dictionary<string, Type>();

		public static Type GetClass(string path, string className)
		{
			lock (_Loaded)
			{
				string key = path + ":" + className;
				Type type0;
				if (_Loaded.TryGetValue(key, out type0)) return type0;

				Assembly asm = Assembly.LoadFrom(path);
				Type[] types = asm.GetTypes();
				foreach (Type type in types.Where(type => type.Name == className))
				{
					_Loaded[key] = type;
					return type;
				}
				return null;
			}
		}


		public static object CallMethod(Type type, string methodName, Type[] argTypes, object[] args)
		{
			MethodInfo methodInfo = type.GetMethod(methodName, argTypes);
			return (methodInfo != null) ? methodInfo.Invoke(null, args) : null;
		}

		public static MethodInfo GetMethod(string path, string className, string methodName, Type[] signature)
		{
			Type type = GetClass(path, className);
			return type.GetMethod(methodName, signature);
		}

        /// <summary>
        /// Gets the directory of the current assembly
        /// </summary>
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

		/* Tip:
		 * To get a specific Action<...> returned, use this code:
		 *			MethodInfo method = AssemblyUtils.GetMethod(path, className, methodName, types);
		 *			var action = (Action<your_parameters>)Delegate.CreateDelegate(typeof(Action<your_parameters>), method);
		 */
	}
}