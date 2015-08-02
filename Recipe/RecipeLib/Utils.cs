using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.ServiceProcess;

namespace RecipeLib
{
	public static class Utils
	{

		public static string IndentXml(string input, string indent)
		{
			var doc = new XmlDocument();
			doc.LoadXml(input);
			var sb = new StringBuilder();
			var settings = new XmlWriterSettings
			{
				Indent = true,
				IndentChars = indent,
				ConformanceLevel = ConformanceLevel.Document
			};
			var writer = XmlWriter.Create(sb, settings);
			doc.WriteTo(writer);
			writer.Close();
			return sb.ToString();
		}

		public static string Sort(string input, Regex rx)
		{
			string[] lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			if (rx != null)
			{
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
			return string.Join(Environment.NewLine, lines);
		}

		public static string Unique(string input, Regex rx)
		{
			string[] lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			var result = new List<string>();
			var unique = new HashSet<string>();
			foreach (var line in lines)
			{
				string key = line;
				if (rx != null)
				{
					var match = rx.Match(line);
					if (match.Success && match.Groups.Count > 0) key = match.Groups[1].Value;
				}
				if (!unique.Contains(key))
				{
					result.Add(line);
					unique.Add(key);
				}
			}
			return string.Join(Environment.NewLine, result.ToArray());
		}

		public static string Repeat(string input, int count)
		{
			var sb = new StringBuilder();
			while (count-- > 0) sb.Append(input);
			return sb.ToString();
		}

		//private static string MakeTwoColumns(string input, string sep)
		//{
		//   string[] lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
		//   int m = sep.Length;
		//   int max = int.MinValue;
		//   foreach (string line in lines)
		//   {
		//      int n = line.IndexOf(sep);
		//      if (n > max) max = n;
		//   }
		//   var lines2 = new List<string>();
		//   foreach (string line in lines)
		//   {
		//      int n = line.IndexOf(sep);
		//      if (n < 0)
		//      {
		//         lines2.Add(line);
		//      }
		//      else
		//      {
		//         string line2 = line.Substring(0, n) + new string(' ', max - n) + sep + line.Substring(n + m);
		//         lines2.Add(line2);
		//      }
		//   }
		//   return string.Join(Environment.NewLine, lines2.ToArray());
		//}

		/// <summary>
		/// Use backslash to escape non-ASCII chars, using 2 digit hex code.
		/// Only works on 8-bit chars. 
		/// </summary>
		/// <param name="special">some other characters that also need escaping</param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string EscapeField(char[] special, string text)
		{
			var sb = new StringBuilder();
			sb.Length = 0;
			foreach (char c in text)
			{
				if (c < 32 || c > 127 || Array.IndexOf(special, c) != -1)
				{
					sb.Append('\\').Append(((int)c).ToString("X2"));
				}
				else sb.Append(c);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Unescape a text that was escaped with EscapeField().
		/// </summary>
		/// <param name="text">escaped text</param>
		/// <param name="flipBackslash
		/// <returns>string with unescaped chars, but backslash has been replaced by / </returns>
		public static string UnescapeField(string text, bool flipBackslash)
		{
			char slash = flipBackslash ? '/' : '\\';
			var sb = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '\\')
				{
					if (i < text.Length - 2)
					{
						string x = string.Concat(text[i + 1], text[i + 2]);
						int n;
						if (int.TryParse(x, NumberStyles.HexNumber, null, out n))
						{
							char d = (char)n;
							if (d == '\\' && flipBackslash) sb.Append('/');
							else if (n < 9 || (n > 10 && n < 13) || (n > 13 && n < 32) || n > 126)
							{
								sb.Append('~').Append(n);
							}
							else sb.Append(d);
							i += 2;
						}
						else sb.Append(slash);
					}
					else sb.Append(slash);
				}
				else sb.Append(c);
			}
			return sb.ToString();
		}

		public static string Unescape(this string s)
		{
			var sb = new StringBuilder(s.Length + 1);
			bool escape = false;
			foreach (char c in s)
			{
				if (escape)
				{
					switch (c)
					{
						case 'n': sb.Append(Environment.NewLine);
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


		//public static string ToHex(byte[] buf)
		//{
		//	var sb = new StringBuilder();
		//	foreach (byte b in buf) sb.AppendFormat("{0:X2}", (int)b);
		//	return sb.ToString();
		//} //TODO TEST

		/// <summary>
		/// Start, stop or restart a windows service.
		/// </summary>
		/// <param name="name">service name</param>
		/// <param name="action">-1 for stop, 0 for restart, 1 for start</param>
		/// <param name="timeout">max time to wait for status change to happen</param>
		/// <returns>true if success, false if timeout or exception</returns>
		public static bool WindowsService(string name, int action, TimeSpan timeout)
		{
			ServiceController service = new ServiceController(name);
			try
			{
				switch (action)
				{
					case -1:
						service.Stop();
						service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
						break;
					case 1:
						service.Start();
						service.WaitForStatus(ServiceControllerStatus.Running, timeout);
						break;
					case 0:
						int ticks = Environment.TickCount;
						service.Stop();
						service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
						timeout -= TimeSpan.FromTicks(Environment.TickCount - ticks);
						service.Start();
						service.WaitForStatus(ServiceControllerStatus.Running, timeout);
						break;
				}
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
