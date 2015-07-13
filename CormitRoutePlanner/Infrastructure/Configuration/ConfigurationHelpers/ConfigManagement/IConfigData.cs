using System;
using System.Collections.Generic;
using System.Text;

namespace Imarda360.Infrastructure.ConfigurationService
{
	/// <summary>
	/// Interface for objects that implement the data access layer for the
	/// configuration system.
	/// </summary>
	public interface IConfigData
	{
		/// <summary>
		/// Find the item with the given id
		/// </summary>
		/// <param name="itemID">key for lookup</param>
		/// <returns>the item containing all matching versions</returns>
		ConfigItem FindItem(Guid itemID);

		/// <summary>
		/// Find the default item with the given id. Optimized version of FindItem with all optional fields null.
		/// </summary>
		/// <param name="itemID">uniquely identifies the default item version</param>
		/// <returns></returns>
		ConfigItemVersion FindDefaultItemVersion(Guid itemID);

		/// <summary>
		/// Add a default item. Optional update an existing one.
		/// </summary>
		/// <param name="itemID">unique identifier for the item</param>
		/// <param name="data">data to be added or replace existing item if update==true</param>
		/// <param name="update">true: update item with given values if itemID already exists</param>
		/// <returns>the value type that was stored, or Unknown if nothing stored</returns>
		int AddDefaultItem(Guid itemID, object data, bool update);
	}
}
