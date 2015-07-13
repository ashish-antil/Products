using System;
using Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes;


namespace Imarda360.Infrastructure.ConfigurationService
{
	/// <summary>
	/// Represents a piece of configuration information. A ConfigItem consists of an 
	/// ordered list of one or more ConfigItemVersion objects, each corresponding to
	/// a row in the database. A ConfigItem may be specific with regard to:
	/// language, country, company, user role type, user.
	/// </summary>
	public class ConfigItem
	{
		private ConfigItemVersion _CalculatedValue;
		private ConfigItemVersion _ApplicationParameter;

		#region ConfigItem Members


		public Guid ID { get; set; }

		public ConfigItemVersion[] Versions { get; set; }

		public object Parameter
		{
			get { return _ApplicationParameter; }
			set
			{
				if (value == null) _ApplicationParameter = null;
				else if (value == CfgSystem.First) _ApplicationParameter = CfgSystem.First;
				else if (value is ConfigItemVersion) _ApplicationParameter = (ConfigItemVersion)value;
				else _ApplicationParameter = Conversions.ToConfigItemVersion(value);

				_CalculatedValue = null; // set dirty
			}
		}


		/// <summary>
		/// Calculate the value from the ordered list of versions.
		/// </summary>
		/// <remarks>
		/// Iterate thru the versions from most specific to most general.
		/// For each version first resolve the GUID placeholders then insert
		/// the previously calculated version in the more general version.
		/// Stop this loop when either the top level is reached or if the 
		/// resulting version is not marked as 'Combine' (which means it must be 'Override').
		/// </remarks>
		public ConfigItemVersion CalcValue()
		{
			Guid mostSpecificUID = Versions[0].UID;
			if (_CalculatedValue == null)
			{
				if (_ApplicationParameter == CfgSystem.First) return Versions[0];

				ConfigItemVersion specializedVersion = _ApplicationParameter;
				if (specializedVersion != null) specializedVersion.ResolveGuids();
				for (int i = 0; i < Versions.Length; i++)
				{
					ConfigItemVersion version = Versions[i];
					version.ResolveGuids();
					ConfigItemVersion result = version.Insert(specializedVersion);
					if (!result.Combine)
					{
						_CalculatedValue = result;
						break;
					}
					version = result;
					specializedVersion = version;
				}
			}
			_CalculatedValue.UID = mostSpecificUID;
			return _CalculatedValue;
		}

		/// <summary>
		/// The default value type in case
		/// </summary>
		public int DefaultValueType
		{
			get { return Versions[0].ValueType; }
		}

		#endregion


	}
}
