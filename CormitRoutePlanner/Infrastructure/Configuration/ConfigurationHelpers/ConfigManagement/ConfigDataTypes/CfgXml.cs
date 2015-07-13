using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ImardaConfigurationBusiness;

namespace Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes
{
	/// <summary>
	/// Base class for XML documents.
	/// </summary>
	public class CfgXml : ConfigItemVersion
	{
		public CfgXml()
		{
			ValueType = ConfigValueType.Xml;
		}

		public override void Initialize(string s)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(s);
			VersionValue = doc;
		}

		public override string ToString()
		{
			return Doc.OuterXml;
		}

		/// <summary>
		/// Override in the subclass to define what 'Insert' means for that particular
		/// Xml schema.
		/// </summary>
		/// <param name="specific"></param>
		/// <returns></returns>
		public override ConfigItemVersion Insert(ConfigItemVersion specific)
		{
			return this;
		}

		/// <summary>
		/// Just a wrapper for getting the XmlDocument type rather than Object.
		/// </summary>
		private XmlDocument Doc
		{
			get { return VersionValue as XmlDocument; }
			set { VersionValue = value; }
		}
	}
}
