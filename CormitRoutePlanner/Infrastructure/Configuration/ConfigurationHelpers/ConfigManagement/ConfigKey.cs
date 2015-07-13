using System;
using System.Linq;

namespace Imarda360.Infrastructure.ConfigurationService
{
	public sealed class ConfigKey
	{
		private Guid _ItemID;
		private BaseContext _Context;
		private int _HashCode;

		public ConfigKey(Guid itemID, params Guid[] levels) 
			: this(itemID, new BaseContext(levels))
		{
		}

		public ConfigKey(Guid itemID, BaseContext context)
		{
			_ItemID = itemID;
			_Context = context;
			_HashCode = _Context.GetHashCode() ^ _ItemID.GetHashCode();
		}

		public Guid ID { get { return _ItemID; } }


		public override int GetHashCode()
		{
			return _HashCode;
		}

		public BaseContext Context
		{
			get { return _Context; }
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != typeof(ConfigKey)) return false;
			var other = (ConfigKey) obj;
			if (other._ItemID != _ItemID) return false;
			return _Context.Equals(other._Context);
		}

		public override string ToString()
		{
			string levels = string.Join(", ", _Context.Levels.Select(guid => guid.ToString()).ToArray());
			return string.Format("CK({0}: {1}", _ItemID, levels);
		}
	}
}
