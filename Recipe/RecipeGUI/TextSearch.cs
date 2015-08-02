using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecipeGUI
{
	public class TextSearch
	{
		private string _Text;
		private Regex _Pattern;
		private Match _Match;

		public TextSearch(string text, string pattern, bool ignoreCase, bool regex = true)
		{
			_Text = text;
			if (!regex) pattern = Regex.Escape(pattern);
			_Pattern = new Regex(pattern, RegexOptions.CultureInvariant | (ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None));
		}


		public bool FindNext(ref int start, out int length)
		{
			if (_Match == null)
			{
				_Match = _Pattern.Match(_Text, start);
			}
			bool found = _Match.Success;
			if (found)
			{
				start = _Match.Index;
				length = _Match.Length;
				_Match = _Match.NextMatch();
			}
			else
			{
				length = 0;
				_Match = null;
			}
			return found;
		}

	}
}
