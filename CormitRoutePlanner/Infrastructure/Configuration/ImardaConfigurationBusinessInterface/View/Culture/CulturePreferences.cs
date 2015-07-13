using System;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaConfigurationBusiness
{
	partial interface IImardaConfiguration
	{
		/// <summary>
		/// Gets the culture preferences in a k|v string. This includes date time info and measurement unit preferences.
		/// </summary>
		/// <param name="req">ID=Person.ID, [0]=Company.ID(Guid)</param>
		/// <returns></returns>
		[OperationContract]
		SimpleResponse<string> GetCulturePreferences(GenericRequest req);

		/// <summary>
		/// Remove all culture related entries from the cache for the given company.
		/// </summary>
		/// <param name="req">req.ID = companyID</param>
		/// <returns></returns>
		[OperationContract]
		BusinessMessageResponse RemoveCachedCulture(IDRequest req);

		/// <summary>
		/// Set a configuration version value for a given user, A null for the value means: inherit company vale for this user(person).
		/// </summary>
		/// <param name="req">new ConfigRequest(cfgItemID, value, companyGUID, personGUID)
		/// <returns></returns>
		[OperationContract]
		BusinessMessageResponse SetUserLocale(ConfigRequest req);

		/// <summary>
		/// Set a configuration version value for a given company, A null for the value means: inherit root vale for this company
		/// </summary>
		/// <param name="req">new ConfigRequest(cfgItemID, value, companyGUID, personGUID)
		/// <returns></returns>
		[OperationContract]
		BusinessMessageResponse SetCompanyValue(ConfigRequest req);

		[OperationContract]
		BusinessMessageResponse SetCompanyCustomUnits(ConfigRequest req);

		/// <summary>
		/// Get the customized measurement units of the given company
		/// </summary>
		/// <param name="req">req.ID = companyID</param>
		/// <returns>string with customized units only, not inherited units</returns>
		[OperationContract]
		SimpleResponse<string> GetCompanyCustomUnits(IDRequest req);
	}
}