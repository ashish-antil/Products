namespace ConnectionTest
{
	partial class MainForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this._ImardaReportSvc = new System.Windows.Forms.TextBox();
			this._MicrosoftReportingSvc = new System.Windows.Forms.TextBox();
			this._User = new System.Windows.Forms.TextBox();
			this._Password = new System.Windows.Forms.TextBox();
			this._Domain = new System.Windows.Forms.TextBox();
			this._Network = new System.Windows.Forms.RadioButton();
			this._Result = new System.Windows.Forms.TextBox();
			this._Connect = new System.Windows.Forms.Button();
			this._Default = new System.Windows.Forms.RadioButton();
			this.label7 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(21, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(138, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Imarda Report Service URL";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(21, 53);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(138, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Microsft Report Server URL";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(21, 109);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(29, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "User";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(21, 135);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(53, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Password";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(21, 160);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(43, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Domain";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(21, 81);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(81, 13);
			this.label6.TabIndex = 4;
			this.label6.Text = "Credential Type";
			// 
			// _ImardaReportSvc
			// 
			this._ImardaReportSvc.Location = new System.Drawing.Point(165, 24);
			this._ImardaReportSvc.Name = "_ImardaReportSvc";
			this._ImardaReportSvc.Size = new System.Drawing.Size(376, 20);
			this._ImardaReportSvc.TabIndex = 1;
			this._ImardaReportSvc.Text = "http://60.234.77.199:8100/ImardaReport";
			// 
			// _MicrosoftReportingSvc
			// 
			this._MicrosoftReportingSvc.Location = new System.Drawing.Point(165, 50);
			this._MicrosoftReportingSvc.Name = "_MicrosoftReportingSvc";
			this._MicrosoftReportingSvc.Size = new System.Drawing.Size(376, 20);
			this._MicrosoftReportingSvc.TabIndex = 3;
			this._MicrosoftReportingSvc.Text = "http://60.234.77.199:80/ReportServer/ReportService2005.asmx";
			// 
			// _User
			// 
			this._User.Location = new System.Drawing.Point(82, 106);
			this._User.Name = "_User";
			this._User.Size = new System.Drawing.Size(108, 20);
			this._User.TabIndex = 8;
			this._User.Text = "Administrator";
			// 
			// _Password
			// 
			this._Password.Location = new System.Drawing.Point(82, 132);
			this._Password.Name = "_Password";
			this._Password.Size = new System.Drawing.Size(108, 20);
			this._Password.TabIndex = 10;
			this._Password.Text = "imarda1234";
			// 
			// _Domain
			// 
			this._Domain.Location = new System.Drawing.Point(82, 157);
			this._Domain.Name = "_Domain";
			this._Domain.Size = new System.Drawing.Size(108, 20);
			this._Domain.TabIndex = 12;
			// 
			// _Network
			// 
			this._Network.AutoSize = true;
			this._Network.Checked = true;
			this._Network.Location = new System.Drawing.Point(165, 79);
			this._Network.Name = "_Network";
			this._Network.Size = new System.Drawing.Size(65, 17);
			this._Network.TabIndex = 5;
			this._Network.Text = "Network";
			this._Network.UseVisualStyleBackColor = true;
			// 
			// _Result
			// 
			this._Result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this._Result.Location = new System.Drawing.Point(24, 206);
			this._Result.Multiline = true;
			this._Result.Name = "_Result";
			this._Result.ReadOnly = true;
			this._Result.Size = new System.Drawing.Size(514, 90);
			this._Result.TabIndex = 15;
			// 
			// _Connect
			// 
			this._Connect.Location = new System.Drawing.Point(275, 146);
			this._Connect.Name = "_Connect";
			this._Connect.Size = new System.Drawing.Size(263, 31);
			this._Connect.TabIndex = 13;
			this._Connect.Text = "Don\'t be afraid, try it!";
			this._Connect.UseVisualStyleBackColor = true;
			this._Connect.Click += new System.EventHandler(this.Connect_Click);
			// 
			// _Default
			// 
			this._Default.AutoSize = true;
			this._Default.Location = new System.Drawing.Point(236, 79);
			this._Default.Name = "_Default";
			this._Default.Size = new System.Drawing.Size(99, 17);
			this._Default.TabIndex = 6;
			this._Default.Text = "Cached Default";
			this._Default.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(24, 187);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(37, 13);
			this.label7.TabIndex = 14;
			this.label7.Text = "Result";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(550, 308);
			this.Controls.Add(this.label7);
			this.Controls.Add(this._Connect);
			this.Controls.Add(this._Result);
			this.Controls.Add(this._Default);
			this.Controls.Add(this._Network);
			this.Controls.Add(this._Domain);
			this.Controls.Add(this._Password);
			this.Controls.Add(this._User);
			this.Controls.Add(this._MicrosoftReportingSvc);
			this.Controls.Add(this._ImardaReportSvc);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.MinimumSize = new System.Drawing.Size(562, 346);
			this.Name = "MainForm";
			this.Text = "Test Connection";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox _ImardaReportSvc;
		private System.Windows.Forms.TextBox _MicrosoftReportingSvc;
		private System.Windows.Forms.TextBox _User;
		private System.Windows.Forms.TextBox _Password;
		private System.Windows.Forms.TextBox _Domain;
		private System.Windows.Forms.RadioButton _Network;
		private System.Windows.Forms.TextBox _Result;
		private System.Windows.Forms.Button _Connect;
		private System.Windows.Forms.RadioButton _Default;
		private System.Windows.Forms.Label label7;
	}
}

