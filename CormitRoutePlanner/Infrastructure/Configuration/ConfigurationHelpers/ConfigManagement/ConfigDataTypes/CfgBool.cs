using System;
using System.Collections.Generic;
using System.Text;

namespace Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes
{
	public class CfgBool : ConfigItemVersion
	{
		public static readonly CfgBool False = new CfgBool { VersionValue = false };
		public static readonly CfgBool True = new CfgBool { VersionValue = true };

		public CfgBool()
		{
			ValueType = ConfigValueType.Bool;
		}

		public override void Initialize(string s)
		{
			VersionValue = (s == "1");
		}

		public override string ToString()
		{
			return (bool)VersionValue ? "1" : "0";
		}

		public override ConfigItemVersion Insert(ConfigItemVersion specific)
		{
			if (specific != null && specific.ValueType == ConfigValueType.Bool)
			{
				bool result = (bool)specific.VersionValue ^ (bool)this.VersionValue;
				return Clone(result);
			}
			else return this;
		}
	}
}
