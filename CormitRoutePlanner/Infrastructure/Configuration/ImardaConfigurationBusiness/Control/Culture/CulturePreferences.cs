using System;
using System.Collections.Generic;
using System.Data;
using FernBusinessBase;
using Imarda360.Infrastructure.ConfigurationService;
using Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes;
using FernBusinessBase.Errors;
using System.Runtime.Serialization;
using System.Globalization;
using Imarda.Lib;


namespace ImardaConfigurationBusiness
{
	partial class ImardaConfiguration
	{
		public SimpleResponse<string> GetCulturePreferences(GenericRequest req)
		{
			try
			{
				Guid personID = req.ID;
				Guid companyID = (Guid)req[0];

				RemoveCachedCulture(new IDRequest(companyID));

				var config = new CultureConfigGroup();
				Guid[] ids = ConfigGroup.GetIDs(config);
				ConfigListRequest request2;
				if (personID != Guid.Empty)
				{
					request2 = new ConfigListRequest(ids, companyID, personID);
				}
				else
				{
					request2 = new ConfigListRequest(ids, companyID);
				}
				request2.IgnoreCache = true;
				var resp2 = GetConfigValueList(request2);
				ConfigValue[] values = resp2.List.ToArray();
				ConfigGroup.SetValues(config, values);
				string preferences = CultureHelper.CalcPreferences(config, this);
				return new SimpleResponse<string>(preferences);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SimpleResponse<string>>(ex);
			}
		}


		public BusinessMessageResponse RemoveCachedCulture(IDRequest req)
		{
			try
			{
				foreach (Guid id in ConfigGroup.GetIDs(new CultureConfigGroup()))
				{
					ConfigCache.Instance.RemoveMatch(new ConfigKey(id, req.ID));
				}
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SetUserLocale(ConfigRequest req)
		{

			// 
			// L0 L1 L2 | null			| not null		
			// ---------+-----------------+-----------------
			// 1  0  0  | do nothing [a]  | create L1,L2 [d]
			// 1  1  0  | do nothing [b]  | create L2	[e]
			// 1  1  1  | delete L2  [c]  | update L2	[f]
			//

			BusinessMessageResponse resp = null;
			try
			{
				Guid[] levels = req.GetLevels();
				Guid companyID = levels[0]; // Database column LEVEL1
				Guid personID = levels[1];  // Database column LEVEL2

				Guid localeItemID = new Guid(CultureConfigGroup.LocaleItemID);
				req.ID = localeItemID;
				string locale = (string)req.AppParameter ?? string.Empty;
				req.AppParameter = null;

				// root level
				var req0 = new ConfigRequest(localeItemID, null) { IgnoreCache = true };
				var resp0 = GetConfigValue(req0);
				ErrorHandler.CheckItem(resp0);
				Guid uid0 = resp0.Item.UID;

				// company level (L1)
				var req1 = new ConfigRequest(localeItemID, null, companyID) { IgnoreCache = true };
				var resp1 = GetConfigValue(req1);
				ErrorHandler.CheckItem(resp1);
				Guid uid1 = resp1.Item.UID;

				// user/person level (L2)
				var req2 = new ConfigRequest(localeItemID, null, companyID, personID) { IgnoreCache = true };
				var resp2 = GetConfigValue(req2);
				ErrorHandler.CheckItem(resp2);
				Guid uid2 = resp2.Item.UID;

				if (uid1.Equals(uid0))
				{
					// [a] or [d]
					if (locale == string.Empty) 
					{
						// [a]
						resp = new BusinessMessageResponse();
					}
					else 
					{
						// [d]
						string locale0 = (string)resp0.Item.Value;
						resp = CreateLocaleConfig(locale0, companyID, Guid.Empty);
						ErrorHandler.Check(resp);
						resp = CreateLocaleConfig(locale, companyID, personID);
					}
				}
				else if (uid2.Equals(uid1))
				{
					// [b] or [e]
					if (locale == string.Empty)
					{
						// [b]
						resp = new BusinessMessageResponse();
					}
					else
					{
						// [e]
						resp = CreateLocaleConfig(locale, companyID, personID);
					}
				}
				else // level 2 record exists
				{
					// [c] or [f]
					if (locale == string.Empty)
					{
						// [c]
						resp = DeleteConfigurationByUID(new IDRequest(uid2));
					}
					else
					{
						// [f]
						resp2.Item.Value = locale;
						resp = UpdateConfigValue(new SaveRequest<ConfigValue>(resp2.Item));
					}
				}
				ErrorHandler.Check(resp);
				return resp;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		private BusinessMessageResponse CreateLocaleConfig(string locale, Guid companyID, Guid personID)
		{
			var cfg = new Configuration
			{
				UID = SequentialGuid.NewDbGuid(),
				ID = new Guid(CultureConfigGroup.LocaleItemID),
				ValueType = 1,
				VersionValue = locale,
				Notes = personID == Guid.Empty ? "Company Locale" : "User Locale",
				CompanyID = companyID,
				Level1 = companyID,
				Level2 = personID
			};
			return SaveConfigurationByUID(new SaveRequest<Configuration>(cfg));
		}

		public SimpleResponse<string> GetCompanyCustomUnits(IDRequest request)
		{
			try
			{
				Guid companyID = request.ID;
				RemoveCachedCulture(new IDRequest(companyID));

				string result = string.Empty;

				// first get root level
				var req0 = new ConfigRequest(CultureHelper.PreferredMeasurementUnitsID, null) { IgnoreCache = true };
				var resp0 = GetConfigValue(req0);
				ErrorHandler.CheckItem(resp0);
				Guid uid0 = resp0.Item.UID;

				// then get level1, but this may not exist
				var req1 = new ConfigRequest(CultureHelper.PreferredMeasurementUnitsID, null, companyID)  { IgnoreCache = true };
				var resp1 = GetConfigValue(req1);
				ErrorHandler.CheckItem(resp1);

				Guid uid1 = resp1.Item.UID;
				if (!uid0.Equals(uid1))
				{
					// UIDs are different, this means that level1 exists: get the value on this level
					var resp = GetConfigurationByUID(new IDRequest(uid1));
					result = resp.Item.VersionValue;
				}
				return new SimpleResponse<string>(result);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SimpleResponse<string>>(ex);
			}
		}

		public BusinessMessageResponse SetCompanyCustomUnits(ConfigRequest req)
		{
			// 
			// L0 L1 | null			| not null		
			// ------+-----------------+-----------------
			// 1  0  | do nothing  [a] | create L1 [c]
			// 1  1  | update L1   [b]*| update L1 [d]
			//
			// *assign L0 value to L1, coz combine=false

			BusinessMessageResponse resp = null;
			try
			{
				// first get root level
				var req0 = new ConfigRequest(CultureHelper.PreferredMeasurementUnitsID, null) { IgnoreCache = true };
				var resp0 = GetConfigValue(req0);
				ErrorHandler.CheckItem(resp0);
				Guid uid0 = resp0.Item.UID; // we need this to compare to level 1

				string newValue = (string)req.AppParameter;
				Guid companyID = req.GetLevels()[0]; // Database column LEVEL1
				
				// now get level 1, may not exist
				var req1 = new ConfigRequest(CultureHelper.PreferredMeasurementUnitsID, null, companyID) { IgnoreCache = true };
				GetItemResponse<ConfigValue> resp1 = GetConfigValue(req1);
				ErrorHandler.CheckItem(resp1);
				if (resp1.Item.UID.Equals(uid0))
				{
					// Case [a] or [c]
					if (string.IsNullOrEmpty(newValue))
					{
						// Case [a]
						resp = new BusinessMessageResponse();
					}
					else
					{
						// Case [c]
						// L1 record not in database, create one
						Configuration cfg = new Configuration
						{
							UID = SequentialGuid.NewDbGuid(),
							ID = CultureHelper.PreferredMeasurementUnitsID,
							Combine = true,
							ValueType = 11,
							VersionValue = newValue,
							Notes = req.Notes,
							Level1 = companyID,
						};
						resp = SaveConfigurationByUID(new SaveRequest<Configuration>(cfg));
					}
				}
				else
				{
					// Case [b] or [d]
					string val = newValue ?? string.Empty;
					resp = UpdateConfigValue(new SaveRequest<ConfigValue>(resp1.Item));
					// case [b] or [d]
					if (string.IsNullOrEmpty(newValue))
					{
						// case [b]
						val = (string)resp0.Item.Value;
					}
					else
					{
						// case [d]
						val = newValue;
					}
					resp1.Item.Value = val;
					resp = UpdateConfigValue(new SaveRequest<ConfigValue>(resp1.Item));
				}
				ErrorHandler.Check(resp);
				return resp;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SetCompanyValue(ConfigRequest req)
		{
			// 
			// L0 L1 | null			| not null		
			// ------+-----------------+-----------------
			// 1  0  | do nothing  [a] | create L1 [c]
			// 1  1  | update L1   [b]*| update L1 [d]
			//
			// *assign L0 value to L1, coz combine=false

			BusinessMessageResponse resp = null;
			try
			{
				Guid id = req.ID;

				// first get root level (L0)
				var req0 = new ConfigRequest(id, null) { IgnoreCache = true };
				var resp0 = GetConfigValue(req0);
				Guid uid0 = resp0.Item.UID; // we need this to compare to uid1

				string newValue = (string)req.AppParameter;
				Guid companyID = req.GetLevels()[0]; // Database column LEVEL1

				// now get L1 (company level)
				var req1 = new ConfigRequest(id, null, companyID) { IgnoreCache = true };
				GetItemResponse<ConfigValue> resp1 = GetConfigValue(req1);
				ErrorHandler.CheckItem(resp1);
				if (resp1.Item.UID.Equals(uid0))
				{
					// Csae [a] or [c]
					if (newValue == null)
					{
						// Case [a]
						resp = new BusinessMessageResponse();
					}
					else 
					{
						// Case [c]
						Configuration cfg = new Configuration
						{
							UID = SequentialGuid.NewDbGuid(),
							ID = id,
							Combine = false,
							ValueType = 1,
							VersionValue = newValue,
							Notes = req.Notes,
							Level1 = companyID
						};
						resp = SaveConfigurationByUID(new SaveRequest<Configuration>(cfg));
					}
				}
				else
				{
					// case [b] or [d]
					if (newValue == null)
					{
						// case [b]
						resp1.Item.Value = resp0.Item.Value;
					}
					else
					{
						// case [d]
						resp1.Item.Value = newValue;
					}
					resp = UpdateConfigValue(new SaveRequest<ConfigValue>(resp1.Item));
				}
				ErrorHandler.Check(resp);
				return resp;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}


	}


	/// <summary>
	/// Level1 = Company, Level2 = User
	/// </summary>
	[DataContract]
	public class CultureConfigGroup : ICultureConfigGroup
	{
		//TIP code snippets: cfga (simple), cfgat (template). Ask MV

		#region Some Configuration Item IDs

		private const string MetricUnitSystemID = "3A5ED551-E429-4754-8727-7EA566DD402C";

		public const string LocaleItemID = "5E93C6A7-3EE3-4A75-A6F8-B783296AFA7D";
		public const string UnitSystemItemID = "A7A52B80-BC9F-4FA5-9E31-8A7B21783736";
		public const string PreferredMeasurementUnitsID = "F59CCF57-588F-4467-AF61-E4E1A8D3E32D";

		[DataMember]
		[ConfigAssign(LocaleItemID, "en-NZ")]
		public string _Locale;

		public string Locale { get { return _Locale; } } // required by interface.

		[DataMember]
		[ConfigAssign(UnitSystemItemID, MetricUnitSystemID)]
		public string UnitSystemID;

		/// <summary>
		/// Use this ID to identify level 1 in the national culture hierarchy in the configuration table.
		/// </summary>
		public Guid UnitSystem
		{
			get { return string.IsNullOrEmpty(UnitSystemID.Trim()) ? Guid.Empty : new Guid(UnitSystemID); }
		}

		/// <summary>
		/// Get the part of the locale after the first dash, e.g. "en-NZ"=>"NZ"
		/// </summary>
		public string Region
		{
			get { return CultureHelper.GetRegion(Locale); }
		}
		


		/// <summary>
		/// Will get merged with item C2F6EC0D-E1CB-4978-9C27-DEF434ED48B2" (regional)
		/// overriding any values with the same key.
		/// </summary>
		[DataMember]
		[ConfigAssign(PreferredMeasurementUnitsID, "")] // company-user hierarchy
		public string _PreferredMeasurementUnits;

		public string PreferredMeasurementUnits { get { return _PreferredMeasurementUnits; } } // required by interface.

		#endregion Culture Settings
	}
}