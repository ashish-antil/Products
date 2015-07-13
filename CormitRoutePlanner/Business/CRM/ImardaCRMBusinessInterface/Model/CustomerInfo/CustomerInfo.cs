//& gs-104
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
	public class CustomerInfo : FullBusinessEntity 
	{			
		#region Constructor	
		public CustomerInfo()
		{
		}
		#endregion

		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public int NrActiveDevices { get; set; }
		[DataMember]
		public int NrInactiveDevices { get; set; }
		[DataMember]
		public int NrUsers { get; set; }
		[DataMember]
		public int NrLogOns { get; set; }
		[DataMember]
		public DateTime LastLogon { get; set; }




#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			Name = DatabaseSafeCast.Cast<string>(dr["Name"]);
			NrActiveDevices = DatabaseSafeCast.Cast<int>(dr["NrActiveDevices"]);
			NrInactiveDevices = DatabaseSafeCast.Cast<int>(dr["NrInactiveDevices"]);

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif

		//	Date = BusinessBase.ReadyDateForTransport(DatabaseSafeCast.Cast<DateTime>(dr["Date"]));

#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
		}
	
	}
}