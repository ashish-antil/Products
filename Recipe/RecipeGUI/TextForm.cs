using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RecipeGUI
{
	public partial class TextForm : Form
	{
		public TextForm(string text)
		{
			InitializeComponent();
			txtBox.Text = text;
		}

		public TextForm(string[] lines)
		{
			InitializeComponent();
			txtBox.Lines = lines;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			txtBox.Select(0,0);
		}

		private void txtBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 27) Close();
			if (e.KeyChar == '\r') WindowState = WindowState != FormWindowState.Maximized ? FormWindowState.Maximized : FormWindowState.Normal;
		}
	}
}
