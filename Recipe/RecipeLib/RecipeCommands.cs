using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TextGen;

namespace RecipeLib
{

	partial class Recipe
	{
		private void C_Edit(string args)
		{
			string contents;
			if (TryGetMacro(args, out contents)) SendMessage("@edit {0}", contents);
		}

		/// <summary>
		/// Converts dates found in the macro text or files, using the given spec.
		/// </summary>
		/// <param name="spec">4 lines: 1. input format (*=any), 2. input time zone (*=UTC), 3. output time zone. (*=same as input), 4. output format (*=same as input)</param>
		/// <param name="args">rxline: rx macro/paths</param>
		private void C_Date(LineReader reader, string args)
		{
			string spec = ReadHereString(reader);
			string srx, macro;
			string[] paths;
			if (GetRxLine(args, out srx, out paths, out macro))
			{
				Regex rx = new Regex(srx, RegexOptions.CultureInvariant | RegexOptions.Compiled);
				if (macro != null)
				{
					string text;
					if (TryGetMacro(macro, out text))
					{
						string text1 = ConvertDateTime(text, rx, spec);
						SetMacro(macro, text1);
					}
				}
				else
				{
					foreach (string p in paths)
					{
						string text = File.ReadAllText(p);
						string text1 = ConvertDateTime(text, rx, spec);
						File.WriteAllText(p, text1);
					}
				}
			}
		}

		private void C_Default(LineReader reader, string args, string cmd)
		{
			string key = args.Trim();
			CustomCommand cc;
			if (_CustomCommands.TryGetValue(cmd, out cc))
			{
				string text = ReadHereString(reader, true);
				string val = ExecuteCustomCommand(cc, text);
				if (key != "") SetMacro(key, val);
			}
			else if (cmd != "tags" && cmd != "auto")
			{
				throw new Exception("Unknown command: #" + cmd);
			}
		}

		private void C_Count(string args)
		{
			string[] nv = Chop(args, 2, '=');
			string key = nv[0];
			string[] parts = nv[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string path = parts[0];
			string text = parts.Length > 1 ? parts[1] : string.Empty;

			//_COMMANDS_END_

			string contents;
			if (TryGetMacro(path, out contents))
			{
				switch (text.ToLowerInvariant())
				{
					case "length":
					case "chars":
						SetMacro(key, contents.Length.ToString());
						break;
					case "":
					case "lines":
						SetMacro(key, contents.Split(new string[] { "\r\n" }, StringSplitOptions.None).Length.ToString());
						break;
					default:
						SendMessage("@count invalid unit: " + text);
						break;
				}
			}
			else if (File.Exists(path))
			{
				switch (text.ToLowerInvariant())
				{
					case "b":
					case "bytes":
						SetMacro(key, new FileInfo(path).Length.ToString());
						break;
					case "kb":
						SetMacro(key, (new FileInfo(path).Length / 1024).ToString("0.0"));
						break;
					case "":
					case "lines":
						SetMacro(key, File.ReadAllLines(path).ToList().Count.ToString());
						break;
					default:
						SendMessage("@count invalid unit: " + text);
						break;

				}
			}
			else if (Directory.Exists(path))
			{
				if (text == "") text = "*";
				SetMacro(key, Directory.GetFiles(path, text).Length.ToString());
			}
			else
			{
				SetMacro(key, "");
			}
		}

		private void C_Wstart_Wstop_Wrestart(string args, string cmd)
		{
			string val;
			if (Write)
			{
				int n;
				if (!TryGetMacro("wtimeout", out val) || !int.TryParse(val, out n)) n = 45;
				bool ok = Utils.WindowsService(args, cmd == "wstart" ? 1 : cmd == "wstop" ? -1 : 0, TimeSpan.FromSeconds(n));
				SetMacro("exitcode", val = ok ? "0" : "1");
			}
			else
			{
				val = "skip";
			}
			SendMessage("@{0} {1} -> {2}", cmd, args, val);
		}

		private void C_Def(LineReader reader, string args)
		{
			string[] parts = Chop(args);
			string key = parts[0];
			if (key.Length < 1 || key[0] < 'A' || key[0] > 'Z') throw new ArgumentException("#def name must start with capital A..Z");
			string text = ReadCData(reader, "#enddef");
			string[] arr = new string[parts.Length - 1];
			Array.Copy(parts, 1, arr, 0, parts.Length - 1);
			_CustomCommands[key] = new CustomCommand(key, text, arr);
		}

		private void C_While_Until(LineReader reader, string args, string cmd)
		{
			string[] parts = Chop(args, 2, ' ');
			string macro = parts[0];
			string rx = parts[1];
			bool not = cmd == "until";
			string contents = ReadCData(reader, "#loop");
			SendMessage("@{0} {1} {2}", cmd, macro, rx);
			string text;
			while (not ^ (TryGetMacro(macro, out text) && Regex.IsMatch(text, rx)))
			{
				text = Expand(contents);
				using (var sr = new LineReader(text, name: args))
				{
					if (Run(sr) == null && !_Run) break;
				}
			}
		}

		private void C_For(LineReader reader, string args)
		{
			string[] parts = Chop(args);
			string fmt = parts[0];
			int min = Convert.ToInt32(parts[1]);
			int step = Convert.ToInt32(parts[2]);
			int max = Convert.ToInt32(parts[3]);
			string contents = ReadCData(reader, "#next");
			SendMessage("@for {0}, {1}..{3} step {2}", fmt, min, step, max);
			for (int i = min; i <= max; i += step)
			{
				SetMacro("_", i.ToString(fmt, CultureInfo.InvariantCulture));
				string text = Expand(contents);
				using (var sr = new LineReader(text, args))
				{
					if (Run(sr) == null && !_Run) break;
				}
			}
		}

		private void C_Mode(string args)
		{
			if (args.Contains("write")) Write = true;
			if (args.Contains("test")) Write = false;
			if (args.Contains("int-")) Interactive = false;
			if (args.Contains("int+")) Interactive = true;
			if (args.Contains("log-")) Log = false;
			if (args.Contains("log+")) Log = true;
			if (args.Contains("warn-")) Warn = false;
			if (args.Contains("warn+")) Warn = true;
			if (args.Contains("hide")) Show = false;
			if (args.Contains("show")) Show = true;
			SendMessage("@mode " + args);
		}

		private void C_Csharp(LineReader reader, string args)
		{
			string script = ReadHereString(reader);
			var result = CSharp.Build(script);
			string text;
			if (result.Errors.HasErrors)
			{
				text = string.Join(Environment.NewLine, CSharp.Errors(result));
				SendMessage("@csharp {0}", text);
			}
			else if (Write)
			{
				string[] parts = Chop(args);
				string key = parts[0];
				parts = parts.Where((s, i) => i > 0).Select(s => GetMacro(s)).ToArray();
				text = CSharp.Execute(result.CompiledAssembly, parts);
				if (key != string.Empty) SetMacro(key, text);
			}
			else SendMessage("@csharp skip (test mode)");
		}

		private void C_Exists(string args)
		{
			string[] nv = Chop(args, 2, '=');
			string key = nv[0];
			string path = nv[1];
			string contents;
			if (TryGetMacro(path, out contents))
			{
				SetMacro(key, "macro");
			}
			else if (File.Exists(path))
			{
				SetMacro(key, "file");
			}
			else if (Directory.Exists(path))
			{
				SetMacro(key, "dir");
			}
			else
			{
				SetMacro(key, "");
			}
		}

		private void C_Version(string args)
		{
			var version = Assembly.GetExecutingAssembly().GetName().Version;
			if (!VersionOK(args, version))
			{
				SendMessage("@version required {0}: actual {1} -> stopped execution", args, version);
				_Run = false;
			}
		}

		private void C_Product(string args)
		{
			string[] nv = Chop(args, 2, '=');
			string key = nv[0];
			string[] parts = nv[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string contents, text;
			if (TryGetMacro(parts[0], out contents) && TryGetMacro(parts[1], out text))
			{
				var list1 = contents.Split(_LineBrk, StringSplitOptions.None);
				var list2 = text.Split(_LineBrk, StringSplitOptions.None);
				SetMacro(key, string.Join(Environment.NewLine, (from line1 in list1 from line2 in list2 select line1 + line2).ToArray()));
			}
		}

		private void C_SetOperations(string args, string cmd)
		{
			string[] nv = Chop(args, 2, '=');
			string key = nv[0];
			string[] parts = nv[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			IEnumerable<string> aggregate = null;
			foreach (string s in parts)
			{
				string contents;
				if (TryGetMacro(s, out contents))
				{
					var lines = contents.Split(_LineBrk, StringSplitOptions.None);
					if (aggregate == null) aggregate = lines;
					else aggregate = cmd == "union"
									? aggregate.Union(lines)
									: cmd == "intersect"
												? aggregate.Intersect(lines)
												: aggregate.Except(lines);
				}
			}
			SetMacro(key, string.Join(Environment.NewLine, aggregate.ToArray()));
		}

		private void C_Encrypt_Decrypt(string args, string cmd)
		{
			string[] parts = Chop(args, 2, ' ');
			SendMessage("{0} {1}", cmd, parts[0]);
			string key = (parts.Length > 1) ? parts[1] : GetMacro("password");
			if (string.IsNullOrEmpty(key)) throw new Exception("no password for " + cmd);
			string text = GetMacro(parts[0]);
			if (string.IsNullOrEmpty(text)) throw new Exception("nothing to " + cmd);
			text = cmd == "encrypt"
				? Crypto.EncryptStringToBase64(text, key)
				: Crypto.DecryptStringFromBase64(text, key);
			SetMacro(parts[0], text);
		}

		private void C_Watch(LineReader reader, string args)
		{
			if (Write)
			{
				string script = ReadCData(reader, null);
				string[] parts = Chop(args, 2, ' ');
				_Watcher = new Watcher(this, script);
				new Thread(() => _Watcher.ExecutionLoop(parts[0], parts[1])).Start();
				SendMessage("@watch {0}", args);
			}
		}

		private void C_Write(LineReader reader, string args)
		{
			if (Write)
			{
				string script = ReadCData(reader, null);
				string[] parts = args.Split(':');
				string host = parts[0] == string.Empty ? "0.0.0.0" : parts[0];
				int port = parts.Length < 2 || string.IsNullOrEmpty(parts[1]) ? DefaultPort : Convert.ToInt32(parts[1]);
				if (_Server != null) throw new Exception("Can run only one instance of a server at port " + port);
				_Server = new RecipeServer(host, port, this, script);
				new Thread(_Server.Listen).Start();
				SendMessage("@server {0}", port);
			}
		}

		private void C_Post(LineReader reader, string args)
		{
			string[] parts = Chop(args, 2, '=');
			string contents = ReadHereString(reader);
			if (Write && contents.Length > 0)
			{
				var request = (HttpWebRequest)WebRequest.Create(parts[1]);
				byte[] buf = Encoding.UTF8.GetBytes(contents);
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = buf.Length;
				request.Credentials = CredentialCache.DefaultCredentials;
				request.UserAgent = "RecipeTool";

				using (Stream st = request.GetRequestStream())
				{
					st.Write(buf, 0, buf.Length);
				}
				using (var response = request.GetResponse())
				{
					using (Stream st = response.GetResponseStream())
					{
						using (var sr = new StreamReader(st))
						{
							_Macros[parts[0]] = sr.ReadToEnd();
						}
					}
				}
			}
		}

		private void C_Udp(LineReader reader, string args)
		{
			string[] parts = Chop(args);
			string contents = ReadHereString(reader);
			using (var u = new UdpClient(parts[0], Convert.ToInt32(parts[1])))
			{
				if (Write)
				{
					byte[] bytes = Encoding.UTF8.GetBytes(contents);
					u.Send(bytes, bytes.Length);
				}
				SendMessage("@udp {0}:{1} {2}", parts[0], parts[1], Truncate(contents, 70));
			}
		}

		private void C_Pause(string args)
		{
			int ms = Convert.ToInt32(args);
			SendMessage("@pause {0}ms", ms);
			Thread.Sleep(ms);
		}

		private void C_With(LineReader reader, string args)
		{
			if (!string.IsNullOrEmpty(args))
			{
				string[] parts = Chop(args, 2, '=');
				string key = parts[0];
				string script = parts.Length == 2 ? parts[1].TrimStart() : ReadHereString(reader);
				string text;
				string[] paths;
				if (TryGetMacro(key, out text))
				{
					text = Apply(script, text);
					SetMacro(key, text);
					SendKeyValueMessage(key, script);
				}
				else
				{
					paths = GetFiles(args);
					foreach (string p in paths)
					{
						text = File.ReadAllText(p);
						text = Apply(script, text);
						if (Write) File.WriteAllText(p, text);
						SendMessage("@with {0}", p);
					}
				}
			}
		}

		private void C_Unique(string args)
		{
			string rx, macro;
			string[] paths;

			if (GetRxLine(args, out rx, out paths, out macro))
			{
				var regex = (rx == "!") ? null : new Regex(rx);
				string text;
				if (macro != null && TryGetMacro(macro, out text))
				{
					text = Utils.Unique(text, regex);
					_Macros[macro] = text;
					SendMessage("@unique {0}", macro);
				}
				else if (paths.Length != 0)
				{
					foreach (string p in paths)
					{
						string contents = File.ReadAllText(p);
						contents = Utils.Unique(contents, regex);
						if (Write) File.WriteAllText(p, contents);
						SendMessage("@unique {0}", p);
					}
				}
			}
		}

		private void C_Repeat(string args)
		{
			string[] nv = Chop(args);
			string contents;
			int n;
			if (nv.Length == 2 && int.TryParse(nv[1], out n) && TryGetMacro(nv[0], out contents))
			{
				_Macros[nv[0]] = Utils.Repeat(contents, n);
			}
		}

		private void C_Sort(string args)
		{
			string rx, macro;
			string[] paths;
			if (GetRxLine(args, out rx, out paths, out macro))
			{
				if (rx == "date")
				{
					rx = @"(\d{4}-\d\d-\d\d[T\ ]\d\d:\d\d(?::\d\d(?:\.\d+)?)?)";
				}
				Regex regex = rx == "!" ? null : new Regex(rx, RegexOptions.CultureInvariant);
				string text;
				if (macro != null && TryGetMacro(macro, out text))
				{
					text = Utils.Sort(text, regex);
					_Macros[macro] = text;
					SendMessage("@sort {0}", macro);
				}
				else if (paths.Length != 0)
				{
					foreach (string p in paths)
					{
						string contents = File.ReadAllText(p);
						contents = Utils.Sort(contents, regex);
						if (Write) File.WriteAllText(p, contents);
						SendMessage("@sort {0}", p);
					}
				}
			}
		}

		private void C_IndexXml(string args)
		{
			string[] paths = GetFiles(args);
			foreach (string p in paths)
			{
				string s = File.ReadAllText(p);
				s = Utils.IndentXml(s, "\t");
				if (Write)
				{
					File.WriteAllText(p, s);
					SendMessage("@indent {0}", p);
				}
			}
		}

		private void C_EscXml(LineReader reader, string args)
		{
			string[] nv = Chop(args, 2, '=');
			string xml = SecurityElement.Escape(nv.Length == 2 ? nv[1] : ReadHereString(reader));
			SetMacro(nv[0], xml);
			SendMessage("@escxml {0}", nv[0]);
		}

		private void C_MakeXml(LineReader reader, string args)
		{
			if (string.IsNullOrEmpty(args)) throw new Exception("#makexml needs key and here-text");
			var contents = ReadHereString(reader, true);
			string xml = LightXml.ToNormalXml(contents);
			SetMacro(args, xml);
			SendMessage("@makexml {0}", args);
		}

		private bool C_If_Ifnot(LineReader reader, string args, string cmd, out string val)
		{
			string[] parts = Chop(args, 2, ' ');
			string macro = parts[0];
			string rx = parts[1];
			bool isElse;
			bool not = cmd == "ifnot";
			string text, contents;
			if (not ^ (TryGetMacro(macro, out text) && Regex.IsMatch(text, rx)))
			{
				contents = Expand(ReadConditionalBlock(reader, out isElse));
				using (var sr = new LineReader(contents, args + " (then)"))
				{
					SendMessage("@{0} ({1})->then", cmd, macro);
					val = Run(sr);
					if (val != null) return true;
				}
				if (isElse) ReadCData(reader, "#endif");
			}
			else
			{
				ReadConditionalBlock(reader, out isElse);
				if (isElse)
				{
					contents = Expand(ReadCData(reader, "#endif"));
					using (var sr = new LineReader(contents, args + " (else)"))
					{
						SendMessage("@{0} ({1})->else", cmd, macro);
						val = Run(sr);
						if (val != null) return true;
					}
				}
			}
			val = null;
			return false;
		}

		private void C_Path(LineReader reader, string args)
		{
			string key = args;
			string contents = ReadHereString(reader);
			string[] parts = contents.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			string text = Path.Combine(parts);
			SetMacro(key, text);
		}

		private string C_Mkdir(LineReader reader, string args)
		{
			args = args.Trim();
			bool force = args.StartsWith("-f");
			string key = force ? args.Substring(3).TrimStart() : args;
			string contents = ReadHereString(reader);
			string[] parts = contents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			if (Write) Directory.CreateDirectory(key);
			foreach (string part in parts)
			{
				bool copy = part.StartsWith("+");
				string text = copy ? "copy" : "move";
				var file = (copy ? part.Substring(1) : part).Trim();
				string[] paths = GetFiles(file);
				foreach (string p in paths)
				{
					string dest = Path.Combine(key, Path.GetFileName(p));
					if (force || !File.Exists(dest))
					{
						if (Write)
						{
							if (File.Exists(dest))
							{
								File.Delete(dest);
								text = "repl";
							}
							if (copy) File.Copy(p, dest);
							else File.Move(p, dest);
						}
						SendMessage("@file{0} {1}", text, dest);
					}
					else
					{
						SendMessage("@skip file{0}, already exists: {1}", text, dest);
					}
				}
			}
			return args;
		}

		private void C_Dir(string args)
		{
			string[] parts = Chop(args, 2, '=');
			string key = parts[0];
			string[] paths = GetFiles(parts[1]);
			SetMacro(key, string.Join(Environment.NewLine, paths));
		}

		private void C_Treecopy(string args)
		{
			string[] parts = Chop(args, 2, '>');
			string src = parts[0].Trim();
			string dest = parts[1].Trim();
			string[] paths = GetFiles("-r " + src + @"\*");
			string rx = null;
			string text;
			Regex regex = null;
			if (TryGetMacro("exclude", out text))
			{
				var sb = new StringBuilder();
				var sr = new StringReader(text);
				while ((text = sr.ReadLine()) != null)
				{
					sb.Append("(?:").Append(text.Trim()).Append(")|");
				}
				if (sb.Length > 1) sb.Length--;
				rx = sb.ToString();
				if (rx != "") regex = new Regex(rx, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
			}
			foreach (string p in paths)
			{
				string s = dest + p.Substring(src.Length);
				if (regex != null && regex.IsMatch(s)) continue;
				if (Write)
				{
					Directory.CreateDirectory(Path.GetDirectoryName(s));
					File.Copy(p, s, true);
				}
				SendMessage("@file {0} -> {1}{2}", p, s, Write ? "" : " test only");
			}
		}

		private void C_Rand(string args)
		{
			string[] parts = Chop(args);
			string key = parts[0];
			string fmt = parts.Length > 1 ? parts[1] : "0";
			int min = parts.Length > 2 ? int.Parse(parts[2]) : 0;
			int max = parts.Length > 3 ? 1 + int.Parse(parts[3]) : 10;
			int seed = parts.Length > 4 ? int.Parse(parts[4]) : 0;
			_NumberGenerators[key] = new RandomNumberGenerator(fmt, min, max, seed).GetEnumerator();
		}

		private void C_Seq(string args)
		{
			string[] parts = Chop(args);
			string key = parts[0];
			string fmt = parts.Length > 1 ? parts[1] : "0";
			int min = parts.Length > 2 ? int.Parse(parts[2]) : 1;
			int step = parts.Length > 3 ? int.Parse(parts[3]) : 1;
			int max = parts.Length > 4 ? int.Parse(parts[4]) : int.MaxValue;
			_NumberGenerators[key] = new SequenceNumberGenerator(fmt, min, step, max).GetEnumerator();
		}

		private void C_Email(LineReader reader, string args)
		{
			string[] ra = Chop(args, 2, ' ');
			if (ra.Length > 0)
			{
				string recip = ra[0];
				string body = ReadHereString(reader);
				string[] att = ra.Length > 1 ? ra[1].Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries) : new string[0];
				string sender;
				if (!TryGetMacro("smtp-sender", out sender))
				{
					SendMessage("@skip email: macro smtp-sender not defined");
				}
				else
				{
					var mm = new MailMessage(sender, recip, _MarkerName, body);
					foreach (string file in att)
					{
						mm.Attachments.Add(new Attachment(file));
					}
					SmtpClient client = GetSmtpClient();
					if (client == null)
					{
						SendMessage("@skip email: macro smtp-host not defined");
					}
					else
					{
						if (Write)
						{
							try
							{
								client.Send(mm);
							}
							catch
							{
								// email doesn't work, just write it out
								SendMessage("~{0}", body);
							}
						}
						SendMessage("@email " + recip);
					}
				}
			}
		}

		private void C_Trap(string args)
		{
			string[] parts = Chop(args, 2);
			string macro = parts[0];
			string rx = parts.Length > 1 ? parts[1] : null;
			_ExceptionHandlers.Add(new ExcHandler(rx, macro));
		}

		private void C_Rmdir(string args)
		{
			string[] parts = Chop(args, 2);
			if (parts.Length == 2)
			{
				string text;
				string rx = parts[0];
				string path = parts[1];
				if (Path.GetPathRoot(path) == path) throw new Exception("Will not delete from drive root " + path);
				string[] paths = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
				Array.Reverse(paths);
				var regex = new Regex(rx, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
				foreach (var d in paths.Where(p => regex.IsMatch(p)))
				{
					try
					{
						if (Write)
						{
							string[] files = Directory.GetFiles(d);
							foreach (var f in files) File.Delete(f);
							Directory.Delete(d);
							text = "deleted";
						}
						else text = "delete if write mode";
					}
					catch (Exception ex)
					{
						text = ex.Message;
					}
					SendMessage("@rmdir {0} -> {1}", d, text);
				}
				text = "delete if write mode";
				if (regex.IsMatch(path))
				{
					if (Write)
					{
						text = "deleted";
						paths = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
						foreach (var file in paths) File.Delete(file);
						try
						{
							Directory.Delete(path);
						}
						catch (Exception ex)
						{
							text = ex.Message;
						}
					}
					else text = "delete if write mode";
					SendMessage("@rmdir {0} -> {1}", path, text);
				}
			}
		}

		private void C_Del(string args)
		{
			string[] paths = GetFiles(args);
			if (paths.Length == 0)
			{
				SendMessage("@skip del {0}", args);
			}
			else
			{
				foreach (string p in paths)
				{
					if (Write)
					{
						try
						{
							File.Delete(p);
						}
						catch
						{
						}
					}
					SendMessage("@del {0}", p);
				}
			}
		}

		private void C_Output(LineReader reader, string args, string cmd, string rawArgs)
		{
			// info mbox warn write
			string text = (rawArgs == string.Empty) ? ReadHereString(reader) : args;
			if (cmd == "write" || cmd == "print") SendMessage("~{0}", text);
			else SendMessage("@{0} {1}", cmd, text);
		}

		private void C_Printv(string args)
		{
			if (string.IsNullOrEmpty(args)) throw new ArgumentException("#printv needs 1 macro argument");
			string contents;
			if (TryGetMacro(args, out contents))
			{
				SendMessage("~{0}", contents);
			}
		}

		private void C_Recipe(string args)
		{
			string[] parts = Chop(args, 2, '=');
			string key, path, contents, val;
			if (parts.Length == 2)
			{
				key = parts[0];
				path = parts[1];
			}
			else
			{
				key = null;
				path = parts[0];
			}
			val = null;
			if (TryGetMacro(path, out contents))
			{
				using (var sr = new LineReader(contents, name: args))
				{
					SendMessage("@recipe {0}", path);
					val = Run(sr);
				}
			}
			else if (File.Exists(path))
			{
				val = Subroutine(path);
			}
			else
			{
				SendMessage("@skip recipe \"{0}\" -> not a file or macro", args);
			}
			if (val != null)
			{
				if (key != null) SetMacro(key, val);
				else key = "";
				SendMessage("@retval {0}={1}", key, Truncate(val, 70));
			}
		}

		private void C_Stop_Assert(string args, string cmd)
		{
			if (args.Trim() == "" && cmd == "stop")
			{
				SendMessage("@stop");
				_Run = false;
			}
			else // #stop macro regex  or  #assert macro regex
			{
				string[] parts = Chop(args, 2, ' ');
				string macro = parts[0];
				string rx = parts[1];
				bool assert = cmd == "assert";
				string text;
				if (TryGetMacro(macro, out text) && assert ^ Regex.IsMatch(text, rx))
				{
					SendMessage(assert ? "@assert {0} did not match {1}" : "@stop {0} matches {1}", macro, rx);
					_Run = false;
				}
				else SendMessage("@{0} -> skip", cmd);
			}
		}

		private void C_Exit()
		{
			if (Write) SendMessage("@exit");
			else
			{
				SendMessage("@skip exit (test mode)");
				_Run = false;
			}
		}

		private void C_CallExternal(LineReader reader, string args, string cmd)
		{
			if (cmd == "xcopy")
			{
				args = "{xcopy} " + args;
				cmd = "cmd";
			}

			string fmt = "@{0} {1} -> {2}";
			string key, script, text;
			string[] nv;
			if (cmd == "ps" && args.Contains("="))
			{
				nv = Chop(args, 2, '=');
				key = nv[0];
				script = nv[1] + " | Out-String";
				int exitCode = RunPowerShell(script, out text);
				SetMacro("exitcode", exitCode.ToString());
				if (key != string.Empty) SetMacro(key, text);
				SendMessage(fmt, "run here", script, exitCode);
			}
			else if ((cmd == "call" || cmd == "cmd" || cmd == "ps") && !args.Contains("{"))
			{
				script = ReadHereString(reader);
				int exitCode = cmd == "ps" ? RunPowerShell(script, out text) : RunShell(script, out text);
				SetMacro("exitcode", exitCode.ToString());
				key = args;
				if (key != string.Empty) SetMacro(key, text);
				SendMessage(fmt, "run here", script, exitCode);
			}
			else if (cmd == "git")
			{
				if (args.Contains("="))
				{
					nv = Chop(args, 2, '=');
					key = nv[0];
					script = "git.exe " + nv[1]; //"C:\Program Files (x86)\Git\cmd\"
				}
				else
				{
					key = args;
					script = ReadHereString(reader);
					string[] parts = script.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Select(s => "git.exe " + s).ToArray();
					script = string.Join(Environment.NewLine, parts);
				}
				string macro = GetMacro("repo");
				if (macro == "")
				{
					SendMessage("@git -> unknown repo, use #put repo=<drive>:<folder>");
					SetMacro("exitcode", "test only");
					throw new Exception("git repo name missing");
				}
				script = "cd \"" + macro + "\"" + Environment.NewLine + script;
				if (macro.Length > 1 && macro[1] == ':') script = macro.Substring(0, 2) + Environment.NewLine + script;
				int exitCode = RunShell(script, out text);
				SetMacro("exitcode", exitCode.ToString());
				text = Regex.Replace(text, @"(?<!\r)\n", "\r\n");
				if (key != string.Empty) SetMacro(key, text);
				SendMessage(fmt, "run git: ", script, exitCode);
			}
			else
			{
				var match = Regex.Match(args, @"\{([^}]+)\}\s*(.*)$"); // #cmd/#run {program} arg1 arg2 arg3 ...
				string prog = match.Groups[1].Value;
				string progArgs = match.Groups[2].Value;
				Process p = Process.Start(prog, progArgs);
				if (cmd == "run")
				{
					SendMessage(fmt, cmd, args, "async");
				}
				else if (p != null)
				{
					p.WaitForExit();
					SetMacro("exitcode", p.ExitCode.ToString());
					SendMessage(fmt, cmd, args, p.ExitCode);
				}
			}
		}

		private void C_Require(string args)
		{
			string[] nc = Chop(args, 2, ' ', '\t');
			string key = nc[0];
			if (!_Macros.ContainsKey(key))
			{
				string msg = string.Format("Required macro " + args);
				throw new Exception(msg);
			}
		}

		private void C_Iter(LineReader reader, string args)
		{
			string[] parts = Chop(args, 3, ' ');
			string key, text, contents;
			if (parts.Length >= 3)
			{
				// #iter list recipe i
				key = parts[2];
				text = GetMacro(parts[1]); // recipe
				contents = GetMacro(parts[0]);
				SetMacro(key, contents);
			}
			else if (parts.Length == 2)
			{
				// #putv i=list
				// #iter i recipe
				key = parts[0];
				text = GetMacro(parts[1]); // recipe
				contents = GetMacro(key);
			}
			else
			{
				// #iter list 
				// ... `_` ... 
				// #next
				key = "_";
				text = ReadCData(reader, "#next"); // recipe
				contents = GetMacro(parts[0]);
			}
			string[] items = contents.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			string itemKey = key;
			int n = 0;
			foreach (string item in items)
			{
				n++;
				SendMessage("@iter {0} = {1}", key, item);
				using (var sr = new LineReader(text, string.Format("{0} #{1}", args, n)))
				{
					_Macros[itemKey] = item;
					if (Run(sr) == null && !_Run) break;
				}
			}
			_Macros.Remove(itemKey);
		}

		private void C_Expn(LineReader reader, string args)
		{
			/* compare #expn vs. #with template
				#put a
				one|two|three
				four|five|six
				#end

				#with a
				template insert $1 into $2 yield $3
				#end

				#info `a`

				---

				#cdata b
				insert `1` into `2` yield `3`...

				#end b


				#expn b
				one|two|three
				four|five|six
				#end

				#info `b`
			 */
			string key = args;
			string text;
			if (TryGetMacro(key, out text))
			{
				string contents = ReadHereString(reader, true);
				var sb = new StringBuilder();
				string[] parts = contents.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string s in new TemplateExpander(text, parts, Put, Expand).Instances())
				{
					sb.Append(s);
				}
				SetMacro(key, sb.ToString());
			}
		}

		private void C_Exp(string args)
		{
			string key = args;
			string contents;
			if (TryGetMacro(key, out contents))
			{
				contents = Expand(contents);
				SetMacro(key, contents);
				SendKeyValueMessage(key, contents);
			}
		}

		private void C_CData(LineReader reader, string args)
		{
			string key = args;
			string contents = ReadCData(reader, "#end " + key);
			SetMacro(key, contents);
			SendKeyValueMessage(key, contents);
		}

		private void C_Ask(LineReader reader, string args)
		{
			string[] nv = Chop(args, 2, '=');
			string key = nv[0];
			string text;
			if (!TryGetMacro(key, out text)) text = "";
			if (Interactive)
			{
				if (nv.Length > 1)
				{
					text = Input(nv[1], true, text);
				}
				else
				{
					string contents = ReadHereString(reader);
					string[] lines = contents.Split(new[] { "\r\n" }, StringSplitOptions.None);
					var sb = new StringBuilder();
					int i;
					for (i = 0; i < lines.Length; i++)
					{
						string s = lines[i];
						if (s == "-") break;
						sb.AppendLine(s);
					}
					i++;
					var parts = new string[lines.Length - i];
					Array.Copy(lines, i, parts, 0, parts.Length);
					text = Input(sb.ToString(), true, text, parts);
				}
				//Cancel->null, OK without selection -> ""
				if (text == null)
				{
					SendMessage("@ask -> cancel");
					_Run = false;
				}
				else
				{
					SetMacro(key, text);
					SendKeyValueMessage(key, text);
				}
			}
			else if (nv.Length == 1) ReadCData(reader, "#end");
		}

		private void C_Putm(string args)
		{
			string contents;
			if (TryGetMacro(args, out contents))
			{
				Import(contents);
			}
			else
			{
				foreach (string p in GetFiles(args))
				{
					contents = File.ReadAllText(p);
					Import(contents);
				}
			}
		}

		private void C_Put_Putx_Putv_Default_Load(LineReader reader, string args, string cmd)
		{
			string[] nv = Chop(args, 2, '=');
			string key = nv[0];
			string contents;
			if (nv.Length == 2)
			{
				if (cmd == "load")
				{
					if (Regex.IsMatch(nv[1], @"^https?://[^/]+/?", RegexOptions.IgnoreCase))
					{
						SendKeyValueMessage(key, SetMacro(key, new WebClient().DownloadString(nv[1])));
					}
					else
					{
						string[] paths = GetFiles(nv[1]);
						if (paths.Length != 0)
						{
							var sb = new StringBuilder();
							foreach (var p in paths)
							{
								SetFile(p);
								contents = File.ReadAllText(p);
								sb.AppendLine(contents);
							}
							if (sb.Length > 2) sb.Length -= 2;
							SendKeyValueMessage(key, SetMacro(key, sb.ToString()));
						}
						else
						{
							SendMessage("@skip {0} {1}", cmd, nv[1]);
						}
					}
				}
				else if (cmd == "!" || cmd.StartsWith("put") || !_Macros.ContainsKey(key))
				{
					string val = nv[1];
					if (cmd == "putv" || cmd == "putx")
					{
						string val2;
						val = TryGetMacro(val, out val2) ? val2 : string.Empty;
					}
					if (cmd == "putx") val = Expand(val);
					SetMacro(key, val);
					SendKeyValueMessage(key, val);
				}
			}
			else if (nv.Length == 1 && cmd != "putv" && cmd != "putx") // nv.Length == 1
			{
				contents = ReadHereString(reader);
				if (cmd == "default" && _Macros.ContainsKey(key)) return;
				SetMacro(key, contents);
				SendKeyValueMessage(key, contents);
			}

		}

		private void C_Extract(LineReader reader, string args, string cmd)
		{
			string rx, macro;
			string[] paths;
			if (GetRxLine(args, out rx, out paths, out macro))
			{
				string text, contents;
				if (macro != null && TryGetMacro(macro, out text))
				{
					contents = ReadHereString(reader);
					text = Replace(contents, text, rx);
					_Macros[macro] = text;
					SendKeyValueMessage(macro, text);
				}
				else
				{
					if (paths.Length == 0)
					{
						ReadHereString(reader, false);
						SendMessage("@skip {0} {1}", cmd, args);
					}
					else
					{
						contents = ReadHereString(reader, false);
						foreach (string p in paths)
						{
							SetFile(p);
							string inst = Expand(contents);
							text = File.ReadAllText(p);
							text = Replace(inst, text, rx);
							if (Write)
							{
								File.WriteAllText(p, text);
							}
							SendMessage("@{0} {1}", cmd, p);
						}
					}
				}
			}
			else
			{
				ReadHereString(reader, false);
				SendMessage("@nomatch {0}: {1}", cmd, args);
			}
		}

		private void C_Map(string args)
		{
			string[] parts = Chop(args, 2, ' '); // <dictionary> <macro> _or_  <dictionary> <paths>
			if (parts.Length > 1)
			{
				string key = parts[0]; //dict
				string contents;
				if (TryGetMacro(key, out contents))
				{
					var map = new Dictionary<string, string>();
					foreach (string s in contents.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
					{
						int p = s.IndexOf('|');
						if (p == -1) continue;
						map[s.Substring(0, p)] = s.Substring(p + 1);
					}
					string macro = parts[1];
					string text;
					if (TryGetMacro(macro, out text))
					{
						text = Replace(map, text);
						SetMacro(macro, text);
					}
					else
					{
						string[] paths = GetFiles(parts[1]);
						foreach (string p in paths)
						{
							text = File.ReadAllText(p);
							text = Replace(map, text);
							if (Write) File.WriteAllText(p, text);
							SendMessage("@map " + p);
						}
					}
				}
			}
		}

		private void C_Before_Subs_After(LineReader reader, string args, string cmd)
		{
			string rx, macro;
			string[] paths;
			if (GetRxLine(args, out rx, out paths, out macro))
			{
				string text;
				string contents;
				if (macro != null && TryGetMacro(macro, out text))
				{
					contents = ReadHereString(reader);
					string repl =
						cmd == "subs"
							? contents
							: cmd == "before" ? contents + "$0" : "$0" + contents;
					text = Regex.Replace(text, rx, repl);
					_Macros[macro] = text;
					SendKeyValueMessage(macro, text);
				}
				else
				{
					if (paths.Length == 0)
					{
						ReadHereString(reader, false);
						SendMessage("@skip {0} {1}", cmd, args);
					}
					else
					{
						contents = ReadHereString(reader, false);
						foreach (string p in paths)
						{
							SetFile(p);
							string inst = Expand(Generate(contents));
							string repl =
								cmd == "subs"
									? inst
									: cmd == "before" ? inst + "$0" : "$0" + inst;

							text = File.ReadAllText(p);
							var text1 = Regex.Replace(text, rx, repl);
							if (Write && text != text1)
							{
								File.WriteAllText(p, text1);
								SendMessage("@{0} {1}", cmd, p);
							}
						}
					}
				}
			}
			else
			{
				ReadHereString(reader, false);
				SendMessage("@nomatch {0}: {1}", cmd, args);
			}
		}

		private void C_Append(LineReader reader, string args)
		{
			string[] parts = Chop(args);
			if (parts[0] != "-r" && parts.Length >= 2)
			{
				string key = parts[0];
				var sb = new StringBuilder();
				string contents;
				if (TryGetMacro(key, out contents))
				{
					sb.Append(contents);
				}
				for (int i = 1; i < parts.Length; i++)
				{
					string val = parts[i];
					sb.AppendLine().Append(GetMacro(val));
				}
				SetMacro(key, sb.ToString());
				SendMessage("@append {0}", args);
			}
			else
			{
				var append = ReadHereString(reader, false);
				string key = args;
				bool ok = false;
				string contents;
				if (TryGetMacro(key, out contents))
				{
					SetMacro(key, contents + Expand(append));
					ok = true;
				}
				else
				{
					string[] paths = GetFiles(args);
					if (paths.Length != 0)
					{
						append = Expand(append);
						foreach (var p in paths)
						{
							SetFile(p);
							if (Write) File.AppendAllText(p, Generate(append));
							ok = true;
						}
					}
				}
				if (ok) SendMessage("@append {0}", args);
				else SendMessage("@skip append {0}");
			}
		}

		private void C_New_Newer(LineReader reader, string args, string cmd)
		{
			string path;
			SetFile(path = args);
			string contents = ReadHereString(reader);
			string directory = Path.GetDirectoryName(path);
			if (string.IsNullOrEmpty(directory))
			{
				SendMessage("@skip new {0} -> directory missing", path);
			}
			else
			{
				if (!Directory.Exists(directory))
				{
					if (Write) Directory.CreateDirectory(directory);
					SendMessage("@new {0}", directory);
				}
				if (cmd == "newer" || !File.Exists(path))
				{
					if (Write) File.WriteAllText(path, contents);
					SendMessage("@{0} {1}", cmd, path);
				}
				else
				{
					SendMessage("@skip new {0}", path);
				}
			}
		}

		private void C_Do(string args)
		{
			string[] paths = GetFiles(args);
			if (paths.Length == 0)
			{
				SendMessage("@skip do {0}", args);
			}
			else
			{
				foreach (string p in paths)
				{
					SetFile(p);
					string pattern = GetPattern(p);
					string contents = File.ReadAllText(p);
					contents = Regex.Replace(contents, pattern,
											 m => Expand(m.Groups[1].Value) + m.Groups[0].Value,
											 RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

					if (Write)
					{
						File.WriteAllText(p, contents);
					}
					SendMessage("@do {0}", p);
				}
			}
		}

		private void C_Esc(LineReader reader, string args)
		{
			string[] nv = Chop(args, 2, '=');
			string rx = Regex.Escape(nv.Length == 2 ? nv[1] : ReadHereString(reader));
			SetMacro(nv[0], rx);
			SendMessage("@esc {0}", rx);
		}

		private void C_Line(LineReader reader)
		{
			SendMessage(GetLine(reader));
		}


	}
}
