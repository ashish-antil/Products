using System;
using System.Runtime.Serialization;

namespace Imarda360Application.Task
{
	[DataContract]
	public class SystemEventTask : AppTask
	{
		public SystemEventTask()
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
			
			

		}
	}
}
