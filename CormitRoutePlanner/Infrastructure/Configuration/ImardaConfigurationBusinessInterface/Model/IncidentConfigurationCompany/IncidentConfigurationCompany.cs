using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaConfigurationBusiness
{
	[DataContract]
	public class IncidentConfigurationCompany : FullBusinessEntity
	{
		#region Constructor
		public IncidentConfigurationCompany()
		{
		}
		#endregion

		[DataMember]
		public Guid IncidentConfigurationID { get; set; }

		[DataMember]
		public string CompanyName { get; set; }

#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			IncidentConfigurationID = DatabaseSafeCast.Cast<Guid>(dr["IncidentConfigurationID"]);
			if (HasColumn(dr, "CompanyName"))
				CompanyName = DatabaseSafeCast.Cast<string>(dr["CompanyName"]);

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif

#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
		}

	}
}