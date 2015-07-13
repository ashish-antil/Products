using System;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaConfigurationBusiness
{
	partial interface IImardaConfiguration
	{
		/// <summary>
		/// Save an item version into the hierarchy.
		/// </summary>
		/// <param name="req">ID=itemID, AppParameter=value, Level*=ID@level, ValueType and Combine also required in</param>
		/// <returns>number of records added (+) or deleted (-)</returns>
		[OperationContract]
		SimpleResponse<int> SaveHierarchy(ConfigRequest req);

		/// <summary>
		/// Get all Configuration objects in the requested path.
		/// </summary>
		/// <param name="req">ID=itemID, Level[n]=ID@level</param>
		/// <returns>Configuration path, first L0, then L1, then L2...</returns>
		[OperationContract]
		GetListResponse<Configuration> GetHierarchy(ConfigRequest req);

		/// <summary>
		/// Get Child Configuration objects at given level in the tree.
		/// </summary>
		/// <param name="req">ID=itemID, Level[n]=ID@level, identifying parent node</param>
		/// <returns>unordered list of child Configuration objects, can be long</returns>
		[OperationContract]
		GetListResponse<Configuration> GetChildren(ConfigRequest req);

		/// <summary>
		/// Get number of child Configuration objets in the tree at given parent.
		/// </summary>
		/// <param name="req">ID=itemID, Level[n]=ID@level, identifying parent node</param>
		/// <returns></returns>
		[OperationContract]
		SimpleResponse<int> GetChildrenCount(ConfigRequest req);
	}
}