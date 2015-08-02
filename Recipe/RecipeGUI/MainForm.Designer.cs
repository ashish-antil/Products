namespace RecipeGUI
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this._MainMenu = new System.Windows.Forms.MenuStrip();
			this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mniNewRecipe = new System.Windows.Forms.ToolStripMenuItem();
			this.mniOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSaveRecipe = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSaveRecipeAs = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSaveLog = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCopyLog = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCopyRecipePath = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.mniEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.mniNotepad = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSearchTags = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mniExit = new System.Windows.Forms.ToolStripMenuItem();
			this.sepLastUsed = new System.Windows.Forms.ToolStripSeparator();
			this.mniLastOpened = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRun = new System.Windows.Forms.ToolStripMenuItem();
			this.mniClearMacros = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSetParameters = new System.Windows.Forms.ToolStripMenuItem();
			this.mniTestOnly = new System.Windows.Forms.ToolStripMenuItem();
			this.mniRun = new System.Windows.Forms.ToolStripMenuItem();
			this.mniStop = new System.Windows.Forms.ToolStripMenuItem();
			this.mniStopServer = new System.Windows.Forms.ToolStripMenuItem();
			this.mniStopWatcher = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mniExpandClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.mniRunClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSelectedMacro = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuWindow = new System.Windows.Forms.ToolStripMenuItem();
			this.mniAlwaysOnTop = new System.Windows.Forms.ToolStripMenuItem();
			this.mniDictionary = new System.Windows.Forms.ToolStripMenuItem();
			this.mniGlobalDictionary = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mniLog = new System.Windows.Forms.ToolStripMenuItem();
			this.mniWrite = new System.Windows.Forms.ToolStripMenuItem();
			this.mniWarn = new System.Windows.Forms.ToolStripMenuItem();
			this.mniInteractive = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.mniFolderSelectedFile = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCopyPath = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.mniFind = new System.Windows.Forms.ToolStripMenuItem();
			this.mniFindNext = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.mniAddSelection = new System.Windows.Forms.ToolStripMenuItem();
			this.mniRemoveAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mniRemoveLast = new System.Windows.Forms.ToolStripMenuItem();
			this.sepAddSelection = new System.Windows.Forms.ToolStripSeparator();
			this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.mniAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCommands = new System.Windows.Forms.ToolStripMenuItem();
			this.mniWith = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.mniEscapeRegex = new System.Windows.Forms.ToolStripMenuItem();
			this.mniEnclose = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.mniAutocompletion = new System.Windows.Forms.ToolStripMenuItem();
			this.mniArgChecks = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCheckDeprecated = new System.Windows.Forms.ToolStripMenuItem();
			this.txtRecipe = new System.Windows.Forms.TextBox();
			this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
			this.dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.label1 = new System.Windows.Forms.Label();
			this.lblRunning = new System.Windows.Forms.Label();
			this.lblMode = new System.Windows.Forms.Label();
			this.tabPages = new System.Windows.Forms.TabControl();
			this.tabLog = new System.Windows.Forms.TabPage();
			this.txtLog = new System.Windows.Forms.TextBox();
			this.tabModified = new System.Windows.Forms.TabPage();
			this.lstModified = new System.Windows.Forms.ListBox();
			this.tabCreated = new System.Windows.Forms.TabPage();
			this.lstNew = new System.Windows.Forms.ListBox();
			this.tabNotepad = new System.Windows.Forms.TabPage();
			this.btnCopyNotepad = new System.Windows.Forms.Button();
			this.btnClearNotepad = new System.Windows.Forms.Button();
			this.btnRunNotepad = new System.Windows.Forms.Button();
			this.txtNotepad = new System.Windows.Forms.TextBox();
			this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
			this.lblVersion = new System.Windows.Forms.Label();
			this._MainMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabPages.SuspendLayout();
			this.tabLog.SuspendLayout();
			this.tabModified.SuspendLayout();
			this.tabCreated.SuspendLayout();
			this.tabNotepad.SuspendLayout();
			this.SuspendLayout();
			// 
			// _MainMenu
			// 
			this._MainMenu.BackColor = System.Drawing.Color.LightGray;
			this._MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuRun,
            this.mnuWindow,
            this.mnuPaste,
            this.mnuHelp});
			this._MainMenu.Location = new System.Drawing.Point(0, 0);
			this._MainMenu.Name = "_MainMenu";
			this._MainMenu.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
			this._MainMenu.Size = new System.Drawing.Size(1066, 24);
			this._MainMenu.TabIndex = 0;
			this._MainMenu.Text = "mnuMain";
			// 
			// mnuFile
			// 
			this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniNewRecipe,
            this.mniOpen,
            this.mniSaveRecipe,
            this.mniSaveRecipeAs,
            this.mniSaveLog,
            this.mniCopyLog,
            this.mniCopyRecipePath,
            this.toolStripMenuItem4,
            this.mniEdit,
            this.mniNotepad,
            this.mniSearchTags,
            this.toolStripMenuItem1,
            this.mniExit,
            this.sepLastUsed,
            this.mniLastOpened});
			this.mnuFile.Name = "mnuFile";
			this.mnuFile.Size = new System.Drawing.Size(37, 20);
			this.mnuFile.Text = "&File";
			// 
			// mniNewRecipe
			// 
			this.mniNewRecipe.Name = "mniNewRecipe";
			this.mniNewRecipe.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.mniNewRecipe.Size = new System.Drawing.Size(200, 22);
			this.mniNewRecipe.Text = "&New Recipe";
			this.mniNewRecipe.Click += new System.EventHandler(this.mniNewRecipe_Click);
			// 
			// mniOpen
			// 
			this.mniOpen.Image = ((System.Drawing.Image)(resources.GetObject("mniOpen.Image")));
			this.mniOpen.Name = "mniOpen";
			this.mniOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mniOpen.Size = new System.Drawing.Size(200, 22);
			this.mniOpen.Text = "&Open Recipe";
			this.mniOpen.Click += new System.EventHandler(this.mniOpen_Click);
			// 
			// mniSaveRecipe
			// 
			this.mniSaveRecipe.Image = ((System.Drawing.Image)(resources.GetObject("mniSaveRecipe.Image")));
			this.mniSaveRecipe.Name = "mniSaveRecipe";
			this.mniSaveRecipe.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mniSaveRecipe.Size = new System.Drawing.Size(200, 22);
			this.mniSaveRecipe.Text = "&Save Recipe";
			this.mniSaveRecipe.Click += new System.EventHandler(this.mniSaveRecipe_Click);
			// 
			// mniSaveRecipeAs
			// 
			this.mniSaveRecipeAs.Name = "mniSaveRecipeAs";
			this.mniSaveRecipeAs.Size = new System.Drawing.Size(200, 22);
			this.mniSaveRecipeAs.Text = "Save Recipe &As ...";
			this.mniSaveRecipeAs.Click += new System.EventHandler(this.mniSaveRecipeAs_Click);
			// 
			// mniSaveLog
			// 
			this.mniSaveLog.Name = "mniSaveLog";
			this.mniSaveLog.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.mniSaveLog.Size = new System.Drawing.Size(200, 22);
			this.mniSaveLog.Text = "Save &Log";
			this.mniSaveLog.Click += new System.EventHandler(this.mniSaveLog_Click);
			// 
			// mniCopyLog
			// 
			this.mniCopyLog.Image = ((System.Drawing.Image)(resources.GetObject("mniCopyLog.Image")));
			this.mniCopyLog.Name = "mniCopyLog";
			this.mniCopyLog.Size = new System.Drawing.Size(200, 22);
			this.mniCopyLog.Text = "&Copy Log";
			this.mniCopyLog.Click += new System.EventHandler(this.mniCopyLog_Click);
			// 
			// mniCopyRecipePath
			// 
			this.mniCopyRecipePath.Name = "mniCopyRecipePath";
			this.mniCopyRecipePath.Size = new System.Drawing.Size(200, 22);
			this.mniCopyRecipePath.Text = "Copy Recipe &Path";
			this.mniCopyRecipePath.Click += new System.EventHandler(this.mniCopyRecipePath_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(197, 6);
			// 
			// mniEdit
			// 
			this.mniEdit.Name = "mniEdit";
			this.mniEdit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F2)));
			this.mniEdit.Size = new System.Drawing.Size(200, 22);
			this.mniEdit.Text = "Open in &Editor";
			this.mniEdit.Click += new System.EventHandler(this.mniEdit_Click);
			// 
			// mniNotepad
			// 
			this.mniNotepad.Name = "mniNotepad";
			this.mniNotepad.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.mniNotepad.Size = new System.Drawing.Size(200, 22);
			this.mniNotepad.Text = "Open in Notepa&d++";
			this.mniNotepad.Click += new System.EventHandler(this.mniNotepad_Click);
			// 
			// mniSearchTags
			// 
			this.mniSearchTags.Name = "mniSearchTags";
			this.mniSearchTags.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
			this.mniSearchTags.Size = new System.Drawing.Size(200, 22);
			this.mniSearchTags.Text = "Search #&tags";
			this.mniSearchTags.Click += new System.EventHandler(this.mniSearchTags_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(197, 6);
			// 
			// mniExit
			// 
			this.mniExit.Name = "mniExit";
			this.mniExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.mniExit.Size = new System.Drawing.Size(200, 22);
			this.mniExit.Text = "E&xit";
			this.mniExit.Click += new System.EventHandler(this.mniExit_Click);
			// 
			// sepLastUsed
			// 
			this.sepLastUsed.Name = "sepLastUsed";
			this.sepLastUsed.Size = new System.Drawing.Size(197, 6);
			// 
			// mniLastOpened
			// 
			this.mniLastOpened.Name = "mniLastOpened";
			this.mniLastOpened.Size = new System.Drawing.Size(200, 22);
			this.mniLastOpened.Click += new System.EventHandler(this.mniLastOpened_Click);
			// 
			// mnuRun
			// 
			this.mnuRun.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniClearMacros,
            this.mniSetParameters,
            this.mniTestOnly,
            this.mniRun,
            this.mniStop,
            this.mniStopServer,
            this.mniStopWatcher,
            this.toolStripMenuItem2,
            this.mniExpandClipboard,
            this.mniRunClipboard,
            this.mniSelectedMacro});
			this.mnuRun.Name = "mnuRun";
			this.mnuRun.Size = new System.Drawing.Size(40, 20);
			this.mnuRun.Text = "&Run";
			// 
			// mniClearMacros
			// 
			this.mniClearMacros.Name = "mniClearMacros";
			this.mniClearMacros.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
			this.mniClearMacros.Size = new System.Drawing.Size(218, 22);
			this.mniClearMacros.Text = "&Clear Macros";
			this.mniClearMacros.Click += new System.EventHandler(this.mniClearMacros_Click);
			// 
			// mniSetParameters
			// 
			this.mniSetParameters.Name = "mniSetParameters";
			this.mniSetParameters.ShortcutKeys = System.Windows.Forms.Keys.F4;
			this.mniSetParameters.Size = new System.Drawing.Size(218, 22);
			this.mniSetParameters.Text = "Set &Parameters";
			this.mniSetParameters.Click += new System.EventHandler(this.mniSetParameters_Click);
			// 
			// mniTestOnly
			// 
			this.mniTestOnly.Checked = true;
			this.mniTestOnly.CheckOnClick = true;
			this.mniTestOnly.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniTestOnly.Name = "mniTestOnly";
			this.mniTestOnly.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.mniTestOnly.Size = new System.Drawing.Size(218, 22);
			this.mniTestOnly.Text = "&Test Only";
			this.mniTestOnly.Click += new System.EventHandler(this.mniTestOnly_Click);
			// 
			// mniRun
			// 
			this.mniRun.Image = ((System.Drawing.Image)(resources.GetObject("mniRun.Image")));
			this.mniRun.Name = "mniRun";
			this.mniRun.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.mniRun.Size = new System.Drawing.Size(218, 22);
			this.mniRun.Text = "&Run";
			this.mniRun.Click += new System.EventHandler(this.mniRun_Click);
			// 
			// mniStop
			// 
			this.mniStop.Enabled = false;
			this.mniStop.Image = ((System.Drawing.Image)(resources.GetObject("mniStop.Image")));
			this.mniStop.Name = "mniStop";
			this.mniStop.Size = new System.Drawing.Size(218, 22);
			this.mniStop.Text = "Stop";
			this.mniStop.Click += new System.EventHandler(this.mniStop_Click);
			// 
			// mniStopServer
			// 
			this.mniStopServer.Image = ((System.Drawing.Image)(resources.GetObject("mniStopServer.Image")));
			this.mniStopServer.Name = "mniStopServer";
			this.mniStopServer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.mniStopServer.Size = new System.Drawing.Size(218, 22);
			this.mniStopServer.Text = "&Stop Server";
			this.mniStopServer.Visible = false;
			this.mniStopServer.Click += new System.EventHandler(this.mniStopServer_Click);
			// 
			// mniStopWatcher
			// 
			this.mniStopWatcher.Name = "mniStopWatcher";
			this.mniStopWatcher.Size = new System.Drawing.Size(218, 22);
			this.mniStopWatcher.Text = "Stop &File Watcher";
			this.mniStopWatcher.Visible = false;
			this.mniStopWatcher.Click += new System.EventHandler(this.mniStopWatcher_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(215, 6);
			// 
			// mniExpandClipboard
			// 
			this.mniExpandClipboard.Name = "mniExpandClipboard";
			this.mniExpandClipboard.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
			this.mniExpandClipboard.Size = new System.Drawing.Size(218, 22);
			this.mniExpandClipboard.Text = "E&xpand Clipboard";
			this.mniExpandClipboard.Click += new System.EventHandler(this.mniExpandClipboard_Click);
			// 
			// mniRunClipboard
			// 
			this.mniRunClipboard.Name = "mniRunClipboard";
			this.mniRunClipboard.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
			this.mniRunClipboard.Size = new System.Drawing.Size(218, 22);
			this.mniRunClipboard.Text = "Run Clipboard";
			this.mniRunClipboard.Click += new System.EventHandler(this.mniRunClipboard_Click);
			// 
			// mniSelectedMacro
			// 
			this.mniSelectedMacro.Name = "mniSelectedMacro";
			this.mniSelectedMacro.ShortcutKeys = System.Windows.Forms.Keys.F8;
			this.mniSelectedMacro.Size = new System.Drawing.Size(218, 22);
			this.mniSelectedMacro.Text = "Selected Macro";
			this.mniSelectedMacro.Click += new System.EventHandler(this.mniSelectedMacro_Click);
			// 
			// mnuWindow
			// 
			this.mnuWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniAlwaysOnTop,
            this.mniDictionary,
            this.mniGlobalDictionary,
            this.toolStripMenuItem3,
            this.mniLog,
            this.mniWrite,
            this.mniWarn,
            this.mniInteractive,
            this.toolStripMenuItem6,
            this.mniFolderSelectedFile,
            this.mniCopyPath,
            this.toolStripMenuItem7,
            this.mniFind,
            this.mniFindNext});
			this.mnuWindow.Name = "mnuWindow";
			this.mnuWindow.Size = new System.Drawing.Size(63, 20);
			this.mnuWindow.Text = "&Window";
			// 
			// mniAlwaysOnTop
			// 
			this.mniAlwaysOnTop.CheckOnClick = true;
			this.mniAlwaysOnTop.Name = "mniAlwaysOnTop";
			this.mniAlwaysOnTop.Size = new System.Drawing.Size(229, 22);
			this.mniAlwaysOnTop.Text = "Always on &Top";
			this.mniAlwaysOnTop.Click += new System.EventHandler(this.mniAlwaysOnTop_Click);
			// 
			// mniDictionary
			// 
			this.mniDictionary.Image = ((System.Drawing.Image)(resources.GetObject("mniDictionary.Image")));
			this.mniDictionary.Name = "mniDictionary";
			this.mniDictionary.ShortcutKeys = System.Windows.Forms.Keys.F7;
			this.mniDictionary.Size = new System.Drawing.Size(229, 22);
			this.mniDictionary.Text = "&Dictionary";
			this.mniDictionary.Click += new System.EventHandler(this.mniDictionary_Click);
			// 
			// mniGlobalDictionary
			// 
			this.mniGlobalDictionary.Name = "mniGlobalDictionary";
			this.mniGlobalDictionary.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F7)));
			this.mniGlobalDictionary.Size = new System.Drawing.Size(229, 22);
			this.mniGlobalDictionary.Text = "&Global Dictionary";
			this.mniGlobalDictionary.Click += new System.EventHandler(this.mniGlobalDictionary_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(226, 6);
			// 
			// mniLog
			// 
			this.mniLog.Checked = true;
			this.mniLog.CheckOnClick = true;
			this.mniLog.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniLog.Name = "mniLog";
			this.mniLog.Size = new System.Drawing.Size(229, 22);
			this.mniLog.Text = "Log";
			// 
			// mniWrite
			// 
			this.mniWrite.Checked = true;
			this.mniWrite.CheckOnClick = true;
			this.mniWrite.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniWrite.Name = "mniWrite";
			this.mniWrite.Size = new System.Drawing.Size(229, 22);
			this.mniWrite.Text = "Write";
			// 
			// mniWarn
			// 
			this.mniWarn.Checked = true;
			this.mniWarn.CheckOnClick = true;
			this.mniWarn.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniWarn.Name = "mniWarn";
			this.mniWarn.Size = new System.Drawing.Size(229, 22);
			this.mniWarn.Text = "Warn";
			// 
			// mniInteractive
			// 
			this.mniInteractive.Checked = true;
			this.mniInteractive.CheckOnClick = true;
			this.mniInteractive.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniInteractive.Name = "mniInteractive";
			this.mniInteractive.Size = new System.Drawing.Size(229, 22);
			this.mniInteractive.Text = "Interactive";
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(226, 6);
			// 
			// mniFolderSelectedFile
			// 
			this.mniFolderSelectedFile.Name = "mniFolderSelectedFile";
			this.mniFolderSelectedFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.mniFolderSelectedFile.Size = new System.Drawing.Size(229, 22);
			this.mniFolderSelectedFile.Text = "Folder of Selected File";
			this.mniFolderSelectedFile.Click += new System.EventHandler(this.mniFolderSelectedFile_Click);
			// 
			// mniCopyPath
			// 
			this.mniCopyPath.Name = "mniCopyPath";
			this.mniCopyPath.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
			this.mniCopyPath.Size = new System.Drawing.Size(229, 22);
			this.mniCopyPath.Text = "Copy Path";
			this.mniCopyPath.Click += new System.EventHandler(this.mniCopyPath_Click);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(226, 6);
			// 
			// mniFind
			// 
			this.mniFind.Name = "mniFind";
			this.mniFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.mniFind.Size = new System.Drawing.Size(229, 22);
			this.mniFind.Text = "Find";
			this.mniFind.Click += new System.EventHandler(this.mniFind_Click);
			// 
			// mniFindNext
			// 
			this.mniFindNext.Name = "mniFindNext";
			this.mniFindNext.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.mniFindNext.Size = new System.Drawing.Size(229, 22);
			this.mniFindNext.Text = "Find Next";
			this.mniFindNext.Click += new System.EventHandler(this.mniFindNext_Click);
			// 
			// mnuPaste
			// 
			this.mnuPaste.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniAddSelection,
            this.mniRemoveAll,
            this.mniRemoveLast,
            this.sepAddSelection});
			this.mnuPaste.Name = "mnuPaste";
			this.mnuPaste.Size = new System.Drawing.Size(47, 20);
			this.mnuPaste.Text = "&Paste";
			// 
			// mniAddSelection
			// 
			this.mniAddSelection.Name = "mniAddSelection";
			this.mniAddSelection.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
			this.mniAddSelection.Size = new System.Drawing.Size(241, 22);
			this.mniAddSelection.Text = "&Add Selected Text";
			this.mniAddSelection.Click += new System.EventHandler(this.mniAddSelection_Click);
			// 
			// mniRemoveAll
			// 
			this.mniRemoveAll.Name = "mniRemoveAll";
			this.mniRemoveAll.Size = new System.Drawing.Size(241, 22);
			this.mniRemoveAll.Text = "Remove All";
			this.mniRemoveAll.Click += new System.EventHandler(this.mniClearMenu_Click);
			// 
			// mniRemoveLast
			// 
			this.mniRemoveLast.Name = "mniRemoveLast";
			this.mniRemoveLast.Size = new System.Drawing.Size(241, 22);
			this.mniRemoveLast.Text = "Remove Last";
			this.mniRemoveLast.Click += new System.EventHandler(this.mniRemoveLast_Click);
			// 
			// sepAddSelection
			// 
			this.sepAddSelection.Name = "sepAddSelection";
			this.sepAddSelection.Size = new System.Drawing.Size(238, 6);
			// 
			// mnuHelp
			// 
			this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniAbout,
            this.mniCommands,
            this.mniWith,
            this.toolStripMenuItem5,
            this.mniEscapeRegex,
            this.mniEnclose,
            this.toolStripMenuItem8,
            this.mniAutocompletion,
            this.mniArgChecks,
            this.mniCheckDeprecated});
			this.mnuHelp.Name = "mnuHelp";
			this.mnuHelp.Size = new System.Drawing.Size(44, 20);
			this.mnuHelp.Text = "&Help";
			// 
			// mniAbout
			// 
			this.mniAbout.Name = "mniAbout";
			this.mniAbout.Size = new System.Drawing.Size(228, 22);
			this.mniAbout.Text = "&Version";
			this.mniAbout.Click += new System.EventHandler(this.Version_Click);
			// 
			// mniCommands
			// 
			this.mniCommands.Name = "mniCommands";
			this.mniCommands.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.mniCommands.Size = new System.Drawing.Size(228, 22);
			this.mniCommands.Text = "&Commands";
			this.mniCommands.Click += new System.EventHandler(this.mniCommands_Click);
			// 
			// mniWith
			// 
			this.mniWith.Name = "mniWith";
			this.mniWith.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
			this.mniWith.Size = new System.Drawing.Size(228, 22);
			this.mniWith.Text = "#with Operations";
			this.mniWith.Click += new System.EventHandler(this.mniWith_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(225, 6);
			// 
			// mniEscapeRegex
			// 
			this.mniEscapeRegex.Name = "mniEscapeRegex";
			this.mniEscapeRegex.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.mniEscapeRegex.Size = new System.Drawing.Size(228, 22);
			this.mniEscapeRegex.Text = "Escape Regex";
			this.mniEscapeRegex.Click += new System.EventHandler(this.mniEscapeRegex_Click);
			// 
			// mniEnclose
			// 
			this.mniEnclose.Name = "mniEnclose";
			this.mniEnclose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
			this.mniEnclose.Size = new System.Drawing.Size(228, 22);
			this.mniEnclose.Text = "Insert `` or Enclose `...`";
			this.mniEnclose.Click += new System.EventHandler(this.mniEnclose_Click);
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Size = new System.Drawing.Size(225, 6);
			// 
			// mniAutocompletion
			// 
			this.mniAutocompletion.Checked = true;
			this.mniAutocompletion.CheckOnClick = true;
			this.mniAutocompletion.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniAutocompletion.Name = "mniAutocompletion";
			this.mniAutocompletion.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F1)));
			this.mniAutocompletion.Size = new System.Drawing.Size(228, 22);
			this.mniAutocompletion.Text = "Auto #end";
			this.mniAutocompletion.Click += new System.EventHandler(this.mniAutocompletion_Click);
			// 
			// mniArgChecks
			// 
			this.mniArgChecks.CheckOnClick = true;
			this.mniArgChecks.Name = "mniArgChecks";
			this.mniArgChecks.Size = new System.Drawing.Size(228, 22);
			this.mniArgChecks.Text = "Argument checks";
			// 
			// mniCheckDeprecated
			// 
			this.mniCheckDeprecated.Checked = true;
			this.mniCheckDeprecated.CheckOnClick = true;
			this.mniCheckDeprecated.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mniCheckDeprecated.Name = "mniCheckDeprecated";
			this.mniCheckDeprecated.Size = new System.Drawing.Size(228, 22);
			this.mniCheckDeprecated.Text = "Check deprecated";
			// 
			// txtRecipe
			// 
			this.txtRecipe.AcceptsReturn = true;
			this.txtRecipe.AcceptsTab = true;
			this.txtRecipe.AllowDrop = true;
			this.txtRecipe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRecipe.BackColor = System.Drawing.SystemColors.Window;
			this.txtRecipe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtRecipe.Location = new System.Drawing.Point(3, 18);
			this.txtRecipe.MaxLength = 200000;
			this.txtRecipe.Multiline = true;
			this.txtRecipe.Name = "txtRecipe";
			this.txtRecipe.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtRecipe.Size = new System.Drawing.Size(1063, 314);
			this.txtRecipe.TabIndex = 1;
			this.txtRecipe.WordWrap = false;
			this.txtRecipe.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtRecipe_DragDrop);
			this.txtRecipe.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtRecipe_DragEnter);
			this.txtRecipe.Enter += new System.EventHandler(this.txt_Enter);
			// 
			// dlgOpenFile
			// 
			this.dlgOpenFile.DefaultExt = "rcp";
			this.dlgOpenFile.Filter = "All|*.*|Recipe|*.rcp|Text|*.txt";
			this.dlgOpenFile.FilterIndex = 2;
			// 
			// dlgFolder
			// 
			this.dlgFolder.Description = "Recipe Folder";
			// 
			// splitContainer1
			// 
			this.splitContainer1.BackColor = System.Drawing.Color.Beige;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
			this.splitContainer1.Panel1.Controls.Add(this.lblVersion);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.lblRunning);
			this.splitContainer1.Panel1.Controls.Add(this.lblMode);
			this.splitContainer1.Panel1.Controls.Add(this.txtRecipe);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tabPages);
			this.splitContainer1.Size = new System.Drawing.Size(1066, 521);
			this.splitContainer1.SplitterDistance = 333;
			this.splitContainer1.SplitterWidth = 10;
			this.splitContainer1.TabIndex = 10;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(47, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "Recipe";
			// 
			// lblRunning
			// 
			this.lblRunning.AutoSize = true;
			this.lblRunning.BackColor = System.Drawing.Color.Cyan;
			this.lblRunning.Location = new System.Drawing.Point(72, 3);
			this.lblRunning.Name = "lblRunning";
			this.lblRunning.Size = new System.Drawing.Size(54, 12);
			this.lblRunning.TabIndex = 2;
			this.lblRunning.Text = "RUNNING";
			this.lblRunning.Visible = false;
			// 
			// lblMode
			// 
			this.lblMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblMode.AutoSize = true;
			this.lblMode.BackColor = System.Drawing.Color.Lime;
			this.lblMode.Location = new System.Drawing.Point(1021, 3);
			this.lblMode.Name = "lblMode";
			this.lblMode.Size = new System.Drawing.Size(33, 12);
			this.lblMode.TabIndex = 2;
			this.lblMode.Text = "TEST";
			// 
			// tabPages
			// 
			this.tabPages.Controls.Add(this.tabLog);
			this.tabPages.Controls.Add(this.tabModified);
			this.tabPages.Controls.Add(this.tabCreated);
			this.tabPages.Controls.Add(this.tabNotepad);
			this.tabPages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabPages.Location = new System.Drawing.Point(0, 0);
			this.tabPages.Name = "tabPages";
			this.tabPages.SelectedIndex = 0;
			this.tabPages.Size = new System.Drawing.Size(1066, 178);
			this.tabPages.TabIndex = 3;
			// 
			// tabLog
			// 
			this.tabLog.Controls.Add(this.txtLog);
			this.tabLog.Location = new System.Drawing.Point(4, 22);
			this.tabLog.Name = "tabLog";
			this.tabLog.Padding = new System.Windows.Forms.Padding(3);
			this.tabLog.Size = new System.Drawing.Size(1058, 152);
			this.tabLog.TabIndex = 0;
			this.tabLog.Text = "Log";
			this.tabLog.UseVisualStyleBackColor = true;
			// 
			// txtLog
			// 
			this.txtLog.BackColor = System.Drawing.Color.Ivory;
			this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLog.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtLog.Location = new System.Drawing.Point(3, 3);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtLog.Size = new System.Drawing.Size(1052, 146);
			this.txtLog.TabIndex = 1;
			// 
			// tabModified
			// 
			this.tabModified.Controls.Add(this.lstModified);
			this.tabModified.Location = new System.Drawing.Point(4, 22);
			this.tabModified.Name = "tabModified";
			this.tabModified.Padding = new System.Windows.Forms.Padding(3);
			this.tabModified.Size = new System.Drawing.Size(1058, 152);
			this.tabModified.TabIndex = 1;
			this.tabModified.Text = "Modified";
			this.tabModified.UseVisualStyleBackColor = true;
			// 
			// lstModified
			// 
			this.lstModified.BackColor = System.Drawing.Color.Ivory;
			this.lstModified.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lstModified.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstModified.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstModified.FormattingEnabled = true;
			this.lstModified.HorizontalScrollbar = true;
			this.lstModified.ItemHeight = 16;
			this.lstModified.Location = new System.Drawing.Point(3, 3);
			this.lstModified.Name = "lstModified";
			this.lstModified.Size = new System.Drawing.Size(1052, 146);
			this.lstModified.TabIndex = 1;
			this.lstModified.DoubleClick += new System.EventHandler(this.ShowFile);
			// 
			// tabCreated
			// 
			this.tabCreated.Controls.Add(this.lstNew);
			this.tabCreated.Location = new System.Drawing.Point(4, 22);
			this.tabCreated.Name = "tabCreated";
			this.tabCreated.Padding = new System.Windows.Forms.Padding(3);
			this.tabCreated.Size = new System.Drawing.Size(1058, 152);
			this.tabCreated.TabIndex = 2;
			this.tabCreated.Text = "Created";
			this.tabCreated.UseVisualStyleBackColor = true;
			// 
			// lstNew
			// 
			this.lstNew.BackColor = System.Drawing.Color.Ivory;
			this.lstNew.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lstNew.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstNew.FormattingEnabled = true;
			this.lstNew.HorizontalScrollbar = true;
			this.lstNew.ItemHeight = 16;
			this.lstNew.Location = new System.Drawing.Point(3, 3);
			this.lstNew.Name = "lstNew";
			this.lstNew.Size = new System.Drawing.Size(1052, 146);
			this.lstNew.TabIndex = 3;
			this.lstNew.DoubleClick += new System.EventHandler(this.ShowFile);
			// 
			// tabNotepad
			// 
			this.tabNotepad.Controls.Add(this.btnCopyNotepad);
			this.tabNotepad.Controls.Add(this.btnClearNotepad);
			this.tabNotepad.Controls.Add(this.btnRunNotepad);
			this.tabNotepad.Controls.Add(this.txtNotepad);
			this.tabNotepad.Location = new System.Drawing.Point(4, 22);
			this.tabNotepad.Name = "tabNotepad";
			this.tabNotepad.Padding = new System.Windows.Forms.Padding(3);
			this.tabNotepad.Size = new System.Drawing.Size(1058, 152);
			this.tabNotepad.TabIndex = 3;
			this.tabNotepad.Text = "Notepad";
			this.tabNotepad.UseVisualStyleBackColor = true;
			// 
			// btnCopyNotepad
			// 
			this.btnCopyNotepad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCopyNotepad.Location = new System.Drawing.Point(7, 75);
			this.btnCopyNotepad.Name = "btnCopyNotepad";
			this.btnCopyNotepad.Size = new System.Drawing.Size(75, 23);
			this.btnCopyNotepad.TabIndex = 2;
			this.btnCopyNotepad.Text = "Copy";
			this.btnCopyNotepad.UseVisualStyleBackColor = true;
			this.btnCopyNotepad.Click += new System.EventHandler(this.btnCopyNotepad_Click);
			// 
			// btnClearNotepad
			// 
			this.btnClearNotepad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnClearNotepad.Location = new System.Drawing.Point(7, 46);
			this.btnClearNotepad.Name = "btnClearNotepad";
			this.btnClearNotepad.Size = new System.Drawing.Size(75, 23);
			this.btnClearNotepad.TabIndex = 2;
			this.btnClearNotepad.Text = "Clear";
			this.btnClearNotepad.UseVisualStyleBackColor = true;
			this.btnClearNotepad.Click += new System.EventHandler(this.btnClearNotepad_Click);
			// 
			// btnRunNotepad
			// 
			this.btnRunNotepad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnRunNotepad.Location = new System.Drawing.Point(7, 17);
			this.btnRunNotepad.Name = "btnRunNotepad";
			this.btnRunNotepad.Size = new System.Drawing.Size(75, 23);
			this.btnRunNotepad.TabIndex = 1;
			this.btnRunNotepad.Text = "Run";
			this.btnRunNotepad.UseVisualStyleBackColor = true;
			this.btnRunNotepad.Click += new System.EventHandler(this.btnRunNotepad_Click);
			// 
			// txtNotepad
			// 
			this.txtNotepad.AcceptsReturn = true;
			this.txtNotepad.AcceptsTab = true;
			this.txtNotepad.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtNotepad.BackColor = System.Drawing.Color.PaleGoldenrod;
			this.txtNotepad.Location = new System.Drawing.Point(95, 4);
			this.txtNotepad.Multiline = true;
			this.txtNotepad.Name = "txtNotepad";
			this.txtNotepad.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtNotepad.Size = new System.Drawing.Size(964, 145);
			this.txtNotepad.TabIndex = 0;
			this.txtNotepad.Enter += new System.EventHandler(this.txt_Enter);
			// 
			// dlgSaveFile
			// 
			this.dlgSaveFile.DefaultExt = "rcp";
			this.dlgSaveFile.FileName = "(Unknown).rcp";
			this.dlgSaveFile.Filter = "All|*.*|Recipe|*.rcp|Text|*.txt";
			this.dlgSaveFile.FilterIndex = 2;
			this.dlgSaveFile.Title = "Save Recipe";
			// 
			// lblVersion
			// 
			this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVersion.AutoSize = true;
			this.lblVersion.Font = new System.Drawing.Font("Lucida Console", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVersion.Location = new System.Drawing.Point(881, 3);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(40, 9);
			this.lblVersion.TabIndex = 4;
			this.lblVersion.Text = "version";
			this.lblVersion.Click += new System.EventHandler(this.Version_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1066, 545);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this._MainMenu);
			this.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this._MainMenu;
			this.MinimumSize = new System.Drawing.Size(600, 400);
			this.Name = "MainForm";
			this.Text = "Recipe";
			this._MainMenu.ResumeLayout(false);
			this._MainMenu.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tabPages.ResumeLayout(false);
			this.tabLog.ResumeLayout(false);
			this.tabLog.PerformLayout();
			this.tabModified.ResumeLayout(false);
			this.tabCreated.ResumeLayout(false);
			this.tabNotepad.ResumeLayout(false);
			this.tabNotepad.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip _MainMenu;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStripMenuItem mniOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mniExit;
		private System.Windows.Forms.ToolStripMenuItem mnuRun;
		private System.Windows.Forms.TextBox txtRecipe;
		private System.Windows.Forms.ToolStripMenuItem mniSetParameters;
		private System.Windows.Forms.ToolStripMenuItem mniTestOnly;
		private System.Windows.Forms.ToolStripMenuItem mniRun;
		private System.Windows.Forms.ToolStripMenuItem mniSaveLog;
		private System.Windows.Forms.ToolStripMenuItem mniSaveRecipe;
		private System.Windows.Forms.ToolStripMenuItem mniNewRecipe;
		private System.Windows.Forms.ToolStripMenuItem mniSaveRecipeAs;
		private System.Windows.Forms.ToolStripMenuItem mniClearMacros;
		private System.Windows.Forms.ToolStripMenuItem mnuWindow;
		private System.Windows.Forms.ToolStripMenuItem mniAlwaysOnTop;
		private System.Windows.Forms.OpenFileDialog dlgOpenFile;
		private System.Windows.Forms.ToolStripMenuItem mniDictionary;
		private System.Windows.Forms.ToolStripMenuItem mniEdit;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mniExpandClipboard;
		private System.Windows.Forms.ToolStripMenuItem mniStopServer;
		private System.Windows.Forms.ToolStripMenuItem mniLog;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem mniWrite;
		private System.Windows.Forms.ToolStripMenuItem mniWarn;
		private System.Windows.Forms.ToolStripMenuItem mniCopyLog;
		private System.Windows.Forms.ToolStripMenuItem mniStopWatcher;
		private System.Windows.Forms.ToolStripMenuItem mniGlobalDictionary;
		private System.Windows.Forms.ToolStripMenuItem mnuHelp;
		private System.Windows.Forms.ToolStripMenuItem mniAbout;
		private System.Windows.Forms.ToolStripMenuItem mniInteractive;
		private System.Windows.Forms.ToolStripMenuItem mniSearchTags;
		private System.Windows.Forms.FolderBrowserDialog dlgFolder;
		private System.Windows.Forms.ToolStripMenuItem mniCommands;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem mniNotepad;
		private System.Windows.Forms.ToolStripMenuItem mniWith;
		private System.Windows.Forms.ToolStripMenuItem mniEscapeRegex;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem mniEnclose;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblMode;
		private System.Windows.Forms.TabControl tabPages;
		private System.Windows.Forms.TabPage tabLog;
		private System.Windows.Forms.TextBox txtLog;
		private System.Windows.Forms.TabPage tabModified;
		private System.Windows.Forms.ListBox lstModified;
		private System.Windows.Forms.TabPage tabCreated;
		private System.Windows.Forms.ListBox lstNew;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem mniFolderSelectedFile;
		private System.Windows.Forms.ToolStripMenuItem mniCopyPath;
		private System.Windows.Forms.SaveFileDialog dlgSaveFile;
		private System.Windows.Forms.ToolStripMenuItem mnuPaste;
		private System.Windows.Forms.ToolStripMenuItem mniAddSelection;
		private System.Windows.Forms.ToolStripSeparator sepAddSelection;
		private System.Windows.Forms.ToolStripMenuItem mniRemoveAll;
		private System.Windows.Forms.ToolStripMenuItem mniRemoveLast;
		private System.Windows.Forms.ToolStripMenuItem mniAutocompletion;
		private System.Windows.Forms.ToolStripMenuItem mniArgChecks;
		private System.Windows.Forms.ToolStripMenuItem mniCheckDeprecated;
		private System.Windows.Forms.ToolStripMenuItem mniStop;
		private System.Windows.Forms.Label lblRunning;
		private System.Windows.Forms.ToolStripMenuItem mniRunClipboard;
		private System.Windows.Forms.TabPage tabNotepad;
		private System.Windows.Forms.Button btnCopyNotepad;
		private System.Windows.Forms.Button btnClearNotepad;
		private System.Windows.Forms.Button btnRunNotepad;
		private System.Windows.Forms.TextBox txtNotepad;
		private System.Windows.Forms.ToolStripMenuItem mniSelectedMacro;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
		private System.Windows.Forms.ToolStripMenuItem mniFind;
		private System.Windows.Forms.ToolStripMenuItem mniFindNext;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
		private System.Windows.Forms.ToolStripSeparator sepLastUsed;
		private System.Windows.Forms.ToolStripMenuItem mniLastOpened;
		private System.Windows.Forms.ToolStripMenuItem mniCopyRecipePath;
		private System.Windows.Forms.Label lblVersion;
	}
}

