using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RecipeGUI.Properties;

namespace RecipeGUI
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Form form = new MainForm(args);
			Application.Run(form);
		}
	}
}
