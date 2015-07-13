using System;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;
using ImardaConfigurationBusiness.Interfaces.Profiles;

namespace ImardaConfigurationBusiness.Model.ExtensionProfile
{
	/// <summary>
	/// Generic implementation of profiles and profile caches for Extension e.g. RTR
	/// </summary>
	[DataContract]
	public class ExtensionProfileRule : FullBusinessEntity, IProfileRule
	{
		[DataMember]
		public Guid ProfileId { get; set; }
		[DataMember]
		public Guid ParentId { get; set; }
		[DataMember]
		public Guid ValueKindId { get; set; }
		[DataMember]
		public string ValueKindName { get; set; }
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public string Value1 { get; set; }
		[DataMember]
		public string Value2 { get; set; }

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			ProfileId = DatabaseSafeCast.Cast<Guid>(dr["EntityID"]);
			ParentId = DatabaseSafeCast.Cast<Guid>(dr["ParentId"]);
			ValueKindId = DatabaseSafeCast.Cast<Guid>(dr["ValueKindId"]);
			ValueKindName = DatabaseSafeCast.Cast<string>(dr["Name"]);
			Description = DatabaseSafeCast.Cast<string>(dr["Description"]);
			Value1 = DatabaseSafeCast.Cast<string>(dr["Value1"]);
			Value2 = DatabaseSafeCast.Cast<string>(dr["Value2"]);
		}


	}
}
