using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using Imarda.Lib;
using ImardaTaskBusiness;

namespace Cormit.Application.RouteApplication.Task
{
	public static class AlertTaskHelper
	{
		/// <summary>
		/// This method queues a new Alert. It will be executed by the i360 Task Manager: it finds all
		/// alertplan actions triggered by the given event and executes them.
		/// There is a similar method in the gateway, but the set of data is different. Also, the senderID
		/// in the gateway is always a device, but in this method it would normally be a person or company (CRM id)
		/// but can also be a device, vehicle etc.
		/// </summary>
		/// <param name="companyID">company that raises and handles event</param>
		/// <param name="eventID">id of a valid event (from Event table)</param>
		/// <param name="ownerID">id of entity where event applies to, e.g. a user, a vehicle, a driver, a device etc.</param>
		/// <param name="senderID">entity id that raises event; for use in MessageCenter</param>
		/// <param name="senderName">sender column in the MessageCenter</param>
		/// <param name="receiverID">In case actions get executed that need a target, e.g. Unit.ID for outbound MDT messages</param>
		/// <param name="sbTypedData">list with typed values for Alert Plan Rule evaluation and Notification template</param>
		/// <param name="tzi">time zone used to format datetime values in the notification</param>
		/// <param name="templateID">Override id of template (NotificationItem) to use with the task. Leave to Guid.Empty to use the default one</param>
		public static void SaveAlertTask(Guid companyID, Guid eventID, Guid ownerID, Guid senderID, string senderName, Guid receiverID, StringBuilder sbTypedData, TimeZoneInfo tzi, Guid templateID) //# IM-5534 
		{
			sbTypedData.AppendKV("now", DateTime.UtcNow);
			sbTypedData.AppendKV("TZ", tzi.Id); // required
			string typedData = sbTypedData.ToString();

			var alertTask = new AlertTask
			{
				ID = SequentialGuid.NewDbGuid(),
				Arguments = new AlertTask.Args
				{
					EventID = eventID,
					EventOwnerID = ownerID,
					Parameters = typedData,
					SenderID = senderID,
					SenderName = senderName,
					ReceiverID = receiverID,
					MessageID = 0,
					TemplateID = templateID, //& IM-5534 
				},
				StartTime = DateTime.UtcNow,
				TimeZoneID = tzi.Id,
				TaskOwnerID = ownerID,
				CompanyID = companyID,
				TimeAllowed = TimeSpan.FromMinutes(5),
			};

			var schtask = AppTaskHelper.Convert(alertTask);
			schtask.ManagerID = TaskManagerParameters.ManagerID;
			schtask.OwnerID = alertTask.TaskOwnerID;
			schtask.UserID = alertTask.TaskOwnerID;
			schtask.AlgorithmID = (byte)Algorithms.Alerts;
			schtask.ProgramID = (int)Programs.AlertHandler;
			schtask.Arguments = XmlUtils.Serialize(alertTask.Arguments, false);

			var service = ImardaTaskProxyManager.Instance.IImardaTaskProxy;
			ChannelInvoker.Invoke(delegate(out IClientChannel channel)
			{
				channel = service as IClientChannel;
				var response = service.SaveScheduledTask(new SaveRequest<ScheduledTask>(schtask));
				ErrorHandler.Check(response);
			});
		}

	}
}
