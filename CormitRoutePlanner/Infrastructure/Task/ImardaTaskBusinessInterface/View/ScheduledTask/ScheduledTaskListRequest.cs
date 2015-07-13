using System;
using System.ServiceModel;

namespace ImardaTaskBusiness
{
	[MessageContract]
	public class ScheduledTaskListRequest
	{
		[MessageBodyMember]
		public Guid CompanyID { get; set; }

		[MessageBodyMember]
		public Guid OwnerID { get; set; }

		[MessageBodyMember]
		public bool IncludeInactive { get; set; }

		/// <summary>
		/// Use large number for all reports.
		/// </summary>
		[MessageBodyMember]
		public int TopN { get; set; }

		/// <summary>
		/// Use TimeUtils.MinValue if not a criterion
		/// </summary>
		[MessageBodyMember]
		public DateTime StartTime { get; set; }

		/// <summary>
		/// 1 = application task manager (e.g. reports)
		/// </summary>
		[MessageBodyMember]
		public byte ManagerID { get; set; }

		/// <summary>
		/// 0 = any, 1 = report handler
		/// </summary>
		[MessageBodyMember]
		public int ProgramID { get; set; }

		/// <summary>
		/// Set to false when any of (IncludeNewAndQueued, IncludeSuccessful, IncludeFailed, IncludeCancelled) is true
		/// </summary>
		[MessageBodyMember]
		public bool AnyStatus { get; set; }

		[MessageBodyMember]
		public bool IncludeNewAndQueued { get; set; }

		[MessageBodyMember]
		public bool IncludeSuccessful { get; set; }

		[MessageBodyMember]
		public bool IncludeFailed { get; set; }

		[MessageBodyMember]
		public bool IncludeCancelled { get; set; }
	}
}
