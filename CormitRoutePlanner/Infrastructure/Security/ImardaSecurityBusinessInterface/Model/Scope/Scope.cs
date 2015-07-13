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
	public class Scope : FullBusinessEntity
	{
		#region Constructor
		public Scope()
		{
		}
		#endregion

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string DisplayName { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public byte ScopeType { get; set; }  //API, OpenID, ....

		//from ClientScope
		[DataMember]
		public byte ConsentType { get; set; }

#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			Name = DatabaseSafeCast.Cast<string>(dr["Name"]);
			DisplayName = DatabaseSafeCast.Cast<string>(dr["DisplayName"]);
			Description = DatabaseSafeCast.Cast<string>(dr["Description"]);
			if (HasColumn(dr, "ScopeType"))
				ConsentType = DatabaseSafeCast.Cast<byte>(dr["ScopeType"]);
			if (HasColumn(dr, "ConsentType"))
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