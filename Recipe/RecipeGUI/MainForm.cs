#define NET45

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using RecipeLib;
using System.Collections;
using System.Diagnostics;
using RecipeGUI.Properties;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace RecipeGUI
{
	public partial class MainForm : Form
	{
		private readonly Recipe _Recipe;
		private Dictionary<string, string> _Macros;
		private string _RecipePath;
		private readonly Editor _Editor1;
		private readonly Editor _Editor2;
		private Editor _CurrentEditor;

		private readonly FileSystemWatcher _ScriptWatch;
		private DateTime _FileModified;
		private bool _Serving;

		private byte[] _TextHash;
		private readonly SHA1 _Sha1 = SHA1.Create();



		public MainForm(string[] args)
		{
			InitializeComponent();
			InitializeMacros();
			_Recipe = new Recipe();
			_Recipe.Message += HandleMessage;
			_Recipe.Input = GetInput;
			_Recipe.Interactive = true;
			_Recipe.Write = false;
			_Recipe.Warn = true;
			_Recipe.Log = true;
			_Recipe.Show = true;
			_Editor1 = CreateEditor(txtRecipe);
			_Editor2 = CreateEditor(txtNotepad);

			_ScriptWatch = new FileSystemWatcher();
			_ScriptWatch.Changed += new FileSystemEventHandler(ScriptWatch_Changed);
			if (args.Length > 0) Open(args[0]);
		}

		private Editor CreateEditor(TextBox tbx)
		{
			var autoCompleter = new AutoCompleter(tbx)
			{
				AutoComplete = () => mniAutocompletion.Checked,
				ArgsChecks = () => mniArgChecks.Checked,
				CheckDeprecated = () => mniCheckDeprecated.Checked,
			};

			var editor = new Editor(tbx)
			{
				AutoCompletion = autoCompleter.Handle,
				IsWordChar = c => char.IsLetterOrDigit(c) || c == '_'
			};

			return editor;
		}

		private const string _Dump =
		  "#def Dump :dumpfile\r\n"
		+ "#forget dictionary\r\n"
		+ "#! dictionary=`*`\r\n"
		+ "#with dictionary\r\n"
		+ "delblank\r\n"
		+ "enclose #@ []``nl```bq`[]`bq` ``nl``#end []``nl``\r\n"
		+ "#end\r\n"
		+ "#exp dictionary\r\n"
		+ "#info `:dumpfile`\r\n"
		+ "#newer `:dumpfile`\r\n"
		+ "`dictionary`\r\n"
		+ "#end\r\n"
		+ "#enddef\r\n";


		#region Recipe File Watcher

		private void ScriptWatch_Changed(object sender, FileSystemEventArgs e)
		{
			if (0 == string.Compare(_RecipePath, e.FullPath, true, CultureInfo.InvariantCulture))
			{
				WatchFile(false);
				var modified = File.GetLastWriteTimeUtc(e.FullPath);
				if (modified - _FileModified < TimeSpan.FromMilliseconds(10)) return; // workaround for duplicate event bug
				_FileModified = modified;
				Reload();
			}
		}

		private void Reload()
		{
			if (InvokeRequired) Invoke(new Action(Reload));
			else
			{
				string msg = string.Format("{0}\r\nThis file has been modified by another program. Reload?", _RecipePath);
				if (DialogResult.Yes == MessageBox.Show(msg, "File Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
				{
					txtRecipe.Text = File.ReadAllText(_RecipePath);
				}
				WatchFile(true);
			}
		}

		private void WatchFile(bool watch)
		{
			if (!string.IsNullOrEmpty(_ScriptWatch.Path))
			{
				_ScriptWatch.EnableRaisingEvents = watch;
			}
		}

		private void CheckWatch()
		{
			if (_RecipePath != null)
			{
				WatchFile(true);
				if (File.GetLastWriteTimeUtc(_RecipePath) > _FileModified) Reload();
			}
		}
		#endregion Recipe File Watcher


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			AddPasteMenuItem(_Dump);
			if (!string.IsNullOrEmpty(Settings.Default.Folder)) dlgFolder.SelectedPath = Settings.Default.Folder;
			SetLastOpened(Settings.Default.LastOpened);
			var text = txtRecipe.Text;
			lblVersion.Text = Recipe.Version;
			if (text.StartsWith("#auto"))
			{
				_Recipe.Write = true;

				if (text.Contains(" log-")) _Recipe.Log = false;
				if (text.Contains(" warn-")) _Recipe.Warn = false;
				if (text.Contains(" hide")) _Recipe.Show = false;
				if (text.Contains(" int-"))
				{
					_Recipe.Interactive = false;
					_Recipe.Show = false;
					InputParameters();
				}
				SetWriteMode();
				SetLogMode();
				SetWarnMode();
				SetInteractiveMode();
				SetShowMode();
				Run(txtRecipe);
			}
		}

		private void InitializeMacros()
		{
			_Macros = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			IDictionary env = Environment.GetEnvironmentVariables();
			foreach (string key in env.Keys) _Macros.Add(key, (string)env[key]);
		}

		public Dictionary<string, string> Macros { get { return _Macros; } }

		private void HandleMessage(string s)
		{
			if (InvokeRequired)
			{
				Invoke(new Action<string>(HandleMessage), s);
				return;
			}
			if (s.StartsWith("~") && mniWrite.Checked)
			{
				txtLog.AppendText(s.Substring(1) + Environment.NewLine);
			}
			else if (s.StartsWith("@"))
			{
				Application.DoEvents();
				if (mniLog.Checked) txtLog.AppendText(s + Environment.NewLine);
				int p = s.IndexOf(' ');
				if (p != -1)
				{
					string cmd = s.Substring(1, p - 1);
					string arg = s.Substring(p + 1);

					switch (cmd)
					{
						case "new":
						case "newer":
						case "filecopy":
						case "filemove":
							AddNoDup(lstNew, arg);
							MarkChanged(tabCreated, true);
							break;
						case "filerepl":
						case "do":
						case "before":
						case "after":
						case "subs":
						case "append":
						case "map":
							AddNoDup(lstModified, arg);
							MarkChanged(tabModified, true);
							break;
						case "warn":
							if (mniWarn.Checked)
							{
								DialogResult res = MessageBox.Show(this, arg, "Attention", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
								if (res == DialogResult.Cancel) _Recipe.Stop();
							}
							break;
						case "mbox":
							if (mniInteractive.Checked)
							{
								MessageBox.Show(this, arg, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
							}
							break;
						case "clip":
							if (!string.IsNullOrEmpty(arg)) Clipboard.SetText(arg);
							break;
						case "edit":
							txtRecipe.Text = arg;
							break;
						case "server":
							SetServerModeVisible(true);
							_Serving = true;
							break;
						case "watch":
							mniStopWatcher.Visible = true;
							txtRecipe.ReadOnly = true;
							_Serving = true;
							break;
						case "mode":
							SetWriteMode();
							SetInteractiveMode();
							SetLogMode();
							SetWarnMode();
							SetShowMode();
							break;
					}
				}
				else
				{
					switch (s.Substring(1))
					{
						case "exit":
							_Recipe.Stop();
							WindowState = FormWindowState.Minimized;
							Application.Exit();
							break;
					}
				}
			}
		}

		private static void MarkChanged(TabPage page, bool yes)
		{
			if (yes && !page.Text.EndsWith("*")) page.Text += "*";
			else if (!yes && page.Text.EndsWith("*")) page.Text = page.Text.TrimEnd('*');
		}

		public string GetInput(string prompt, bool multiline, string initial, params string[] options)
		{
			if (mniInteractive.Checked)
			{
				string answer = null;
				var dlg = new InputForm(prompt, multiline, initial, options);
				if (DialogResult.OK == dlg.ShowDialog(this))
				{
					answer = dlg.UserInput;
				}
				return answer;
			}
			_Recipe.Stop();
			return null;
		}

		private void AddNoDup(ListBox box, string s)
		{
			for (int i = 0; i < box.Items.Count; i++)
			{
				if (box.Items[i].Equals(s)) return;
			}
			box.Items.Add(s);
		}

		private void mniExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		private bool ExitProgram()
		{
			WatchFile(false);
			if (HasChanged())
			{
				var result = MessageBox.Show("Save changes?", "Recipe", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				switch (result)
				{
					case System.Windows.Forms.DialogResult.Yes:
						if (_TextHash == null) SaveAs();
						else Save();
						break;
					case System.Windows.Forms.DialogResult.Cancel:
						WatchFile(true);
						return false;
				}
			}
			return true;
		}

		private void ConfirmSave()
		{
			if (HasChanged())
			{
				var result = MessageBox.Show("Save changes?", "Recipe", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result == DialogResult.Yes)
				{
					if (_TextHash == null) SaveAs();
					else Save();
				}
			}
		}


		private void mniOpen_Click(object sender, EventArgs e)
		{
			if (DialogResult.OK == dlgOpenFile.ShowDialog())
			{
				WatchFile(false);
				ConfirmSave();
				Open(dlgOpenFile.FileName);
			}
		}

		private void Open(string path)
		{
			_Recipe.SetRootPath(path);
			txtRecipe.Text = File.ReadAllText(_RecipePath = path);
			mniLastOpened.Tag = path;
			mniLastOpened.Text = Path.GetFileName(path);
			SetTextHash();
			Text = "Recipe: " + Path.GetFileNameWithoutExtension(path);
			_ScriptWatch.Path = Path.GetDirectoryName(path);
			WatchFile(true);

		}

		private void mniSetParameters_Click(object sender, EventArgs e)
		{
			InputParameters();
		}

		private void InputParameters()
		{
			var lines = txtRecipe.Lines;
			IEnumerable<string[]> req, opt;
			ParameterHelper.Extract(lines, _Macros, out req, out opt);
			if (req.Count() + opt.Count() > 0)
			{
				using (var dlg = new ParameterInputForm(req, opt))
				{
					if (DialogResult.OK == dlg.ShowDialog(this))
					{
						var result = dlg.Result;
						foreach (string[] x in req) _Macros.Remove(x[0]);
						foreach (string[] x in opt) _Macros.Remove(x[0]);
						foreach (string key in result.Keys) _Macros[key] = result[key];
					}
				}
			}
			txtLog.Focus();
		}

		private void mniRun_Click(object sender, EventArgs e)
		{
			if (_CurrentEditor == _Editor1 || tabPages.SelectedTab != tabNotepad) Run(txtRecipe);
			else if (_CurrentEditor == _Editor2) Run(txtNotepad);
		}

#if NET45
		private async void Run(TextBoxBase tbb)
#else
		private void Run(TextBoxBase tbb)
#endif
		{
			try
			{
				WatchFile(false);
				Clear();
				_Recipe.ClearHandlers();
				string text = tbb.SelectedText == "" ? tbb.Text : tbb.SelectedText;
				using (var sr = new LineReader(text, name: tbb.Name))
				{
					string clipboard = Clipboard.ContainsText() ? Clipboard.GetText() : string.Empty;
					SetBusy(true);
#if NET45
					if (mniInteractive.Checked) _Recipe.Run(sr, _Macros, clipboard);
					else await Task.Run(() => _Recipe.Run(sr, _Macros, clipboard));
#else
					_Recipe.Run(sr, _Macros, clipboard);
#endif
					if (mniLog.Checked) txtLog.AppendText("@done.\r\n");
				}
			}
			catch (Exception ex)
			{
				Error(ex);
			}
			finally
			{
				SetBusy(false);
				if (!_Serving) WatchFile(true);
				tabPages.SelectedTab = tabLog;
			}
		}

		private void SetBusy(bool busy)
		{
			mniRun.Enabled = !busy;
			mniStop.Enabled = busy;
			lblRunning.Visible = busy;
		}

		private void Clear()
		{
			lstNew.Items.Clear();
			MarkChanged(tabCreated, false);
			lstModified.Items.Clear();
			MarkChanged(tabModified, false);
			txtLog.Clear();
		}

		private void mniSaveLog_Click(object sender, EventArgs e)
		{
			try
			{
				File.WriteAllText("RecipeLog.txt", txtLog.Text);
			}
			catch (Exception ex)
			{
				Error(ex);
			}
		}

		private void ShowFile(object sender, EventArgs e)
		{
			try
			{
				var lst = (ListBox)sender;
				if (lst.SelectedIndex != -1)
				{
					string path = (string)lst.Items[lst.SelectedIndex];
					Process.Start(path);
				}
			}
			catch (Exception ex)
			{
				Error(ex);
			}
		}

		private void mniTestOnly_Click(object sender, EventArgs e)
		{
			_Recipe.Write = !mniTestOnly.Checked;
			SetWriteMode();
		}

		private void SetWriteMode()
		{
			mniTestOnly.Checked = !_Recipe.Write;
			if (_Recipe.Write)
			{
				lblMode.BackColor = Color.Orange;
				lblMode.Text = "WRITE";
			}
			else
			{
				lblMode.BackColor = Color.Lime;
				lblMode.Text = "TEST";
			}
		}

		private void SetInteractiveMode()
		{
			mniInteractive.Checked = _Recipe.Interactive;
		}

		private void SetLogMode()
		{
			mniLog.Checked = _Recipe.Log;
		}

		private void SetWarnMode()
		{
			mniWarn.Checked = _Recipe.Warn;
		}

		private void SetShowMode()
		{
			WindowState = _Recipe.Show ? FormWindowState.Normal : FormWindowState.Minimized;
		}

		private void Error(Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Error");
		}

		private void mniSaveRecipe_Click(object sender, EventArgs e)
		{
			try
			{
				WatchFile(false);
				if (_RecipePath == null) SaveAs();
				else Save();
			}
			catch (Exception ex)
			{
				Error(ex);
			}
			finally
			{
				WatchFile(true);
			}
		}

		private void Save()
		{
			File.WriteAllText(_RecipePath, txtRecipe.Text);
			SetTextHash();
		}

		private void mniNewRecipe_Click(object sender, EventArgs e)
		{
			WatchFile(false);
			ConfirmSave();
			Text = "Recipe";
			txtRecipe.Clear();
			Clear();
			_TextHash = null;
			InitializeMacros();
			_RecipePath = null;
		}

		private void mniSaveRecipeAs_Click(object sender, EventArgs e)
		{
			WatchFile(false);
			SaveAs();
			WatchFile(true);
		}

		private void SaveAs()
		{
			if (DialogResult.OK == dlgSaveFile.ShowDialog(this))
			{
				SetLastOpened(_RecipePath = dlgSaveFile.FileName);
				Save();
				Text = "Recipe: " + Path.GetFileNameWithoutExtension(_RecipePath);
				_ScriptWatch.Path = Path.GetDirectoryName(_RecipePath);
			}
		}

		private void mniClearMacros_Click(object sender, EventArgs e)
		{
			InitializeMacros();
		}


		private void txtRecipe_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("FileDrop"))
			{
				string[] files = (string[])e.Data.GetData("FileDrop");
				txtRecipe.AppendText(string.Join(Environment.NewLine, files) + Environment.NewLine);
			}
		}

		private void txtRecipe_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Copy;
		}


		protected override void OnClosing(CancelEventArgs e)
		{
			_Recipe.StopServer();
			_Recipe.StopWatcher();
			base.OnClosing(e);
			Settings.Default.Folder = dlgFolder.SelectedPath;
			Settings.Default.LastOpened = mniLastOpened.Tag as string;
			Settings.Default.Save();
			e.Cancel = !ExitProgram();
		}

		private void mniAlwaysOnTop_Click(object sender, EventArgs e)
		{
			TopMost = mniAlwaysOnTop.Checked;
		}

		public static void ShowInFolder(string file)
		{
			var p = new Process();
			var ps = p.StartInfo;
			string path = new FileInfo(file).FullName;
			ps.FileName = "explorer";
			ps.Arguments = "/select, \"" + path + "\"";
			ps.WorkingDirectory = Environment.GetEnvironmentVariable("SystemRoot");
			ps.UseShellExecute = true;
			p.Start();
		}

		private void mniDictionary_Click(object sender, EventArgs e)
		{
			var form = new DictionaryForm(_Macros) { Text = _Macros.Count + " Macros" };
			form.ShowDialog(this);
		}

		private void mniGlobalDictionary_Click(object sender, EventArgs e)
		{
			var form = new DictionaryForm(Recipe.GlobalMacros) { Text = Recipe.GlobalMacros.Count + " Global Macros" };
			form.ShowDialog(this);
		}

		private void mniEdit_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(_RecipePath))
			{
				string editor;
				if (_Macros.TryGetValue("editor", out editor))
				{
					Process.Start(editor, _RecipePath);
				}
				else
				{
					Process.Start(_RecipePath);
				}
			}
		}

		private void mniExpandClipboard_Click(object sender, EventArgs e)
		{
			if (Clipboard.ContainsText())
			{
				var lines = new[] {
					@"#put input=`paste`",
					@"#put requirements=`input`",
					@"#find (?<!``)``([^``]+)``(?!``) requirements",
					@"` `#require $1",
					@"",
					@"#end",
					@"#subs #require\ (?:ts:[^\r\n]+|timestamp|newguid|newseqguid|recipe|&\S+)\r\n requirements",
					@"#end",
					@"#with requirements",
					@"unique",
					@"delblank",
					@"#end",
					@"#put text",
					@"` `#cdata template",
					@"`paste`",
					@"` `#end template",
					@"`requirements`",
					@"` `#exp template",
					@"` `#copy ``template``",
					@"#end",
					@"#edit text",
				};
				var recipe = string.Join(Environment.NewLine, lines);
				_Recipe.Run(new LineReader(recipe), _Macros, Clipboard.GetText());
				InputParameters();
				_Recipe.Run(new LineReader(txtRecipe.Text), _Macros, string.Empty);
			}
		}

		private void mniStopServer_Click(object sender, EventArgs e)
		{
			_Serving = false;
			_Recipe.StopServer();
			SetServerModeVisible(false);
		}

		private void mniStopWatcher_Click(object sender, EventArgs e)
		{
			_Serving = false;
			_Recipe.StopWatcher();
			mniStopWatcher.Visible = false;
			txtRecipe.ReadOnly = false;
			CheckWatch();
		}

		private void SetServerModeVisible(bool b)
		{
			mniStopServer.Visible = b;
			txtRecipe.ReadOnly = b;
			txtRecipe.BackColor = b ? Color.DarkGray : SystemColors.Window;
		}

		private void mniCopyLog_Click(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(txtLog.Text)) Clipboard.SetText(txtLog.Text);
			}
			catch { }
		}

		private void Version_Click(object sender, EventArgs e)
		{
			MessageBox.Show(string.Join(Environment.NewLine, _Recipe.GetHistory()), "Version");
		}

		private void mniSearchTags_Click(object sender, EventArgs e)
		{
			var form = new SearchForm(dlgFolder);
			if (DialogResult.OK == form.ShowDialog(this))
			{
				string path = form.SelectedPath;
				if (string.IsNullOrEmpty(path))
				{
					MessageBox.Show(this, "Not found", "Tag search", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					ConfirmSave();
					Open(path);
				}
			}
		}

		private void mniCommands_Click(object sender, EventArgs e)
		{
			var form = new TextForm(RecipeHelp.Commands);
			form.Text = "Commands";
			form.Show();
		}

		private void mniNotepad_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(_RecipePath))
			{
				Process.Start("notepad++.exe", _RecipePath);
			}

		}

		private void mniWith_Click(object sender, EventArgs e)
		{
			var sb = new StringBuilder();
			foreach (var kvp in RecipeHelp.ScriptOperations)
			{
				sb.Append("   #with text=").AppendLine(kvp.Key).AppendLine(kvp.Value).AppendLine();
			}
			var form = new TextForm(sb.ToString());
			form.Text = "#with - Script Operations";
			form.Show();
		}

		private void SetTextHash()
		{
			_TextHash = _Sha1.ComputeHash(Encoding.UTF8.GetBytes(txtRecipe.Text));
		}

		private bool HasChanged()
		{
			if (_TextHash == null)
			{
				return txtRecipe.Text.Length > 0;
			}
			else
			{
				byte[] current = _Sha1.ComputeHash(Encoding.UTF8.GetBytes(txtRecipe.Text));
				for (int i = 0; i < current.Length; i++)
				{
					if (current[i] != _TextHash[i]) return true;
				}
				return false;
			}
		}

		private void mniEscapeRegex_Click(object sender, EventArgs e)
		{
			var text = txtRecipe.SelectedText;
			if (!string.IsNullOrEmpty(text))
			{
				txtRecipe.SelectedText = Regex.Escape(text);
			}
			else
			{
				MessageBox.Show(this, "First select some text in the Recipe pane", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void mniEnclose_Click(object sender, EventArgs e)
		{
			_CurrentEditor.EncloseWord('`', '`');
		}

		private void mniFolderSelectedFile_Click(object sender, EventArgs e)
		{
			try
			{
				string item = GetSelectedPath();
				if (item != null) ShowInFolder(item);
			}
			catch { }
		}

		private void mniCopyPath_Click(object sender, EventArgs e)
		{
			try
			{
				string item = GetSelectedPath();
				if (item != null) Clipboard.SetText(item);
			}
			catch { }
		}

		private string GetSelectedPath()
		{
			return
				tabPages.SelectedIndex == 1 ? (string)lstModified.SelectedItem
				: tabPages.SelectedIndex == 2 ? (string)lstNew.SelectedItem
				: null;
		}

		private void mniAddSelection_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(txtRecipe.SelectedText))
			{
				string text = txtRecipe.SelectedText;
				AddPasteMenuItem(text);
			}
		}

		private void AddPasteMenuItem(string text)
		{
			const int n = 30;
			string text1 = Regex.Replace(text, @"\s", " ");
			string trunc = text1.Length > n ? text1.Substring(0, n - 3) + "..." : text1;
			ToolStripItem item = mnuPaste.DropDownItems.Add(trunc);
			item.Tag = text;
			item.Click += PasteItem_Click;
		}

		void PasteItem_Click(object sender, EventArgs e)
		{
			txtRecipe.SelectedText = ((ToolStripItem)sender).Tag as string;
		}

		private void mniClearMenu_Click(object sender, EventArgs e)
		{
			int n = mnuPaste.DropDownItems.Count;
			for (int i = 4; i < n; i++) mnuPaste.DropDownItems.RemoveAt(4);
		}

		private void mniRemoveLast_Click(object sender, EventArgs e)
		{
			int n = mnuPaste.DropDownItems.Count;
			if (n > 4) mnuPaste.DropDownItems.RemoveAt(n - 1);
		}

		private void mniAutocompletion_Click(object sender, EventArgs e)
		{
			bool ac = mniAutocompletion.Checked;
			mniArgChecks.Enabled = ac;
			mniCheckDeprecated.Enabled = ac;
		}

		private void mniStop_Click(object sender, EventArgs e)
		{
			_Recipe.Stop();
		}

		private void mniRunClipboard_Click(object sender, EventArgs e)
		{
			if (Clipboard.ContainsText())
			{
				var recipe = Clipboard.GetText();
				try
				{
					SetBusy(true);
					_Recipe.Run(new LineReader(recipe, name: "clipboard"), _Macros, Clipboard.GetText());
				}
				finally
				{
					SetBusy(false);
				}
			}

		}

		private void btnRunNotepad_Click(object sender, EventArgs e)
		{
			Run(txtNotepad);
		}

		private void btnClearNotepad_Click(object sender, EventArgs e)
		{
			txtNotepad.Clear();
		}

		private void btnCopyNotepad_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(txtNotepad.Text)) return;
			if (string.IsNullOrEmpty(txtNotepad.SelectedText)) Clipboard.SetText(txtNotepad.Text);
			else Clipboard.SetText(txtNotepad.SelectedText);
		}

		private void mniSelectedMacro_Click(object sender, EventArgs e)
		{
			try
			{
				_Editor1.SelectWord();
				string key = txtRecipe.SelectedText;
				string contents;
				if (_Macros.TryGetValue(key, out contents))
				{
					string text = string.Format("\r\n#@ {0}\r\n{1}\r\n#end {0}", key, contents);
					txtLog.AppendText(text);
				}
			}
			catch { }
		}

		private void mniFind_Click(object sender, EventArgs e)
		{
			var input = new InputForm("Regex to search for:", false, txtRecipe.SelectedText);
			if (DialogResult.OK == input.ShowDialog(this))
			{
				string pattern = input.UserInput;
				txtLog.AppendText("Search for: \"" + pattern + "\"\r\n");
				if (!_Editor1.FindFirst(pattern))
				{
					txtLog.AppendText("Not found.\r\n");
				}
			}
		}

		private void mniFindNext_Click(object sender, EventArgs e)
		{
			if (!_Editor1.FindNext())
			{
				txtRecipe.SelectionLength = 0;
				txtLog.AppendText("End of search.\r\n");
			}
		}

		private void txt_Enter(object sender, EventArgs e)
		{
			if (sender == txtRecipe) _CurrentEditor = _Editor1;
			else if (sender == txtNotepad) _CurrentEditor = _Editor2;
		}

		private void mniLastOpened_Click(object sender, EventArgs e)
		{
			var path = mniLastOpened.Tag as string;
			if (!string.IsNullOrEmpty(path) && File.Exists(path)) Open(path);
		}

		private void SetLastOpened(string path)
		{
			mniLastOpened.Tag = path;
			mniLastOpened.Text = Path.GetFileName(path);
		}

		private void mniCopyRecipePath_Click(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(_RecipePath)) Clipboard.SetText(_RecipePath);
			}
			catch { }
		}

	}
}