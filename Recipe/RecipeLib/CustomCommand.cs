using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeLib
{
	internal class CustomCommand
	{
		internal string Name { get; private set; }
		internal string Text { get; private set; }
		internal string[] Args { get; private set; }

		internal CustomCommand(string name, string text, string[] args)
		{
			Name = name;
			Text = text;
			Args = args;
		}
	}
}
