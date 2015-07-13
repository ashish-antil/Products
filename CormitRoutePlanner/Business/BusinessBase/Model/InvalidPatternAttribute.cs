using System;
using System.Text.RegularExpressions;

namespace FernBusinessBase
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class InvalidPatternAttribute : Attribute
	{
		private readonly Regex _Pattern;

		/// <summary>
		/// Create a validation constraint.
		/// </summary>
		/// <param name="pattern">regular expression, use ^ and $ for begin & end match</param>
		/// <param name="options">RegexOptions.Compiled will always be applied</param>
		public InvalidPatternAttribute(string pattern, RegexOptions options)
		{
			if (string.IsNullOrEmpty(pattern)) throw new ArgumentNullException("pattern");
			_Pattern = new Regex(pattern, options | RegexOptions.Compiled);
		}

		public string Pattern
		{
			get { return _Pattern.ToString(); }
		}

		public bool IsMatch(string s)
		{
			return s == null || !_Pattern.IsMatch(s);
		}

		public override string ToString()
		{
			return string.Format("!Pattern(«{0}»)", _Pattern);
		}

	}
}
