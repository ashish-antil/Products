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
	public class MessageItem : FullBusinessEntity
	{
		public MessageItem()
		{
		}


		[DataMember]
		public string MessageType { get; set; }
		[DataMember]
		public string MessageTitle { get; set; }
		[DataMember]
		public string MessageContent { get; set; }
		[DataMember]
		public DateTime MessageDate { get; set; }
		[DataMember]
		public bool Viewed { get; set; }

        [DataMember]
        public string Users { get; set; }

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			MessageType = GetValue<string>(dr, "MessageType");
			MessageTitle = GetValue<string>(dr, "MessageTitle");
			MessageContent = GetValue<string>(dr, "MessageContent");
			MessageDate = GetDateTime(dr, "DateCreated");
			Viewed = GetValue<bool>(dr, "Viewed");
		}
	}
}