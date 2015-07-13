using System;
using System.Collections.Generic;
using System.Text;
using ImardaConfigurationBusiness;

namespace Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes
{
	public class CfgNumber : ConfigItemVersion
	{
		public CfgNumber()
		{
			ValueType = ConfigValueType.Integer;
		}

		public CfgNumber(int type)
		{
			ValueType = type;
		}

		public override void Initialize(string s)
		{
			switch (ValueType)
			{
				case ConfigValueType.Integer:
					int i;
					if (int.TryParse(s, out i)) VersionValue = i;
					break;
				case ConfigValueType.Decimal:
					decimal m;
					if (decimal.TryParse(s, out m)) VersionValue = m;
					break;
				case ConfigValueType.Real:
					double d;
					if (double.TryParse(s, out d)) VersionValue = d;
					break;
			}
		}

		public override string ToString()
		{
			return VersionValue.ToString();
		}

		public override ConfigItemVersion Insert(ConfigItemVersion specific)
		{
			if (specific == null) return this;

			object result = null;

			switch (ValueType)
			{
				case ConfigValueType.Integer:
					int i1 = (int)VersionValue;
					switch (specific.ValueType)
					{
						case ConfigValueType.Integer:
							result = (int)specific.VersionValue + i1;
							break;
						case ConfigValueType.Decimal:
							result = Convert.ToInt32((decimal)specific.VersionValue + i1);
							break;
						case ConfigValueType.Real:
							result = Convert.ToInt32((double)specific.VersionValue * i1); // factor
							break;
					}
					break;
				case ConfigValueType.Decimal:
					decimal m1 = (decimal)VersionValue;
					switch (specific.ValueType)
					{
						case ConfigValueType.Integer:
							result = (int)specific.VersionValue + m1;
							break;
						case ConfigValueType.Decimal:
							result = (decimal)specific.VersionValue + m1;
							break;
						case ConfigValueType.Real:
							result = (decimal)specific.VersionValue * m1; // factor
							break;
					}
					break;
				case ConfigValueType.Real: // factor
					double d1 = (double)VersionValue;
					switch (specific.ValueType)
					{
						case ConfigValueType.Integer:
							result = (int)specific.VersionValue * d1;
							break;
						case ConfigValueType.Decimal:
							result = Convert.ToDecimal((double)(decimal)specific.VersionValue * d1);
							break;
						case ConfigValueType.Real:
							result = (double)specific.VersionValue * d1;
							break;
					}
					break;
			}
			return result == null ? this : Clone(result);
		}
	}
	
}
