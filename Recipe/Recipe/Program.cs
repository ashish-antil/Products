using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using RecipeLib;


/// <summary>
/// This program takes as input a "recipe" file which contains commands to change text and create new text files
/// while instantiating the templates contained in the recipe file and the existing and created files. 
/// Use this program to make routine changes to sets of text files.
/// 
/// The #name command sets a recipe name. 
/// Subsequent #name commands can be set as needed to identify the texts to change.
/// 
/// The #do command takes a path of an existing file, looks for a template block in that file,
/// with the given recipe name, and then duplicates that and finally 
/// replaces "`"-enclosed macros in that block with associated values from in the dictionary.
/// 
/// A template block syntax depends on the file type. For C#, use #if recipeName...#endif, for 
/// xml and html use <?template ... ?>
/// 
/// The #new command creates a new file after filling in the parameters in the path name. Then it will copy
/// the text after #new upto #end, into that new file and fill in the command line values. 
/// Directories get created as needed.
/// 
/// The #put and #default commands add macros to the dictionary. The #default command only adds
/// the macro if it does not already exist (e.g. on commandline). There are two syntaxes: #put key=value
/// and #put key[newline].....#end which will assign all the lines between #put and #end as a value.
/// Same for #default.
/// </summary>
/// <example>
/// recipe example (e.g. filename c:\newthingrecipe.txt)
/// 
///   #name NewThingy
///   #put foo=bar_`mymacro1`
///   #do c:\sourcetree\file1.cs
///   #do c:\sourcetree\dir\file2.cs
///   #new c:\sourcetree\`foldername`\`filename`.cs
///   Hello
///   This text goes into `filename`
///   Bye.
///   #end
///   #do c:\sourcetree\file3.cs
/// 
/// The command line to run this recipe is e.g.
/// 
/// recipe.exe c:\newthingrecipe.txt foldername=foo filename=f_1.cs "test=hello world"
/// 
/// </example>
class Program
{
	static void Main(string[] args)
	{
		try
		{
			string recipePath = args[0];

			var macros = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			// add all environment variables to the dictionary
			IDictionary env = Environment.GetEnvironmentVariables();
			foreach (string key in env.Keys) macros.Add(key, (string)env[key]);

			// parse command line arguments and add to dictionary
			for (int i = 1; i < args.Length; i++)
			{
				string[] nv = args[i].Split('=');
				if (nv.Length == 2) macros[nv[0]] = nv[1];
				else throw new Exception("Invalid argument: " + args[i]);
			}
			var program = new Recipe();
			program.Write = true;
			program.SetRootPath(recipePath);
			program.ClearHandlers();
			program.Message += PrintLine;

			using (var sr = new LineReader(File.OpenText(recipePath), name: recipePath))
			{
				program.Run(sr, macros, string.Empty);
			}
		}
		catch (ApplicationException)
		{
			//exit
		}
		catch (Exception ex)
		{
			Console.WriteLine("Usage: recipe.exe recipe-file key1=val1 key2=val2...");
			Console.WriteLine(ex.Message);
		}
	}

	static void PrintLine(string s)
	{
		Console.WriteLine(s);
		if (s == "@exit") throw new ApplicationException(s);
	}
}
