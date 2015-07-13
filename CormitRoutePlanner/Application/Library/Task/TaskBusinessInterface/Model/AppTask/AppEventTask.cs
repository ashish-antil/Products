using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imarda360Base;
using FernBusinessBase;
using System.Runtime.Serialization;
using Imarda.Lib;


namespace Imarda360Application.Task
{
	[DataContract]
	[KnownType(typeof(ReportTask))]
	[KnownType(typeof(AlertTask))]
	[KnownType(typeof(EventLogTask))]
	public class AppEventTask : SolutionEntity
	{

		public AppEventTask()
		{
			Status = AppTaskStatus.New;
			Active = true;
			//NOTE do not add a constructor that takes a ScheduledTask, 
			//because that would create an unwanted dependency on an infrastructure interface.
		}

		[IgnoreDataMember]
		public Guid CompanyID { get; set; }
		
		[DataMember]
		public AppTaskStatus Status { get; set; }

		[DataMember]
		[DisplayFormat("dd'/'MM'/'yyyy HH:mm:ss")]
		public DateTime StartTime { get; set; }

		[DataMember]
		public DateTime DueTime { get; set; }

		/// <summary>
		/// E.g. Windows name, e.g. "New Zealand Standard Time"
		/// </summary>
		[DataMember]
		public string TimeZoneID { get; set; }
		
		[DataMember]
		public TimeSpan TimeOfDay { get; set; }

		[DataMember]
		public string Recurrence { get; set; }

		[DataMember]
		public Guid TaskOwnerID { get; set; }


		public TimeSpan TimeAllowed
		{
			get { return DueTime - StartTime; }
			set { DueTime = StartTime.Add(value); }
		}

		public void CalculateStart(TimeSpan timeRequired)
		{
			StartTime = DueTime.Subtract(timeRequired);
		}

		public void SetPattern(DateGenerator gen)
		{
			Recurrence = DateGeneratorFactory.GetString(gen);
		}

		public TimeZoneInfo GetTimeZoneInfo()
		{
			return TimeZoneInfo.FindSystemTimeZoneById(TimeZoneID);
		}

		public override string ToString()
		{
			return string.Format("AppTask({0}, {1}~{2}, {3})", ID, StartTime, DueTime, Recurrence);
		}
	}
}
