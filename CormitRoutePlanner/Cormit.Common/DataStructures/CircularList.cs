using System;

namespace Imarda.Lib
{
	/// <summary>
	/// Threadsafe circular buffer.
	/// </summary>
	public class CircularList<T>
	{
		private readonly object _Sync = new object();

		private int _Count;
		private Node _Current;
		private Node _Start;


		public CircularList()
		{
		}

		public CircularList(T nullItem)
		{
			NullItem = nullItem;
		}

		public T NullItem { get; private set; }

		public T CurrentItem
		{
			get
			{
				lock (_Sync)
				{
					return _Current == null ? NullItem : _Current.Item;
				}
			}
		}

		public bool AtStart
		{
			get { lock (_Sync) return _Current == _Start; }
		}

		public IPosition Position
		{
			get { lock (_Sync) return _Current; }
			set
			{
				var n = (Node) value;
				lock (_Sync)
				{
					if (Exists(n)) _Current = n;
					else throw new ArgumentException("Node not found");
				}
			}
		}

		public int Count
		{
			get { lock (_Sync) return _Count; }
		}

		public T MoveNext()
		{
			lock (_Sync)
			{
				if (_Current == null)
				{
					return NullItem;
				}
				else
				{
					_Current = _Current.Next;
					return _Current.Item;
				}
			}
		}

		private bool Exists(Node node)
		{
			lock (_Sync)
			{
				Node n = _Start;
				if (n == null) return false;
				do
				{
					if (n == node) return true;
					n = n.Next;
				} while (n != _Start);
				return false;
			}
		}

		public void Reset()
		{
			lock (_Sync) _Current = null;
		}

		public T[] ToArray()
		{
			lock (_Sync)
			{
				var arr = new T[_Count];
				Node n = _Start;
				for (int i = 0; i < _Count; i++)
				{
					arr[i] = n.Item;
					n = n.Next;
				}
				return arr;
			}
		}

		public void Do(Func<T, T> func)
		{
			lock (_Sync)
			{
				Node n = _Start;
				for (int i = 0; i < _Count; i++)
				{
					n.Item = func(n.Item);
					n = n.Next;
				}
			}
		}

		public T First(Predicate<T> condition)
		{
			lock (_Sync)
			{
				Node n = _Start;
				for (int i = 0; i < _Count; i++)
				{
					if (condition(n.Item)) return n.Item;
					n = n.Next;
				}
				return NullItem;
			}
		}

		public void InsertPrevious(T item)
		{
			lock (_Sync) InsertBeforeNode(_Current, new Node(item));
		}

		public void InsertNext(T item)
		{
			lock (_Sync) InsertAfterNode(_Current, new Node(item));
		}

		public void InsertAtEnd(T item)
		{
			lock (_Sync) InsertBeforeNode(_Start, new Node(item));
		}

		public void InsertAtStart(T item)
		{
			lock (_Sync)
			{
				var node = new Node(item);
				InsertAfterNode(_Start, node);
				_Start = node;
			}
		}

		public void MakeStart()
		{
			lock (_Sync) _Start = _Current;
		}

		public void Restart()
		{
			lock (_Sync) _Current = _Start;
		}

		public void Clear()
		{
			lock (_Sync) _Current = _Start = null;
		}


		public T RemoveCurrent()
		{
			lock (_Sync)
			{
				T item;
				if (_Current == null)
				{
					item = NullItem;
				}
				else
				{
					Node c = _Current;
					item = c.Item;
					if (--_Count == 0)
					{
						_Current = _Start = null;
					}
					else
					{
						c.Previous.Next = c.Next;
						c.Next.Previous = c.Previous;
						_Current = c.Next;
						if (c == _Start) _Start = _Current;
					}
				}
				return item;
			}
		}

		private void InsertBeforeNode(Node n0, Node n1)
		{
			if (n0 == null)
			{
				n1.Next = n1.Previous = _Start = _Current = n1;
			}
			else
			{
				n1.Next = n0;
				n1.Previous = n0.Previous;
				n0.Previous.Next = n1;
				n0.Previous = n1;
			}
			_Count++;
		}

		private void InsertAfterNode(Node n0, Node n1)
		{
			if (n0 == null)
			{
				n1.Next = n1.Previous = _Start = _Current = n1;
			}
			else
			{
				n1.Next = n0.Next;
				n1.Previous = n0;
				n0.Next.Previous = n1;
				n0.Next = n1;
			}
			_Count++;
		}

		#region Nested type: IPosition

		public interface IPosition
		{
		}

		#endregion

		#region Nested type: Node

		private class Node : IPosition
		{
			internal T Item;
			internal Node Next;
			internal Node Previous;

			internal Node(T item)
			{
				Item = item;
			}
		}

		#endregion
	}
}