using System;
using System.Runtime.Serialization;

namespace Imarda360Application.Task
{
	[DataContract]
	public class AddressTask : AppTask
	{
		public AddressTask()
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

			//[DataMember]
			//public Type Name { get; set; }
			//
			//...more properties...

		}
	}
}
