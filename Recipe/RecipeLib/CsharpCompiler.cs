using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Reflection;

namespace RecipeLib
{
	public static class CSharp
	{
		private static Dictionary<string, CompilerResults> _Cache = new Dictionary<string, CompilerResults>();

		public static CompilerResults Build(string source)
		{
			CompilerResults cr;
			if (_Cache.TryGetValue(source, out cr)) return cr;

			var csource =
				@"using System;" +
				@"using System.Collections.Generic;" +
				@"using System.Linq;" +
				@"using System.Text;" +
				@"public class Program " +
				@"{" +
				@"	public static void Main(string[] args) {}" +
				@"	public static string Get(string[] args)" +
				@"	{" +
				source +
				@"	}" +
				@"}";

			var csu = new CodeSnippetCompileUnit(csource);
			var provider = new CSharpCodeProvider();
			var options = new CompilerParameters();
			options.GenerateInMemory = true;
			options.IncludeDebugInformation = true;
			options.ReferencedAssemblies.Add("System.dll");
			options.ReferencedAssemblies.Add("System.Core.dll");
			options.MainClass = "Program";
			cr = provider.CompileAssemblyFromSource(options, new string[] { csource });
			_Cache[source] = cr;
			return cr;
		}

		public static string Execute(Assembly asm, object[] args)
		{
			var methodInfo = asm.GetExportedTypes()[0].GetMethod("Get", new Type[] { typeof(string[]) });
			return methodInfo.Invoke(null, new object[] { args }) as string ?? "";
		}


		public static string[] Errors(CompilerResults result)
		{
			var list = new List<string>();
			foreach (CompilerError e in result.Errors)
			{
				list.Add(e.ErrorText);
			}
			return list.ToArray();
		}


		/*
		 * 
#csharp a
using System;
public class Program 
{
	public static string Main(string[] args)
	{
		return "Hello World";
	}
}
#end
		 */
	}
}
