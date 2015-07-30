using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Cormit.Application.RouteApplication.Task;
using Imarda.Lib;

namespace Cormit.Application.RouteApplication.Task
{
	[DataContract]
	public class ReportTask : AppTask
	{
		public ReportTask()
		{
			Arguments = new Args();
		}

		public ReportDef Definition { get { return Arguments.Definition; } }

		public NotificationDef Notification { get { return Arguments.Notification; } }


		[DataMember]
		public Args Arguments { get; set; }

		[DataContract]
		public class Args
		{
			public Args()
			{
				Definition = new ReportDef();
				Notification = new NotificationDef();
			}

			[DataMember]
			public ReportDef Definition { get; set; }

			[DataMember]
			public NotificationDef Notification { get; set; }
		}

		public override string ToString()
		{
			return string.Format("{0}+ReportTask({1}+{2})", base.ToString(), Definition, Notification);
		}
	}


}
