using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FernBusinessBase.Model
{
	/// <summary>
	/// An enumeration of all logical operators that could be applied to search condition.
	/// </summary>
	[Serializable]
	public enum LogicalOperator : int
	{

		/// <summary>
		/// Specifies that current condition must be met
		/// </summary>
		AND = 1,

		/// <summary>
		/// Specifies that current condition is optional
		/// </summary>
		OR = 2
	}
}
