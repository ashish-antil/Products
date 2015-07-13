using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaCRMBusiness 
{
	[DataContract]
	public class ContactMap : FullBusinessEntity 
	{				
		public ContactMap()
		{
		}

		[DataMember] public Guid ContactId { get; set; }

		[DataMember] public Guid ContactPersonId { get; set; }

		[DataMember] public bool DefaultPerson { get; set; }



#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			ContactId = GetValue<Guid>(dr, "ContactId");
			ContactPersonId = GetValue<Guid>(dr, "ContactPersonId");
			DefaultPerson = GetValue<bool>(dr, "DefaultPerson");

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		}
	
	}
}