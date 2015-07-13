using System.Collections.Generic;
using System.Linq;

namespace Imarda.Lib
{
	public interface IMatch
	{
		bool IsMatch(string s);
		bool IsEqual(string s);
	}

	public enum MatchType
	{
		NoMatch = 0,
		Pattern = 1,
		Equal = 2
	}

	/// <summary>
	/// Builds a temporary tree structure of objects to find out the depth and then
	/// creates a list ordered by deepest node descending. The FindMatch tries
	/// to find a equal node, if that fails, tries to match against pattern.
	/// </summary>
	/// <typeparam name="K">id/key type, e.g. int, Guid</typeparam>
	/// <typeparam name="T">application object</typeparam>
	public class Classifier<K, T> where T : IMatch
	{
		private List<T> _Ordered = new List<T>();

		public Builder BeginBuild()
		{
			return new Builder();
		}

		public void Add(Builder builder, T obj, K id, K idParent)
		{
			builder.Map[id] = new Node(obj, idParent);
		}

		public void EndBuild(Builder builder)
		{
			Dictionary<K, Node> b = builder.Map;
			foreach (Node node in b.Values)
			{
				Node n = node;
				int i = 0;
				while (b.TryGetValue(n.ParentID, out n)) i++;
				node.Depth = i;
			}
			_Ordered = b.Values.OrderByDescending(n => n.Depth).Select(n => n.Obj).ToList();
		}

		public MatchType FindMatch(string s, out T value)
		{
			value = default(T);
			foreach (T n in _Ordered)
			{
				if (n.IsEqual(s))
				{
					value = n;
					return MatchType.Equal;
				}
				else if (n.IsMatch(s))
				{
					value = n;
					return MatchType.Pattern;
				}
			}
			return MatchType.NoMatch;
		}

		#region Nested type: Builder

		public class Builder
		{
			internal Builder()
			{
				Map = new Dictionary<K, Node>();
			}

			internal Dictionary<K, Node> Map { get; private set; }
		}

		#endregion

		#region Nested type: Node

		internal class Node
		{
			internal Node(T obj, K parentID)
			{
				Obj = obj;
				ParentID = parentID;
			}

			internal K ParentID { get; set; }
			internal int Depth { get; set; }
			internal T Obj { get; set; }
		}

		#endregion
	}
}