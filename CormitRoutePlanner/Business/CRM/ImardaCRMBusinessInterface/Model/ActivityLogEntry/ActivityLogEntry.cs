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
	public class ActivityLogEntry : FullBusinessEntity
	{
		#region Constructor
		public ActivityLogEntry()
		{
		}
		#endregion

		[DataMember]
		public DateTime LogDateTime { get; set; }

		[DataMember]
		public string Operation { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public string AdditionalDescription { get; set; }

		[DataMember]
		public string Username { get; set; }


#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			LogDateTime = GetValue<DateTime>(dr, "LogDateTime");
			Operation = DatabaseSafeCast.Cast<string>(dr["Operation"]);
			Description = DatabaseSafeCast.Cast<string>(dr["Description"]);
			AdditionalDescription = DatabaseSafeCast.Cast<string>(dr["AdditionalDescription"]);
			if (HasColumn(dr, "Username"))
			{
				Username = GetValue<string>(dr, "Username");
			}

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
		}

	}
}