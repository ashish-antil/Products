
using System;
using System.Runtime.Serialization;

namespace Cormit.Application.RouteApplication.Task
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

			[DataMember]
			public Guid UnitTraceID { get; set; }

			[DataMember]
			public double Latitude { get; set; }

			[DataMember]
			public double Longitude { get; set; }

			[DataMember]
			public string ErrorCode { get; set; }

			[DataMember]
			public string GoogleCode { get; set; }
		}
	}
}
