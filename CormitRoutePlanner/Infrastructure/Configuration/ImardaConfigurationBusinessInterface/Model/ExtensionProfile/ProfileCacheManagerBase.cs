using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Imarda.Lib.MVVM.Extensions;
using Imarda.Logging;
using ImardaConfigurationBusiness.Interfaces.Profiles;

namespace ImardaConfigurationBusiness.Model.ExtensionProfile
{
	public abstract class ProfileCacheManagerBase : LoggableBase, IProfileCacheManager
	{
		private const int CounterMaxDefault = 10000;
		private const int ProfileMaxCountDefault = 1000;
		private static int _counter;

		protected ProfileCacheManagerBase(ILogger log, IProfileCache profileCache, IProfileProvider profileProvider)
			: base(log)
		{
			ProfileProvider = profileProvider;
			ProfileCache = profileCache;
			CounterMax = CounterMaxDefault;
			ProfileMaxCount = ProfileMaxCountDefault;
		}

		/// <summary>
		/// If no profile found in DB this populates the cache with dummy inactive profile for that company so we don't check DB next time
		/// </summary>
		/// <returns></returns>
		protected abstract IProfile MakeDefaultInactiveProfile(Guid companyId, Guid extensionId);

		public void ClearProfiles()
		{
			ProfileCache.Clear();
			Log.Info("ProfileCache cleared");
		}

		public IProfileProvider ProfileProvider { get; private set; }
		public IProfileCache ProfileCache { get; private set; }

		public List<IProfile> GetProfileListForExtension(Guid companyId, Guid extensionId, bool loadDefaultRules)
		{
			List<IProfile> results = null;
			try
			{
				results = ProfileCache.GetEntries(p => p.CompanyID == companyId).ToList();

				if (results.Count > 0)
				{
					var activeResults = results.Where(p => p.Active).ToList();
					return activeResults.Any() ? activeResults: null;
				}
				else
				{
					results = null;
				}

				var addResults = new Action<IEnumerable<IProfile>>(lst =>
				{
					//Add values found to cache
					lst.ForEach(profile =>
					{
						ProfileCache.AddEntry(profile);
						_counter++;
					});

					if (_counter >= CounterMax) { ProfileCache.Purge(ProfileMaxCount); }
				});

				var profiles = ProfileProvider.GetProfileListForExtension(companyId, extensionId);
				var hasProfiles = null != profiles && profiles.Count > 0;
				if (hasProfiles)
				{
					results = profiles;
					foreach (var profile in profiles)
					{
						//if(loadDefaultRules)
						//{
							var rules = ProfileProvider.GetProfileRules(extensionId, profile.ID, loadDefaultRules);
							profile.Rules = rules;
						//}
					}
					addResults(results);
				}

				//If no profile is returned (e.g. RSL inactive for company) the cache also stores the info so that further request for that company do not go back to DB either.
				else
				{
					var defaults = new List<IProfile> { MakeDefaultInactiveProfile(companyId, extensionId) };
					Debug.Assert(false == defaults[0].Active); //the default profile should be inactive and the caller should ignore it
					addResults(defaults); //deafult added to cache but not returned
				}
			}
			catch (Exception x)
			{
				Log.Error(x);
			}
			return results;
		}

		public int CounterMax { get; set; }
		public int ProfileMaxCount { get; set; }
	}
}