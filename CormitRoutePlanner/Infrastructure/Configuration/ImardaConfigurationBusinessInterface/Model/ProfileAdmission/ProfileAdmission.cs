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
	public class ProfileAdmission : FullBusinessEntity 
	{			
		#region Constructor	
		public ProfileAdmission()
		{
		}
		#endregion

		[DataMember] public Guid ProfileID { get; set; }

		[DataMember] public bool AllowCustomersToUse { get; set; }




#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			ProfileID = DatabaseSafeCast.Cast<Guid>(dr["ProfileID"]);
			AllowCustomersToUse = DatabaseSafeCast.Cast<bool>(dr["AllowCustomersToUse"]);

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
		}
	
	}
}