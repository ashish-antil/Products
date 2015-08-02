namespace RecipeGUI
{
	partial class InputForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputForm));
			this.lblPrompt = new System.Windows.Forms.Label();
			this.txtInput = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnFiles = new System.Windows.Forms.Button();
			this.dlgSelectFiles = new System.Windows.Forms.OpenFileDialog();
			this.btnFolder = new System.Windows.Forms.Button();
			this.dlgSelectFolder = new System.Windows.Forms.FolderBrowserDialog();
			this.SuspendLayout();
			// 
			// lblPrompt
			// 
			this.lblPrompt.AutoSize = true;
			this.lblPrompt.Location = new System.Drawing.Point(13, 13);
			this.lblPrompt.Name = "lblPrompt";
			this.lblPrompt.Size = new System.Drawing.Size(45, 13);
			this.lblPrompt.TabIndex = 0;
			this.lblPrompt.Text = "(prompt)";
			// 
			// txtInput
			// 
			this.txtInput.AcceptsReturn = true;
			this.txtInput.AcceptsTab = true;
			this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtInput.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtInput.Location = new System.Drawing.Point(16, 57);
			this.txtInput.Multiline = true;
			this.txtInput.Name = "txtInput";
			this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtInput.Size = new System.Drawing.Size(490, 65);
			this.txtInput.TabIndex = 1;
			this.txtInput.Visible = false;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(336, 131);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(431, 131);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnFiles
			// 
			this.btnFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnFiles.Image = ((System.Drawing.Image)(resources.GetObject("btnFiles.Image")));
			this.btnFiles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnFiles.Location = new System.Drawing.Point(16, 131);
			this.btnFiles.Name = "btnFiles";
			this.btnFiles.Size = new System.Drawing.Size(75, 23);
			this.btnFiles.TabIndex = 2;
			this.btnFiles.Text = "&Files";
			this.btnFiles.UseVisualStyleBackColor = true;
			this.btnFiles.Visible = false;
			this.btnFiles.Click += new System.EventHandler(this.btnFiles_Click);
			// 
			// dlgSelectFiles
			// 
			this.dlgSelectFiles.AddExtension = false;
			this.dlgSelectFiles.Filter = "All|*|Text|*.txt";
			this.dlgSelectFiles.Multiselect = true;
			this.dlgSelectFiles.Title = "Select File(s)";
			// 
			// btnFolder
			// 
			this.btnFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnFolder.Image")));
			this.btnFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnFolder.Location = new System.Drawing.Point(111, 131);
			this.btnFolder.Name = "btnFolder";
			this.btnFolder.Size = new System.Drawing.Size(75, 23);
			this.btnFolder.TabIndex = 3;
			this.btnFolder.Text = "Fol&der";
			this.btnFolder.UseVisualStyleBackColor = true;
			this.btnFolder.Visible = false;
			this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
			// 
			// InputForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(518, 166);
			this.Controls.Add(this.btnFolder);
			this.Controls.Add(this.btnFiles);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtInput);
			this.Controls.Add(this.lblPrompt);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "InputForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Input";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InputForm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblPrompt;
		private System.Windows.Forms.TextBox txtInput;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnFiles;
		private System.Windows.Forms.OpenFileDialog dlgSelectFiles;
		private System.Windows.Forms.Button btnFolder;
		private System.Windows.Forms.FolderBrowserDialog dlgSelectFolder;


	}
}