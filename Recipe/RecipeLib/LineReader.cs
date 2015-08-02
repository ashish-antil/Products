using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RecipeLib
{

	/// <summary>
	/// TextReader that keeps track of the line numbers.
	/// </summary>
	public class LineReader : IDisposable
	{
		private TextReader _Underlying;
		private int _LineNumber;
		private string _Line;

		public LineReader(TextReader reader, string name = "")
		{
			Name = name;
			_Underlying = reader;
		}

		public LineReader(string text, string name = "")
		{
			Name = name;
			_Underlying = new StringReader(text);
		}

		public int LineNumber
		{
			get { return _LineNumber; }
		}

		public string Line
		{
			get { return _Line; }
		}

		public string Name { get; set; }

		public string ReadLine()
		{
			_LineNumber++;
			return _Line = _Underlying.ReadLine();
		}

		public string ReadToEnd()
		{
			_LineNumber = -1;
			_Line = null;
			return _Underlying.ReadToEnd();
		}

		public void Close()
		{
			_Underlying.Close();
		}

		public void Dispose()
		{
			_Underlying.Dispose();
		}
	}
}
