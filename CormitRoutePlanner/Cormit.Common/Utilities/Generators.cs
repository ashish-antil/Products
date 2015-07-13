using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace Imarda.Lib
{
	public class Range : IEnumerable<string>
	{
		private static readonly Regex _RxRange = new Regex(@"(-?\d+)\.\.(-?\d+)(?:@(\d+))?", RegexOptions.Compiled);
		private const string Marker = "[HERE]";

		private readonly Series _Generator;
		private readonly string _Template;

		public Range(string input)
		{
			Match m = _RxRange.Match(input);
			_Generator = new Series(m.Groups[1].Value.Int(1), m.Groups[2].Value.Int(1), m.Groups[3].Value.Int(0));
			_Template = _RxRange.Replace(input, Marker);
		}

		public IEnumerator<string> GetEnumerator()
		{
			return _Generator.Select(i => _Template.Replace(Marker, i.ToString(CultureInfo.InvariantCulture))).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private class Series : IEnumerable<int>
		{
			private int First;
			private int Last;
			private int Step;

			public Series(int first, int last, int step)
			{
				First = first;
				Last = last;
				Step = step;
				if (Last == First) Step = 1;
				else if (Step == 0) Step = Math.Sign(Last - First);
				else if (Last < First) Step = -Step;
			}

			public IEnumerator<int> GetEnumerator()
			{
				if (Step < 0) for (int i = First; i >= Last; i += Step) yield return i;
				else for (int i = First; i <= Last; i += Step) yield return i;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

	}

	public static class BasicExtensions
	{
		public static int Int(this string s, int dflt)
		{
			int n;
			if (!int.TryParse(s, out n)) n = dflt;
			return n;
		}
	}
}

