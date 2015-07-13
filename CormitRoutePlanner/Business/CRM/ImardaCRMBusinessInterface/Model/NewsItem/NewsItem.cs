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
	public class NewsItem : FullBusinessEntity 
	{			
		#region Constructor	
		public NewsItem()
		{
		}
		#endregion

		[DataMember] public DateTime Date { get; set; }

		[DataMember] public string Subject { get; set; }

		[DataMember] public string Body { get; set; }

		[DataMember] public int Type { get; set; }




#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			Subject = DatabaseSafeCast.Cast<string>(dr["Subject"]);
			Body = DatabaseSafeCast.Cast<string>(dr["Body"]);
			Type = DatabaseSafeCast.Cast<int>(dr["Type"]);

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif

			Date = BusinessBase.ReadyDateForTransport(DatabaseSafeCast.Cast<DateTime>(dr["Date"]));

#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
		}
	
	}
}