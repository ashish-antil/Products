using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using ImardaCRMBusiness;
using System.ServiceModel;
using Imarda360Base;
using Imarda.Lib;

namespace Imarda360Application.CRM
{
	partial class ImardaCRM
	{
		#region Install NotificationItems To NotificationPlan
		public SolutionMessageResponse InstallNotificationItemsToNotificationPlan(SaveListRequest<NotificationItem> entities)
		{
			Guid notificationPlanID;
			if (entities.Get<Guid>("NotificationPlanID", out notificationPlanID))
			{
				Guid companyID = entities.CompanyID;
				Guid userID = entities.Get<Guid>("UserID", Guid.Empty);

				SolutionMessageResponse response = new SolutionMessageResponse();

				ImardaCRMBusiness.IImardaCRM service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					//Delete NotificationItems first associated with the NotificationPlanID
					IDRequest req = new IDRequest(notificationPlanID) { CompanyID = companyID };
					
					service.DeleteNotificationPlanItemsByPlanID(req);
					var links = new List<NotificationPlanItem>();
					var piRequest = new SaveListRequest<NotificationPlanItem>();
					foreach (NotificationItem entity in entities.List)
					{
						links.Add(new NotificationPlanItem
						{
							ID = SequentialGuid.NewDbGuid(),
							CRMID = entity.CRMID,
							CompanyID = companyID,
							UserID = userID,
							DeliveryMethod = entity.DeliveryMethod,
							NotificationItemID = entity.ID,
							NotificationPlanID = notificationPlanID,
						});
					}
					piRequest.List = links;
					BusinessMessageResponse busResponse = service.SaveNotificationPlanItemList(piRequest);
					ServiceMessageHelper.Copy(busResponse, response);
				});
				return response;
			}
			else return new SolutionMessageResponse { Status = false, StatusMessage = "No PlanID" };
		}
		#endregion


		public SolutionMessageResponse SendNewPassword(GenericRequest request)
		{
			SolutionMessageResponse response = new SolutionMessageResponse();
			ImardaCRMBusiness.IImardaCRM service = ImardaProxyManager.Instance.IImardaCRMProxy;
			ChannelInvoker.Invoke(delegate(out IClientChannel channel)
			{
				channel = service as IClientChannel;
				service.SendNotificationEmail(request);
			});
			response.Status = true;
			return response;
		}

		/// <summary>
		/// Send an email or sms
		/// </summary>
		/// <param name="request">.ID=Notification.ID, ["PersonID"] = Person.ID, ["Data"]=typedDataKV, ["TZ"]=Windows time zone name, ["Type"]="email" or "sms"</param>
		/// <returns></returns>
		public SolutionMessageResponse SendNotification(IDRequest request)
		{
			try
			{
				SolutionMessageResponse appResponse = null;
				var service = ImardaProxyManager.Instance.IImardaCRMProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					Guid notificationItemID = request.ID;
					var data = request.GetString("Data");
					var tzid = request.GetString("TZ");
					var personID = request.Get("PersonID", Guid.Empty);
					var req = new GenericRequest(personID, notificationItemID, request.CompanyID, data, tzid);
					string type = request.GetString("Type");
					BusinessMessageResponse response = null;
					if (type == "email")
					{
						response = service.SendNotificationEmail(req);
					}
					else if (type == "sms")
					{
						response = service.SendNotificationSMS(req);
					}
					else throw new ArgumentException("'Type' missing: 'sms' or 'email'");

					ErrorHandler.Check(response);
					//TODO more stuff here...?
					appResponse = new SolutionMessageResponse();
				});
				return appResponse;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SolutionMessageResponse>(ex);
			}
		}
}
}