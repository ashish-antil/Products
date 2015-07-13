using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Imarda.Lib;

namespace Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes
{
	public class CfgParams : ConfigItemVersion
	{
		private static readonly string[] _Separator = new string[] { "||" };

		public CfgParams()
		{
			VersionValue = "";
			ValueType = ConfigValueType.Parameters;
		}

		public override void Initialize(string s)
		{
			VersionValue = s;
		}

		public IDictionary Map
		{
			get
			{
				string s = (string)VersionValue;
				return s.KeyValueMap(ValueFormat.Strings, true);
			}
		}

		public override string ToString()
		{
			return (string)VersionValue;
		}

		public override ConfigItemVersion Insert(ConfigItemVersion specific)
		{
			try
			{
				if (specific == null) return this;
				return (specific.ValueType == ConfigValueType.Parameters)
					? Clone(StringUtils.MergeNonArrayKeyValuePairs(ToString(), specific.ToString()))
					: this;
			}
			catch
			{
				return this;
			}
		}

		internal override void ResolveGuids()
		{
			VersionValue = CfgText.StringWithGuids.Replace(ToString(), new MatchEvaluator(CfgText.Substitute));
		}

	}
}
