using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FernBusinessBase
{
	/// <summary>
	/// Business entity classes with this interface have a link to other data, usually customer specific data.
	/// The DataPath contains a string that describes how and where to find the data. The Attributes property
	/// implements the retrieval and caching of the data as EntityAttributes, not necessariliy from an EntityAttribute table.
	/// </summary>
	public interface IHasLinkedData
	{
		/// <summary>
		/// Defines the data in conjunction with 'this' instance. 
		/// e.g.: "sql://Woolworths/SPGetLoadInfo({ID})",  "sql://Acme/SPGetEmployees({Name},50)"
		/// </summary>
		Iri DataPath { get; set; }
		EntityAttributes Attributes { get; }
	}
}
