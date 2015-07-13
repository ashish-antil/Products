using System;
using System.ServiceModel;
using FernBusinessBase;
using Imarda.Lib;

namespace Imarda360Application.Task
{
	[MessageContract]
	public class ReportTaskListRequest : IRequestBase
	{
		[MessageBodyMember]
		public Guid SessionID { get; set; }

		public object SID
		{
			set { SessionID = new Guid(value.ToString()); }
		}

		/// <summary>
		/// Filled in in app facade, do not provide in web tier
		/// </summary>
		[MessageBodyMember]
		public Guid CompanyID { get; set; }

		/// <summary>
		/// Filled in in app facade, do not provide in web tier
		/// </summary>
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

		/// <summary>
		/// Local use only. Not transported across services.
		/// </summary>
		public object DebugInfo { get; set; }

		public override string ToString()
		{
			return string.Format("ReportTaskListRequest({0} {1} {2:s} Filters: {3} {4} {5} {6} {7})", 
				CompanyID.ShortString(), 
				OwnerID.ToString(),
				StartTime, 
				AnyStatus, IncludeNewAndQueued, IncludeSuccessful, IncludeFailed, IncludeCancelled);
		}
	}
}