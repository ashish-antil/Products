using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System;

namespace RecipeGUI
{
	public partial class DictionaryForm : Form
	{
		public DictionaryForm(Dictionary<string,string> dict)
		{
			InitializeComponent();
			var ar = new string[dict.Count][];
			int i = 0;
			foreach (string key in dict.Keys) 
			{
				ar[i++] = new[] {key, dict[key]};
			}
			Array.Sort(ar, (a, b) => string.Compare(a[0], b[0]));
			foreach (string[] kv in ar) grdDict.Rows.Add(kv[0], kv[1]);
		}

		private void grdDict_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int row = e.RowIndex;
			if (row >= 0)
			{
				var text = grdDict[1, row].Value as string;
				var form = new TextForm(text) { Text = grdDict[0, row].Value as string };
				form.ShowDialog(this);
			}
		}
	}
}
