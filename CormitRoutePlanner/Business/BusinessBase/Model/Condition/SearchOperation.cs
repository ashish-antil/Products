using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FernBusinessBase.Model
{
	/// <summary>
	/// An enumeration of all search operations that could be applied to searchable fields.
	/// Multiple searches can be allowed on each field.
	/// </summary>
	[Serializable]
	public enum SearchOperation : int
	{

		/// <summary>
		/// Specifies that you're searching for objects with the searchable field equal to
		/// the search argument.
		/// </summary>
		Equal = 1,

		/// <summary>
		/// Specifies that you're searching for objects with the searchable field not equal to
		/// the search argument.
		/// </summary>
		NotEqual = 2,

		/// <summary>
		/// Specifies that you're searching for objects with the searchable field greater than
		/// the search argument.
		/// </summary>
		GreaterThan = 4,

		/// <summary>
		/// Specifies that you're searching for objects with the searchable field less than
		/// the search argument.
		/// </summary>
		LessThan = 8,

		/// <summary>
		/// Specifies that you're searching for objects with the searchable field greater than
		/// or equal to the search argument.
		/// </summary>
		GreaterThanOrEqual = 16,

		/// <summary>
		/// Specifies that you're searching for objects with the searchable field less than
		/// or equal to the search argument.
		/// </summary>
		LessThanOrEqual = 32,

		/// <summary>
		/// Specifies that you're searching for objects with the searchable field "Like"
		/// the search argument (according to the SQL LIKE statement)
		/// </summary>
		Like = 64,

		/// <summary>
		/// Specifies that you're searching for objects with the searchable field within
		/// the given range, inclusive (i.e. lower lessthanorequal x lessthanorequal upper)
		/// </summary>
		RangeInclusive = 128,

		/// <summary>
		/// Specifies that you're searching for objects with the searchable field within
		/// the given range, inclusive (i.e. lower lessthan x lessthan upper)
		/// </summary>
		RangeExclusive = 256,

		/// <summary>
		/// For internal use only. PLEASE don't EVER use this yourself or a space-time rift
		/// will open and destroy the universe.
		/// </summary>
		GetAllValues = int.MaxValue,

		/// <summary>
		/// For internal use only. PLEASE don't EVER use this yourself or a space-time rift
		/// will open and destroy the universe.
		/// </summary>
		NoOp = int.MinValue
	}
}
