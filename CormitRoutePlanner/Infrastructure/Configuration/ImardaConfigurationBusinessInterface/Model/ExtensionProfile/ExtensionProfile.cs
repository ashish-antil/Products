using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;
using Imarda.Lib.Extensions;
using ImardaConfigurationBusiness.Interfaces.Profiles;

namespace ImardaConfigurationBusiness.Model.ExtensionProfile
{
	public class ExtensionProfileException : Exception
	{
		public ExtensionProfileException(string message)
        : base(message)
		{
		}
	}

	/// <summary>
	/// Generic implementation of profiles and profile caches for Extension e.g. RTR
	/// </summary>
	[DataContract]
	public class ExtensionProfile : FullBusinessEntity, IProfile
	{
		public void ThrowMissingParameterException(string parameter)
		{
			throw new ExtensionProfileException(string.Format("Extension {0} - Missing profile parameter: {1}", Name,parameter)); 
		}

		public ExtensionProfile()
		{
			
		}

		public ExtensionProfile(IProfile profile)
		{
			ID = profile.ID;
			CompanyID = profile.CompanyID;
			EntityID = profile.EntityID;
			ExtensionID = profile.ExtensionID;
			Name = profile.Name;
			Description = profile.Description;
			Active = profile.Active;
			Rules = profile.Rules;
			Country = profile.Country;
		}

		[DataMember]
		public Guid EntityID { get; set; }
		[DataMember]
		public Guid ExtensionID { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string Description { get; set; }

		/// <summary>
		/// Transient properties - Must be set from DB by code before initializing the profile
		/// </summary>
		public List<IProfileRule> Rules { get; set; }

		/// <summary>
		/// Transient properties - Must be set by code before initializing the profile
		/// </summary>
		public string Country { get; set; }

		public virtual void InitializeRules()
		{
		}

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			EntityID = DatabaseSafeCast.Cast<Guid>(dr["EntityID"]);
			ExtensionID = DatabaseSafeCast.Cast<Guid>(dr["ExtensionID"]);
			Name = DatabaseSafeCast.Cast<string>(dr["Name"]);
			Description = DatabaseSafeCast.Cast<string>(dr["Description"]);
		}

		public string GetRuleValue1(Guid valueKindId)
		{
			var rule = Rules.FirstOrDefault(r => r.ValueKindId == valueKindId);
			return null != rule ? rule.Value1 : null;
		}

		public IProfileRule GetRule(Guid valueKindId)
		{
			return Rules.FirstOrDefault(r => r.ValueKindId == valueKindId);
		}

		public IProfileRule[] GetRules(Guid valueKindId)
		{
			return Rules.Where(r => r.ValueKindId == valueKindId).ToArray();
		}

		public string[] GetArrayFromRules(Guid valueKindId)
		{
			return Rules.Where(r => r.ValueKindId == valueKindId).Select(r => r.Value1).ToArray();
		}

		public bool HasFleetOrDefaultConfiguration(Guid fleetId)
		{
			var fleetRules = Rules.Where(r => r.ValueKindId == ProfileConstants.FleetConfiguration);
			var rule = fleetRules.FirstOrDefault(r => r.ParentId == fleetId);
			if( null != rule)
			{
				return true;
			}
			var dc = GetRule(ProfileConstants.DefaultConfiguration);
			return (dc != null);
		}

		public List<IProfileRule> GetProfileCountries()
		{
			return Rules.Where(r => r.ValueKindId == ProfileConstants.CountryIso && r.ParentId == ID).ToList();
		}

		/// <summary>
		/// True if the profile is not country based or it is for this country
		/// </summary>
		/// <param name="countryIso31661A2"></param>
		/// <returns></returns>
		public bool HasCountry(string countryIso31661A2)
		{
			var rules = GetProfileCountries();
			var firstRule = rules.FirstOrDefault();
			var hasRuleForCountry = rules.Any(r => r.Value1.CompareOrdinal(countryIso31661A2, true));
			return null == firstRule || hasRuleForCountry; //no country rules at all or the country is found
		}
	}
}
