using System;
using System.Linq;

namespace FernBusinessBase
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class ValidASCIIAttribute : Attribute
	{
		public bool IsASCII(string s)
		{
			return s.All(c => c >= 32 && c <= 126);
		}

		public override string ToString()
		{
			return "ASCII";
		}
	}
}
