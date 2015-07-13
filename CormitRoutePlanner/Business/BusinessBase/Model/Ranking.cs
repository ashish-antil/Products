using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FernBusinessBase;

namespace AbstractEntity
{
	/// <summary>
	/// Helps ordering BaseEntity objects in a way that they can get saved without
	/// violating foreign key constraints.
	/// </summary>
	/// <remarks>
	/// This is used to determine which entities should be saved first in order
	/// to not violate foreign key constraints in the database.
	/// A tool "C:\TeamImarda\Imarda360\Imarda360.Tools\GraphOrderTool".
	/// </remarks>
	public sealed class Ranking
	{
		#region static
		private static Dictionary<string, Ranking> _Instances = new Dictionary<string, Ranking>();

		public static Ranking Instance(string key)
		{
			Ranking rank;
			if (!_Instances.TryGetValue(key, out rank))
			{
				_Instances[key] = rank = new Ranking();
			}
			return rank;
		}
		#endregion

		/// <summary>
		/// Constructs the one and only Rank.
		/// </summary>
		private Ranking()
		{
			_Map = new Dictionary<Type, int>();
		}


		private Dictionary<Type, int> _Map;

		public void Add(Type type, int rank)
		{
			_Map.Add(type, rank);
		}

		public void Order(BaseEntity[] array)
		{
			Array.Sort<BaseEntity>(array, (e1, e2) => _Map[e1.GetType()] - _Map[e2.GetType()]);
		}
	}
}
