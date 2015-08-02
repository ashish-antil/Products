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
	public partial class ParameterInputForm : Form
	{
		public ParameterInputForm()
		{
			InitializeComponent();
			grdParameters.Font = new Font("Lucida Console", 9f);
		}

		public ParameterInputForm(
			IEnumerable<string[]> required,
			IEnumerable<string[]> optional
		)
			: this()
		{
			foreach (string[] a in required) grdParameters.Rows.Add(a[0], a[1]);
			foreach (string[] a in optional)
			{
				string[] options = a[2].Split('|');
				grdParameters.Rows.Add(a[0], a[1], a[2]);
				if (options.Length > 1)
				{
					var cbb = new DataGridViewComboBoxCell();
					foreach (string o in options) cbb.Items.Add(o);
					string s = a[1] ?? "";
					if (cbb.Items.Contains(s)) cbb.Value = s;
					grdParameters[1, grdParameters.Rows.Count-1] = cbb;
				}
			}
		}

		public IDictionary<string, string> Result
		{
			get
			{
				var map = new Dictionary<string, string>();
				foreach (DataGridViewRow r in grdParameters.Rows)
				{
					string key = (string)r.Cells[0].Value;
					string val = (string)r.Cells[1].Value;
					if (val != null) map.Add(key, val);
					else if (r.Cells[1] is DataGridViewComboBoxCell) val = "`empty`";
				}
				return map;
			}
		}

		private void grdParameters_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			DataGridViewComboBoxCell cell = grdParameters.CurrentCell as DataGridViewComboBoxCell;

			if (cell != null && !cell.Items.Contains(e.FormattedValue))
			{
				cell.Items.Insert(0, e.FormattedValue);
				if (grdParameters.IsCurrentCellDirty)
				{
					grdParameters.CommitEdit(DataGridViewDataErrorContexts.Commit);
				}
				cell.Value = cell.Items[0];
			}

		}

		private void grdParameters_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			DataGridViewComboBoxEditingControl comboControl = e.Control as DataGridViewComboBoxEditingControl;
			if (comboControl != null)
			{
				// Set the DropDown style to get an editable ComboBox
				if (comboControl.DropDownStyle != ComboBoxStyle.DropDown)
				{
					comboControl.DropDownStyle = ComboBoxStyle.DropDown;
				}
			}

		}
	}
}
