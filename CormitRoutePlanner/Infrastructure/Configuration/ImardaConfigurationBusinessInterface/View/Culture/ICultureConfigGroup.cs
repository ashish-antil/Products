using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using FernBusinessBase.Errors;
using Imarda.Lib;
using FernBusinessBase;

namespace ImardaConfigurationBusiness
{
	public interface ICultureConfigGroup
	{
		Guid UnitSystem { get; }
		string Region { get; }
		string Locale { get; }
		string PreferredMeasurementUnits { get; }
	}

	public static class CultureHelper
	{
		private static string UnitSystemName(Guid id)
		{
			if (id == MetricUnitSystemID) return "metric";
			if (id == USCustomaryUnitSystemID) return "us";
			if (id == ImperialUnitSystemID) return "imp";
			return "";
		}


		public static Guid UnitSystemGuid(string name)
		{
			var guid = Guid.Empty;
			if (name.Contains("metric")) guid = CultureHelper.MetricUnitSystemID;
			else if (name.Contains("us")) guid = CultureHelper.USCustomaryUnitSystemID;
			else if (name.Contains("imp")) guid = CultureHelper.ImperialUnitSystemID;
			return guid;
		}


		public static string EnglishName(Guid id)
		{
			if (id == MetricUnitSystemID) return "Metric";
			if (id == USCustomaryUnitSystemID) return "US Customary";
			if (id == ImperialUnitSystemID) return "Imperial";
			return "(Regional)";
		}

		public static string EnglishNameExtended(Guid id)
		{
			if (string.IsNullOrEmpty(UnitSystemName(id)))
			{
				if (id == FullBusinessEntity.SystemGuid || id == Guid.Empty) return "Company";
				else return "User";
			}
			else return EnglishName(id);
		}

		public readonly static Guid UnitSystemID = new Guid("A7A52B80-BC9F-4FA5-9E31-8A7B21783736");

		public readonly static Guid PreferredMeasurementUnitsID = new Guid("F59CCF57-588F-4467-AF61-E4E1A8D3E32D");

		public readonly static Guid RegionMeasurementUnitsID = new Guid("C2F6EC0D-E1CB-4978-9C27-DEF434ED48B2");

		private const string StrMetricUnitSystemID = "3A5ED551-E429-4754-8727-7EA566DD402C";
		/// <summary>
		/// Level 1 in the config table where ID = RegionMeasurementUnitsID
		/// </summary>
		public readonly static Guid MetricUnitSystemID = new Guid(StrMetricUnitSystemID);
		/// <summary>
		/// Level 1 in the config table where ID = RegionMeasurementUnitsID
		/// </summary>
		public readonly static Guid USCustomaryUnitSystemID = new Guid("3D707257-52C5-4DE2-AE14-B808AD36F325");

		/// <summary>
		/// Level 1 in the config table where ID = RegionMeasurementUnitsID
		/// </summary>
		public readonly static Guid ImperialUnitSystemID = new Guid("1FC4B4B5-5559-4A1E-93A4-C6155F7BB0E1");

		public static string CalcPreferences(ICultureConfigGroup config, IImardaConfiguration iConfig)
		{
			Guid unitSystemID = config.UnitSystem;
			if (unitSystemID == Guid.Empty)
			{
				unitSystemID = new RegionInfo(config.Region).IsMetric
					? MetricUnitSystemID
					: USCustomaryUnitSystemID;
			}
			Guid regionID = CultureIDs.Instance.GetCountryGuid(config.Region);
			var req = new ConfigRequest(RegionMeasurementUnitsID, null, unitSystemID, regionID);
			var resp = iConfig.GetConfigValue(req);
			ErrorHandler.Check(resp);
			string regionMeasurementUnits = resp.Item.As<string>();
			string preferences = StringUtils.MergeNonArrayKeyValuePairs(
				regionMeasurementUnits,
				config.PreferredMeasurementUnits,
				"Region|" + config.Region,
				"Locale|" + config.Locale
				);
			return preferences;
		}

		public static string GetRegion(string locale)
		{
			if (string.IsNullOrEmpty(locale)) return null;
			int p = locale.IndexOf('-');
			return p == -1 ? null : locale.Substring(p + 1);
		}
	}
}
