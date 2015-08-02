namespace RecipeGUI
{
	partial class SearchForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForm));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.txtTags = new System.Windows.Forms.TextBox();
			this.lstFiles = new System.Windows.Forms.ListBox();
			this.btnOpen = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSelectFolder = new System.Windows.Forms.Button();
			this.chkAll = new System.Windows.Forms.CheckBox();
			this.btnSearch = new System.Windows.Forms.Button();
			this.lblFoundTags = new System.Windows.Forms.Label();
			this.chkMarker = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Folder:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(12, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Tag(s):";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(13, 111);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Found in files:";
			// 
			// txtPath
			// 
			this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPath.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPath.Location = new System.Drawing.Point(97, 10);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(398, 18);
			this.txtPath.TabIndex = 3;
			this.txtPath.TextChanged += new System.EventHandler(this.ValidateInput);
			// 
			// txtTags
			// 
			this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTags.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTags.Location = new System.Drawing.Point(97, 39);
			this.txtTags.Name = "txtTags";
			this.txtTags.Size = new System.Drawing.Size(398, 18);
			this.txtTags.TabIndex = 3;
			this.txtTags.TextChanged += new System.EventHandler(this.ValidateInput);
			// 
			// lstFiles
			// 
			this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstFiles.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstFiles.FormattingEnabled = true;
			this.lstFiles.ItemHeight = 12;
			this.lstFiles.Location = new System.Drawing.Point(16, 127);
			this.lstFiles.Name = "lstFiles";
			this.lstFiles.Size = new System.Drawing.Size(530, 112);
			this.lstFiles.TabIndex = 4;
			this.lstFiles.SelectedIndexChanged += new System.EventHandler(this.lstFiles_SelectedIndexChanged);
			this.lstFiles.DoubleClick += new System.EventHandler(this.lstFiles_DoubleClick);
			// 
			// btnOpen
			// 
			this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpen.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOpen.Location = new System.Drawing.Point(390, 271);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(75, 23);
			this.btnOpen.TabIndex = 5;
			this.btnOpen.Text = "Open";
			this.btnOpen.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(471, 271);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnSelectFolder
			// 
			this.btnSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectFolder.Image")));
			this.btnSelectFolder.Location = new System.Drawing.Point(501, 8);
			this.btnSelectFolder.Name = "btnSelectFolder";
			this.btnSelectFolder.Size = new System.Drawing.Size(45, 23);
			this.btnSelectFolder.TabIndex = 6;
			this.btnSelectFolder.UseVisualStyleBackColor = true;
			this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
			// 
			// chkAll
			// 
			this.chkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkAll.AutoSize = true;
			this.chkAll.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkAll.Location = new System.Drawing.Point(501, 41);
			this.chkAll.Name = "chkAll";
			this.chkAll.Size = new System.Drawing.Size(45, 16);
			this.chkAll.TabIndex = 7;
			this.chkAll.Text = "All";
			this.chkAll.UseVisualStyleBackColor = true;
			// 
			// btnSearch
			// 
			this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
			this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnSearch.Location = new System.Drawing.Point(97, 76);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(85, 23);
			this.btnSearch.TabIndex = 8;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// lblFoundTags
			// 
			this.lblFoundTags.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblFoundTags.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFoundTags.Location = new System.Drawing.Point(12, 263);
			this.lblFoundTags.Name = "lblFoundTags";
			this.lblFoundTags.Size = new System.Drawing.Size(282, 41);
			this.lblFoundTags.TabIndex = 9;
			this.lblFoundTags.Text = "___";
			// 
			// chkMarker
			// 
			this.chkMarker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkMarker.AutoSize = true;
			this.chkMarker.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkMarker.Location = new System.Drawing.Point(438, 63);
			this.chkMarker.Name = "chkMarker";
			this.chkMarker.Size = new System.Drawing.Size(108, 16);
			this.chkMarker.TabIndex = 7;
			this.chkMarker.Text = "Also #marker";
			this.chkMarker.UseVisualStyleBackColor = true;
			// 
			// SearchForm
			// 
			this.AcceptButton = this.btnOpen;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(571, 306);
			this.Controls.Add(this.lblFoundTags);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.chkMarker);
			this.Controls.Add(this.chkAll);
			this.Controls.Add(this.btnSelectFolder);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOpen);
			this.Controls.Add(this.lstFiles);
			this.Controls.Add(this.txtTags);
			this.Controls.Add(this.txtPath);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "SearchForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Search Tags In Recipe Files";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtPath;
		private System.Windows.Forms.TextBox txtTags;
		private System.Windows.Forms.ListBox lstFiles;
		private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSelectFolder;
		private System.Windows.Forms.CheckBox chkAll;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Label lblFoundTags;
		private System.Windows.Forms.CheckBox chkMarker;
	}
}