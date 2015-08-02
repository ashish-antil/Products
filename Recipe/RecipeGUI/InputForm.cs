using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RecipeGUI
{
	public partial class InputForm : Form
	{
		public InputForm()
		{
			InitializeComponent();
		}

		public InputForm(string prompt, bool multiline, string initial, params string[] args)
			: this()
		{
			lblPrompt.Text = prompt;
			if (args.Length == 0)
			{
				AcceptButton = multiline ? null : btnOK;
				txtInput.Visible = true;
				btnFiles.Visible = true;
				btnFolder.Visible = true;
				txtInput.Multiline = multiline;
				txtInput.Text = initial;
			}
			else
			{
				AcceptButton = btnOK;
				int x = lblPrompt.Location.X;
				int y = txtInput.Location.Y;

				bool useCombo = (args.Length > 5);
				Control ctl = null;
				if (useCombo)
				{
					ctl = new ComboBox {
						Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
						FormattingEnabled = true,
						Name = "cbb",
						Location = new Point(x, y),
						DropDownStyle = ComboBoxStyle.DropDownList,
						Size = new Size(txtInput.Width, 21),
					};
					Controls.Add(ctl);
				}

				for (int i = 0; i < args.Length; i++)
				{
					string s = args[i];
					if (!s.Contains('|')) s += "|" + s;
					int p = s.IndexOf('|');
					string txt = s.Substring(p + 1);
					string tag = s.Substring(0, p);
					if (useCombo)
					{
						((ComboBox)ctl).Items.Add(new ComboItem { Text = txt, Tag = tag });
					}
					else
					{
						ctl = new RadioButton { Text = txt, Location = new Point(x, y), Tag = tag, AutoSize = true };
						y += ctl.Height;
						Controls.Add(ctl);
					}
				}
				
			}
		}

		private class ComboItem
		{
			public string Text;
			public string Tag;
			public override string ToString()
			{
				return Text;
			}
		}

		public string UserInput { get; set; }

		private void InputForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			UserInput = "";
			if (txtInput.Visible)
			{
				UserInput = txtInput.Text;
			}
			else
			{
				var controls = Controls.Find("cbb", false);
				if (controls.Length > 0)
				{
					var cbb = controls[0] as ComboBox;
					int j = cbb.SelectedIndex;
					UserInput = (j == -1) ? "" : ((ComboItem)cbb.Items[j]).Tag;
				}
				else
				{
					for (int i = 0; i < Controls.Count; i++)
					{
						if (Controls[i] is RadioButton)
						{
							var a = (RadioButton)Controls[i];
							if (a.Checked)
							{
								UserInput = a.Tag as string;
								break;
							}
						}
					}
				}
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnFiles_Click(object sender, EventArgs e)
		{
			string path = txtInput.Text.Trim();
			if (File.Exists(path))
			{
				dlgSelectFiles.FileName = path;
				dlgSelectFiles.InitialDirectory = Path.GetDirectoryName(path);
			}
			if (DialogResult.OK == dlgSelectFiles.ShowDialog(this))
			{
				if (txtInput.Text.Length > 0) txtInput.AppendText(Environment.NewLine);
				txtInput.AppendText(string.Join(Environment.NewLine, dlgSelectFiles.FileNames));
			}
		}

		private void btnFolder_Click(object sender, EventArgs e)
		{
			string path = txtInput.Text.Trim();
			if (Directory.Exists(path)) dlgSelectFolder.SelectedPath = path;
			if (DialogResult.OK == dlgSelectFolder.ShowDialog(this))
			{
				if (txtInput.Text.Length > 0) txtInput.AppendText(Environment.NewLine);
				txtInput.AppendText(dlgSelectFolder.SelectedPath);
			}
		}


	}
}
