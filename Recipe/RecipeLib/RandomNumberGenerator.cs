using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeLib
{
	class RandomNumberGenerator : IEnumerable<string>
	{
		private string _Format;
		private Random _Random;
		private int _Min;
		private int _Max;

		public RandomNumberGenerator(string format, int min, int max, int seed)
		{
			if (min > max) throw new Exception("min must be less than max");
			_Random = seed == 0 ? new Random() : new Random(seed);
			_Min = min;
			_Max = max;
			_Format = format;
		}

		public IEnumerator<string> GetEnumerator()
		{
			while (true)
			{
				yield return _Random.Next(_Min, _Max).ToString(_Format);
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
