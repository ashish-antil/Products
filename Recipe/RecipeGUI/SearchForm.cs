using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace RecipeGUI
{
	public partial class SearchForm : Form
	{
		private static readonly char[] _Sep = new char[] { ' ', ',', ';', '\t' };
		private readonly FolderBrowserDialog _Dlg = new FolderBrowserDialog();

		public SearchForm(FolderBrowserDialog dlg)
		{
			InitializeComponent();
			_Dlg = dlg;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			txtPath.Text = _Dlg.SelectedPath;
			lblFoundTags.Text = null;
		}

		private void ValidateInput(object sender, EventArgs e)
		{
			bool ready = Directory.Exists(txtPath.Text) && txtTags.Text.Trim().Length != 0;
			AcceptButton = ready ? btnSearch : null;
			btnSearch.Enabled = ready;
		}

		private void btnSelectFolder_Click(object sender, EventArgs e)
		{
			if (DialogResult.OK == _Dlg.ShowDialog())
			{
				txtPath.Text = _Dlg.SelectedPath;
			}
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				Search();
			}
			catch
			{
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void Search()
		{
			lblFoundTags.Text = null;
			string text = txtTags.Text.ToLowerInvariant();
			string[] search = text.Split(_Sep, StringSplitOptions.RemoveEmptyEntries);
			string[] files = Directory.GetFiles(txtPath.Text, "*.rcp", SearchOption.AllDirectories);
			var list = new List<string>();
			bool all = chkAll.Checked;
			bool marker = chkMarker.Checked;
			foreach (string file in files)
			{
				var fileTags = new List<string>();
				foreach (string line in File.ReadAllLines(file).Where(line => line.StartsWith("#tags ") || (marker && line.StartsWith("#marker "))))
				{
					string[] tags = line.ToLowerInvariant().Split(_Sep, StringSplitOptions.RemoveEmptyEntries);
					if (all)
					{
						fileTags.AddRange(tags);
					}
					else
					{
						if (search.Intersect(tags).Count() > 0)
						{
							list.Add(file);
							break;
						}
					}
				}
				if (all)
				{
					if (search.Except(fileTags).Count() == 0) list.Add(file);
				}
			}
			lstFiles.BeginUpdate();
			lstFiles.Items.Clear();
			lstFiles.Items.AddRange(list.ToArray());
			lstFiles.EndUpdate();
		}

		private string[] GetFileTags(string file)
		{
			var list = new List<string>();
			foreach (string line in File.ReadAllLines(file).Where(line => line.StartsWith("#tags ")))
			{
				string[] tags = line.Split(_Sep, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 1; i < tags.Length; i++) list.Add(tags[i]);
			}
			return list.ToArray();
		}

		public string SelectedPath { get; private set; }

		private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			int i = lstFiles.SelectedIndex;
			if (i != -1)
			{
				string file = lstFiles.Items[i].ToString();
				SelectedPath = file;
				lblFoundTags.Text = string.Join(", ", GetFileTags(file));
				AcceptButton = btnOpen;
				btnOpen.Enabled = true;
			}
			else
			{
				lblFoundTags.Text = null;
				AcceptButton = btnSearch;
				btnOpen.Enabled = false;
			}
		}

		private void lstFiles_DoubleClick(object sender, EventArgs e)
		{
			int i = lstFiles.SelectedIndex;
			if (i != -1)
			{
				string file = lstFiles.Items[i].ToString();
				SelectedPath = file;
				DialogResult = DialogResult.OK;
				Close();
			}
		}
	}
}
