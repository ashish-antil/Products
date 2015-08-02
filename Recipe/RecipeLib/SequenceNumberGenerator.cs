using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeLib
{
	class SequenceNumberGenerator : IEnumerable<string>, ICloneable
	{
		private string _Format;
		private int _Start;
		private int _Step;
		private int _End;

		public SequenceNumberGenerator(string format, int start, int step, int end)
		{
			if (step < 1) throw new Exception("step must be > 0");
			if (start > end) throw new Exception("start must be <= end");
			_Format = format;
			_Start = start;
			_Step = step;
			_End = end;
		}

		public IEnumerator<string> GetEnumerator()
		{
			while (true)
			{
				for (int i = _Start; i <= _End; i += _Step)
				{
					yield return i.ToString(_Format);
				}
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public object Clone()
		{
			return new SequenceNumberGenerator(_Format, _Start, _Step, _End);
		}
	}
}
