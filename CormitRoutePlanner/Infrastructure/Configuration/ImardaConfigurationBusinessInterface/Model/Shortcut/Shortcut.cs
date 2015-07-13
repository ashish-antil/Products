using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaConfigurationBusiness 
{
	[DataContract]
	public class Shortcut : FullBusinessEntity 
	{			
		#region Constructor	
		public Shortcut()
		{
		}
		#endregion

		[DataMember] public int ItemType { get; set; }

		[DataMember] public Guid ItemID { get; set; }

		[DataMember] public string Name { get; set; }

		[DataMember] public Guid OwnerID { get; set; }

		[DataMember] public int Sequence { get; set; }




#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			ItemType = DatabaseSafeCast.Cast<int>(dr["ItemType"]);
			ItemID = DatabaseSafeCast.Cast<Guid>(dr["ItemID"]);
			Name = DatabaseSafeCast.Cast<string>(dr["Name"]);
			OwnerID = DatabaseSafeCast.Cast<Guid>(dr["OwnerID"]);
			Sequence = DatabaseSafeCast.Cast<int>(dr["Sequence"]);

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
		}
	
	}
}