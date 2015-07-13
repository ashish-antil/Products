using System;
using System.Text.RegularExpressions;

namespace FernBusinessBase
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class ValidPatternAttribute : Attribute
	{
		private readonly Regex _Pattern;
		private readonly bool _Nullable;

		/// <summary>
		/// Create a validation constraint.
		/// </summary>
		/// <param name="pattern">regular expression, use ^ and $ for begin & end match</param>
		/// <param name="options">RegexOptions.Compiled will always be applied</param>
		/// <param name="nullable">true to allow null fields</param>
		public ValidPatternAttribute(string pattern, RegexOptions options, bool nullable)
		{
			if (string.IsNullOrEmpty(pattern)) throw new ArgumentNullException("pattern");
			_Pattern = new Regex(pattern, options | RegexOptions.Compiled);
			_Nullable = nullable;
		}

		public string Pattern
		{
			get { return _Pattern.ToString(); }
		}

		public bool IsMatch(string s)
		{
			if (s == null) return _Nullable;
			return _Pattern.IsMatch(s);
		}

		public override string ToString()
		{
			return string.Format("Pattern(«{0}», {1})", _Pattern, _Nullable ? "nullable" : "not null");
		}

	}
}
