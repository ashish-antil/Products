using System.Collections.Generic;
using System.ServiceModel;
using FernBusinessBase;

// ReSharper disable once CheckNamespace
namespace ImardaConfigurationBusiness 
{
	partial interface IImardaConfiguration
	{
		/// <summary>
		/// Get a single configuration record by UID
		/// </summary>
		/// <param name="request">request.ID = UID</param>
		/// <returns></returns>
		[OperationContract]
		GetItemResponse<Configuration> GetConfigurationByUID(IDRequest request);

		/// <summary>
		/// Remove all items from the cache with the given IDs and levels. If for a certain ID only
		/// level1 is given, then all items with ID and level1 will be removed, if no level is given,
		/// then all items with the matching ID will be removed. If no ID is given, remove all cache
		/// entries that match the level GUIDs.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[OperationContract]
		BusinessMessageResponse RemoveFromCache(ConfigListRequest request);

		/// <summary>
		/// Save the configuration with the given UID.
		/// </summary>
		/// <param name="request">request.ID should contain the UID, not the ID</param>
		/// <returns></returns>
		[OperationContract]
		BusinessMessageResponse SaveConfigurationByUID(SaveRequest<Configuration> request);

		/// <summary>
		/// Delete all records of a single configuration item identified by the ID, not the UID
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[OperationContract]
		BusinessMessageResponse DeleteConfiguration(IDRequest request);

		/// <summary>
		/// Delete a single record identified by ID.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[OperationContract]
		BusinessMessageResponse DeleteConfigurationByUID(IDRequest request);

		[OperationContract]
		GetItemResponse<ConfigValue> GetConfigValue(ConfigRequest request);

		[OperationContract]
		GetListResponse<ConfigValue> GetConfigValueList(ConfigListRequest request);

        [OperationContract]
        SimpleResponse<List<ConfigValueAndDescr>> GetConfigValueDescrByIDs(ConfigListRequest request);

        [OperationContract]
		BusinessMessageResponse UpdateConfigValue(SaveRequest<ConfigValue> request);
    }
}
