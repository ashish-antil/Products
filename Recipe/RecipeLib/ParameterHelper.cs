using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RecipeLib
{
	public static class ParameterHelper
	{
		public static void Extract(string[] recipe, Dictionary<string, string> macros, out IEnumerable<string[]> required, out IEnumerable<string[]> optional)
		{
			char[] sep = { ' ', '\t' };

			var rx = new Regex(@"#default ([^=]+)=(.*)$");

			required = from s in recipe
						  where s.StartsWith("#require")
						  let key = s.Split(sep, 3)[1]
						  select new string[] { key, Val(macros, key) };

			optional = from s in recipe
						  where s.StartsWith("#default")
						  let m = rx.Match(s)
						  let key = m.Groups[1].Value
						  let dflt = m.Groups[2].Value
						  select new string[] { key, Val(macros, key), dflt };

		}

		private static string Val(Dictionary<string, string> macros, string key)
		{
			string s;
			macros.TryGetValue(key, out s);
			return s;
		}


	}
}
