using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RecipeGUI
{
	public class Editor
	{
		private TextBoxBase _TextBoxBase;
		private bool _ControlKey;
		private TextSearch _Search;
		public Action AutoCompletion;

		public Editor(TextBoxBase tbb)
		{
			_TextBoxBase = tbb;
			_TextBoxBase.KeyDown += new KeyEventHandler(KeyDown);
			_TextBoxBase.KeyUp += new KeyEventHandler(KeyUp);
			_TextBoxBase.KeyPress += new KeyPressEventHandler(KeyPress);
		}

		public Predicate<char> IsWordChar { get; set; }


		private void KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control) _ControlKey = true;
		}

		private void KeyUp(object sender, KeyEventArgs e)
		{
			_ControlKey = false;
		}

		private void KeyPress(object sender, KeyPressEventArgs e)
		{
			if (_ControlKey)
			{
				bool handled = true;
				switch (e.KeyChar)
				{
					case '\u0001': // Ctrl-A
						_TextBoxBase.SelectAll();
						break;
					case '\u007F': // 127 = Ctrl-Backspace
						string t = _TextBoxBase.Text;
						int ss = _TextBoxBase.SelectionStart;
						int i = ss - 1;
						while (i >= 0 && !char.IsWhiteSpace(t[i])) i--;
						while (i >= 0 && char.IsWhiteSpace(t[i])) i--;
						i++;
						_TextBoxBase.Text = t.Substring(0, i) + t.Substring(ss);
						_TextBoxBase.SelectionLength = 0;
						_TextBoxBase.SelectionStart = i;
						break;
					case '\u0015':
						_TextBoxBase.AppendText("\r\n");
						_TextBoxBase.SelectionStart = _TextBoxBase.Text.Length;
						_TextBoxBase.ScrollToCaret();
						break;
					default:
						handled = false;
						break;
				}
				e.Handled = handled;
			}
			else if (e.KeyChar == '\r' && AutoCompletion != null)
			{
				AutoCompletion();
			}
		}


		public int SelectWord()
		{
			TextBoxBase t = _TextBoxBase;
			int ss = t.SelectionStart;
			int n = t.Text.Length;
			int start = ss, end = ss;
			while (start >= 0 && IsWordChar(t.Text[start])) start--;
			if (start != ss && start != ss - 1)
			{
				while (end < n && IsWordChar(t.Text[end])) end++;
				t.SelectionStart = ++start;
				t.SelectionLength = Math.Max(0, end - start);
			}
			return start;
		}

		private string _SearchPattern;

		public bool FindFirst(string pattern)
		{
			_SearchPattern = pattern;
			return FindNext();
		}


		public bool FindNext()
		{
			if (_SearchPattern != null)
			{
				_Search = new TextSearch(_TextBoxBase.Text, _SearchPattern, ignoreCase: true, regex: false);
				int start = _TextBoxBase.SelectionStart;
				if (_TextBoxBase.SelectionLength > 0) start += _TextBoxBase.SelectionLength;
				int length;
				if (_Search.FindNext(ref start, out length))
				{
					_TextBoxBase.SelectionStart = start;
					_TextBoxBase.SelectionLength = length;
					_TextBoxBase.ScrollToCaret();
					return true;
				}
			}
			return false;
		}


		/// <summary>
		/// Put `...` around first selected word, or around word with cursor in it, or insert `` at end of line.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void EncloseWord(char open, char close)
		{
			var txt = _TextBoxBase;
			if (txt == null) return;
			txt.ScrollToCaret();
			int i;
			int ss = txt.SelectionStart;
			if (txt.SelectionLength == 0)
			{
				if (ss == txt.Text.Length || (ss + 1 < txt.Text.Length && txt.Text[ss + 1] == '\n'))
				{
					txt.SelectedText = new string(new char[] { open, close });
					txt.SelectionStart--;
					return;
				}
				else
				{
					i = SelectWord();
				}
			}
			else
			{
				i = ss;
			}
			string st = txt.SelectedText;
			string r = Regex.Replace(txt.SelectedText, @"(\S+)(.*)", open + "$1" + close + "$2");
			if (r != st)
			{
				txt.SelectedText = r;
				txt.SelectionStart = ss + 1;
			}
			else
			{
				txt.SelectedText = new string(new char[] { open, close });
				txt.SelectionStart = ss + 1;
			}
		}

	}
}
