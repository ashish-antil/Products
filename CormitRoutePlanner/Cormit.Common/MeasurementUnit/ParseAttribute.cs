using System;

namespace Imarda.Lib
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	public sealed class ParseAttribute : Attribute
	{
		private readonly string _UnitSymbol;

		// This is a positional argument
		public ParseAttribute(string unitSymbol)
		{
			_UnitSymbol = unitSymbol;
		}

		public string UnitSymbol
		{
			get { return _UnitSymbol; }
		}
	}
}