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
	public class ApplicationFeatureCategory : FullBusinessEntity 
	{				
		public ApplicationFeatureCategory()
		{
		}

		[DataMember] public Guid ApplicationID { get; set; }

		public const int NameMaxLen = 50;
		[ValidLength(-1, NameMaxLen)]
		[DataMember] public string Name { get; set; }

		public const int DescriptionMaxLen = 200;
		[ValidLength(-1, DescriptionMaxLen)]
		[DataMember] public string Description { get; set; }



#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			ApplicationID = GetValue<Guid>(dr, "ApplicationID");
			Name = GetValue<string>(dr, "Name");
			Description = GetValue<string>(dr, "Description");

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		}
	
	}
}