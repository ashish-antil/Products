using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RecipeLib
{
	public class TemplateExpander
	{
		private string _Template;
		private IList<string> _ArgLines;
		private Action<string, string> _Put;
		private Func<string, string> _Instantiate;

		public TemplateExpander(string template, IList<string> argLines, Action<string, string> put, Func<string, string> instantiate)
		{
			_Template = template;
			_ArgLines = argLines;
			_Put = put;
			_Instantiate = instantiate;
		}

		public IEnumerable<string> Instances()
		{
			for (int k = 0; k < _ArgLines.Count; k++)
			{
				_Put("#", (k + 1).ToString());  // `#` represents the sequence number of the argument line, starting at 1
				string argLine = _ArgLines[k];
				string[] args = argLine.Split('|');
				for (int i = 0; i < args.Length; i++)
				{
					string arg = args[i];
					_Put("0", argLine); // `0` is the full argument line
					int p = arg.IndexOf('=');
					if (p != -1) _Put(arg.Substring(0, p), arg = arg.Substring(p + 1));
					_Put((i + 1).ToString(), arg); // `1`, `2`, etc. can always be used to refer to the arguments in sequential order
				}
				string instance = _Instantiate(_Template);
				yield return instance;
			}
		}
	}
}
