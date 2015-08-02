using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Security.Cryptography;
using System.IO;

namespace RecipeLib
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	sealed class CommandAttribute : Attribute
	{
		public CommandAttribute()
		{
		}

		public CommandAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; set; }
	}

	public interface IScriptContext
	{
		void SendMessage(string s, params object[] args);
		void Put(string key, string value);
		string Get(string key);
	}

	public class Script
	{
		private readonly static char[] _Sep = new[] { ' ' };
		private readonly static string[] _LineBrk = new[] { "\r\n" };
		private string[] _CommandLines;
		private string _Text;
		private Dictionary<string, MethodInfo> _Map;
		private IScriptContext _Context;

		public Script(string script, IScriptContext context)
		{
			_Context = context;
			_CommandLines = script.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			_Map = new Dictionary<string, MethodInfo>(StringComparer.OrdinalIgnoreCase);
			var methods = GetType().GetMethods();
			foreach (MethodInfo mi in methods)
			{
				var attr = (CommandAttribute[])mi.GetCustomAttributes(typeof(CommandAttribute), false);
				foreach (CommandAttribute a in attr) _Map[a.Name ?? mi.Name] = mi;
			}
		}

		public string Execute(string text)
		{
			_Text = text;
			foreach (string cmdLine in _CommandLines)
			{
				if (cmdLine.StartsWith(";")) continue; // skip comments
				Apply(cmdLine);
			}
			string result = _Text;
			_Text = null;
			return result;
		}

		void Apply(string cmdLine)
		{
			string[] parts = cmdLine.Split(_Sep, 2);
			string cmd = parts[0];
			string args = parts.Length > 1 ? parts[1] : string.Empty;
			MethodInfo mi;
			if (_Map.TryGetValue(cmd, out mi))
			{
				mi.Invoke(this, new object[] { args });
			}
			else throw new Exception(string.Format("Invalid text command: [{0}]", cmd));
		}

		string[] Lines
		{
			get { return _Text.Split(_LineBrk, StringSplitOptions.None); }
		}

		void Text(IEnumerable<string> lines)
		{
			_Text = string.Join(Environment.NewLine, lines.ToArray());
		}

		void Text(StringBuilder sb)
		{
			_Text = sb.ToString();
		}

		void Text(string s)
		{
			_Text = s;
		}

		//@ unesc
		// change  \r=>carriage-return \n->newline \t->tab \s->space \\->backslash
		[Command]
		public void Unesc(string args)
		{
			Text(Unescape(_Text));
		}

		//@ esc
		// change newline=>\n carriage-return=>\r tab=>\t backslash=>\\
		[Command]
		public void Esc(string args)
		{
			Text(Escape(_Text));
		}

		private static string Unescape(string s)
		{
			var sb = new StringBuilder(s.Length + 1);
			bool escape = false;
			foreach (char c in s)
			{
				if (escape)
				{
					switch (c)
					{
						case 'n': sb.Append('\n');
							break;
						case 'r': sb.Append('\r');
							break;
						case 't': sb.Append('\t');
							break;
						case 's': sb.Append(' ');
							break;
						case '\\': sb.Append('\\');
							break;
					}
					escape = false;
				}
				else if (c == '\\')
				{
					escape = true;
				}
				else
				{
					sb.Append(c);
				}
			}
			return sb.ToString();
		}

		private static string Escape(string s)
		{
			var sb = new StringBuilder(s.Length + 1);
			foreach (char c in s)
			{
				switch (c)
				{
					case '\n': sb.Append(@"\n");
						break;
					case '\r': sb.Append(@"\r");
						break;
					case '\t': sb.Append(@"\t");
						break;
					case '\\': sb.Append(@"\\");
						break;
					default:
						sb.Append(c);
						break;
				}
			}
			return sb.ToString();
		}



		//@ sort regex
		// sort the lines, using the first capture group as the string to sort on
		[Command]
		public void Sort(string args)
		{
			string[] lines = Lines;
			if (!string.IsNullOrEmpty(args))
			{
				if (!Regex.IsMatch(args, @"\([^)]*\)")) throw new Exception("Missing ()-group in parameter");
				var rx = new Regex(args);
				string[] keys = lines.Select(
					delegate(string line)
					{
						var m = rx.Match(line);
						return (m.Success && m.Groups.Count > 0) ? m.Groups[1].Value : "";
					}).ToArray();
				Array.Sort(keys, lines);
			}
			else
			{
				Array.Sort(lines);
			}
			Text(lines);
		}

		//@ unique
		// remove duplicate lines
		[Command]
		public void Unique(string args)
		{
			var unique = new HashSet<string>();
			var list = new List<string>();
			foreach (var line in Lines)
			{
				if (!unique.Contains(line)) list.Add(line);
				unique.Add(line);
			}
			Text(list);
		}

		//@ prefix text
		// prefix each line with the given text
		[Command]
		public void Prefix(string prefix)
		{
			Text(Lines.Select(s => prefix + s));
		}

		//@ suffix text
		// append the given text to each line
		[Command]
		public void Suffix(string suffix)
		{
			Text(Lines.Select(s => s + suffix));
		}

		//@ enclose text-with-[]
		// replace each line with the given text but substitute any occurrence of "[]" in the argument with the original line
		[Command]
		public void Enclose(string args)
		{
			var result = new List<string>();
			foreach (string s in Lines)
			{
				result.Add(args.Replace("[]", s));
			}
			Text(result);
		}

		//@ reverse
		// reverse the line order
		[Command]
		public void Reverse(string args)
		{
			string[] lines = Lines;
			Array.Reverse(lines);
			Text(lines);
		}

		//@ subs /regex/repl/mode
		// find each match of the regex with the replacement; "/" can be replaced by any other separator; mode-chars: s m i n x
		[Command("Subs")]
		//@ replace
		// alias for "subs"
		[Command]
		public void Replace(string args)
		{
			string[] split = args.Split(new[] { args[0] });
			if (split.Length != 4) throw new FormatException("Format: /pattern/replacement/[options]");

			string replacement = Unescape(split[2]);
			string options = split[3];
			string pattern = (options == string.Empty) ? split[1] : "(?" + options + ":" + split[1] + ")";
			Text(Regex.Replace(_Text, pattern, replacement));
		}

		//@ append text
		// unescape the arg text and append it
		[Command]
		public void Append(string args)
		{
			_Text += Unescape(args);
		}

		//@ modify /regex/func/mode
		// change matched text, functions: ^=>uppercase, v=>lowercase, trim, compact=>change whitespace to single space, scc=>space-camelcase, more... see source
		[Command]
		public void Modify(string args)
		{
			string[] split = args.Split(new char[] { args[0] });
			if (split.Length != 4) throw new FormatException("Format: /pattern/modifier/[options]");

			string oper = Unescape(split[2]);
			string options = split[3];
			string pattern = (options == string.Empty) ? split[1] : "(?" + options + ":" + split[1] + ")";
			Text(Regex.Replace(_Text, pattern, delegate(Match m) { return ApplyOperationToMatch(oper, m); }));
		}

		//@ line i [n]
		// select the lines in the range [i, i+n-1]; n is 1 by default, i is 0-based; negative i counted from end
		[Command("Line")]
		public void LineRange(string args)
		{
			string[] split = args.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			var lines = Lines;
			int start = Convert.ToInt32(split[0]);
			int n = 0;
			if (split.Length > 1) n = Convert.ToInt32(split[1]) - 1;
			start = (start < 0) ? lines.Length + start : start - 1;
			int end = start + n;
			if (start < 0) start = 0;
			if (end >= lines.Length) end = Lines.Length - 1;
			var sb = new StringBuilder();
			for (int i = start; i <= end; i++) sb.AppendLine(lines[i]);
			if (sb.Length > 2) sb.Length -= 2;
			Text(sb.ToString());
		}

		static Random _Random = new Random();

		string ApplyOperationToMatch(string oper, Match m)
		{
			string value = m.Value;
			switch (oper)
			{
				case "^":
				case "upper":
					return value.ToUpperInvariant();

				case "v":
				case "lower":
					return value.ToLowerInvariant();

				case "trim":
					return value.Trim();

				case "compact":
					return Regex.Replace(m.Value, @"\s+", " ");

				case "shuffle":
					var sb1 = new StringBuilder();
					List<char> list = new List<char>(value.ToCharArray());
					int n = list.Count;
					while (n-- > 0)
					{
						int i = _Random.Next(list.Count);
						sb1.Append(list[i]);
						list.RemoveAt(i);
					}
					return sb1.ToString();

				case "scc": // space camelcase (preserves sequences of capitals)
					return Regex.Replace(value, @"(?<=\p{Ll})(\p{Lu})", " $1");

				default: // with arguments
					string[] arr = oper.Split('|');
					string cmd = arr[0];
					switch (cmd)
					{
						// Change date time format.
						case "@": // e.g. !\d+/\d+/\d{4}!@|M/d/yyyy|yyyy-MM-dd (ddd)!
						case "datetime":
							string oldfmt = arr[1];
							string newfmt = arr[2];
							DateTime dt = DateTime.ParseExact(value, oldfmt, CultureInfo.CurrentUICulture);
							return dt.ToString(newfmt);

						// Change number format
						case "#":  // e.g. !\d+(\.\d+)?!#|0.00!
						case "number":
							string newfmt2 = arr[1];
							decimal result;
							if (decimal.TryParse(value, out result)) return result.ToString(newfmt2);
							else return value;

						// Concatenate all groups in a match, insert optional separator
						case "&":  // e.g. !(sub)\s+(tree)!&|-!
						case "concat":
							string sep = arr.Length > 1 ? arr[1] : "";
							var sb = new StringBuilder();
							for (int i = 1; i < m.Groups.Count; i++) sb.Append(m.Groups[i].Value).Append(sep);
							if (sb.Length > 0) sb.Length -= sep.Length;
							return sb.ToString();

						// Insert separator between characters of the match
						case "i":
						case "insert": // e.g. ![A-Z]+!i|-!
							sep = arr.Length > 1 ? arr[1] : "";
							sb = new StringBuilder();
							for (int i = 0; i < value.Length; i++) sb.Append(value[i]).Append(sep);
							if (sb.Length > 0) sb.Length -= sep.Length;
							return sb.ToString();

						// Replace by random int number in the given min-max range
						case "?": // e.g. !RND!?|15|90|00!        
						case "random":
							int min = 0, max = 10;
							string fmt = "0";
							if (arr.Length > 1) int.TryParse(arr[1], out min);
							if (arr.Length > 2) int.TryParse(arr[2], out max);
							if (arr.Length > 3) fmt = arr[3];
							double random = _Random.Next(min, max);
							return random.ToString(fmt);

						// pick a random value from the words separated by |
						case "pick":
							int range = arr.Length;
							return arr[_Random.Next(1, range)];
					}
					break;
			}
			return m.Value;

		}

		//@ touni
		// escapes all characters with code > 127 as \uXXXX where XXXX is Unicode hex
		[Command]
		public void ToUni(string args)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < _Text.Length; i++)
			{
				char c = _Text[i];
				if (c > 127) sb.AppendFormat(@"\u{0:X4}", (int)c);
				else sb.Append(c);
			}
			Text(sb);
		}

		//@  fromuni
		// unescapes all \uXXXX to the actual character where XXXX is Unicode hex
		[Command]
		public void FromUni(string args)
		{
			var text = Regex.Replace(_Text, @"\\u[0-9A-Fa-f]{4}",
				delegate(Match m)
				{
					int i = int.Parse(m.Value.Substring(2), NumberStyles.HexNumber);
					return ((char)i).ToString();
				});
			Text(text);
		}

		//@ toxx [chars]
		// escape chars outside the range [32, 127] and any specified chars as \XX where XX is ASCII hex code
		[Command("ToXX")]
		public void ToHex(string args)
		{
			char[] special = GetParam(args, false, @",\").ToCharArray();
			Text(Utils.EscapeField(special, _Text));
		}

		//@ fromxx
		// unescape \XX chars, where XX is ASCII hex code
		[Command("FromXX")]
		public void FromHex(string args)
		{
			Text(Utils.UnescapeField(_Text, false));
		}

		static string Wrap(string text, int max)
		{
			return Regex.Replace(text, @"(^| +)([^\r\n]{0," + max + @"}(?![\w\p{P}]))", "$2\r\n", RegexOptions.Multiline);
		}

		//@ wrap [length]
		// line wrap, arg is the max length of a line, default 40
		[Command]
		public void Wrap(string args)
		{
			string s = GetParam(args, true, 40);
			int max;
			if (int.TryParse(s, out max))
			{
				Text(Wrap(_Text, max));
			}
			else
			{
				throw new ArgumentException("Format error: wrap <number>");
			}
		}

		string GetParam(string s, bool trim, object dflt)
		{
			if (trim) s = s.Trim();
			if (s == string.Empty) s = dflt.ToString();
			return s;
		}

		#region hash

		void HashMD5(Encoding enc)
		{
			var md5 = MD5.Create();
			byte[] buf = enc.GetBytes(_Text);
			buf = md5.ComputeHash(buf);
			Text(Convert.ToBase64String(buf));
		}

		//@ md5ascii
		// calculate MD5 checksum, use ASCII encoding
		[Command]
		public void MD5Ascii(string args)
		{
			HashMD5(Encoding.ASCII);
		}

		//@ md5utf8
		// calculate MD5 checksum, use UTF8 encoding
		[Command]
		public void MD5Utf8(string args)
		{
			HashMD5(Encoding.UTF8);
		}

		void HashSHA1(Encoding enc)
		{
			var sha1 = SHA1.Create();
			byte[] buf = enc.GetBytes(_Text);
			buf = sha1.ComputeHash(buf);
			Text(Convert.ToBase64String(buf));
		}

		//@ sha1ascii
		// calculate SHA-1 checksum, use ASCII encoding
		[Command]
		public void SHA1Ascii(string args)
		{
			HashSHA1(Encoding.ASCII);
		}

		//@ sha1utf8
		// calculate SHA-1 checksum, use UTF8 encoding
		[Command]
		public void SHA1Utf8(string args)
		{
			HashSHA1(Encoding.UTF8);
		}


		void HashSHA256(Encoding enc)
		{
			var sha256 = SHA256.Create();
			byte[] buf = enc.GetBytes(_Text);
			buf = sha256.ComputeHash(buf);
			Text(Convert.ToBase64String(buf));
		}

		//@ sha256ascii
		// calculate SHA-256 checksum, use ASCII encoding
		[Command]
		[Command]
		public void Sha256Ascii(string args)
		{
			HashSHA256(Encoding.ASCII);
		}

		//@ sha256utf8
		// calculate SHA-256 checksum, use UTF8 encoding
		[Command]
		public void Sha256Utf8(string args)
		{
			HashSHA256(Encoding.UTF8);
		}

		void HashSHA512(Encoding enc)
		{
			var sha512 = SHA512.Create();
			byte[] buf = enc.GetBytes(_Text);
			buf = sha512.ComputeHash(buf);
			Text(Convert.ToBase64String(buf));
		}

		//@ sha512ascii
		// calculate SHA-512 checksum, use ASCII encoding
		[Command]
		public void Sha512Ascii(string args)
		{
			HashSHA512(Encoding.ASCII);
		}

		//@ sha512utf8
		// calculate SHA-512 checksum, use UTF8 encoding
		[Command]
		public void Sha512Utf8(string args)
		{
			HashSHA512(Encoding.UTF8);
		}

		#endregion hash

		//@ pad char[(+|-)length]
		// pad each line with a given char: .+5 pad with "." 5 beyond max length, x-3, pad with "x" 3 less than max length; default is " +0": use space, pad to max length
		[Command]
		public void Pad(string args)
		{
			string s = GetParam(args, false, " +0").Unescape();
			if (s.Length == 1) s += "+0";
			char paddingChar = s[0];
			bool relative = s[1] == '+' || s[1] == '-';
			int n;
			if (!int.TryParse(s.Substring(1), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out n))
			{
				throw new ArgumentException("Invalid parameter, use: <padding-char>[+|-]<number>");
			}
			string[] lines = Lines;
			if (relative)
			{
				int max = 0;
				foreach (string line in lines)
				{
					if (line.Length > max) max = line.Length;
				}
				n += max; // e.g. n = -2, max = 10, result is 8
				if (n < 0) n = 0;
			}
			var sb = new StringBuilder();
			foreach (string line in lines)
			{
				int k = n - line.Length;
				if (k > 0)
				{
					sb.Append(line).AppendLine(new string(paddingChar, k));
				}
				else
				{
					sb.AppendLine(line);
				}
			}
			Text(sb);
		}


		//@ tobase64
		// turn text into UTF8 encoded base-64
		[Command]
		public void ToBase64(string args)
		{
			Text(Convert.ToBase64String(Encoding.UTF8.GetBytes(_Text), Base64FormattingOptions.InsertLineBreaks));
		}

		//@ frombase64
		// interpret text as base-64 encoded
		[Command]
		public void FromBase64(string args)
		{
			Text(Encoding.UTF8.GetString(Convert.FromBase64String(_Text)));
		}

		//@ validate regex
		// apply regex to each line and output per line: number of matches, then "|", then the line
		[Command]
		public void Validate(string args)
		{
			string param = GetParam(args, true, ".");
			Regex rx = new Regex(param, RegexOptions.Multiline | RegexOptions.Compiled);
			var sb = new StringBuilder();
			foreach (string line in Lines)
			{
				int count = rx.Matches(line).Count;
				if (count > 0) sb.AppendFormat("{0,4}|{1}\r\n", count, line);
				else sb.AppendFormat("    |{0}\r\n", line);
			}
			Text(sb);
		}

		//@ chksum
		// 8-bit ASCII checksum of the input
		[Command]
		public void ChkSum(string args)
		{
			Text(CheckSum(_Text));
		}

		static string CheckSum(string s)
		{
			byte[] b = Encoding.ASCII.GetBytes(s);
			int checksum = 0;
			for (int i = 0; i < b.Length; i++) checksum ^= (int)b[i];
			return checksum.ToString("X2");
		}

		//@ xmatch regex
		// extract all matches of the regex, output one match per line
		[Command("XMatch")]
		public void ExtractMatch(string args)
		{
			string regex = GetParam(args, false, @"(\w+)");
			string text = _Text;
			var matches = Regex.Matches(text, regex);
			var sb = new StringBuilder();
			foreach (Match m in matches)
			{
				for (int i = 1; i < m.Groups.Count; i++)
				{
					sb.Append(m.Groups[i].Value).Append('|');
				}
				if (m.Groups.Count > 1)
				{
					sb.Length--;
					sb.AppendLine();
				}
			}
			Text(sb);
		}

		//@ first regex
		// find first match of regex, if contains capture-groups, return first group, or else return total match
		[Command]
		public void First(string args)
		{
			string regex = GetParam(args, false, @"(\w+)");
			string text = _Text;
			var match = Regex.Match(text, regex);
			var s = match.Groups.Count > 1 ? match.Groups[1].Value : match.Value;
			Text(s);
		}


		//@ xguid sep
		// extract all GUIDs, when more on 1 line, use given separator in between
		[Command("XGuid")]
		public void ExtractGuids(string args)
		{
			string sep = GetParam(args, false, ",").Unescape();
			string[] lines = Lines;
			string pattern = "AA-A-A-A-AAA".Replace("A", "[A-Za-z0-9]{4}");
			Regex regex = new Regex(pattern, RegexOptions.Compiled);

			var sb = new StringBuilder();
			foreach (string line in lines)
			{
				var matches = regex.Matches(line);
				foreach (Match m in matches)
				{
					sb.Append(m.Value).Append(sep);
				}
				if (matches.Count > 0)
				{
					sb.Length--;
					sb.Append("\r\n");
				}
			}
			Text(sb);
		}

		//@ indentxml indent-string
		// interpret text as XML, format it, using given indentation string (default tab)
		[Command]
		public void IndentXml(string args)
		{
			string indent = string.IsNullOrEmpty(args) ? "\t" : args;
			Text(Utils.IndentXml(_Text, indent));
		}


		//@ col /sep/columns
		// select/reorder columns, look for sep; columns: space-separated ordinals of columns to select, *=remaining, +=next e.g.  /,/1 3 + ...all but #2
		[Command("Col")]
		public void SelectColumns(string args)
		{
			var par = GetParam(args, false, "/,/1").Unescape();
			string[] parts = par.Split(par[0]);
			string sep = parts[1];
			string[] sIndices = parts[2].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			int[] indices = (from s in sIndices select (s == "*") ? -1 : (s == "+") ? -2 : int.Parse(s) - 1).ToArray();
			var list = new List<string>();
			var sb = new StringBuilder();
			foreach (string line in Lines)
			{
				string[] cols = line.Split(new[] { sep }, StringSplitOptions.None);
				sb.Length = 0;
				int lastIndex = -1;
				foreach (int i in indices)
				{
					if (i < 0)
					{
						int start = (i == -1) ? 0 : lastIndex + 1;
						for (int j = start; j < cols.Length; j++)
						{
							string t = cols[lastIndex = j];
							if (!indices.Contains(j)) sb.Append(t).Append(sep);
						}
					}
					else if (i < cols.Length)
					{
						string t = cols[lastIndex = i];
						sb.Append(t).Append(sep);
					}
				}
				if (sb.Length >= sep.Length) sb.Length -= sep.Length;
				list.Add(sb.ToString());
			}
			Text(list.ToArray());
		}


		//@ twocol sep
		// format the text into two columns based on the separator
		[Command]
		public void TwoCol(string args)
		{
			string sep = GetParam(args, false, "=");
			int m = sep.Length;
			int max = int.MinValue;
			string[] lines = Lines;
			foreach (string line in lines)
			{
				int n = line.IndexOf(sep);
				if (n > max) max = n;
			}
			var lines2 = new List<string>();
			foreach (string line in lines)
			{
				int n = line.IndexOf(sep);
				if (n < 0)
				{
					lines2.Add(line);
				}
				else
				{
					string line2 = line.Substring(0, n) + new string(' ', max - n) + sep + line.Substring(n + m);
					lines2.Add(line2);
				}
			}
			Text(lines2);
		}

		//@ table /sep/sep2/options
		// format a table from the text, sep=column separator, replace by sep2; opt: n=>eq num col, w=>eq width, r=>right align all, z=>right align num
		[Command]
		public void Table(string args)
		{
			string par = GetParam(args, false, "/,/|/n").Unescape();
			// options: n = equal number of columns on each line
			//          w = equal width columns
			//          r = right align all columns
			//          z = right align numbers
			string[] parts = par.Split(par[0]);
			string sep = parts[1];
			string repl = parts.Length > 2 && parts[2] != "" ? parts[2] : sep;
			bool nEqualNumCols = false;
			bool wSameWidth = false;
			bool rRightAlign = false;
			bool zRightAlignNumbers = false;
			if (parts.Length > 3)
			{
				string options = parts[3];
				nEqualNumCols = options.Contains('n');
				wSameWidth = options.Contains('w');
				rRightAlign = options.Contains('r');
				zRightAlignNumbers = options.Contains('z');
			}
			int m = sep.Length;
			string[] separr = new string[] { sep };
			string[] lines = Lines;
			var maxx = new Dictionary<int, int>();
			int maxCols = 0;
			int maxWidth = 0;

			// analysis loop
			foreach (string line in lines)
			{
				string[] cols = line.Split(separr, StringSplitOptions.None);
				for (int i = 0; i < cols.Length; i++)
				{
					int width = cols[i].Length;
					int max;
					if (maxx.TryGetValue(i, out max))
					{
						if (width > max) maxx[i] = width;
					}
					else
					{
						maxx.Add(i, cols[i].Length);
						if (i > maxCols) maxCols = i; // for n option
					}
					if (width > maxWidth) maxWidth = width;
				}
			}
			string fmt = rRightAlign ? "{0," : "{0,-";
			var sb = new StringBuilder();
			if (wSameWidth) fmt += maxWidth + "}{1}";
			string fmtz = zRightAlignNumbers ? "{0," + maxWidth + "}{1}" : fmt;

			// formatting loop
			foreach (string line in lines)
			{
				string[] cols = line.Split(separr, StringSplitOptions.None);
				if (wSameWidth)
				{
					for (int i = 0; i < cols.Length; i++)
					{
						string thefmt = zRightAlignNumbers && IsNumberOnly(cols[i]) ? fmtz : fmt;
						sb.Append(string.Format(thefmt, cols[i], repl));
					}
				}
				else
				{
					for (int i = 0; i < cols.Length; i++)
					{
						int max;
						if (!maxx.TryGetValue(i, out max)) max = 0;
						if (zRightAlignNumbers && IsNumberOnly(cols[i]))
						{
							sb.Append(string.Format("{0," + max + "}{1}", cols[i], repl));
						}
						else
						{
							sb.Append(string.Format(fmt + max + "}{1}", cols[i], repl));
						}
					}
				}
				if (nEqualNumCols && cols.Length <= maxCols)
				{
					if (wSameWidth)
					{
						string ww = new string(' ', maxWidth) + repl;
						for (int i = cols.Length; i <= maxCols; i++)
						{
							sb.Append(ww);
						}
					}
					else
					{
						for (int i = cols.Length; i <= maxCols; i++)
						{
							sb.Append(new string(' ', maxx[i])).Append(repl);
						}
					}
				}
				sb.Length -= repl.Length;
				sb.AppendLine();
			}
			Text(sb);

		}

		static bool IsNumberOnly(string s)
		{
			return Regex.IsMatch(s, @"^\s*([-+]?\d+(?:\.\d+)?)\s*$");
		}

		//@ match regex
		// remove all lines that do not contain a match of the regex
		[Command]
		public void Match(string args)
		{
			RemoveMatching(args, false);
		}

		//@ nomatch regex
		// remove all lines that contain a match of the regex
		[Command]
		public void NoMatch(string args)
		{
			RemoveMatching(args, true);
		}

		void RemoveMatching(string regex, bool matching)
		{
			var list = new List<string>();
			var sb = new StringBuilder();
			foreach (string line in Lines)
			{
				if (matching ^ Regex.IsMatch(line, regex, RegexOptions.Multiline))
				{
					sb.AppendLine(line);
				}
			}
			Text(sb);
		}

		//@ trim chars
		// trim given chars off start and end of string, default chars are space and tab
		[Command]
		public void Trim(string args)
		{
			char[] chars = GetCharArrayFromParam(args, @" \t");
			Text(Lines.Select<string, string>(s => s.Trim(chars)));
		}

		//@ strim chars
		// trim given chars off start of string, default chars are space and tab
		[Command("STrim")]
		public void TrimStart(string args)
		{
			char[] chars = GetCharArrayFromParam(args, @" \t");
			Text(Lines.Select<string, string>(s => s.TrimStart(chars)));
		}

		//@ etrim chars
		// trim given chars off end of string, default chars are space and tab
		[Command("ETrim")]
		public void TrimEnd(string args)
		{
			char[] chars = GetCharArrayFromParam(args, @" \t");
			Text(Lines.Select<string, string>(s => s.TrimEnd(chars)));
		}

		char[] GetCharArrayFromParam(string args, string dflt)
		{
			return GetParam(args, false, dflt).Unescape().ToCharArray();
		}

		//@ repeat n
		// repeat the text n times
		[Command]
		public void Repeat(string args)
		{
			string par = GetParam(args, true, 2);
			int n = int.Parse(par);
			string text = _Text;
			var sb = new StringBuilder();
			while (n-- > 0) sb.Append(text);
			Text(sb);
		}

		//@ linenum format
		// prefix with line numbers starting at 1, replace "[]" in format by the number
		[Command("LineNum")]
		public void LineNumbers(string args)
		{
			string prefix, suffix;
			GetPrefixSuffix(args, "[] ", out prefix, out suffix);

			string[] lines = Lines;
			int n = 1 + (int)Math.Log10(lines.Length);
			string fmt = "D" + n;
			var list = new List<string>();
			for (int i = 0; i < lines.Length; i++) list.Add(prefix + (i + 1).ToString(fmt) + suffix + lines[i]);
			Text(list);
		}

		private void GetPrefixSuffix(string args, string pattern, out string prefix, out string suffix)
		{
			string par = GetParam(args, false, pattern).Unescape();
			int p = par.IndexOf("[]");
			if (p == -1) throw new Exception(string.Format("Param: prefix{0}suffix e.g. ({0}) or {0}:", pattern));
			prefix = par.Substring(0, p);
			suffix = par.Substring(p + 2);
		}

		//@ num s fmt start step end
		// find each occurence of the string  s and replace it by a sequence number, default string="NUM", format="0", start=1, step=1, end=maxvalue
		[Command("Num")]
		public void Autonumber(string args)
		{
			string param = GetParam(args, true, "NUM 0 1");
			string[] par = param.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string marker = par[0];
			string fmt = par.Length > 1 ? par[1] : "#";
			int start = par.Length > 2 ? int.Parse(par[2]) : 1;
			int step = par.Length > 3 ? int.Parse(par[3]) : 1;
			int end = par.Length > 4 ? int.Parse(par[4]) : int.MaxValue;
			int number = start;
			string text = _Text;
			text = Regex.Replace(text, marker, delegate(Match m)
			{
				string s = number.ToString(fmt);
				if ((number += step) > end) number = start;
				return s;
			});
			Text(text);
		}

		//@ delblank
		// delete all blank lines
		[Command("DelBlank")]
		public void DeleteBlankLines(string args)
		{
			Text(Lines.Where(line => line.Trim() != string.Empty));
		}

		//@ chomp
		// trim any string of \t, \r, \n from the end of the text 
		[Command("Chomp")]
		public void ChopCharactersAtEnd(string args)
		{
			char[] chars = GetCharArrayFromParam(args, @" \t\r\n");
			Text(_Text.TrimEnd(chars));
		}

		//@ split [sep]
		// split the line on the given string (default a " ") into multiple lines
		[Command]
		public void Split(string args)
		{
			string[] sep = { GetParam(args, false, " ").Unescape() };
			string[] lines = Lines;
			var list = new List<string>();
			foreach (string line in lines)
			{
				string[] sublines = line.Split(sep, StringSplitOptions.None);
				list.AddRange(sublines);
			}
			Text(list.ToArray());
		}

		//@ join [sep]
		// join all lines into one line adding the optional separator in between
		[Command("Join")]
		public void JoinAll(string args)
		{
			string sep = GetParam(args, false, "").Unescape();
			Text(string.Join(sep, Lines));
		}

		//@ join [n [sep]]
		// join every n (default 2) lines into one line, using the given (unescaped) separator (default " ")
		[Command]
		public void JoinN(string args)
		{
			string t = GetParam(args, false, "2").Unescape();
			string[] par = t.Split(new char[] { ' ' }, 2);
			string sep = "";
			int n = 2;

			int.TryParse(par[0], out n);
			if (par.Length > 1) sep = par[1];
			var list = new List<string>();
			var group = new List<string>();
			int i = 0;
			foreach (string s in Lines)
			{
				group.Add(s);
				if (++i >= n)
				{
					list.Add(string.Join(sep, group.ToArray()));
					group.Clear();
					i = 0;
				}
			}
			list.Add(string.Join(sep, group.ToArray()));
			Text(list);
		}

		//@ join /regex/sep/mode
		// group the lines separated by lines that matches the regex, put them on one line using the given separator (default "|")
		[Command("JoinX")]
		public void JoinRegex(string args)
		{
			string param = GetParam(args, true, "/^$/|/");
			string[] split = param.Split(new[] { param[0] });
			if (split.Length != 4) throw new FormatException("Format: /pattern/separator/[options]");

			string sep = split[2].Unescape();
			string options = split[3];
			string pattern = (options == string.Empty) ? split[1] : "(?" + options + ":" + split[1] + ")";
			var regex = new Regex(pattern);

			var list = new List<string>();
			var group = new List<string>();
			foreach (string s in Lines)
			{
				Match m = regex.Match(s);
				if (m.Success)
				{
					if (m.Groups[1].Value != string.Empty) group.Add(m.Groups[1].Value);
					list.Add(string.Join(sep, group.ToArray()));
					group.Clear();
				}
				else group.Add(s);
			}
			list.Add(string.Join(sep, group.ToArray()));
			Text(list);
		}

		//@ makeguid [string]
		// find each occurrence of the given string (default "GUID") and replace by a Guid
		[Command("mkguid")]
		public void MakeGuid(string args)
		{
			string[] lines = Lines;
			string marker = GetParam(args, false, "GUID");
			Regex rx = new Regex(marker);
			Text(from s in Lines select rx.Replace(s, m => Guid.NewGuid().ToString().ToLowerInvariant()));
		}

		//@ count
		// list unique lines and put number of occurrences then "|" then line
		[Command]
		public void Count(string args)
		{
			var map = new SortedDictionary<string, int>();
			foreach (string line in Lines)
			{
				int count;
				if (map.TryGetValue(line, out count)) map[line]++;
				else map[line] = 1;
			}
			Text(from k in map.Keys select map[k] + "|" + k);
		}

		//@ insert n line
		// insert the given line of text after every n'th line from the input
		[Command]
		public void Insert(string args)
		{
			string[] par = args.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string count = par[0];
			int n = Convert.ToInt32(count);
			string s = par[1];

			int k = 0;
			var list = new List<string>();
			foreach (string line in Lines)
			{
				list.Add(line);
				if (++k == n)
				{
					list.Add(s);
					k = 0;
				}
			}
			Text(list);
		}

		//@ info [text]
		// print the given text or else the intermediate result
		[Command]
		public void Info(string args)
		{
			string msg = string.IsNullOrEmpty(args) ? _Text : args;
			_Context.SendMessage("~" + msg);
		}

		//@ store macro
		// put the intermediate result of the script into the dictionary
		[Command]
		public void Store(string args)
		{
			_Context.Put(args, _Text);
		}

		//@ template template-line
		// take "|"-separated values found in text lines and fill in in template-line as $1, $2, $3..., $# is 1-based seq number
		[Command]
		public void Template(string args)
		{
			var list = new List<string>();
			for (int k = 0; k < Lines.Length; k++)
			{
				string line = Lines[k];
				string[] fields = line.Split('|');
				string s = args.Replace("$#", (k + 1).ToString(CultureInfo.InvariantCulture));
				for (int i = 0; i < fields.Length; i++)
				{
					s = s.Replace("$" + (i + 1), fields[i]);
				}
				list.Add(s);
			}
			Text(list);
		}


		//@ sclip macro regex
		// replace a pattern in each line in the text with a line in a macro, restart line 0 if less lines than matches
		[Command(Name = "SClip")]
		public void ReplaceKeywordByMacroLines(string args)
		{
			var parts = GetParam(args, true, @"paste \[\]").Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
			string key = parts[0];
			string rx = parts.Length == 2 ? parts[1] : @"\[\]";
			string list = _Context.Get(key);
			if (!string.IsNullOrEmpty(list))
			{
				string[] items = list.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				string text = _Text;
				int i = 0;
				string t = Regex.Replace(text, rx, delegate
				{
					if (i >= items.Length) i = 0;
					return items[i++];
				});
				Text(t);
			}

		}

		//@ crlf
		// make line endings -\r- -\n- -\r\r\n- -\r\n- all like -\r\n- 
		[Command(Name = "Crlf")]
		public void EnsureCrlf(string args)
		{
			Text(Regex.Replace(_Text, @"\r(?!\n)|(?<!\r)\n|\r+\n", "\r\n"));
		}

		//@ tocsv [sep]
		// convert lines with given separator (default ",") to CSV format, using ".." and escape " where necessary
		[Command]
		public void ToCsv(string args)
		{
			string sep = GetParam(args, false, ",");
			var list = Lines.Select(line => MakeCsv(sep, line)).ToList();
			Text(list);
		}

		
		private string MakeCsv(string sep, string line)
		{
			var sb = new StringBuilder();
			string[] fields = line.Split(new string[] { sep }, StringSplitOptions.None);
			foreach (string field in fields)
			{
				if (field.Contains(',') || field.Contains('"') || field.StartsWith(" ") || field.EndsWith(" "))
				{
					sb.Append('"').Append(field.Replace("\"", "\"\"")).Append('"');
				}
				else if (field == "")
				{
					sb.Append("\"\"");
				}
				else
				{
					sb.Append(field);
				}
				sb.Append(',');
			}
			if (sb.Length > 1) sb.Length--;
			return sb.ToString();
		}

		//@ fromcsv [sep]
		// convert from comma separated values to use another separator (default "|"), unescape \" and ""
		[Command]
		public void FromCsv(string args)
		{
			string sep = GetParam(args, false, "|");
			var list = Lines.Select(line => UndoCsv(sep, line)).ToList();
			Text(list);
		}

		// From the book "Mastering Regular Expressions"
		private static readonly Regex _RxCSV = new Regex(
			@"(?:^|,)(?:""((?>[^""]+|"""")*)""|([^"",]*))", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

		private string UndoCsv(string customSep, string csvLine)
		{
			var sb = new StringBuilder();
			Match m = _RxCSV.Match(csvLine);
			while (m.Success)
			{
				string field = null;
				if (m.Groups[1].Success) field = m.Groups[1].Value.Replace("\"\"", "\"");
				else field = m.Groups[2].Value;
				m = m.NextMatch();
				sb.Append(field).Append(customSep);
			}
			if (sb.Length >= customSep.Length) sb.Length -= customSep.Length;
			return sb.ToString();
		}

		//@ path function
		// interpret each line as a path and apply function: file=>filename, dir=>directory, ext=>extention, root=>root/drive, noext=>fn w/o ext
		[Command("Path")]
		public void AnalyzePath(string args)
		{
			var cmd = GetParam(args, true, "full");
			var sb = new StringBuilder();
			foreach (var line in Lines)
			{
				string s = null;
				switch (cmd)
				{
					case "file":
						s = Path.GetFileName(line);
						break;
					case "dir":
						s = Path.GetDirectoryName(line);
						break;
					case "ext":
						s = Path.GetExtension(line);
						break;
					case "root":
						s = Path.GetPathRoot(line);
						break;
					case "full":
						s = Path.GetFullPath(line);
						break;
					case "noext":
						s = Path.GetFileNameWithoutExtension(line);
						break;
				}
				if (s != null) sb.AppendLine(s);
			}
			Text(sb);
		}

		/*
		 * converting script methods from TextTools to Recipe:
			def tmp
				%\[Script\("([^"]+)"[^\r\n]*%[Command("$1")]%
				s %_Box2\.Lines\s*=\s*([^;]+)%Text($1)%
				s %_Box1\.Lines%Lines%
				s %_Box1\.Text%_Text%
				s %_Box2\.Text\s*=\s*([^;]+)%Text($1)%
				s %GetParam\(%GetParam(args, %
				s %Mni_Click[^\r\n]+%(string args)%
			end
		 */
	}
}
