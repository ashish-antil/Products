using System;
using System.Reflection;
using System.Xml;
using System.Collections.Generic;
using ImardaConfigurationBusiness;

namespace Imarda360.Infrastructure.ConfigurationService
{
	/// <summary>
	/// Base class for classes that contain a list of members that should get the
	/// values passed to the constructor.
	/// </summary>
	public abstract class ConfigGroup
	{
		public static Guid[] GetIDs(object group)
		{
			var ids = new List<Guid>();
			FieldInfo[] fields = group.GetType().GetFields();

			foreach (FieldInfo info in fields)
			{
				var attr = Attribute.GetCustomAttribute(info, typeof(ConfigAssignAttribute));
				if (attr != null)
				{
					Guid id = ((ConfigAssignAttribute)attr).ItemID;
					ids.Add(id);
				}
			}
			return ids.ToArray();
		}

		public Guid[] GetIDs()
		{
			return GetIDs(this);
		}

		public static void SetValues(object group, ConfigValue[] values)
		{
			FieldInfo[] fields = group.GetType().GetFields();

			int i = 0;
			foreach (FieldInfo info in fields)
			{
				// scope allows variables to be used in the catch{}
				Guid id = Guid.Empty;
				int vt = ConfigValueType.Unknown;
				object val = null;
				try
				{
					ConfigAssignAttribute attr = (ConfigAssignAttribute)Attribute.GetCustomAttribute(info, typeof(ConfigAssignAttribute));
					if (attr != null)
					{
						ConfigValue cfg = values[i++];
						if (cfg != null)
						{
							val = cfg.Value;
							vt = cfg.Type;
						}
						else // use default value taken from the attribute
						{
							val = attr.DefaultValue;
							vt = attr.ValueType;
							if (val is string)
							{
								if (typeof(XmlDocument).IsAssignableFrom(info.FieldType)) vt = ConfigValueType.Xml;
								val = (vt == ConfigValueType.Unknown) 
									? Convert.ChangeType(val, info.FieldType) 
									: Conversions.ToValue((string)val, vt);
							}
							else
							{
								ConfigItemVersion version = Conversions.ToConfigItemVersion(val);
								val = version.VersionValue;
							}
						}
						info.SetValue(group, val);
					}
				}
				catch (Exception ex)
				{
					throw new ConfigException("Field initialization", ex) { ID = id, ValueType = vt, Value = val };
				}
			}
		}


		public void SetValues(ConfigValue[] values)
		{
			SetValues(this, values);
		}
	}
}

