using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Imarda.Lib;


namespace IXLCompiler
{
	/// <summary>
	/// Mark up for methods that become part of the 'built-in' functions.
	/// Method must be public static.
	/// Recommended that method name starts lowercase.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	internal sealed class ImardaCalcAttribute : Attribute
	{
	}

	public delegate object ApplicationFunction(Dictionary<string, object> args, string entity, string attributeName);

	/// <summary>
	/// Compiles simple expressions into assemblies. Caches assemblies in memory, lookup key
	/// is full expression source.
	/// </summary>
	public class ImardaCompiler
	{
		private readonly Dictionary<string, string> _MathMembers;
		private readonly Dictionary<string, string> _ImardaMembers;
		private readonly CompilerParameters _Parameters;
		private readonly Dictionary<string, string> _Language;
		private readonly Dictionary<string, CompilerResults> _Cache;
		private Dictionary<string, Type> _TypeInfo;
		private ApplicationFunction _GetEntityAttributeCallback;

		/// <summary>
		/// Required for thread-safe singleton
		/// </summary>
		static ImardaCompiler()
		{
		}
		public static readonly ImardaCompiler Instance = new ImardaCompiler();




		/// <summary>
		/// Create compiler. Set parameters and initialize lookup tables.
		/// </summary>
		private ImardaCompiler()
		{
			_Cache = new Dictionary<string, CompilerResults>(StringComparer.Ordinal);
			
			string extraopt = ConfigUtils.GetString("CompilerOptions") ?? "";
			string copt = "/target:library /optimize " + extraopt;

			string[] assemblies = ConfigUtils.GetArray("IXLReferencedAssemblies", ""); // we need full paths in the config!

			_Parameters = new CompilerParameters();
			_Parameters.CompilerOptions = copt;
            _Parameters.GenerateInMemory = true;

		    var ixlPath = ConfigUtils.GetString("IXLPath") ?? Path.Combine(Path.GetTempPath(), "IXL");
		    if (!Directory.Exists(ixlPath))
		    {
		        Directory.CreateDirectory(ixlPath);
		    }
		    else
		    {
		        var files = Directory.GetFiles(ixlPath);
		        foreach (var file in files)
		        {
		            try
		            {
		                File.Delete(file);
		            }
                    // ReSharper disable once EmptyGeneralCatchClause
		            catch (Exception)
		            {
                        // try to clean up the IXL folder, suppress all errors
		            }
		        }
		    }
            _Parameters.TempFiles = new TempFileCollection(ixlPath, ConfigUtils.GetFlag("IXLKeepFiles"));

			var refa = _Parameters.ReferencedAssemblies;
		    refa.AddRange(new[]
		    {
		        "mscorlib.dll",
		        "System.dll",
		        "Imarda.Common.dll",
		        "IXLCompiler.dll"
		    });
			if (assemblies != null)
			{
                refa.AddRange(assemblies);
			}

			//refa.Add("Calculator.dll"); // for test program only
			//refa.Add("ImardaAlertingBusiness.dll");

			_Language = new Dictionary<string, string>();
			_Language["and"] = "&&";
			_Language["or"] = "||";
			_Language["not"] = "!";
			_Language["then"] = "?";
			_Language["else"] = ":";

			// Get all class Math members
			_MathMembers = new Dictionary<string, string>();
			foreach (var mi in typeof(Math).GetMembers())
			{
				_MathMembers[mi.Name.ToLower()] = mi.Name;
			}
			_MathMembers.Remove("equals");
			_MathMembers.Remove("gethashcode");
			_MathMembers.Remove("tostring");

			// Get all [ImardaCalc] functions in this class
			_ImardaMembers = new Dictionary<string, string>();
			foreach (var mi in GetType().GetMembers())
			{
				if (mi.GetCustomAttributes(typeof(ImardaCalcAttribute), false).Length > 0)
				{
					_ImardaMembers[mi.Name.ToLower()] = mi.Name;
				}
			}

		}

	    private CompilerParameters CreateCompilerParameters()
	    {
            var result = new CompilerParameters
            {
                CompilerOptions = _Parameters.CompilerOptions,
                GenerateInMemory = _Parameters.GenerateInMemory,
                TempFiles = new TempFileCollection(_Parameters.TempFiles.TempDir, _Parameters.TempFiles.KeepFiles)
            };
	        foreach (var refAss in _Parameters.ReferencedAssemblies)
	        {
                result.ReferencedAssemblies.Add(refAss);
	        }
	        return result;
	    }

		public void Initialize(string[] entities, ApplicationFunction callback, Dictionary<string, Type> typeInfo)
		{
			string s = string.Join("|", entities);
			_RxFetch = new Regex(@"(" + s + @")\.(\w+)\b", RegexOptions.Compiled | RegexOptions.CultureInvariant);
			_GetEntityAttributeCallback = callback;
			_TypeInfo = typeInfo;
		}


		/// <summary>
		/// Check if an assembly has already been created for the expression.
		/// Return it without re-compiling.
		/// </summary>
		/// <param name="companyID"> </param>
		/// <param name="expression">key for look up</param>
		/// <returns>compiler results, containing either errors or compiled assembly</returns>
		public CompilerResults Lookup(Guid companyID, string expression)
		{
		    var sourceKey = GetSourceKey(companyID, expression);
		    lock (_Cache)
		    {
		        CompilerResults res;
		        return _Cache.TryGetValue(sourceKey, out res)
                    ? res
                    : null;
		    }
		}

		public void Decache(Guid companyID, string expression)
		{
		    var sourceKey = GetSourceKey(companyID, expression);
			lock (_Cache)
			{
                _Cache.Remove(sourceKey);
			}
		}

		private string GetSourceKey(Guid companyID, string expression)
		{
			return companyID + " " + expression;
		}

	    /// <summary>
	    /// Look up assembly in cache based on expression; if found return, if not found
	    /// compile the assembly and encache the result.
	    /// </summary>
	    /// <param name="companyID"> </param>
	    /// <param name="expr">the expression to be compiled into an assembly</param>
	    /// <param name="returnsBool"></param>
	    /// <param name="variables">used to determine the types of local variables</param>
	    /// <param name="source">generated source code, only available when not getting results from cache</param>
	    /// <returns></returns>
	    public CompilerResults Build(Guid companyID, string expr, bool returnsBool, Dictionary<string, object> variables, out string source)
		{
			var expr1 = expr.Trim();
            var sourceKey = GetSourceKey(companyID, expr1);
            lock (_Cache)
		    {
		        CompilerResults res;
                if (!_Cache.TryGetValue(sourceKey, out res))
                {
                    string localVars = LocalVarDeclarations(expr1);
                    string expr2 = Prepare(expr1);

                    // we like to retry once when duplicate file name happen
                    var compileCount = 2;
                    do
                    {
                        var options = CreateCompilerParameters();
                        using (var compiler = CodeDomProvider.CreateProvider("CSharp"))
                        {
                            source = BuildSource(compiler, localVars, expr2, variables, returnsBool);
                            try
                            {
                                res = compiler.CompileAssemblyFromSource(options, source);
                                compileCount = 0;
                                _Cache[sourceKey] = res;
                            }
                            catch (IOException)
                            {
                                compileCount--;
                                if (compileCount == 0)
                                {
                                    throw;
                                }
                            }                            
                        }
                    } while (compileCount > 0);
                }
                else
                {
                    source = string.Empty;
                }
                return res;
            }
		}

		private static readonly Regex _RxLocalVar = new Regex(@"((?:[nbdst]|m[A-Z][A-Za-z]*)\d+)(?=\s*:=)", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		private static readonly Regex _RxEqualsSign = new Regex(@"(?<!<|>|:)=", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		private static readonly Regex _RxInt = new Regex(@"#-?(\d+)(?!\.)", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		private static readonly Regex _RxIndex = new Regex(@"\[\s*([^\]\s]+)\s*\]", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		private static readonly Regex _RxNumber = new Regex(@"\b(?<!#)\d[\d.]*", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		private static readonly Regex _RxLowerCaseWord = new Regex(@"[a-z]+", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		private static readonly Regex _RxStrings = new Regex(@"""[^\\""]*(?:\\.[^\\""]*)*""", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		private static readonly Regex _RxIdentifiers = new Regex(@"[A-Za-z]+[A-Za-z0-9_.]*", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		private static readonly Regex _RxMeasurements = new Regex(@"{\s*(-?\d[\d.]*)\s*([^}\s]*)\s*}", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		private static readonly Regex _RxArgs = new Regex(@"{([A-Za-z_]\w*)}", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		private static Regex _RxFetch;

		/// <summary>
		/// Make some alterations to the expression to turn it into C#.
		/// </summary>
		/// <param name="expr"></param>
		/// <returns></returns>
		private string Prepare(string expr)
		{
			// split expression into alternating non-string-literal and string-literal parts
			string[] strings = Split(_RxStrings, expr);
			// loop thru parts, only process non-string-literals, leave string literals unchanged
			for (int i = 0; i < strings.Length; i += 2)
			{
				string expr1 = strings[i];
				expr1 = _RxEqualsSign.Replace(expr1, "==").Replace("<>", "!="); // change equals sign
				expr1 = expr1.Replace(":=", "=");
				expr1 = _RxNumber.Replace(expr1, m => m.Value.Contains(".") ? m.Value : m.Value + ".0"); // ensure double
				expr1 = _RxInt.Replace(expr1, "$1"); // e.g. #1 -> 1
				expr1 = _RxIndex.Replace(expr1, "[($1)-1]"); // e.g. x[n*2] --> x[(n*2)-1]
				expr1 = _RxMeasurements.Replace(expr1, MeasurementLiteral); // e.g. {100 km/h} --> Speed.Kph(100)
				expr1 = _RxLowerCaseWord.Replace(expr1, Keyword); // replace keywords by C# operators
				if (_RxFetch != null) expr1 = _RxFetch.Replace(expr1, AddFetchCall);
				expr1 = _RxIdentifiers.Replace(expr1, Qualified); // qualify function calls
				expr1 = _RxArgs.Replace(expr1, " args.ContainsKey(\"$1\") "); // {A12} -> args.ContainsKey("A12") 
				strings[i] = expr1;
			}
			return string.Concat(strings); // reconcatenate all parts without separator
		}
		/// <summary>
		/// Split the string into an array of parts: odd are string-literals, even are the parts in between
		/// </summary>
		/// <param name="rx"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		private string[] Split(Regex rx, string s)
		{
			var list = new List<string>();
			int i = 0;
			foreach (Match m in rx.Matches(s))
			{
				list.Add(s.Substring(i, m.Index - i)); // even: non-string-literal
				list.Add(m.Value); // odd: string-literal
				i = m.Index + m.Length;
			}
			list.Add(s.Substring(i));
			return list.ToArray();
		}

		private string MeasurementLiteral(Match m)
		{
			string val = m.Groups[1].Value;
			string unit = m.Groups[2].Value;
			if (unit == "")
			{
				return string.Format("new Unitless({0}).AsMeasurement()", val);
			}
			MethodInfo mi = UnitParser.Instance.Get(unit);
			if (mi == null) throw new Exception(m.Value + " unit not found");
			return string.Format("{0}.{1}({2})", mi.DeclaringType, mi.Name, val);
		}

		/// <summary>
		/// Replace Imarda language keywords such as 'and', 'or' by C# operators.
		/// </summary>
		/// <param name="match"></param>
		/// <returns>same string correct case</returns>
		private string Keyword(Match match)
		{
			string replacement;
			return _Language.TryGetValue(match.Value, out replacement) ? replacement : match.Value;
		}

		/// <summary>
		/// Lookup the matched string and return the correctly cased replacement, and qualify
		/// it with the class name, e.g. max=>Math.Max
		/// </summary>
		/// <param name="match">identifier found in expression</param>
		/// <returns>qualified identifier, or unchanged if not found in compiler lookup tables</returns>
		private string Qualified(Match match)
		{
			string lookup = match.Value.ToLowerInvariant();
			string name;
			if (_MathMembers.TryGetValue(lookup, out name)) return "Math." + name;
			if (_ImardaMembers.TryGetValue(lookup, out name)) return "ImardaCompiler." + name;
			return match.Value;
		}

		private string AddFetchCall(Match match)
		{
			string entity = match.Groups[1].Value;
			string attr = match.Groups[2].Value;
			Type type;
			if (!_TypeInfo.TryGetValue(entity + "." + attr, out type)) type = typeof(object); // e.g. "flt.vehicles" -> string[]
			return string.Format("({0})ImardaCompiler.Fetch(args, \"{1}\", \"{2}\")", type.FullName, entity, attr);
			// this compiles e.g.:       (string[])ImardaCompiler.Fetch(args, "flt", "vehicles")
		}

		public void CheckResult(CompilerResults res)
		{
			string msg;
			string[] vars;
			if (HasErrors(res, out msg, out vars)) throw new Exception(msg);
		}

		/// <summary>
		/// Check the compiler results. 
		/// See http://msdn.microsoft.com/en-us/library/ms228296.aspx for a list of compiler errors and warnings
		/// </summary>
		/// <param name="res">results to be checked</param>
		/// <param name="message">error message</param>
		/// <param name="unknownVariables">array of variable names not known in the context</param>
		/// <returns>true if ok, false if error</returns>
		public bool HasErrors(CompilerResults res, out string message, out string[] unknownVariables)
		{
			message = null;
			unknownVariables = null;
			var vars = new List<string>();
			if (res.Errors.HasErrors)
			{
				var sb = new StringBuilder();
				foreach (CompilerError error in res.Errors)
				{
					string text = error.ErrorText;
					if (error.ErrorNumber == "CS1668")
					{
						// Invalid search path 'path' specified in 'path string' -- 'system error message'
						continue;
					}
					if (error.ErrorNumber == "CS0103")
					{
						// The name 'identifier' does not exist in the current context
						int p = text.IndexOf('\'') + 1;
						int q = text.IndexOf('\'', p);
						string varname = text.Substring(p, q - p);
						vars.Add(varname);
					}
					sb.Append(text)
						.Append(':')
						.Append(error.Line)
						.Append(',')
						.Append(error.Column)
						.AppendLine();
				}
				message = sb.ToString();
				unknownVariables = vars.ToArray();
				return true;
			}
			else
			{
				unknownVariables = new string[0];
				return false;
			}
		}

		/// <summary>
		/// Evaluate the expression, pass parameters in dictionary.
		/// </summary>
		/// <param name="companyID"> </param>
		/// <param name="expression">expression to be evaluated</param>
		/// <param name="returnsBool">expression should return boolean because it is a rule</param>
		/// <param name="variables">key names are referenced in expression</param>
		/// <returns>expression evaluation result: true or false</returns>
		/// <exception cref="Exception">errors on evaluation or compiling</exception>
		public object Evaluate(Guid companyID, string expression, bool returnsBool, Dictionary<string, object> variables, out string[] unknownVariables)
		{
			string src;
			CompilerResults res = Build(companyID, expression, returnsBool, variables, out src);
			string msg;
			if (HasErrors(res, out msg, out unknownVariables))
			{
				return null;
			}
			else
			{
				object value = Run(res.CompiledAssembly, variables);
				return value;
			}
		}

		public bool EvaluateBool(Guid companyID, string expression, Dictionary<string, object> variables)
		{
			string[] unknownVariables;
			object obj = Evaluate(companyID, expression, true, variables, out unknownVariables);
			return obj is bool && (bool)obj;
		}

	    /// <summary>
	    /// Build the assembly using CodeDom.
	    /// </summary>
	    /// <param name="compiler">instance of compiler</param>
	    /// <param name="localVars">string with local variable declarations, generated from expression</param> 
	    /// <param name="expression">expression to be compiled</param>
	    /// <param name="variables">types of values in this dictionary determine local variable types</param>
	    /// <param name="returnsBool">should be true if the expression must return a boolean</param>
	    /// <returns>C# source</returns>
	    private static string BuildSource(CodeDomProvider compiler, string localVars, string expression, Dictionary<string, object> variables, bool returnsBool)
		{
			var sw = new StringWriter();

			var ns = new CodeNamespace("ExpressionEvaluator");
			ns.Imports.Add(new CodeNamespaceImport("System"));
			ns.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
			ns.Imports.Add(new CodeNamespaceImport("Imarda.Lib"));
			ns.Imports.Add(new CodeNamespaceImport("IXLCompiler"));

			var decl = new CodeTypeDeclaration("Calculator") { IsClass = true, Attributes = MemberAttributes.Public };

			var method = new CodeMemberMethod { Name = "Evaluate" };
			method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(Dictionary<string, object>)), "args"));
			method.ReturnType = new CodeTypeReference(returnsBool ? typeof(bool) : typeof(object));
			method.Comments.Add(new CodeCommentStatement("Calculate an expression", true));
			method.Attributes = MemberAttributes.Public;

			var stmt = method.Statements;
			foreach (var key in variables.Keys)
			{
				// only generate local variable decl/assignment if the variable occurs on the expression
				if (Regex.IsMatch(expression, @"\b" + Regex.Escape(key) + @"\b"))
				{
					object obj = variables[key];
					Type type = obj == null ? typeof(string) : obj.GetType();
					var vardecl = string.Format("{0} {1} = args.ContainsKey(\"{1}\") ? ({0})(args[\"{1}\"]) : default({0})", type, key);
					stmt.Add(new CodeSnippetExpression(vardecl));

				}
			}
			if (!string.IsNullOrEmpty(localVars)) stmt.Add(new CodeSnippetStatement(localVars));
			stmt.Add(new CodeSnippetStatement("#line 1"));
			stmt.Add(new CodeAssignStatement(new CodeSnippetExpression(returnsBool ? "bool result" : "object result"), new CodeSnippetExpression(expression)));
			stmt.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("result")));
			decl.Members.Add(method);
			ns.Types.Add(decl);
            compiler.GenerateCodeFromNamespace(ns, sw, new CodeGeneratorOptions
			{
				IndentString = "\t",
				BlankLinesBetweenMembers = false
			});
			sw.Close();
			return sw.ToString();
		}

		private string LocalVarDeclarations(string expression)
		{
			var lvars = new Dictionary<string, string>();
			foreach (Match m in _RxLocalVar.Matches(expression))
			{
				string name = m.Groups[1].Value;
				string type;
				switch (name[0])
				{
					case 'n':
						type = "System.Double";
						break;
					case 'b':
						type = "System.Boolean";
						break;
					case 'd':
						type = "System.DateTime";
						break;
					case 's':
						type = "System.String";
						break;
					case 't':
						type = "System.TimeSpan";
						break;
					case 'm':
						int i = name.Length - 1;
						while (char.IsNumber(name[i])) i--;
						string mtype = name.Substring(1, i);
						type = "Imarda.Lib." + mtype;
						break;
					default:
						type = "System.Object";
						break;
				}
				lvars[name] = type;
			}
			var sb = new StringBuilder();
			foreach (string lvar in lvars.Keys)
			{
				string type = lvars[lvar];
				sb.Append(type).Append(' ').Append(lvar).Append("; ");
			}
			return sb.ToString();
		}

		/// <summary>
		/// Run the assembly's Evaluate method.
		/// </summary>
		private object Run(Assembly assembly, Dictionary<string, object> variables)
		{
			object calculator = assembly.CreateInstance("ExpressionEvaluator.Calculator");
			Type type = calculator.GetType();
			MethodInfo method = type.GetMethod("Evaluate");
			object obj = method.Invoke(calculator, new object[] { variables });
			return obj;
		}

		public static object Fetch(Dictionary<string, object> args, string entity, string attributeName)
		{
			return Instance._GetEntityAttributeCallback(args, entity, attributeName);
		}

		#region Build-in functions: public static <type> <lowerCamelCaseMethodName>(parameters...)


		[ImardaCalc]
		public static bool weekday(DateTime dt)
		{
			return dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday;
		}

		[ImardaCalc]
		public static double hours(DateTime dt1, DateTime dt2)
		{
			return Math.Abs((dt2 - dt1).TotalHours);
		}

		[ImardaCalc]
		public static DateTime utc(DateTime dt, string tzid)
		{
			if (dt.Kind == DateTimeKind.Utc) return dt;
			if (dt.Kind == DateTimeKind.Local) throw new Exception("Cannot convert local system time to utc");
			var tzi = TimeZoneInfo.FindSystemTimeZoneById(tzid);
			return TimeZoneInfo.ConvertTimeToUtc(dt, tzi);
		}

		[ImardaCalc]
		public static DateTime local(DateTime dt, string tzid)
		{
			if (dt.Kind == DateTimeKind.Unspecified) return dt;
			if (dt.Kind == DateTimeKind.Local) throw new Exception("Cannot convert local system time to user time");
			var tzi = TimeZoneInfo.FindSystemTimeZoneById(tzid);
			return TimeZoneInfo.ConvertTimeFromUtc(dt, tzi);
		}

		[ImardaCalc]
		public static DateTime now(string tzid)
		{
			var tzi = TimeZoneInfo.FindSystemTimeZoneById(tzid);
			return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);
		}

		[ImardaCalc]
		public static DateTime utcnow()
		{
			return DateTime.UtcNow;
		}

		[ImardaCalc]
		public static bool match(string wildcard, string s)
		{
			return Regex.IsMatch(s, ToPattern(wildcard), RegexOptions.IgnoreCase);
		}

		[ImardaCalc]
		public static int index(string wildcard, string[] arr)
		{
			var rx = new Regex(ToPattern(wildcard), RegexOptions.IgnoreCase);

			for (int i = 0; i < arr.Length; i++)
			{
				if (rx.IsMatch(arr[i])) return i + 1;
			}
			return 0;
		}

		private static string ToPattern(string wildcard)
		{
			string pattern = Regex.Escape(wildcard.Replace('*', '\u001A'));
			return "^" + pattern.Replace("\u001A", ".*") + "$";
		}

		[ImardaCalc]
		public static bool equal(Measurement m1, Measurement m2, Measurement tolerance)
		{
			var diff = Measurement.Subtract(m1, m2);
			if (diff.Unit == tolerance.Unit)
			{
				return Math.Abs(diff.Value) <= tolerance.Value;
			}
			throw new ArgumentException("equal(): tolerance has different unit");
		}

		[ImardaCalc]
		public static Length dist(Angle lat1, Angle lon1, Angle lat2, Angle lon2)
		{
			return Length.Metre(GeoUtils.Distance(lat1.InRadians, lon1.InRadians, lat2.InRadians, lon2.InRadians));
		}

		[ImardaCalc]
		public static Angle bearing(Angle lat1, Angle lon1, Angle lat2, Angle lon2)
		{
			return Angle.Radians(GeoUtils.Bearing(lat1.InRadians, lon1.InRadians, lat2.InRadians, lon2.InRadians));
		}

		[ImardaCalc]
		public static string dirx(Angle a)
		{
			return a.Bearing4; // returns one of: "N" "E" "S" "W"
		}

		/// <summary>
		/// Find a substring or return a default one.
		/// </summary>
		/// <param name="s">string in which to search</param>
		/// <param name="marker">marker to look for, substring starts after marker</param>
		/// <param name="delimiter">terminates the string, if not found then take substring to the end of the original string</param>
		/// <param name="dflt">if not found</param>
		/// <returns>substring if found, or dflt if not found</returns>
		[ImardaCalc]
		public static string substr(string s, string marker, string delimiter, string dflt)
		{
			try
			{
				int p = s.IndexOf(marker, StringComparison.InvariantCulture);
				if (p == -1) return dflt;
				p += marker.Length;
				int q = s.IndexOf(delimiter, p, StringComparison.InvariantCulture);
				if (s.Length > p && q == -1) return s.Substring(p);
				return s.Substring(p, q - p);
			}
			catch
			{
				return dflt;
			}
		}

		/// <summary>
		/// Look inside a string trying to match against the regex and return the captured text if found.
		/// </summary>
		/// <param name="s">original string being searched</param>
		/// <param name="regex">pattern (.NET regex) to look for</param>
		/// <param name="dflt">returned if no match possible</param>
		/// <returns>captured text ($0)</returns>
		[ImardaCalc]
		public static string find(string s, string regex, string dflt)
		{
			try
			{
				Match m = Regex.Match(s, regex, RegexOptions.CultureInvariant);
				return m.Success ? m.Groups[0].Value : dflt;
			}
			catch
			{
				return dflt;
			}
		}

		/// <summary>
		/// Find a number in a string, e.g. findnum("hello 123.4", #1, 0.0)  -> 123.4
		/// Note that you have to put a # in front of a number that you want to be an integer
		/// </summary>
		/// <param name="s">input string</param>
		/// <param name="i">1-based occurrence of match, e.g. i=1 is first occurrence, i=2 is second...</param>
		/// <param name="dflt">default if occurrence not found</param>
		/// <returns>floating point number</returns>
		[ImardaCalc]
		public static double findnum(string s, int i, double dflt)
		{
			try
			{
				if (--i < 0) return dflt; // make it zero-based.
				var m = Regex.Matches(s, @"-?\d+(?:\.\d+)?", RegexOptions.CultureInvariant | RegexOptions.Compiled);
				if (m.Count > i) return Convert.ToDouble(m[i].Groups[0].Value);
			}
			catch
			{
				// ignore
			}
			return dflt;
		}



		// find measurement literal in a string
		public static Measurement findnum(string s, int i, string dflt)
		{
			var im0 = Measurement.ParseInput<IMeasurement>(dflt);
			var name = im0.GetType().Name;
			if (--i < 0) return im0.AsMeasurement(); // make it zero-based.

			var m = Regex.Matches(s, @"(-?\d+(?:\.\d+)?)\ ([^\s\d,.;:?!()\b]+\d?)", RegexOptions.CultureInvariant | RegexOptions.Compiled);
			var list = new List<IMeasurement>();
			for (int j = 0; j < m.Count; j++)
			{
				string num = m[j].Groups[1].Value;
				string unit = m[j].Groups[2].Value;
				IMeasurement im = UnitParser.Instance.Create(Convert.ToDouble(num), unit);
				if (im != null && im.GetType().Name == name)
				{
					list.Add(im);
				}
			}
			var im1 = list.Count > i ? list[i] : im0;
			return im1.AsMeasurement();
		}


		/// <summary>
		/// Find a string s in itemString where itemString is of the format "|item1|item2|item3|....|lastItem|"
		/// </summary>
		/// <param name="itemString"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		[ImardaCalc]
		public static bool contains(string itemString, string s)
		{
			return string.IsNullOrEmpty(itemString) ? false : itemString.Contains('|' + s + '|');
		}

		[ImardaCalc]
		public static bool contains(string[] arr, string s)
		{
			return arr != null && Array.IndexOf(arr, s) != -1;
		}

		[ImardaCalc]
		public static string join(string itemString, string separator)
		{
			return itemString.Trim('|').Replace("|", separator);
		}

		[ImardaCalc]
		public static Measurement sum(params IMeasurement[] arr)
		{
			Measurement m = arr[0].AsMeasurement();
			for (int i = 1; i < arr.Length; i++)
			{
				m = Measurement.Add(m, arr[i].AsMeasurement());
			}
			return m;
		}

		[ImardaCalc]
		public static Measurement product(params IMeasurement[] arr)
		{
			Measurement m = arr[0].AsMeasurement();
			for (int i = 1; i < arr.Length; i++)
			{
				m = Measurement.Multiply(m, arr[i].AsMeasurement());
			}
			return m;
		}

		#endregion

	}
}
