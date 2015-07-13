using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imarda.Lib.Extensions
{
	public static class ThrowExtensions
	{
		//some of this code is originally from GoogleApis/ Apis/ Util/ Utilities.cs but there is internal to the Utilities class

		/// <summary>Throws an <seealso cref="System.ArgumentNullException"/> if the object is null.</summary>
		public static T ThrowIfNull<T>(this T obj, string paramName=null)
			where T: class 
		{
			if (null == obj)
			{
				throw new ArgumentNullException(paramName??string.Empty);
			}

			return obj;
		}

		/// <summary>
		/// Throws an <seealso cref="System.ArgumentNullException"/> if the string is <c>null</c> or empty.
		/// </summary>
		/// <returns>The original string</returns>
		public static string ThrowIfNullOrEmpty(this string str, string paramName = null)
		{
			if (string.IsNullOrEmpty(str))
			{
				throw new ArgumentException("Parameter was null or empty", paramName ?? string.Empty);
			}
			return str;
		}

		public static IEnumerable<T> ThrowIfNullOrEmpty<T>(this IEnumerable<T> enumerable, string paramName = null)
		{
			var throwIfNullOrEmpty = enumerable as T[] ?? enumerable.ToArray();
			var isOk = null != enumerable && 0 != throwIfNullOrEmpty.Count();
			if (!isOk)
			{
				throw new ArgumentException("Parameter was null or empty", paramName ?? string.Empty);
			}
			return throwIfNullOrEmpty;
		}

		public static int ThrowIfNonPositive(this int i, string paramName = null)
		{
			if (!(i>0))
			{
				throw new ArgumentException("Parameter is not GT 0", paramName ?? string.Empty);
			}
			return i;
		}
	}
}
