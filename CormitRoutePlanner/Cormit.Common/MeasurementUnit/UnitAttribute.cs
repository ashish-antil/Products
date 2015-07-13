using System;

namespace Imarda.Lib
{
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	public sealed class UnitAttribute : Attribute
	{
		private readonly string _UnitSymbol;

		// This is a positional argument
		public UnitAttribute(string unitSymbol)
		{
			_UnitSymbol = unitSymbol;
			Factor = 1.0;
		}

		public string UnitSymbol
		{
			get { return _UnitSymbol; }
		}

		public double Factor { get; set; }

		public bool UnitBeforeValue { get; set; }

		public string Display { get; set; }
	}
}