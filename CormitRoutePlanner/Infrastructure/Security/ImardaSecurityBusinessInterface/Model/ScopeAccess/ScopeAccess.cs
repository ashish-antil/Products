using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaSecurityBusiness
{
	[DataContract]
	public class ScopeAccess : FullBusinessEntity
	{
		#region Constructor
		public ScopeAccess()
		{
		}
		#endregion

		[DataMember]
		public Guid ScopeID { get; set; }

		[DataMember]
		public Guid AssignedToID { get; set; }

		[DataMember]
		public byte AssignedToType { get; set; }

		[DataMember]
		public byte AccessPermission { get; set; }

		[DataMember]
		public byte ConsentType { get; set; }

#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			ScopeID = DatabaseSafeCast.Cast<Guid>(dr["ScopeID"]);
			AssignedToID = DatabaseSafeCast.Cast<Guid>(dr["AssignedToID"]);
			AssignedToType = DatabaseSafeCast.Cast<byte>(dr["AssignedToType"]);
			AccessPermission = DatabaseSafeCast.Cast<byte>(dr["AccessPermission"]);
			ConsentType = DatabaseSafeCast.Cast<byte>(dr["ConsentType"]);

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
		}

	}
}