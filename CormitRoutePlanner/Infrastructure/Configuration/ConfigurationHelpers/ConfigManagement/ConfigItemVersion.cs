using System;
using System.Collections.Generic;
using System.Text;
using Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes;

namespace Imarda360.Infrastructure.ConfigurationService
{
	/// <summary>
	/// One of the versions of the config item.
	/// </summary>
	public abstract class ConfigItemVersion : ICloneable
	{
		public static ConfigItemVersion Parse(int type, string s, bool combine, Guid uid)
		{
			var version = Create(type);
			version.Initialize(s);
			version.Combine = combine;
			version.UID = uid;
			return version;
		}

		public static ConfigItemVersion Create(int type, object value, bool combine, Guid uid)
		{
			var version = Create(type);
			version.VersionValue = value;
			version.Combine = combine;
			version.UID = uid;
			return version;
		}


		private static ConfigItemVersion Create(int type)
		{
			ConfigItemVersion result;
			switch (type)
			{
				case ConfigValueType.StringLiteral:
				case ConfigValueType.StringWithArgs:
					result = new CfgText(type);
					break;
				case ConfigValueType.Bool:
					result = new CfgBool();
					break;
				case ConfigValueType.Integer:
				case ConfigValueType.Decimal:
				case ConfigValueType.Real:
					result = new CfgNumber(type);
					break;
				case ConfigValueType.Color:
					result = new CfgColor();
					break;
				case ConfigValueType.Parameters:
					result = new CfgParams();
					break;
				case ConfigValueType.Xml:
					result = new CfgXml();
					break;
				case ConfigValueType.Measurement:
					result = new CfgMeasurement();
					break;
				//case ConfigValueType.Uri:
				//case ConfigValueType.Font:
				//case ConfigValueType.RichText:
				default:
					throw new ConfigException(type + ": data type is not yet supported");
			}
			return result;
		}

		/// <summary>
		/// Initialize this object with the string from the data storage. This will
		/// parse the string based on the ValueType and stores the resulting object.
		/// RawValue will return this object.
		/// </summary>
		/// <param name="s"></param>
		public abstract void Initialize(string s);

		/// <summary>
		/// Convert the object to a string for persistence.
		/// </summary>
		/// <returns></returns>
		public abstract new string ToString();

		/// <summary>
		/// The value as persisted, no parameters filled in.
		/// </summary>
		public object VersionValue { get; set; }

		/// <summary>
		/// Defines which ValueType is to be stored in the database.
		/// This helps the version value parser in case we don't store XML.
		/// </summary>
		public int ValueType { get; set; }

		/// <summary>
		/// Operation to be applied in the Combine method. Defined in the database.
		/// </summary>
		public bool Combine { get; set; }

		/// <summary>
		/// The primary key for saving configuration records.
		/// </summary>
		public Guid UID { get; set; }

		/// <summary>
		/// Defines how the given more specific version can be inserted into placeholders of this version.
		/// </summary>
		/// <param name="general"></param>
		/// <returns></returns>
		public abstract ConfigItemVersion Insert(ConfigItemVersion specific);

		#region ICloneable Members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		public ConfigItemVersion Clone(object val)
		{
			ConfigItemVersion c = (ConfigItemVersion)Clone();
			c.VersionValue = val;
			return c;
		}

		internal virtual void ResolveGuids()
		{
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType()) return false;
			ConfigItemVersion other = (ConfigItemVersion)obj;
			if (ValueType != other.ValueType) return false;
			if (VersionValue == null) return (other.VersionValue == null);
			return VersionValue.Equals(other.VersionValue);
		}

		public override int GetHashCode()
		{
			return (VersionValue == null) ? 0 : VersionValue.GetHashCode();
		}
	}

}
