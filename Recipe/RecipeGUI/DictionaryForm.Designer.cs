namespace RecipeGUI
{
	partial class DictionaryForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grdDict = new System.Windows.Forms.DataGridView();
			this.colKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.grdDict)).BeginInit();
			this.SuspendLayout();
			// 
			// grdDict
			// 
			this.grdDict.AllowUserToAddRows = false;
			this.grdDict.AllowUserToDeleteRows = false;
			this.grdDict.AllowUserToResizeRows = false;
			this.grdDict.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grdDict.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colKey,
            this.colValue});
			this.grdDict.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdDict.Location = new System.Drawing.Point(0, 0);
			this.grdDict.Name = "grdDict";
			this.grdDict.RowHeadersVisible = false;
			this.grdDict.Size = new System.Drawing.Size(809, 428);
			this.grdDict.TabIndex = 0;
			this.grdDict.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdDict_CellDoubleClick);
			// 
			// colKey
			// 
			this.colKey.HeaderText = "Key";
			this.colKey.Name = "colKey";
			this.colKey.ReadOnly = true;
			this.colKey.Width = 160;
			// 
			// colValue
			// 
			this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colValue.HeaderText = "Value";
			this.colValue.Name = "colValue";
			this.colValue.ReadOnly = true;
			// 
			// DictionaryForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 11F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(809, 428);
			this.Controls.Add(this.grdDict);
			this.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "DictionaryForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Dictionary";
			((System.ComponentModel.ISupportInitialize)(this.grdDict)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView grdDict;
		private System.Windows.Forms.DataGridViewTextBoxColumn colKey;
		private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
	}
}