using System;

namespace FernBusinessBase
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class ValidLengthAttribute : Attribute
	{
		private readonly int _Min;
		private readonly int _Max;

		/// <summary>
		/// Minimum and maximum length of the string.
		/// </summary>
		/// <param name="min">minimun length, or special value -1 => value can be null</param>
		/// <param name="max"></param>
		public ValidLengthAttribute(int min, int max)
		{
			if (_Min < -1 || _Min > _Max) 
				throw new ArgumentException(string.Format("[{0},{1}] is not a valid range", min, max));
			_Min = min;
			_Max = max;
		}

		public bool IsValid(string s)
		{
			if (s == null) return _Min == -1;
			return s.Length >= _Min && s.Length <= _Max;
		}

		public override string ToString()
		{
			return string.Format("Length({0}..{1})", _Min, _Max);
		}
	}
}
