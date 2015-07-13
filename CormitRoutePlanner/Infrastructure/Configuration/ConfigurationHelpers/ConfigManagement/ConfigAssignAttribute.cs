using System;
using System.Collections.Generic;
using System.Text;

namespace Imarda360.Infrastructure.ConfigurationService
{
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class ConfigAssignAttribute : Attribute
	{
		public Guid ItemID { get; private set; }
		public object DefaultValue { get; private set; }
		public int ValueType { get; private set; }

		public ConfigAssignAttribute(string guid, object defaultValue)
			: this(guid, defaultValue, ConfigValueType.Unknown)
		{
		}

		public ConfigAssignAttribute(string guid, object defaultValue, int valueType)
		{
			ItemID = new Guid(guid);
			DefaultValue = defaultValue;
			ValueType = valueType;
		}

	}



}
