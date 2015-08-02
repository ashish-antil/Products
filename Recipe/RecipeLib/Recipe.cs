using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management.Automation.Runspaces;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using RockBottom.CommunicationLib;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Linq;
using System.Reflection;
using System.Globalization;

namespace RecipeLib
{
	public partial class Recipe : ICloneable, IScriptContext
	{
		public const int DefaultPort = 7711;
		private readonly string[] _LineBrk = new[] { Environment.NewLine };
		private static readonly Dictionary<string, string> _GlobalMacros = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private readonly Stack<string> _CallStack;
		private Dictionary<string, string> _Macros;
		private Dictionary<string, string> _AltMacros;
		private List<ExcHandler> _ExceptionHandlers;
		private bool _Handling;
		private readonly Dictionary<string, IEnumerator<string>> _NumberGenerators;
		private readonly Dictionary<string, CustomCommand> _CustomCommands;
		private string _MarkerName = "Recipe";
		private bool _Run;
		private SmtpClient _SmtpClient;

		private Watcher _Watcher;
		private RecipeServer _Server;

		/// <summary>
		/// Constructor.
		/// </summary>
		public Recipe()
		{
			_CallStack = new Stack<string>();
			_NumberGenerators = new Dictionary<string, IEnumerator<string>>(StringComparer.OrdinalIgnoreCase);
			_CustomCommands = new Dictionary<string, CustomCommand>();
		}

		public static Dictionary<string, string> GlobalMacros { get { return _GlobalMacros; } }

		public static string Version
		{
			get
			{
				Version version = Assembly.GetExecutingAssembly().GetName().Version;
				return "Version " + version;
			}
		}

		public object Clone()
		{
			var r = new Recipe();
			r.Write = Write;
			r.Message = Message;
			r._Macros = CopyMacros(_Macros);
			r._ExceptionHandlers = _ExceptionHandlers.ToList();
			return r;
		}

		private static Dictionary<string, string> CopyMacros(IDictionary<string, string> source)
		{
			var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			foreach (string key in source.Keys)
			{
				d[key] = source[key];
			}
			return d;
		}

		public bool Write { get; set; }
		public bool Log { get; set; }
		public bool Interactive { get; set; }
		public bool Warn { get; set; }
		public bool Show { get; set; }

		public event Action<string> Message;

		public delegate string InputMethod(string prompt, bool multiline, string initial, params string[] options);

		public InputMethod Input { get; set; }

		public void SetRootPath(string path)
		{
			_CallStack.Clear();
			_CallStack.Push(path.ToLowerInvariant());
		}

		public void SendMessage(string s, params object[] args)
		{
			if (Message != null)
			{
				if (args.Length == 0)
				{
					Message(s);
				}
				else
				{
					Message(string.Format(s, args));
				}
			}
		}

		public void Stop()
		{
			_Run = false;
		}

		private string Subroutine(string path)
		{
			string returnValue = null;
			if (File.Exists(path))
			{
				SendMessage("@recipe {0}", path);
				if (_CallStack.Contains(path.ToLowerInvariant()))
				{
					SendMessage("@skip recipe {0} -> recursion not allowed", path);
				}
				else
				{
					_CallStack.Push(path.ToLowerInvariant());
					LineReader sr = null;
					try
					{
						sr = new LineReader(File.OpenText(path), name: path);
						returnValue = Run(sr);
					}
					finally
					{
						if (sr != null) sr.Close();
						_CallStack.Pop();
					}
					SendMessage("@return {0}", path);
				}
			}
			else
			{
				SendMessage("@skip recipe {0}", path);
			}
			return returnValue;
		}

		public void ClearHandlers()
		{
			_ExceptionHandlers = new List<ExcHandler>();
		}

		#region Command Handling

		public string Run(LineReader reader)
		{
			string paste = GetMacro("paste");
			return Run(reader, _Macros, paste);
		}

		/// <summary>
		/// Run an entire recipe. Caller should close reader.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="macros"></param>
		/// <param name="paste">clipboard text, or empty string if none</param>
		/// <returns>value of #return or null</returns>
		public string Run(LineReader reader, Dictionary<string, string> macros, string paste)
		{
			string returnValue = null;
			_Macros = macros;
			// special macros:
			macros[" "] = macros["empty"] = "";
			macros["nl"] = "\r\n";
			macros["tab"] = "\t";
			macros["sp"] = " ";
			macros["bq"] = "`";
			macros["version"] = Version;
			if (paste != null)
			{
				macros["paste"] = paste;
			}

			string line = "";
			try
			{
				_Run = true;
				returnValue = ProcessStream(reader, ref line);
				//SendMessage("@eor");
			}
			catch (Exception ex)
			{
				if (_Handling)
				{
					_Run = false;
					SendMessage("@err exception in handler; stop execution");
				}
				else
				{
					SetMacro("errormsg", ex.Message);
					string code = HandleException(ex.Message);
					switch (code)
					{
						case "stop":
							_Run = false;
							SendMessage("@stop");
							break;
						case "up":
							_Run = false;
							return null;
						default:
							var ex1 = ex.InnerException ?? ex;
							SendMessage("@fail {0} in {1}: {2} -> {3}", reader.LineNumber, reader.Name, line, ex1.Message);
							if (ex.InnerException == null) throw; else throw ex1;
					}
				}
			}
			return returnValue;
		}

		private string GetLine(LineReader reader)
		{
			return string.Format("@line {0} of {1}: {2}", reader.LineNumber, reader.Name, Truncate(reader.Line, 80));
		}

		private string HandleException(string msg)
		{
			ExcHandler handler = _ExceptionHandlers.Find(eh => eh.IsMatch(msg));
			if (handler != null)
			{
				string recipe;
				if (TryGetMacro(handler.Macro, out recipe))
				{
					using (var reader2 = new LineReader(recipe, name: handler.Macro))
					{
						_Handling = true;
						try
						{
							return Run(reader2);
						}
						finally
						{
							_Handling = false;
						}
					}
				}
			}
			return null;
		}

		private string ProcessStream(LineReader reader, ref string line)
		{
			while (_Run && (line = reader.ReadLine()) != null)
			{
				if (line.StartsWith("#"))
				{
					string[] parts = Chop(line, 2, ' ');
					string cmd = parts[0].Substring(1).TrimEnd();
					if (cmd.Contains('`')) cmd = Expand(cmd);
					string path = parts.Length > 1 ? parts[1].Trim() : string.Empty;
					var returnValue = DoCommand(reader, cmd, path);
					if (returnValue != null) return returnValue;
				}
			}
			GC.Collect();
			return null;
		}

		//private void RemoveLocals()
		//{
		//	var locals = _Macros.Keys.Where(key => key.Length > 0 && key[0] == ':').ToArray();
		//	foreach (string key in locals) _Macros.Remove(key);
		//}

		/// <summary>
		/// Execute a single command.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="cmd"></param>
		/// <param name="args"></param>
		/// <returns>args of #return, or null if no #return</returns>
		private string DoCommand(LineReader reader, string cmd, string rawArgs)
		{
			SendMessage("Cmd: {0}", cmd);
			SetFile("");
			//int min, max, step;
			//string contents, val;
			//string key, rx, text, macro, fmt, script, dest;
			//bool ok;
			//string[] paths, parts;
			//string[] nv;
			//Match match;
			//StringBuilder sb;
			//Regex regex;

			string args = Expand(rawArgs);

			switch (cmd) //_COMMANDS_BEGIN_
			{
				/*
				 * case "tags": // one ore more tags for the RecipeGUI File>Search #tags function
				 * case "auto": // first line in recipe: autorun recipe (dclick in Explorer, if associated .rcp extension)
				 */
				case "do": // modify file, expand macros:  #do [-r ]path\pattern
					C_Do(args);
					break;

				case "new": // create file if new and copy following lines upto #end into it
				case "newer": // create or replace file and copy following lines upto #end into it
					C_New_Newer(reader, args, cmd);
					break;

				case "append": // add to the end of a macro or files: #append m m1 m2...; #append macro ... #end; #append [-r ]path\pattern ... #end
					C_Append(reader, args);
					break;

				case "before": // insert before regex in macro or files: #before rx [-r ]path\pattern ... #end
				case "after": // insert after regex in macro or files: #after rx [-r ]path\pattern ... #end
				case "subs": // substitute regex in macro or files: #subs rx [-r ]path\pattern ... #end
					C_Before_Subs_After(reader, args, cmd);
					break;

				case "map": // apply dictionary to macro or files; dictionary is macro with key|value lines: #map dict [-r ]path\pattern; #map dict m
					C_Map(args);
					break;

				case "extract": // find regex in macro or files and generate here-text for each match in the file or macro: #extract rx (path|macro)... #end
				case "find": // obsolete, use #extract
					C_Extract(reader, args, cmd);
					break;

				case "marker": // marker name to search for when using #do
				case "name": // obsolete, alias for #marker
					_MarkerName = args;
					SendMessage("@name {0}", _MarkerName);
					break;

				case "!": // alias for #put
				case "put": // put macro in dictionary: #put a=macro; #put a ... #end
				case "default": // put macro in dictionary only if not exists
				case "putv": // put macro in dictionary, treat value as a lookup key and put the value of that one in dict
				case "putx": // like putv, but expand the value before storing
				case "load": // load content of file or webpage and store in dictionary
					C_Put_Putx_Putv_Default_Load(reader, args, cmd);
					break;

				case "putm": // #putm macro: the macro contains lines like key=value; put these in the dictionary
					C_Putm(args);
					break;

				case "ask": // ask user for input; input is text or multiple choice #ask a=question; #ask a/q/-/opt1|text1/opt2|text2/.../#end
					C_Ask(reader, args);
					break;

				case "@": // alias for #cdata
				case "cdata": // treat value as unparsed character data and store in dictionary, #cdata macro ... #end macro
					C_CData(reader, args);
					break;

				case "exp": // expand macros inside given macro: #exp template
					C_Exp(args);
					break;

				case "expn": // expand the arg template for each line in the here-text, which contains key1=val1|key2=val2|...
					C_Expn(reader, args);
					break;

				case "iter": // exec a recipe passing each line as arg: 1. #iter list recipe; 2. #iter list // recipe using `_` to represent line // #next
					C_Iter(reader, args);
					break;

				case "copy": // copy to clipboard: #copy text; #copy ... #end
					string contents = string.IsNullOrEmpty(args) ? ReadHereString(reader) : args;
					SendMessage("@clip " + contents);
					break;

				case "require": // throw exception if macro does not exist
					C_Require(args);
					break;

				case "xcopy": // identical to #cmd {xcopy} args...
				case "run": // run a program, don't wait for it to finish: #run {path} args
				case "cmd": // run a program, wait until it finishes; or run here-text in cmd shell; assign output: #cmd a ... #end
				case "call": // obsolete, alias for cmd
				case "ps": // run a powershell program, wait till it finishes; assign output; #ps a=dir|get-member;  #ps a ... #end
				case "git": // run git commands, assign output to first arg: #put repo=...; #git a=status; #git a ... #end; `repo` is required path
					C_CallExternal(reader, args, cmd);
					break;

				case "exit": // exit recipe tool
					C_Exit();
					break;

				case "return": // return to caller, optionally pass a value: #return `val`
					return args;

				case "stop": // stop execution of recipe stack, do not exit GUI tool; optionally provide stop condition: #stop macro regex
				case "assert": // #assert macro regex: stop execution of recipe stack if condition false
					C_Stop_Assert(args, cmd);
					break;

				case "fatal": // raise a fatal error; this will always result in stopping the recipe stack regardless of the #return in the #trap
				case "error": // raise an error; the arg will be stored in macro `errormsg` and execute will proceed in a handler registerd by #trap
					SendMessage("@err {0}", args);
					SetMacro("errormsg", args);
					string key = HandleException(args);
					if (cmd == "fatal") key = "stop";
					switch (key)
					{
						case "internal":
						case "stop":
							_Run = false;
							SendMessage("@stop");
							break;
						case "next":
							break;
						case "up":
						default:
							return args;
					}
					break;

				case "#": // alias for #recipe
				case "recipe": // call given recipe (macro or file) and optionally assign return value: #recipe macro; #recipe retval=path;
					C_Recipe(args);
					break;


				case ".": // alias for #info
				case "info": // print text in log or console: #info The answer = `a`
				case "mbox": // pop up text in box
				case "warn": // pop up warning and ask whether to continue or cancel execution
				case "write": // obsolete, use print
				case "print": // write text to output; #print this line; #print ... #end
					C_Output(reader, args, cmd, rawArgs);
					break;

				case "printv": // print the value of the given macro: #printv macro; for use inside #if, #for, #iter, etc.
					C_Printv(args);
					break;

				case "del": // delete file:  #del [-r ]path\pattern
					C_Del(args);
					break;

				case "rmdir": // remove folders: #rmdir regex rootdir
					C_Rmdir(args);
					break;

				case "trap": // #trap rcp rx; when the `errormsg` matches rx then exec rcp; #return stop|up|next in the rcp, ignored by #fatal
					C_Trap(args);
					break;

				case "email": // send email, requires `smtp-sender`, `smtp-host`
					C_Email(reader, args);
					break;

				case "seq": // define a sequence number generator: #seq key fmt min step max
					C_Seq(args);
					break;

				case "rand": // define a random number generator: #rand key fmt min max seed
					C_Rand(args);
					break;

				case "treecopy": // #treecopy source-dir > dest-dir; copies entire folder structure
					C_Treecopy(args);
					break;

				case "dir": // #dir m=[-r ]path\pattern: get files in directory, assign to macro
					C_Dir(args);
					break;

				case "mkdir": // #mkdir path ... #end: create folder +copy/move files; #mkdir -f path -> force overwrite unless readonly
					args = C_Mkdir(reader, args);
					break;

				case "path": // #path result // path parts to be combined... // #end
					C_Path(reader, args);
					break;

				case "if": // if macro regex // else // endif
				case "ifnot": // ifnot macro regex // else // endif
					string val;
					if (C_If_Ifnot(reader, args, cmd, out val)) return val;
					break;

				case "indentxml": // #indentxml [-r] path\*.ext
					C_IndexXml(args);
					break;

				case "makexml": // make xml from a lite-xml syntax that uses tabs for indent and omits < /> '...'
					C_MakeXml(reader, args);
					break;

				case "sort": // #sort rx macro-or-files
					C_Sort(args);
					break;

				case "repeat": // #repeat macro n
					C_Repeat(args);
					break;

				case "edit": // put contents of macro in editor window
					C_Edit(args);
					break;

				case "unique": // skip lines that already have the regex
					C_Unique(args);
					break;

				case "$": // alias for #with (think $ for $tring manipulation $cript)
				case "with": // apply script operations on the macro or files, #with a ... #end or #with a=(single oper)
					C_With(reader, args);
					break;

				case "pause": // sleep for given milliseconds
					C_Pause(args);
					break;

				case "udp": // send here-text as udp to addr port
					C_Udp(reader, args);
					break;

				case "post": // http post of here-text to given server
					C_Post(reader, args);
					break;

				case "server": // server mode: run recipe commands for each incoming http request
					C_Write(reader, args);
					break;

				case "watch": // file system watcher
					C_Watch(reader, args);
					break;

				case "forget": // remove macro from dictionary
					RemoveMacro(args);
					break;

				case "point": // obsolete, use #stash
				case "stash": // restore point: remember all the macros
					_AltMacros = CopyMacros(_Macros);
					break;

				case "restore": // restore all macros remembered with #stash
					if (_AltMacros != null) _Macros = CopyMacros(_AltMacros);
					break;

				case "encrypt": // encrypt text in macro with given key
				case "decrypt": // decrypt text in macro with given key
					C_Encrypt_Decrypt(args, cmd);
					break;

				case "union": // #union m=m1 m2 m3... -  union of lines, remove duplicates
				case "intersect": // #intersect m=m1 m2 m3... -  intersection of lines
				case "subtract": // #subtract m=m1 m2 m3... -  subtraction of lines
					C_SetOperations(args, cmd);
					break;

				case "product": // #product m=m1 m2 - carthesian product of two lists
					C_Product(args);
					break;

				case "version": // stop execution if program version mismatch; e.g. #version 1.5+, #version 1.5-, #version 1.5; note: +(>=) -(<)
					C_Version(args);
					break;

				case "exists": // checks if an item exists as 'macro', 'file' or 'dir' -> these values get assigned to the variable, '' if not found
					C_Exists(args);
					break;

				case "csharp": // compile and execute the here-text as the body of a C# method. Assign result string to given key. #csharp key ... #end
					C_Csharp(reader, args);
					break;

				case "esc": // escape a string for use in a Regex search: #esc a=(hello...)  -> \(hello\.\.\.\)
					C_Esc(reader, args);
					break;

				case "escxml": // escape xml or html string, e.g. & -> &amp;  ' -> &apos;
					C_EscXml(reader, args);
					break;

				case "mode": // #mode write -> changes to write mode, #mode test -> changes to test mode
					C_Mode(args);
					break;

				case "for": // #for format min step max .. #next: for-loop using `_` as loop variable e.g. #for 000 1 1 10 // #write `_` // #next
					C_For(reader, args);
					break;

				case "while": // #while macro regex .. #loop, keep looping while macro matches regex
				case "until": // #until macro regex .. #loop, keep looping until macro matches regex
					C_While_Until(reader, args, cmd);
					break;

				case "date": // date format and time zone conversions. #date rx macro/paths // parse-fmt // tz-from // tz-to // out-fmt // #end
					C_Date(reader, args);
					break;

				case "def": // define custom command, #def cmd-name [arg1 arg2 ...] .. #enddef, invoke by #cmd-name retval // val1 // val2 ... #end
					C_Def(reader, args);
					break;

				case "wstart": // start a windows service, uses `wtimeout` in seconds e.g. #wstart Recipe Service
				case "wstop": // stop a windows service
				case "wrestart": // restart a windows service
					C_Wstart_Wstop_Wrestart(args, cmd);
					break;

				case "count": // count lines/chars in macro, bytes/kb/lines in file, files in folder, e.g. #count n=c:\temp *.txt, #count n=c:\temp\a.txt lines
					C_Count(args);
					break;

				case "line": // print line number and scope name and any comment for debugging;  #line Delete files...
					C_Line(reader);
					break;

				//_COMMANDS_END_

				case "end":
					throw new Exception("orphan #end");

				default:
					// Custom commands, e.g.  #mymacro retval // arg1 // arg2 #end
					C_Default(reader, args, cmd);
					break;
			}
			return null;
		}


		private void Import(string contents)
		{
			foreach (string s in contents.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
			{
				int p = s.IndexOf('=');
				if (p == -1) continue;
				SetMacro(s.Substring(0, p).Trim(), s.Substring(p + 1));
			}
		}

		private string ExecuteCustomCommand(CustomCommand cc, string argLines)
		{
			string[] args = argLines.Split(new string[] { "\r\n" }, StringSplitOptions.None);
			int n = Math.Min(args.Length, cc.Args.Length);
			for (int i = 0; i < n; i++)
			{
				string key = cc.Args[i];
				string val = args[i];
				_Macros[key] = val;
			}
			for (int i = n; i < cc.Args.Length; i++)
			{
				_Macros.Remove(cc.Args[i]);
			}
			using (var sw = new LineReader(cc.Text, name: cc.Name))
			{
				return Run(sw);
			}
		}

		/// <summary>
		/// Check the required version of the tool as stated in the script against the actual assembly version.
		/// The script version can require a newer assembly by a plus sign suffix, and an older assembly by a minus sign suffix.
		/// A + sign means greater or equal, whereas a - sign means less than. Without suffix the exact version down to the revision is required.
		/// At least the major script version has to be given, but the minor, build and revision are optional.
		/// </summary>
		/// <param name="scriptVersion">e.g. 1.5+  2-  2.7.8832.35129-   1.9.54.15030.</param>
		/// <param name="assemblyVersion">the version of the assembly that is tested</param>
		/// <returns>true if the scriptVersion condition applies, false if syntax wrong or condition does not apply</returns>
		private static bool VersionOK(string scriptVersion, Version assemblyVersion)
		{
			var rx = new Regex(@"(\d+(?:\.\d+){0,3})([+-])?", RegexOptions.Compiled | RegexOptions.CultureInvariant);
			var m = rx.Match(scriptVersion);
			if (m.Success)
			{
				string s = m.Groups[1].Value;
				if (!s.Contains('.')) s += ".0"; // Version constructor requires at least a major and a minor version
				var vscript = new Version(s);
				string sign = m.Groups[2].Value;
				int cmp = assemblyVersion.CompareTo(vscript); // <0 tool is older than recipe version, >0 tool is newer
				if (cmp < 0 && sign == "-") return true;
				if (cmp >= 0 && sign == "+") return true;
				if (cmp == 0 && sign == "") return true;
			}
			return false;
		}

		#endregion Command Handling

		public void StopServer()
		{
			if (_Server != null)
			{
				_Server.Stop();
				_Server = null;
			}
			GC.Collect();
		}

		public void StopWatcher()
		{
			if (_Watcher != null)
			{
				_Watcher.Stop();
				_Watcher = null;
			}
		}

		private string[] Chop(string args)
		{
			return Chop(args, int.MaxValue, ' ', '\t');
		}

		private string[] Chop(string args, int n, params char[] sep)
		{
			return args.Split(sep, n);
		}

		private SmtpClient GetSmtpClient()
		{
			if (_SmtpClient == null)
			{
				string host;
				if (!TryGetMacro("smtp-host", out host))
				{
					return null;
				}
				_SmtpClient = new SmtpClient(host);
			}
			return _SmtpClient;
		}

		private void SendKeyValueMessage(string key, string val)
		{
			SendMessage("@set {0}={1}", key, Truncate(val, 70));
		}

		private string Truncate(string s, int len)
		{
			s = s.Replace("\r\n", @"\r\n").Replace("\t", @"\t");
			if (s.Length > len) s = s.Substring(0, len) + "...";
			return s;
		}

		#region DateTime conversion

		private string ConvertDateTime(string text, Regex rx, string spec)
		{
			string[] parts = spec.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			string fmt0 = parts[0].Trim();
			string fmt1 = parts[3];
			if (fmt1 == "*") fmt1 = fmt0;
			TimeZoneInfo zone0 = parts[1] == "*" ? TimeZoneInfo.FindSystemTimeZoneById("UTC") : GetZone(parts[1]);
			TimeZoneInfo zone1 = parts[2] == "*" ? zone0 : GetZone(parts[2]);

			string result = rx.Replace(text, match => ConvertOneDateTime(match.Groups[0].Value, fmt0, zone0, fmt1, zone1));
			return result;
		}

		private string ConvertOneDateTime(string s, string fmt0, TimeZoneInfo zone0, string fmt1, TimeZoneInfo zone1)
		{
			DateTime dt, dt1;
			if (fmt0 != "*" && DateTime.TryParseExact(s, fmt0, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
			{
			}
			else if (DateTime.TryParse(s, out dt))
			{
			}
			else return s;
			dt1 = TimeZoneInfo.ConvertTime(dt, zone0, zone1);
			return dt1.ToString(fmt1, CultureInfo.InvariantCulture);
		}

		private TimeZoneInfo GetZone(string s)
		{
			const string st = " Standard Time";
			string s1 = s;
			if (!s.Contains("UTC") && !s.Contains("GMT") && !s.EndsWith(st)) s1 += st;
			TimeZoneInfo tz = null;
			try
			{
				tz = TimeZoneInfo.FindSystemTimeZoneById(s1);
			}
			catch
			{
				tz = TimeZoneInfo.FindSystemTimeZoneById(s);
			}
			return tz;
		}

		#endregion DateTime conversion

		#region Parser

		private bool GetRxLine(string args, out string rx, out string[] paths, out string macro)
		{
			paths = new string[0];
			macro = null;
			Match match = Regex.Match(args, @"\s*((?:\S|(?<=\\)\s)+)\s+(.*)");
			if (match.Success)
			{
				rx = match.Groups[1].Value;
				string a = match.Groups[2].Value.Trim();
				if (_Macros.ContainsKey(a))
				{
					macro = a;
				}
				else
				{
					paths = GetFiles(a);
				}
				return true;
			}
			rx = null;
			return false;
		}

		private static string[] GetFiles(string args)
		{
			try
			{
				bool recursive;
				string folder, pattern;
				SplitPath(args, out folder, out pattern, out recursive);
				string[] paths = Directory.GetFiles(folder, pattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
				return paths;
			}
			catch
			{
				return new string[0];
			}
		}

		internal static void SplitPath(string args, out string folder, out string pattern, out bool recursive)
		{
			args = args.Trim();
			recursive = args.StartsWith("-r ");
			var path = recursive ? args.Substring(3).Trim() : args;
			folder = Path.GetDirectoryName(path);
			pattern = Path.GetFileName(path);
		}

		private static string Replace(string here, string text, string rx)
		{
			var sb = new StringBuilder();
			foreach (Match m in Regex.Matches(text, rx))
			{
				sb.Append(m.Result(here));
			}
			string s = sb.ToString();
			return s;
		}

		private static string Replace(Dictionary<string, string> dict, string text)
		{
			string[] list = dict.Keys.Select(v => Regex.Escape(v)).ToArray();
			string rx = string.Join("|", list);
			return Regex.Replace(text, rx, match => dict.ContainsKey(match.Value) ? dict[match.Value] : match.Value);
		}


		/// <summary>
		/// Return lines upto the #end marker (aka 'here-string').
		/// Read just past #end marker itself from the Stream.
		/// Expand macros if indicated.
		/// </summary>
		/// <param name="reader">text to read from</param>
		/// <param name="expand">true to expand, false to take literal</param>
		/// <returns>string upto #end marker</returns>
		private string ReadHereString(LineReader reader, bool expand = true)
		{
			string s = ReadCData(reader, "#end");
			if (expand) s = Expand(s);
			return s;
		}

		private string ReadConditionalBlock(LineReader reader, out bool isElse)
		{
			isElse = false;
			string line;
			var sb = new StringBuilder();
			while ((line = reader.ReadLine()) != null)
			{
				string k = line.Trim();
				isElse = k == "#else";
				if (k == "#endif" || isElse) break;
				sb.AppendLine(line);
			}
			if (line == null) throw new Exception("Missing #else or #endif");
			return sb.Length < 2 ? sb.ToString() : sb.ToString(0, sb.Length - 2);
		}

		private string ReadCData(LineReader reader, string end)
		{
			string line;
			var sb = new StringBuilder();
			while ((line = reader.ReadLine()) != null && line.TrimEnd() != end)
			{
				sb.AppendLine(line);
			}
			if (line == null && !string.IsNullOrEmpty(end)) throw new Exception("Missing " + end);
			return sb.Length < 2 ? sb.ToString() : sb.ToString(0, sb.Length - 2);
		}


		private string Expand(string s)
		{
			return Regex.Replace(s, @"`(?<x>[^`]+)`|``(?<y>[^`]+)``",
													 new MatchEvaluator(Replacement),
													 RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}

		private string Generate(string s)
		{
			return Regex.Replace(s, @"`(?<x>&[^`]+)`",
													 new MatchEvaluator(Replacement),
													 RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}

		private string Replacement(Match m)
		{
			string val;
			string key0 = m.Groups["x"].Value;
			if (key0 == "")
			{
				// 1.  ``key`` => `key`
				key0 = m.Groups["y"].Value;
				return '`' + key0 + '`';
			}

			int p = key0.IndexOf('|');
			string key;
			string dflt;
			if (p == -1)
			{
				key = key0.ToLowerInvariant();
				dflt = "";
			}
			else
			{
				key = key0.Substring(0, p).ToLowerInvariant();
				dflt = key0.Substring(p + 1).Unescape();
			}

			// 2. is this a special key?
			if (key == "newguid")
			{
				return Guid.NewGuid().ToString();
			}
			if (key == "newseqguid")
			{
				return NewSeqGuid(0).ToString();
			}
			if (key == "timestamp")
			{
				return DateTime.UtcNow.ToString("s");
			}
			if (key == "rxtrue")
			{
				return @"(?i)^true$|^y(?:es)?$|^1$";
			}
			if (key == "rxfalse")
			{
				return @"(?i)^false$|^no?$|^0$";
			} 
			if (key.StartsWith("ts:"))
			{
				string fmt = key0.Substring(3);
				string fmt1 = fmt.ToLowerInvariant();
				if (fmt1 == "iso" || fmt1 == "sql") fmt = "yyyy-MM-dd HH:mm:ss";
				else if (fmt1 == "file") fmt = "yyMMdd_HHmmss"; // can be used for timestamping file names
				return DateTime.UtcNow.ToString(fmt);
			}
			if (key == "recipe")
			{
				return _CallStack.Count == 0 ? "" : _CallStack.Peek();
			}
			if (key.StartsWith("&"))
			{
				string name = key.Substring(1);
				var ng = _NumberGenerators[name];
				ng.MoveNext();
				return ng.Current;
			}

			// 3. return list of keys starting with certain string
			if (key.EndsWith("*"))
			{
				string key1 = key.Remove(key.Length - 1);
				var dict = key.StartsWith("$") ? _GlobalMacros : _Macros;
				var sb = new StringBuilder();
				lock (dict)
				{
					foreach (string k in dict.Keys)
					{
						if (k.ToLowerInvariant().StartsWith(key1)) sb.AppendLine(k);
					}
				}
				val = sb.ToString();
				return val;
			}


			// 4. check if key exists in dictionary
			if (TryGetMacro(key, out val))
			{
				return val;
			}

			// 5. check if key is a file path for an existing file: return the contents
			if (0 == string.Compare(Path.GetFullPath(key), key, StringComparison.OrdinalIgnoreCase) && File.Exists(key))
			{
				return File.ReadAllText(key);
			}

			// 6. check if it is a website:
			if (Regex.IsMatch(key, @"^https?://[^/]+/", RegexOptions.IgnoreCase))
			{
				return new WebClient().DownloadString(key);
			}

			// 4. formatting 
			p = key.IndexOf('%');
			if (p != -1)
			{
				string key1 = key.Substring(0, p);
				if (TryGetMacro(key1, out val))
				{
					string fmt = key.Substring(p + 1);
					double num;
					if (double.TryParse(val, out num)) return num.ToString(fmt);
					DateTime dt;
					if (DateTime.TryParse(val, out dt)) return dt.ToString(fmt == "" ? "yyyy-MM-dd HH:mm:ss" : fmt);
					return val;
				}
				return "";
			}

			return dflt;
		}

		#endregion Parser


		private static Guid NewSeqGuid(int accuracy)
		{
			byte[] buf = Guid.NewGuid().ToByteArray();
			byte[] tbuf = BitConverter.GetBytes(UtcNowSeq.Ticks);
			Array.Reverse(tbuf);
			Buffer.BlockCopy(tbuf, accuracy, buf, 10, 6);
			return new Guid(buf);
		}

		#region sequential time

		private static DateTime _LastTimestamp;
		private static readonly object LastTimestampSync = new object();

		/// <summary>
		/// Consecutive calls to this method will result in DateTime objects that are increasing with
		/// at least 7ms difference. Use this if you have to make sure a DateTime column in SQL server
		/// has to have unique times for an ORDER BY datetime. The resolution of a SQL server date time
		/// is 3.33ms.
		/// </summary>
		public static DateTime UtcNowSeq
		{
			get
			{
				lock (LastTimestampSync)
				{
					DateTime now = DateTime.UtcNow;
					if (now <= _LastTimestamp)
					{
						now = _LastTimestamp.AddMilliseconds(7); // > resolution in SQL (3.33ms)
					}
					_LastTimestamp = now;
					return now;
				}
			}
		}

		#endregion


		/// <summary>
		/// Get search pattern for template block based on file type.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private string GetPattern(string path)
		{
			string ext = Path.GetExtension(path).ToLowerInvariant();
			switch (ext)
			{
				case ".cs":
					return @"#if\s+(?:" + _MarkerName + @")\s*\r\n(\s*.*?\r\n)#endif\s*";

				case ".xml":
				case ".config":
				case ".html":
				case ".htm":
					return @"<!--template\s+(?:" + _MarkerName + @")\s*\r\n(\s*.*?\r\n\s*)\-->\s*";

				case "*.ps":
					return @"<##\s+(?:" + _MarkerName + @")\s*\r\n(\s*.*?)#>";

				case ".bat":
					return @"GOTO :template_(?:" + _MarkerName + @")\s*\r\n(\s*.*?):template_(?:" + _MarkerName + ")";

				default: // .sql, .js, .txt
					return @"/\*\*\*\s+(?:" + _MarkerName + @")\s*\r\n(\s*.*?\r\n\s*)\*\*\*/\s*";
			}
		}

		private void SetFile(string path)
		{
			_Macros["fullpath"] = path;
			_Macros["filename.ext"] = Path.GetFileName(path);
			_Macros["filename"] = Path.GetFileNameWithoutExtension(path);
		}


		#region Scripting

		public int RunShell(string script, out string output)
		{
			if (Write)
			{
				const string path = "recipe_tmp_cmd.bat";
				output = "";
				int exitCode = -1;
				try
				{
					if (script.Length > 0)
					{
						File.WriteAllText(path, script);
						var process = new Process();
						process.StartInfo = new ProcessStartInfo
						{
							UseShellExecute = false,
							RedirectStandardOutput = true,
							CreateNoWindow = true,
							WindowStyle = ProcessWindowStyle.Hidden,
							FileName = path
						};
						process.Start();
						output = process.StandardOutput.ReadToEnd();
						process.WaitForExit();
						exitCode = process.ExitCode;
					}
				}
				catch (Exception ex)
				{
					output = ex.Message;
				}
				try
				{
					if (File.Exists(path)) File.Delete(path);
				}
				catch { }
				return exitCode;
			}
			output = "test only";
			return -1;
		}

		public int RunPowerShell(string script, out string output)
		{
			if (Write)
			{
				try
				{
					Runspace myRunSpace = RunspaceFactory.CreateRunspace();
					myRunSpace.Open();
					Pipeline pipe = myRunSpace.CreatePipeline(script);
					var resultObject = pipe.Invoke();
					output = string.Join(Environment.NewLine, resultObject.Select(psobj => psobj.ToString()).ToArray());
					myRunSpace.Close();
					return 0;
				}
				catch (Exception ex)
				{
					output = ex.Message;
					return -1;
				}
			}
			output = "test only";
			return -1;
		}

		public string Apply(string commands, string text)
		{
			var script = new Script(commands, this);
			string result = script.Execute(text);
			return result;
		}

		public void Put(string key, string value) // IScriptContext
		{
			SetMacro(key, value);
		}

		public string Get(string key)
		{
			return GetMacro(key);
		}

		#endregion Scripting


		internal string SetMacro(string key, string value)
		{
			if (key == "") throw new ArgumentException("empty macro name");
			if (key.StartsWith("$")) lock (_GlobalMacros) return _GlobalMacros[key] = value;
			return _Macros[key] = value;
		}

		internal string GetMacro(string key)
		{
			string content;
			if (key.StartsWith("$"))
			{
				lock (_GlobalMacros)
				{
					return _GlobalMacros.TryGetValue(key, out content) ? content : string.Empty;
				}
			}
			return _Macros.TryGetValue(key, out content) ? content : string.Empty;
		}

		private bool TryGetMacro(string key, out string value)
		{
			bool exists;
			string s;
			if (key.StartsWith("$"))
			{
				lock (_GlobalMacros)
				{
					exists = _GlobalMacros.TryGetValue(key, out s);
					value = s ?? string.Empty;
					return exists;
				}
			}
			exists = _Macros.TryGetValue(key, out s);
			value = s ?? string.Empty;
			return exists;
		}

		internal void RemoveMacro(string key)
		{
			if (key.StartsWith("$")) lock (_GlobalMacros) _GlobalMacros.Remove(key);
			else _Macros.Remove(key);
		}

		public string[] GetHistory()
		{
			return new string[] {
				/*version*/
				"1.9.8   Added #print, #printv; made obsolete: #write",
				"1.9.7   #with path.. now applies to each line of here-text, #with-scripts help",
				"1.9.6   Added #with fn=path noext:   file name without extension",
				"1.9.5   Added #makexml for lite-XML and #escxml",
				"1.9.4   `m%format`, eg. `num%0.00`; #assert m rx; #del won't fail",
				"1.9.3   \\r\\n\\t\\ in macro eg `a|\\t`; added `rxFalse` `rxTrue`; #with esc unesc",
				"1.9.2   #with-script comments: start with ';'",
				"1.9.1   #esc..#end, stacktrace w/line-no, #line, better editor, ctrl-U",
				"1.9.0    Restructured",
				"1.8.28   Default values `key|literal`; #csharp result key0 key1 ... (string[] args)",
				"1.8.27   Added #path; #with: path full|root|dir|file|ext; #mode show|hide",
				"1.8.26   Bug fix in #append for global vars",
				"1.8.25   #default a ... #end is now valid",
				"1.8.24   Bug fix in #mkdir",
				"1.8.23   #with a=single-cmd, #mkdir -f, search #marker, look up selected text",
				"1.8.22   #. alias for #info; #$ alias for #with",
				"1.8.21   #rmdir can remove given folder itself",
				"1.8.20   #rmdir now removes files in selected folders too",
				"1.8.19   #append m m1 m2 m3...",
				"1.8.18   bug fixes; added #treecopy `exclude`",
				"1.8.17   Added #with: line <start> [<count>]",
				"1.8.16   Added #wstart, #wstop, #wrestart, `wtimeout`",
				"1.8.15   #mode params added: int-/+ log-/+ warn-/+",
				"1.8.14   Added #xcopy",
				"1.8.13   Added #treecopy",
				"1.8.12   bug fix in error handling",
				"1.8.11   Bug fix in #cmd",
				"1.8.10   Async running of recipe. Added Stop menu item to abort.",
				"1.8.9    Orphan #end check. Auto-#end and args check in UI.",
				"1.8.8    Control flow bug fixes; added #fatal",
				"1.8.7    bug fix in #trap for recipe.exe",
				"1.8.6    Custom command definition: #def .. #enddef",
				"1.8.5    Added exception handling with #trap, #error ",
				"1.8.4    Extended #iter with #next; menu Paste; #ask added defaults",
				"1.8.3    Added #date: time zone and format conversion",
				"1.8.2    Added #for .. #next, #while / #until .. #loop, #putm",
				"1.8.1    Some UI changes",
				"1.8.0    New look with tabs, #newer bug fix.",
				//"1.7.4    Added #esc",
				//"1.7.3    Allow #end in #info..#end, #mbox..#end, #warn..#end",
				//"1.7.2    Added #expn. Added this version history.",
				//"1.7.1    Added #mode. Removed NoEmptyLines script command (dup of delblank)",
				//"1.7.0    Confirm Save on New, Open and Exit. Third level versioning.",
				//"1.6.?    Added #csharp",
				//"1.6.?    Added #count and #exists",
				//"1.6.?    Added #version",
				//"1.5      Added #newer, #@, #!, ##, #marker, #extract, #mkdir. Removed #newr.",
				//"1.4      Added #return, #stop regex",
				//"1.3      Added #rmdir, garbage collection",
			};
		}
	}
}
