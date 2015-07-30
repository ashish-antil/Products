using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImardaTaskBusiness;
using Imarda.Lib;



namespace Cormit.Application.RouteApplication.Task
{
	public static class AppTaskHelper
	{
		public static ScheduledTask Convert(AppTask appTask)
		{
			if (appTask == null) return null;
			TimeZoneInfo tzi = appTask.GetTimeZoneInfo();
			var scheduledTask = new ScheduledTask
			                    	{
			                    		ID = appTask.ID,
			                    		CompanyID = appTask.CompanyID,
			                    		Status = (byte) appTask.Status,
			                    		Active = appTask.Active,
			                    		Deleted = appTask.Deleted,
			                    		StartTime = TimeUtils.ToUtc(appTask.StartTime, tzi),
			                    		DueTime = TimeUtils.ToUtc(appTask.DueTime, tzi),
			                    		Recurrence = appTask.Recurrence,
			                    		TimeZoneID = appTask.TimeZoneID,
			                    		TimeOfDay = appTask.TimeOfDay,
			                    		QueueID = appTask.QueueID,
															TaskID = appTask.TaskID
			                    	};
			return scheduledTask;
		}

		public static List<ScheduledTask> Convert(List<AppTask> appTaskList)
		{
			return appTaskList.ConvertAll(t => Convert(t));
		}


		public static T Convert<T>(ScheduledTask scheduledTask) 
			where T: AppTask, new()
		{
			if (scheduledTask != null)
			{
				TimeZoneInfo tzi = scheduledTask.GetTimeZoneInfo();
				var appTask = new T
				              	{
				              		ID = scheduledTask.ID,
				              		CompanyID = scheduledTask.CompanyID,
				              		Active = scheduledTask.Active,
				              		Deleted = scheduledTask.Deleted,
				              		Status = (AppTaskStatus) scheduledTask.Status,
				              		StartTime = TimeUtils.FromUtc(scheduledTask.StartTime, tzi),
				              		DueTime = TimeUtils.FromUtc(scheduledTask.DueTime, tzi),
				              		Recurrence = scheduledTask.Recurrence,
				              		TaskOwnerID = scheduledTask.OwnerID,
				              		TimeZoneID = scheduledTask.TimeZoneID,
				              		TimeOfDay = scheduledTask.TimeOfDay,
				              		QueueID = scheduledTask.QueueID,
													TaskID = scheduledTask.TaskID
				              	};
				return appTask;
			}
			return null;
		}

		public static List<T> Convert<T>(List<ScheduledTask> list) 
			where T : AppTask, new()
		{
			return list.ConvertAll(t => Convert<T>(t));
		}

	}
}
