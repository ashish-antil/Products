using System;

namespace FernBusinessBase
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class ValidIntAttribute : Attribute
	{
		private readonly int _Min;
		private readonly int _Max;

		public ValidIntAttribute(int min, int max)
		{
			if (min > max) 
				throw new ArgumentException(
					string.Format("min {0} cannot be greater than max {1}", min, max));
			_Min = min;
			_Max = max;
		}

		public bool InRange(int x)
		{
			return x >= _Min && x <= _Max;
		}

		public override string ToString()
		{
			return string.Format("Range({0}..{1})", _Min, _Max);
		}
	}
}
