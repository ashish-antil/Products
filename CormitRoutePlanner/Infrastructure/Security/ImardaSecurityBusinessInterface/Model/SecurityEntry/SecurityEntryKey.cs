using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaSecurityBusiness
{
	/// <summary>
	/// Contains information to identify a SecurityEntry, e.g. to delete it.
	/// </summary>
	[DataContract]
	[Serializable]
	public class SecurityEntryKey : BaseEntity
	{
		[DataMember]
		public Guid EntityID { get; set; }

		[DataMember]
		public Guid SecurityObjectID { get; set; }
	}
}