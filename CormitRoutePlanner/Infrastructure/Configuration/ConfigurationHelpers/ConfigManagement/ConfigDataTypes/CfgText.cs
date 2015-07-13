using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Collections;
using ImardaConfigurationBusiness;
using Imarda.Lib;

namespace Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes
{

	public class CfgText : ConfigItemVersion
	{
		// e.g. "Hello #{mate}!"
		internal static Regex PlaceHolderFinder = new Regex(@"#\{([^\}]*)\}", RegexOptions.Compiled);

		// e.g. "Just click #{Cancel|Ok}."
		internal static Regex BoolPlaceHolderFinder = new Regex(@"#\{([^\|]*)\|([^\}]*)\}", RegexOptions.Compiled);

		internal static Regex StringWithGuids = new Regex(@"@\{\w{8}-\w{4}-\w{4}-\w{4}-\w{12}\}", RegexOptions.Compiled);

		private static char[] _Bar = new char[] { '|' };

		public CfgText() : this(ConfigValueType.StringWithArgs)
		{
		}

		public CfgText(int type)
		{
			VersionValue = "";
			ValueType = type;
		}

		public override void Initialize(string s)
		{
			VersionValue = s;
		}

		public override string ToString()
		{
			return (string)VersionValue;
		}


		public override ConfigItemVersion Insert(ConfigItemVersion specific)
		{
			string fmt = (string)VersionValue;
			string res = null;
			if (ValueType == ConfigValueType.StringLiteral) return this;

			if (specific != null && (specific.ValueType == ConfigValueType.Bool || specific.ValueType == ConfigValueType.Integer))
			{
				if (specific.ValueType == ConfigValueType.Bool)
				{
					// Finds placeholders in phrases like "#{No|Yes}, you #{don't |}have permission to do this."
					// If the boolean specific value is false, the string to the left of | is selected, and if true, the string to the right.
					bool b = (bool)specific.VersionValue;
					res = BoolPlaceHolderFinder.Replace(fmt, match => match.Groups[b ? 2 : 1].Value);
				}
				else // ConfigValueType.Integer
				{
					// Finds placeholders in phrases like "This is the #{first|second|third} time.",  "It takes #{} minute#{|s}"
					int arg = (int)specific.VersionValue;
					res = PlaceHolderFinder.Replace(fmt, delegate(Match m)
					{
						System.Diagnostics.Trace.WriteLine(fmt);
						if (arg == 0) return "";
						string[] options = m.Groups[1].Value.Split('|');
						if (options.Length == 1) return arg.ToString();
						else return options[Math.Min(arg - 1, options.Length - 1)];
					});
				}
			}
			else if (specific != null && specific.ValueType == ConfigValueType.Parameters)
			{
				CfgParams parameters = (CfgParams)specific;
				IDictionary map = parameters.Map;
				res = PlaceHolderFinder.Replace(fmt, 
					delegate (Match match) {
						string betweenBraces = match.Groups[1].Value;
						bool startsWithExclamation = betweenBraces.StartsWith("!");
						string key = startsWithExclamation ? betweenBraces.Substring(1) : betweenBraces;
						return (string)map[key] ?? (startsWithExclamation ? "" : betweenBraces); 
						// if no actual param found: item like "#{!test}" gets replaced by empty string, whereas #{test} gets replaced by "test".
					}					
				);
			}
			else
			{
				if (specific != null && !PlaceHolderFinder.IsMatch(fmt))
				{
					fmt += " #{}"; // append at end, with space between, if no placeholder found
				}

				string arg;
				if (specific == null)
				{
					arg = null;
				}
				else if (specific is CfgMeasurement)
				{
					arg = ((IMeasurement)specific.VersionValue).ToString("^", MeasurementFormatInfo.Default);
				}
				else
				{
					arg = specific.VersionValue.ToString();
				}

				// Finds placeholders as in "Hello #{mate}! How are you #{dude}?"
				// The values between #{ and } are the defaults for when there is no specific version.
				bool empty = string.IsNullOrEmpty(arg);
				res = PlaceHolderFinder.Replace(fmt,
					delegate(Match match)
					{
						string s = match.Groups[1].Value;
						if (empty)
							if (s.StartsWith("!")) return string.Empty;
							else return s;
						else return arg;
					}
				);
			}
			return Clone(res);
		}

		internal override void ResolveGuids()
		{
			VersionValue = StringWithGuids.Replace(ToString(), new MatchEvaluator(Substitute));
		}

		internal static string Substitute(Match m)
		{
			try {
				Guid id = new Guid(((string)m.Value).Substring(1));
				return ConfigServiceContext.Get().DataProvider.FindItem(id).CalcValue().ToString();
			}
			catch {
				return "";
			}
		}

	}
}
