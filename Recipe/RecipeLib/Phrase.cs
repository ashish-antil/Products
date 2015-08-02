using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace RecipeLib
{

	public class Phrase
	{
		private readonly static char[] Space = new char[] { ' ', '\t' };
		private Dictionary<string, string[]> _Patterns;

		public Phrase()
		{
			_Patterns = new Dictionary<string, string[]>();
		}

		public void Add(string cmd, params string[] args)
		{
			_Patterns[cmd] = args; // e.g. args={ "Name", @"Path=c:\program files\", "Test=1|2|3" }
		}

		public string AutoComplete(string input, int caret, out int selectStart, out int selectLength)
		{
			selectStart = caret;
			selectLength = 0;

			string[] a = input.Split(Space, StringSplitOptions.RemoveEmptyEntries);
			string[] pargs;
			if (!_Patterns.TryGetValue(a[0], out pargs)) return input;
			
			throw new NotImplementedException();
		}
	}
}
