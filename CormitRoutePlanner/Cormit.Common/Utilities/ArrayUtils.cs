using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Common.Extensions;

namespace Imarda.Lib
{
	public static class ArrayUtils
	{
		/// <summary>
		/// Extension method to test whether an array is null or has no elements.
		/// Will not throw null ref exception when array is null: array.IsNullOrEmpty().
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(this Array array)
		{
			return array == null || array.Length == 0;
		}

		/// <summary>
		/// Extension method to test whether a list is null or has no elements.
		/// Will not throw null ref exception when array is null: list.IsNullOrEmpty().
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(this IList list)
		{
			return list == null || list.Count == 0;
		}

		/// <summary>
		/// Shifts elements of an array downward (toward index 0). At the highest index, a default value
		/// gets inserted.
		/// </summary>
		/// <typeparam name="T">array base type</typeparam>
		/// <param name="array">the array to be shifted</param>
		/// <returns>The element that was at [0] before it got shifted out</returns>
		public static T ShiftDown<T>(this T[] array)
		{
			return array.ShiftDown(default(T));
		}

		public static T ShiftDown<T>(this T[] array, T newElem)
		{
			if (array.IsNullOrEmpty()) throw new InvalidOperationException("Cannot shift empty array");
			T shiftOut = array[0];
			if (array.Length > 1)
			{
				for (int i = 1; i < array.Length; i++) array[i - 1] = array[i];
				array[array.Length - 1] = newElem;
			}
			return shiftOut;
		}

		/// <summary>
		/// Shifts elements of an array upward (toward highest index). At index 0, a default value
		/// gets inserted.
		/// </summary>
		/// <typeparam name="T">array base type</typeparam>
		/// <param name="array">the array to be shifted</param>
		/// <returns>The element that was at the highest index before it got shifted out</returns>
		public static T ShiftUp<T>(this T[] array)
		{
			return array.ShiftUp(default(T));
		}

		public static T ShiftUp<T>(this T[] array, T newElem)
		{
			if (array.IsNullOrEmpty()) throw new InvalidOperationException("Cannot shift empty array");
			T shiftOut = array[array.Length - 1];
			for (int i = array.Length - 1; i > 0; i--) array[i] = array[i - 1];
			array[0] = newElem;
			return shiftOut;
		}

		/// <summary>
		/// Rotate the elements of an array towards the lowest index
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		public static void RotateDown<T>(this T[] array)
		{
			array[array.Length - 1] = ShiftDown(array);
		}

		/// <summary>
		/// Rotate the elements of an array towards the highest index
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		public static void RotateUp<T>(this T[] array)
		{
			array[0] = ShiftUp(array);
		}


		/// <summary>
		/// Add an item to the list if it is not equal to any element in the list.
		/// The equality is defined by the 'equal' parameter.
		/// O(n) performance, don't use on long lists.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="t"></param>
		/// <param name="equal"></param>
		public static void AddUnique<T>(this List<T> list, T t, Func<T, T, bool> equal)
		{
			if (list.Any(u => equal(t, u)))
			{
				return;
			}
			list.Add(t);
		}

		/// <summary>
		/// Add elements from the range only if it is not equal to any element in the list.
		/// The equality is defined by the 'equal' parameter.
		/// O(n) performance, don't use on long lists.
		/// </summary>
		public static void AddUniqueRange<T>(this List<T> list, IEnumerable<T> range, Func<T, T, bool> equal)
		{
			foreach (T t in from t in range let t1 = t let found = list.Any(u => equal(t1, u)) where !found select t)
			{
				list.Add(t);
			}
		}

		/// <summary>
		/// Remove duplicates from the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TKey"> </typeparam>
		/// <param name="input"></param>
		/// <param name="unique">function that takes an object from the list and returns an id that is supposed to be unique</param>
		/// <returns>new list containing items with unique IDs</returns>
		public static List<T> Distinct<T, TKey>(List<T> input, Func<T, TKey> unique)
		{
			var h = new HashSet<TKey>();
			var output = new List<T>(input.Count);
			foreach (T t in input)
			{
				TKey key = unique(t);
				if (!h.Contains(key))
				{
					h.Add(key);
					output.Add(t);
				}
			}
			return output;
		}


		/// <summary>
		/// Swap two array elements and find their indexes.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array">array containing elements to be swapped</param>
		/// <param name="e1">element 1</param>
		/// <param name="e2">element 2</param>
		/// <param name="i1">index where e1 was found before swap, or -1 if e1 or e2 not found</param>
		/// <param name="i2">index where e2 was found before swap, or -1 if e1 or e2 not found</param>
		/// <returns>true if swapped, false if not</returns>
		public static bool Swap<T>(IList<T> array, T e1, T e2, out int i1, out int i2)
		{
			i2 = -1;
			i1 = array.IndexOf(e1);
			if (i1 == -1) return false;
			i2 = array.IndexOf(e2);
			if (i2 == -1) return false;
			T t = array[i1];
			array[i1] = array[i2];
			array[i2] = t;
			return true;
		}


		#region byte arrays
		/// <summary>
		/// Insert the bytes into a byte array at the given field position. 
		/// Fields are defined by a given separator byte.
		/// </summary>
		/// <remarks>"one,two,,four" insert 123 in field 2 -> "one,two,123,four"</remarks>
		/// <param name="data">original byte array</param>
		/// <param name="insert">bytes to insert</param>
		/// <param name="sep">separator</param>
		/// <param name="field">0 based field number</param>
		/// <returns></returns>
		public static byte[] InsertByteArray(byte[] data, byte[] insert, byte sep, int field)
		{
			int len = insert.Length;
			int i = FindPosition(data, sep, 0, field);
			var data2 = new byte[data.Length + len];
			Buffer.BlockCopy(data, 0, data2, 0, i);
			Buffer.BlockCopy(insert, 0, data2, i, len);
			Buffer.BlockCopy(data, i, data2, i + len, data.Length - i);
			return data2;
		}

		public static byte[] InsertByteArrayAfterSeparator(byte[] data, byte[] insert, byte sep, int field)
		{
			int len = insert.Length;
			int i = FindPosition(data, sep, 0, field) + 1;
			var data2 = new byte[data.Length + len];
			Buffer.BlockCopy(data, 0, data2, 0, i);
			Buffer.BlockCopy(insert, 0, data2, i, len);
			Buffer.BlockCopy(data, i, data2, i + len, data.Length - i);
			return data2;
		}


		//# IM-3377
		public static byte[] RemoveField(byte[] data, byte sep, int field)
		{
			int i, j, n;
			if (GetFieldStartEnd(data, sep, field, out i, out j, out n)) return null;
			var data2 = new byte[data.Length - n];
			Buffer.BlockCopy(data, 0, data2, 0, i);
			Buffer.BlockCopy(data, j, data2, i, data.Length - j);
			return data2;
		}

		public static byte[] GetField(byte[] data, byte sep, int field)
		{
			int i, j, n;
			if (GetFieldStartEnd(data, sep, field, out i, out j, out n)) return null;
			var slice = new byte[n];
			Array.Copy(data, i, slice, 0, n);
			return slice;
		}

		private static bool GetFieldStartEnd(byte[] data, byte sep, int field, out int start, out int end, out int length)
		{
			int i, j;
			if (field == 0)
			{
				i = -1;
				j = FindPosition(data, sep, i + 1, 0);
			}
			else
			{
				i = FindPosition(data, sep, 0, field - 1);
				j = FindPosition(data, sep, i + 1, 0);
			}
			if (j == 0) j = data.Length;
			length = j - ++i;
			start = i;
			end = j;
			return length < 0;
		}


		private static int FindPosition(byte[] data, byte sep, int start, int field)
		{
			int i = start;
			while (i < data.Length)
			{
				if (data[i] == sep && field-- <= 0) break;
				i++;
			}
			return i;
		}

		//. IM-3377

		#endregion byte arrays

	    public static IEnumerable<Enum> GetFlags(this Enum input)
	    {
	        var values = Enum.GetValues(input.GetType());
	        var first = true;
	        foreach (Enum value in values)
	        {
	            if (first)
	            {
                    if (input.Equals(values.First()))
                    {
                        yield return value;
                        yield break;
                    }
	                first = false;
	                continue;
	            }
	            if (input.HasFlag(value))
	            {
	                yield return value;
	            }
	        }
	    }

	}
}