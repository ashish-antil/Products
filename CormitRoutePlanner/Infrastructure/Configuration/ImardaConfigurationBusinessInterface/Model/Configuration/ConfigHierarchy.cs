using System.Runtime.Serialization;
using FernBusinessBase;


namespace ImardaConfigurationBusiness
{
	[DataContract]
	public class ConfigHierarchy : BaseEntity
	{
		private readonly Configuration[] _Values;

		public ConfigHierarchy()
		{
			_Values = new Configuration[5];
		}

		/// <summary>
		/// Find max level. E.g. only root item: Depth()==0; only Level1 filled in: Depth()==1, etc.
		/// </summary>
		/// <returns></returns>
		public int Depth()
		{
			int n = 5;
			while (n-- > 0) if (_Values[n] != null) return n + 1;
			return 0;
		}

		public Configuration this[int i]
		{
			get { return _Values[i]; }
			set { _Values[i] = value; }
		}
	}

}
