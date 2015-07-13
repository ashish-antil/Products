using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Imarda.Lib
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	internal sealed class ScriptCommandAttribute : Attribute
	{
		private readonly string _Name;
		internal MethodInfo Method;

		public ScriptCommandAttribute(string name)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException("Command cannot be empty or null");
			if (Regex.IsMatch(name, @"[^\w.]")) throw new ArgumentException("Invalid character in command " + name);
			_Name = name;
		}

		public string Name
		{
			get { return _Name; }
		}
	}


	/// <summary>
	/// Executes methods in the given order. Pass to the constructor an object that contains parameterless methods 
	/// that are marked by the [ScriptCommand("a_name")]. The script command names are passed in as an array to RunScript.
	/// </summary>
	/// <example>
	/// var handler = new SimpleScriptHandler(myCommands);
	/// handler.Run(new string[] { "dothis", "dothat" });
	/// </example>
	public class SimpleScriptHandler
	{
		private readonly object _CommandImplementation;

		private readonly Dictionary<string, ScriptCommandAttribute> _Commands =
			new Dictionary<string, ScriptCommandAttribute>();

		public SimpleScriptHandler(object commands)
		{
			_CommandImplementation = commands;
			MethodInfo[] methods = commands.GetType().GetMethods();
			foreach (MethodInfo m in methods)
			{
				foreach (ScriptCommandAttribute attr in m.GetCustomAttributes(typeof (ScriptCommandAttribute), false))
				{
					attr.Method = m;
					_Commands.Add(attr.Name, attr);
				}
			}
		}


		/// <summary>
		/// Run the script.
		/// </summary>
		/// <param name="script">each string is the name of a ScriptCommandAttribute</param>
		public void Run(string[] script)
		{
			var args = new object[0];
			foreach (string command in script)
			{
				ScriptCommandAttribute attr;
				if (_Commands.TryGetValue(command, out attr))
				{
					attr.Method.Invoke(_CommandImplementation, args);
				}
			}
		}
	}
}