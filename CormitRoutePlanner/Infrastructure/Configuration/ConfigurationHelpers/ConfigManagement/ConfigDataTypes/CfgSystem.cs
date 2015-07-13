using System;
using System.Collections.Generic;
using System.Text;

namespace Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes
{
	/// <summary>
	/// Used by the Configuration System for special control functions.
	/// A control function is activated by assigning it to the ConfigItem.Parameter
	/// before calling CalcValue().
	/// </summary>
	public class CfgSystem : ConfigItemVersion
	{
		/// <summary>
		/// Assign to ConfigItem.Parameter to grab the first version instead of 
		/// calculating the whole list of versions. Do not replace GUID placeholders either.
		/// </summary>
		public static CfgSystem First = new CfgSystem("First");

		private CfgSystem(string s)
		{
			ValueType = ConfigValueType.System;
			VersionValue = s;
		}

		public override void Initialize(string s)
		{
		}

		public override string ToString()
		{
			return "CfgSystem(" + VersionValue + ")";
		}

		public override ConfigItemVersion Insert(ConfigItemVersion specific)
		{
			return specific;
		}
	}
}
