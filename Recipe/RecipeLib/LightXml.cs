using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace TextGen
{
	public class LightXml
	{
		private static Regex _UnquotedAttrValue = new Regex(@"(\w+)\s*=\s*([^'"" ]\S*)", RegexOptions.Compiled);


		public static string ToNormalXml(string lightText)
		{
			var stack = new Stack<string>();
			var sb = new StringBuilder();
			var xml = new List<string>();
			lightText = lightText.Trim();
			if (lightText == string.Empty) return string.Empty;

			string[] lines = lightText.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			int previousLevel = 0;
			string elem = null;
			bool textMode = false;
			for (int k = 0; k < lines.Length; k++)
			{
				string line = lines[k].TrimEnd();
				if (line.Length == 0) continue;
				int level = 0;
				while (line[level] == '\t') level++;

				char first = line[level];
				if (first == '\'' || first == '\"')
				{
					if (!textMode)
					{
						sb.Append(">");
						if (first == '\"') sb.AppendLine();
						textMode = true;
					}
					if (first == '\"') sb.Append(line, 0, level);
					string text = SecurityElement.Escape(line.Substring(level + 1, line.Length - level - 1));
					sb.Append(text).AppendLine();
				}
				else
				{

					string[] sublines = line.EndsWith(";")
						? line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
						: new string[] { line };

					foreach (string s in sublines)
					{
						if (level > previousLevel)
						{
							sb.AppendLine(">");
							previousLevel = level;
							stack.Push(elem);
						}
						else if (k != 0)
						{
							if (textMode)
							{
								textMode = false;
								sb.AppendFormat("{0}</{1}>", new string('\t', stack.Count), elem).AppendLine();
							}
							else sb.AppendLine("/>");

							while (level < previousLevel)
							{
								previousLevel--;
								sb.Append(new string('\t', previousLevel))
									.Append("</")
									.Append(stack.Pop())
									.AppendLine(">");
							}
						}
						string[] fractions = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						elem = fractions[0].Trim();
						sb.Append(new string('\t', level))
							.Append('<')
							.Append(elem)
							.Append(' ')
							.Append(_UnquotedAttrValue.Replace(string.Join(" ", fractions, 1, fractions.Length - 1), MakeAttribute));
					}
				}
			}
			if (textMode) sb.Append(new string('\t', previousLevel)).Append("</").Append(elem).AppendLine(">");
			else sb.AppendLine("/>");
			while (stack.Count > 0)
			{
				elem = stack.Pop();
				sb.Append(new string('\t', stack.Count))
					.Append("</")
					.Append(elem)
					.AppendLine(">");

			}
			return sb.ToString();

		}

		private static string MakeAttribute(Match m)
		{
			return m.Groups[1].Value + "='" + SecurityElement.Escape(m.Groups[2].Value) + "'";
		}


	}
}