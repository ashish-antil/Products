using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imarda.Lib
{
	public enum SortOrder
	{
		Asc,
		Desc
	}

	/// <summary>
	/// Sorted queue for items of type T, using type S values to sort on.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="S"></typeparam>
	public class SortedQueue<T, S> : IEnumerable<T>
		where S : IComparable
	{
		private readonly IComparer<SortedItem> _Comparer;
		private readonly List<SortedItem> _List;
		private readonly S _LowestPriority;

		/// <summary>
		/// Create a SortedQueue
		/// </summary>
		/// <param name="order">sort asc or desc</param>
		/// <param name="lowestPriority">lowest priority: item to be enqueued at queue end, which is faster then priority queuing</param>
		public SortedQueue(SortOrder order, S lowestPriority)
		{
			_LowestPriority = lowestPriority;
			_List = new List<SortedItem>();
			if (order == SortOrder.Asc) _Comparer = new AscendingComparer();
			else _Comparer = new DescendingComparer();
		}

		/// <summary>
		/// Number of items in the queue.
		/// </summary>
		public int Count
		{
			get { return _List.Count; }
		}

		#region IEnumerable<T> Members

		/// <summary>
		/// Returns head first, tail last. Not threadsafe.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
			return _List.Select(pitem => pitem.Item).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		public T Peek()
		{
			return _List.Count == 0 ? default(T) : _List[0].Item;
		}


		/// <summary>
		/// Enqueue the item with the given sortvalue.
		/// </summary>
		/// <param name="item">item to enqueue</param>
		/// <param name="sortValue">determines insertion into the queue</param>
		public void Enqueue(T item, S sortValue)
		{
			var pItem = new SortedItem(item, sortValue);
			int index = sortValue.Equals(_LowestPriority) ? -_List.Count - 1 : _List.BinarySearch(pItem, _Comparer);
			_List.Insert(~index, pItem);
		}

		/// <summary>
		/// Remove and return the item at the head of the queue.
		/// </summary>
		/// <returns>Same as Peek() but removes the element as well</returns>
		public T Dequeue()
		{
			if (_List.Count == 0) return default(T);
			T item = _List[0].Item;
			_List.RemoveAt(0);
			return item;
		}

		/// <summary>
		/// Remove the item from the queue if it exists
		/// </summary>
		/// <param name="item">item to remove</param>
		/// <returns>true if removed, false if not found</returns>
		public bool Remove(T item)
		{
			int i = 0;
			for (; i < _List.Count; i++)
			{
				if (_List[i].Item.Equals(item))
				{
					_List.RemoveAt(i);
					break;
				}
			}
			return i <= _List.Count;
		}


		public bool RemoveWhere(Predicate<T> predicate)
		{
			int i = 0;
			while (i < _List.Count)
			{
				var item = _List[i].Item;
				if (predicate(item)) _List.RemoveAt(i);
				else i++;
			}
			return i <= _List.Count;
		}


		/// <summary>
		/// Copy the items into an array, head at index 0.
		/// </summary>
		/// <returns></returns>
		public T[] GetItemArray()
		{
			return _List.Select(pitem => pitem.Item).ToArray();
		}

		public override string ToString()
		{
			return string.Join(", ", GetItemArray().Select(t => string.Format("{0}", t)).ToArray());
		}

		#region Nested type: AscendingComparer

		/// <summary>
		/// Compare item to be inserted to queued items. 
		/// Will be added to the end of the subqueue as defined by the priorities.
		/// </summary>
		private class AscendingComparer : IComparer<SortedItem>
		{
			#region IComparer<SortedQueue<T,S>.SortedItem> Members

			public int Compare(SortedItem x, SortedItem y)
			{
				if (x.Item.Equals(y.Item)) return 0;
				int c = y.SortValue.CompareTo(x.SortValue);
				return c == 0 ? -1 : c;
			}

			#endregion
		}

		#endregion

		#region Nested type: DescendingComparer

		private class DescendingComparer : IComparer<SortedItem>
		{
			#region IComparer<SortedQueue<T,S>.SortedItem> Members

			public int Compare(SortedItem x, SortedItem y)
			{
				if (x.Item.Equals(y.Item)) return 0;
				int c = x.SortValue.CompareTo(y.SortValue);
				return c == 0 ? 1 : c;
			}

			#endregion
		}

		#endregion

		#region Nested type: SortedItem

		/// <summary>
		/// Decorates item with Sorted information.
		/// </summary>
		private class SortedItem
		{
			internal readonly T Item;
			internal readonly S SortValue;

			internal SortedItem(T item, S sortValue)
			{
				Item = item;
				SortValue = sortValue;
			}
		}

		#endregion
	}
}