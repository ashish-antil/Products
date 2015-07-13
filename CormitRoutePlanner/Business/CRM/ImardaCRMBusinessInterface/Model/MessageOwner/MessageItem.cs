using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FernBusinessBase;

namespace ImardaCRMBusiness
{
	[DataContract]
	public class MessageOwner : FullBusinessEntity
	{
        public MessageOwner()
		{
		}


		[DataMember]
        public Guid MessageID { get; set; }
		[DataMember]
        public Guid CRMID { get; set; }
		[DataMember]
        public bool Viewed { get; set; }
		
		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
            MessageID = GetValue<Guid>(dr, "MessageID");
            CRMID = GetValue<Guid>(dr, "CRMID");
            Viewed = GetValue<bool>(dr, "Viewed");
		}
	}
}