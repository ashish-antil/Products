using System;
using System.Runtime.Serialization;

namespace Cormit.Application.RouteApplication.Task
{
	[DataContract]
	public class AlertTask : AppTask
	{
		public AlertTask()
		{
			Arguments = new Args();
		}


		[DataMember]
		public Args Arguments { get; set; }

		[DataContract]
		public class Args
		{
			public Args()
			{
			}

			[DataMember]
			public Guid EventID { get; set; }

			[DataMember]
			public Guid EventOwnerID { get; set; }

			[DataMember]
			public string Parameters { get; set; }

			[DataMember]
			public Guid SenderID { get; set; }

			[DataMember]
			public string SenderName { get; set; }

			[DataMember]
			public int MessageID { get; set; }

			[DataMember]
			public Guid ReceiverID { get; set; }

			[DataMember]
			public Guid TemplateID { get; set; }  //& IM-5534 

			public override string ToString()
			{
				return string.Format("Args({0} {1} '{2}' {3} ({4}) {5} {6})", EventID, EventOwnerID, Parameters, SenderID, SenderName, MessageID, ReceiverID);
			}
		}
	}


}
