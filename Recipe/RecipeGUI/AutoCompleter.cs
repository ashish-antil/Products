using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RecipeGUI
{
	public class AutoCompleter
	{
		private TextBoxBase _TextBoxBase;

		private static Regex _AppendRx = new Regex(@"#append\ +(?:\S+(?!\s*\S)|-r)", RegexOptions.CultureInvariant);

		private HashSet<string> _AlwaysEnd = new HashSet<string> { 
			"after", "before", "subs", "mkdir", "expn", "csharp", "date", "email", "extract", "new", "newer", "post", "udp", "makexml",
		};

		private HashSet<string> _EndIfNoEqualsSign = new HashSet<string> {
 			"!", "put", "git", "ps", "with", "$", "default", "esc", "escxml",
		};

		private HashSet<string> _EndIfNoArgs = new HashSet<string> {
			"mbox", "warn", "print", "info", "copy",
		};

		private Dictionary<string, string> _MustHaveEquals = new Dictionary<string, string>
		{
			{"count", "m=macro|dir|file"},
			{"dir", "m=search-pattern"},
			{"exists", "m=macro|dir|file"},
			{"intersect", "m1=m2 m3"},
			{"load", "m=path|http"},
			{"product", "m1=m2 m3"},
			{"putv", "m1=m2"},
			{"putx", "m1=m2"},
			{"subtract", "m1=m2 m3"},
			{"union", "m1=m2 m3"},
		};

		private HashSet<string> _DoesNotNeedArgs = new HashSet<string> {
			"auto", "edit", "error", "exit", "fatal", "return", "server", "stop", "stash", "restore", "point",
			"end", "else", "endif", "enddef", "loop", "next", "printv"
		};

		private HashSet<string> _Deprecated = new HashSet<string> {
			"find", "point", "name", "call", "write"
		};

		public AutoCompleter(TextBoxBase tbb)
		{
			_TextBoxBase = tbb;
		}

		public Func<bool> AutoComplete { get; set; }

		public Func<bool> ArgsChecks { get; set; }

		public Func<bool> CheckDeprecated { get; set; }


		public void Handle()
		{
			try
			{
				if (!AutoComplete()) return;
				var t = _TextBoxBase;
				if (t.Text.Length > 0 && t.SelectionStart < t.Text.Length && t.Text[t.SelectionStart] == '#'
					|| t.SelectionStart < t.Text.Length && t.Text[t.SelectionStart] != '\r')
				{
					return;
				}

				string line = GetCurrentLine().TrimEnd();
				if (line.StartsWith("#") && line.Length > 1)
				{
					int p = line.IndexOf(' ');
					bool hasArgs = (p != -1);
					if (!hasArgs) p = line.Length;
					string cmd = line.Substring(1, p - 1);

					if (_AlwaysEnd.Contains(cmd)
						|| _EndIfNoArgs.Contains(cmd) && !hasArgs
						|| cmd == "cmd" && !line.Contains("{")
						|| line[1] >= 'A' && line[1] <= 'Z' // user defined with #def
						|| _AppendRx.IsMatch(line)
						)
					{
						Insert("#end");
					}
					else if (_EndIfNoEqualsSign.Contains(cmd) && !line.Contains('='))
					{
						if (ArgsChecks() && Regex.IsMatch(line, @"#\w+\s*$"))
						{
							InsertError("ERROR: missing macro name here");
						}
						else
						{
							Insert("#end");
						}
					}
					else if ((cmd == "@" || cmd == "cdata") && hasArgs)
					{
						string name = line.Substring(p + 1);
						Insert("#end" + " " + name);
					}
					else if (cmd == "if" || cmd == "ifnot")
					{
						Insert("#endif");
					}
					else if (cmd == "while" || cmd == "until")
					{
						Insert("#loop");
					}
					else if (cmd == "for" || cmd == "iter" && line.Count(ch => ch == ' ') == 1)
					{
						Insert("#next");
					}
					else if (cmd == "ask" && !line.Contains('='))
					{
						Insert("-\r\na|Option a\r\nb|Option b\r\n#end");
					}
					else if (cmd == "def")
					{
						if (Regex.IsMatch(line, @"#def\s+[A-Z]"))
						{
							Insert("#enddef");
						}
						else if (ArgsChecks())
						{
							InsertError("ERROR: #def identifier must start with capital A..Z");
						}
					}
					else if (ArgsChecks())
					{
						string msg;
						if (_MustHaveEquals.TryGetValue(cmd, out msg) && !line.Contains('='))
						{
							InsertError("EXPECTED: " + msg);
						}
						else if (!hasArgs && !_DoesNotNeedArgs.Contains(cmd))
						{
							InsertError("ERROR");
						}
					}
					if (CheckDeprecated() && _Deprecated.Contains(cmd))
					{
						InsertError("DEPRECATED");
					}
				}
			}
			catch// (Exception ex)
			{
			}
		}

		private void Insert(string text)
		{
			string insert = "\r\n" + text;
			_TextBoxBase.SelectedText = insert;
			_TextBoxBase.SelectionStart -= insert.Length;
			_TextBoxBase.ScrollToCaret();
		}

		private void InsertError(string text)
		{
			_TextBoxBase.SelectedText = " <<< " + text + " >>>";
		}

		private string GetCurrentLine()
		{
			TextBoxBase t = _TextBoxBase;
			int ss = t.SelectionStart;
			int b = ss - 1;
			while (b >= 0 && t.Text[b] != '\n') b--;
			int e = ss;
			while (e < t.Text.Length && t.Text[e] != '\r') e++;
			string text = t.Text.Substring(b + 1, e - b - 1);
			return text;
		}

	}
}

