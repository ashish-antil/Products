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
	public class ApplicationFeatureOwner : FullBusinessEntity
	{
		public ApplicationFeatureOwner()
		{
		}

		[DataMember]
		public Guid ApplicationFeatureID { get; set; }

		[DataMember]
		public Guid OwnerID { get; set; }



#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			ApplicationFeatureID = GetValue<Guid>(dr, "ApplicationFeatureID");
			OwnerID = GetValue<Guid>(dr, "OwnerID");

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		}

	}
}