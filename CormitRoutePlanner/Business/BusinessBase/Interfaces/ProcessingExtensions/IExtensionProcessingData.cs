using System;
using System.Collections.Generic;

namespace FernBusinessBase.Interfaces.ProcessingExtensions
{
	/// <summary>
	/// Defines basic properties that data processed by an extension must support
	/// </summary>
	public interface IExtensionProcessingData
	{
		Guid CompanyID { get; }
		/// <summary>
		/// The company ID is the primary ID when checking if a profile exists for this data - SecondaryProfileLookupIDs are used to retrieve profile based on another ID e.g. FleetID, CountryID, etc. - The key is a valueKindId
		/// </summary>
		Dictionary<Guid,object> SecondaryProfileLookupIDs { get; }
		Guid ProfileId { get; set; }
		Guid ExtensionId { get; }
		string Identifier { get; }
	}
}
