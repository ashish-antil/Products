using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RecipeLib
{
	public class CommandPhrase
	{
		private readonly static char[] Space = new char[] { ' ' };
		private readonly static char[] Start = new char[] { '=', '|' };
		private readonly static char[] End = new char[] { ' ', '|' };
		private Dictionary<string, string[]> _Patterns;


		public CommandPhrase()
		{
			_Patterns = new Dictionary<string, string[]>();
		}

		public void Add(string cmd, params string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				if (!args[i].Contains('=')) args[i] += '=';
			}
			_Patterns[cmd] = args; // e.g. args={ "Name", @"Path=c:\program files\", "Test=1|2|3" }
		}

		public bool CommandSelected { get; set; }

		public int Advance(string input, ref int caret)
		{
			while (caret < input.Length)
			{
				char c = input[caret++];
				if (Start.Contains(c))
				{
					int i = caret;
					while (i < input.Length && !End.Contains(input[i])) i++;
					return i - caret;
				}
			}
			return 0;
		}

		public string Select(string input, ref int caret, ref int length)
		{
			try
			{
				int start = input.LastIndexOf('=', caret) + 1;
				int end = input.IndexOf(' ', caret + length);
				if (end == -1) end = input.Length;
				string output = input.Substring(0, start) + input.Substring(caret, length) + input.Substring(end);
				caret = start;
				return output;
			}
			catch
			{
				return input;
			}
		}

		public string CheckCommand(string input)
		{
			if (CommandSelected)
			{
				return input;
			}
			else
			{
				if (!input.EndsWith(" ")) return input;
				string[] a = input.Split(Space, StringSplitOptions.RemoveEmptyEntries);
				if (a.Length == 0) return input;
				string[] pargs;
				if (!_Patterns.TryGetValue(a[0], out pargs)) return input;
				CommandSelected = true;
				return a[0] + " " + string.Join(" ", pargs);
			}
		}
	}
}
