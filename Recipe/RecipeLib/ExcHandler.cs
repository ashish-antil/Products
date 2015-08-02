using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RecipeLib
{
	internal class ExcHandler
	{
		private Regex _Regex;
		internal string Macro { get; private set; }

		internal ExcHandler(string pattern, string macro)
		{
			_Regex = string.IsNullOrEmpty(pattern) ? null : new Regex(pattern, RegexOptions.CultureInvariant);
			Macro = macro;
		}

		internal bool IsMatch(string input)
		{
			return _Regex == null || _Regex.IsMatch(input);
		}

	}
}
