using System;

namespace Imarda360.Infrastructure.ConfigurationService
{
	public class BaseContext
	{
		//private int _Hierarchy;
		private Guid[] _Levels;
		private int _HashCode;

		public BaseContext(params Guid[] levels)
		{
			_Levels = levels;
			for (int i = 0; i < Depth; i++) _HashCode ^= _Levels[i].GetHashCode();
		}

		public override int GetHashCode()
		{
			return _HashCode;
		}

		public Guid this[int i]
		{
			get { return _Levels[i]; }
		}

		public Guid[] Levels
		{
			get { return (Guid[])_Levels.Clone(); }
		}

		public int Depth { get { return _Levels.Length; } }

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType()) return false;
			var other = (BaseContext) obj;
			if (other._HashCode != _HashCode || other.Depth != Depth) return false;
			for (int i = 0; i < Depth; i++)
			{
				if (other[i] != this[i]) return false;
			}
			return true;
		}
	}
}
