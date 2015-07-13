using System;

namespace FernBusinessBase
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class ValidNonEmptyAttribute : Attribute
	{
		/// <summary>
		/// Check GUID is not empty.
		/// </summary>
		public bool IsNonEmpty(Guid value)
		{
			return value != Guid.Empty;
		}


		public override string ToString()
		{
			return "Non-Empty Guid";
		}
	}
}