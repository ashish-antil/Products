using System;

namespace Imarda.Lib.Extensions
{
	public static class StringExtensions
	{
		public static bool CompareOrdinal(this string me, string other, bool ignoreCase = false)
		{
			var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
			return 0 == String.Compare(me, other, stringComparison);
		}
	}
}