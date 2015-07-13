using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imarda.Lib;

namespace Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes
{
	public class CfgMeasurement : ConfigItemVersion
	{
		public CfgMeasurement()
		{
			ValueType = ConfigValueType.Measurement;
		}

		public override void Initialize(string s)
		{
			VersionValue = Measurement.Parse(s);
		}

		public override string ToString()
		{
			return Measurement.PString((IMeasurement)VersionValue);
		}

		public override ConfigItemVersion Insert(ConfigItemVersion specific)
		{
			if (specific == null) return this;

			ConfigItemVersion result;
			IMeasurement generalValue = (IMeasurement)VersionValue;
			switch (specific.ValueType)
			{
					// Multiply by a scalar.
				case ConfigValueType.Integer:
				case ConfigValueType.Decimal:
				case ConfigValueType.Real:
					double d = Convert.ToDouble(specific.VersionValue);
					result = Clone(this);
					result.VersionValue = generalValue.AsMeasurement() * d;
					return result;

					// If specific string is something like "Speed:kph" then convert the Measurement
					// to a speed and then to a string using the format specifier behind ':'. This
					// format specifier is interpreted by the MeasurementFormatInfo.Default.
					// The resulting item is a CfgText, no longer a CfgMeasurement!
					// You can pass a CfgText as an Application Parameter and influence the formatting 
					// using application preferences.
				case ConfigValueType.StringLiteral:
				case ConfigValueType.StringWithArgs:
					string s = (string)specific.VersionValue;
					int p = s.IndexOf(':');
					string name = s.Substring(0, p);
					string unit = s.Substring(p + 1);
					result = new CfgText(ConfigValueType.StringLiteral)
					{
						Combine = this.Combine,
						UID = specific.UID,
						VersionValue = generalValue.AsMeasurement().ToString(name, unit)
					};
					return result;
			}
			return null;
		}
	}
}
