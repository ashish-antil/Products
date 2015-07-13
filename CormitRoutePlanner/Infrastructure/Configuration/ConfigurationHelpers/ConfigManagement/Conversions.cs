using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes;
using System.Xml;
using System.Drawing;

namespace Imarda360.Infrastructure.ConfigurationService
{
	public class Conversions
	{
		
		/// <summary>
		/// Searches for argument placeholders
		/// </summary>
		public static Regex StringWithArgs = new Regex(@"\{[0-9][^\}]*\}|#\{[^\}]*\}|@\{\w{8}-\w{4}-\w{4}-\w{4}-\w{12}\}", RegexOptions.Compiled);
		// 												{0}  {1:D2}  | #{text}   |   @{C065F955-1FAD-40df-B2FC-285D62F14E0A}


		public static ConfigItemVersion ToConfigItemVersion(object value)
		{
			if (value == null) throw new ConfigException("Cannot convert null");
			int type;
			ConfigItemVersion ver = null;
			string name = value.GetType().FullName;
			switch (name)
			{
				case "System.String":
					type = StringWithArgs.IsMatch((string)value)
						? ConfigValueType.StringWithArgs 
						: ConfigValueType.StringLiteral;
					ver = new CfgText { VersionValue = value, ValueType = type };
					break;
				case "System.Boolean":
					ver = new CfgBool { VersionValue = value };
					break;
				case "System.Drawing.Color":
					ver = new CfgColor { VersionValue = value };
					break;
				case "System.Decimal":
					ver = new CfgNumber{ VersionValue = value, ValueType = ConfigValueType.Decimal };
					break;
				case "System.Double":
				case "System.Float":
					ver = new CfgNumber{ VersionValue = value, ValueType = ConfigValueType.Real };
					break;
				case "System.Uri":
					//TODO ConfigurationService: CfgURI to be implemented
					break;
				case "Imarda.Lib.Measurement":
					ver = new CfgMeasurement { VersionValue = value, ValueType = ConfigValueType.Measurement };
					break;
				default:
					if (name.StartsWith("System.Xml.Xml"))
					{
						ver = new CfgXml { VersionValue = value };
					}
					else if (name.StartsWith("System.Int") || name.StartsWith("System.UInt"))
					{
						ver = new CfgNumber { VersionValue = value };
					}
					break;
			}
			return ver;

		}

		public static object ToValue(string s, int valueType)
		{
			switch (valueType)
			{
				case ConfigValueType.StringLiteral:
				case ConfigValueType.StringWithArgs:
				case ConfigValueType.RichText:
					return s;
				case ConfigValueType.Bool:
					return Convert.ToBoolean(s);
				case ConfigValueType.Integer:
					return Convert.ToInt32(s);
				case ConfigValueType.Decimal:
					return Convert.ToDecimal(s);
				case ConfigValueType.Real:
					return Convert.ToDouble(s);
				//case ConfigValueType.Measurement:
					//TODO convert to Length, Speed, Duration, etc.
					//return ...
				case ConfigValueType.Color:
					return CfgColor.Parse(s) ?? Color.Transparent;
				case ConfigValueType.Uri:
					return new Uri(s, UriKind.RelativeOrAbsolute);
				case ConfigValueType.Xml:
					XmlDocument doc = new XmlDocument();
					doc.LoadXml(s);
					return doc;
				default:
					return null;
			}
		}


	}

}
