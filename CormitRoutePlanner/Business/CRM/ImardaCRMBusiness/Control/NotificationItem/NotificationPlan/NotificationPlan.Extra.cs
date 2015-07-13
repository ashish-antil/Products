using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using ImardaNotificationBusiness;
using System.ServiceModel;
using Imarda.Lib;
using System.Collections;
using Imarda360.Infrastructure.ConfigurationService;
using System.Globalization;


namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="request">ID=notifPlanID, [0]=companyID, [1]=parameters, [2]=tzid [3]=attachments [4]=(byte)priority</param>
		/// <returns></returns>
		public BusinessMessageResponse SendNotificationPlan(GenericRequest request)
		{
			try
			{
				NotificationPlan plan = null;
				Guid planID = request.ID;
				_Log.InfoFormat("SendNotificationPlan - enter, planID: {0}", planID);
				var response = GetNotificationPlan(new IDRequest(planID));
				_Log.InfoFormat("GetNotificationPlan response = {0}", response);
				//IM-5359
			    if (response.Item == null)
			    {
                    _Log.WarnFormat("Could not retrieve Notification Item. PlanID{0}", planID);
                    return new BusinessMessageResponse();
			    }//IM-5359
				plan = response.Item;

				Guid companyID = (Guid)request[0];
				string data = (string)request[1];
				string tzid = (string)request[2];
				string priority = "0";
				string attachments = null;
				if (request.Parameters.Length > 4)
				{
					attachments = (string)request[3];
					priority = ((byte)request[4]).ToString();
				}

				_Log.InfoFormat("GetNotificationPlan data = {0}", data);

				var preq = new IDRequest(planID);
				preq.CompanyID = companyID;
				var listResponse = GetNotificationItemListByNotificationPlanID(preq);
				_Log.InfoFormat("GetNotificationItemListByNotificationPlanID response = {0}", listResponse);
				ErrorHandler.Checklist<NotificationItem>(listResponse);
				List<NotificationItem> notificationItems = listResponse.List;

				string[] param = new string[14];
				param[0] = planID.ToString();//NotificationPlanID
				param[1] = plan.CompanyID.ToString();//CompanyID
				param[2] = plan.UserID.ToString();//UserID
				param[3] = "i360 - Alert Notification";

				foreach (NotificationItem item in notificationItems)
				{
					Person p = GetPerson(new IDRequest(item.CRMID)).Item;
					_Log.InfoFormat("Person = {0}", p);
					string recipientSpec = p.FullName;
					if (p != null && !p.Deleted)
					{
						switch (item.DeliveryMethod.ToUpperInvariant())
						{
							case "EMAIL":
								recipientSpec = p.FullName + "|" + p.Email + "||";
								break;
							case "SMS":
								recipientSpec = p.FullName + "|" + p.MobilePhone + "||";
								break;
							case "FAX":
								recipientSpec = p.FullName + "|" + p.Phone + "||";
								break;
						}
						//ask for Notification service
						IImardaNotification service = ImardaProxyManager.Instance.IImardaNotificationProxy;
						//send email plan
						ChannelInvoker.Invoke(delegate(out IClientChannel channel)
						{
							channel = service as IClientChannel;
							param[4] = item.Content; // body
							param[5] = item.DeliveryMethod;//Method Name, Email, SMS, Fax
							param[6] = recipientSpec;
							//param[7]
							//param[8]
							param[9] = attachments;
							param[10] = new StringBuilder(data).AppendKV("RecipientName", p.FullName).ToString();
							param[11] = p.Email;
							param[12] = tzid;
							param[13] = priority.ToString();
							//no cc and bcc for now
							GenericRequest reqSingle = new GenericRequest(SequentialGuid.NewDbGuid(), param);

							_Log.InfoFormat("Before SendSingle, planID: {0}", planID);
							service.SendSingle(reqSingle);
							_Log.Info("After SendSingle");
						});
					}
					else
					{
						_Log.InfoFormat("No notification sent for CRMID = {0}, Person not found or been deleted.", item.CRMID);
					}
				}
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				_Log.ErrorFormat("Error: {0}", ex);
				return ErrorHandler.Handle(ex);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="request">action.Link1, action.Link2, unit.CompanyID, fmtdata;</param>
		/// <returns></returns>
		public BusinessMessageResponse SendNotificationEmail(GenericRequest request)
		{
			try
			{
				_Log.InfoFormat("SendNotificationEmail - enter");

				Guid personID = request.ID;
				Guid templateID = (Guid)request[0];
				Guid companyID = (Guid)request[1];
				string data = (string)request[2];
				string tzid = (string)request[3];

				Person p = GetPerson(new IDRequest(personID)).Item;
				//IM-5359
			    if (p == null)
			    {
                    _Log.WarnFormat("Person with ID {0} not found", request.ID);
                    return new BusinessMessageResponse();
			    }//IM-5359
			    _Log.InfoFormat("Person = {0}", p);

				if (!p.Deleted)
				{
					var resp = GetNotificationItem(new IDRequest(templateID));
					ErrorHandler.Check(resp);

					NotificationItem item = resp.Item;

					//ask for Notification service
					IImardaNotification service = ImardaProxyManager.Instance.IImardaNotificationProxy;

					ChannelInvoker.Invoke(delegate(out IClientChannel channel)
					{
						channel = service as IClientChannel;
						string[] param = new string[10];

						param[0] = companyID.ToString();
						param[1] = personID.ToString();
						param[2] = item.Content;
						param[3] = string.IsNullOrEmpty(item.Subject) ? null : item.Subject;
						param[4] = "";
						param[5] = p.FullName + '|' + p.Email + "||";
						param[6] = "";
						param[7] = "";
						param[8] = new StringBuilder(data).AppendKV("RecipientName", p.FullName).ToString();
						param[9] = tzid;

						//no cc and bcc for now
						GenericRequest reqSingle = new GenericRequest(personID, param);
						_Log.InfoFormat("Before SendEmail, personID: {0}", personID);
						service.SendEmail(reqSingle);
						_Log.Info("After SendEmail");
					});
				}
				else
				{
					_Log.InfoFormat("The Person {0} has been deleted, so no alert sending", p);
				}
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SendNotificationSMS(GenericRequest request)
		{
			try
			{
				_Log.InfoFormat("SendNotificationSMS - enter");

				Guid personID = request.ID;
				Guid templateID = (Guid)request[0];
				Guid companyID = (Guid)request[1];
				string data = (string)request[2];
				string tzid = (string)request[3];

				Person p = GetPerson(new IDRequest(personID)).Item;
				_Log.InfoFormat("Person = {0}", p);

				if (!p.Deleted)
				{
					var resp = GetNotificationItem(new IDRequest(templateID));
					ErrorHandler.Check(resp);

					NotificationItem item = resp.Item;

					//ask for Notification service
					IImardaNotification service = ImardaProxyManager.Instance.IImardaNotificationProxy;

					ChannelInvoker.Invoke(delegate(out IClientChannel channel)
					{
						channel = service as IClientChannel;
						string[] param = new string[8];

						param[0] = companyID.ToString();
						param[1] = personID.ToString();
						param[2] = item.Content;
						param[3] = "i360 - Alert Notification";
						param[4] = p.FullName + '|' + p.MobilePhone + "||";
						param[5] = new StringBuilder(data).AppendKV("RecipientName", p.FullName).ToString();
						param[6] = p.Email;
						param[7] = tzid;

						//no cc and bcc for now
						GenericRequest reqSingle = new GenericRequest(personID, param);
						_Log.InfoFormat("Before SendSMS, personID: {0}", personID);
						service.SendSMS(reqSingle);
						_Log.Info("After SendSMS");
					});
				}
				else
				{
					_Log.InfoFormat("The Person {0} has been deleted, so no alert sending", p);
				}
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		//used  to charge SMS messages against this user
		private readonly Guid _SystemSmsUser = new Guid("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA");

		//send sms to mobilePhone as passed and chrges against SystemSMSUser
		public BusinessMessageResponse SendDeviceNotificationSMS(GenericRequest request)  //, string mobilePhone)
		{
			try
			{
				_Log.InfoFormat("SendDeviceNotificationSMS - enter");

				Guid personID = _SystemSmsUser;
				Guid templateID = (Guid)request[0];
				string simPhoneNumber = (string)request[1];
				string data = (string)request[2];
				Guid companyID = (Guid)request[3];
				string tzid = (string)request[4];

				Person p = GetPerson(new IDRequest(personID)).Item;
				_Log.InfoFormat("Person = {0}", p);

				if (!p.Deleted)
				{
					var resp = GetNotificationItem(new IDRequest(templateID));
					ErrorHandler.Check(resp);

					NotificationItem item = resp.Item;

					//ask for Notification service
					IImardaNotification service = ImardaProxyManager.Instance.IImardaNotificationProxy;

					ChannelInvoker.Invoke(delegate(out IClientChannel channel)
					{
						channel = service as IClientChannel;
						string[] param = new string[8];

						param[0] = companyID.ToString();
						param[1] = personID.ToString();
						param[2] = item.Content;
						param[3] = "i360 - Message for Device";
						param[4] = p.FullName + '|' + simPhoneNumber + "||";
						param[5] = "DEVSMSMSG$|" + data + "||";   //"new StringBuilder(data).AppendKV("RecipientName", p.FullName).ToString();
						param[6] = p.Email;
						param[7] = tzid;

						//no cc and bcc for now
						GenericRequest reqSingle = new GenericRequest(personID, param);
						_Log.InfoFormat("Before SendSMS, personID: {0}", personID);
						service.SendSMS(reqSingle);
						_Log.Info("After SendSMS");
					});
				}
				else
				{
					_Log.InfoFormat("The Person {0} has been deleted, so no device msg sent via SMS", p);
				}
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public SimpleResponse<string> HttpPost(GenericRequest request)
		{
			try
			{
				_Log.InfoFormat("HttpPost - enter");

				Guid companyID = request.ID;
				string url = (string)request[0];
				Guid templateID = (Guid)request[1];
				string data = (string)request[2];
				string tzid = (string)request[3];

				var resp1 = GetNotificationItem(new IDRequest(templateID));
				ErrorHandler.Check(resp1);

				NotificationItem item = resp1.Item;

				//ask for Notification service
				IImardaNotification service = ImardaProxyManager.Instance.IImardaNotificationProxy;

				SimpleResponse<string> resp2 = null;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					var param = new string[] {
						url,
						item.Content,
						tzid,
					};
					GenericRequest reqSingle = new GenericRequest(companyID, url, item.Content, data, tzid);
					_Log.InfoFormat("Before HttpPost");
					resp2 = service.HttpPost(reqSingle);
					_Log.Info("After HttpPost");
				});
				ErrorHandler.CheckItem(resp1);
				return resp2;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SimpleResponse<string>>(ex);
			}
		}


		/// <summary>
		/// Get the text of a notification, localized and filled in with parameters.
		/// </summary>
		/// <param name="request">.ID = notification item ID, [0]=companyID, [1]=typedData, [2]=tzid, [4]=force default date fmt</param>
		/// <returns></returns>
		public SimpleResponse<string> GetNotification(GenericRequest request)
		{
			try
			{
				Guid templateID = request.ID;
				Guid companyID = (Guid)request[0];
				string data = (string)request[1];
				string tzid = (string)request[2];
				bool forceDefaultDateFormat = (bool)request[3];

				var resp = GetNotificationItem(new IDRequest(templateID));
				ErrorHandler.Check(resp);

				NotificationItem item = resp.Item;
				string msg = FillInTemplate(item.CRMID, companyID, tzid, item.Content, data, forceDefaultDateFormat);

				return new SimpleResponse<string>(msg);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SimpleResponse<string>>(ex);
			}
		}


		//private Regex _rxTemplateParameter = new Regex(@"<([A-Za-z_]\w*)>", RegexOptions.Compiled);

		/// <summary>
		/// Fill in the template, replace identifiers in the template in angular brackets by $(identifier).
		/// Flatten the arrays in the data to simple identifiers, DRV#1 becomes DRV11, DRV21, DRV31
		/// </summary>
		/// <param name="template"></param>
		/// <param name="kvpairs"></param>
		/// <returns></returns>
		private string FillInTemplate(Guid personID, Guid companyID, string timeZoneId, string template, string typedData, bool forceDefaultDateFormat)
		{
			if (string.IsNullOrEmpty(template)) return string.Empty;
			//template = _rxTemplateParameter.Replace(template, @"$$($1)");  // changes, e.g. <DRV1>  to   $(DRV1)
			template = template.Replace("<br>", "\r\n").Replace("&nbsp;", " ");
			if (template.Contains("$("))
			{
				var ifp = GetFormatter(personID, companyID, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
				ifp.ForceDefaultDateFormat = forceDefaultDateFormat;
				IDictionary args = EAHelper.MakeFormattedValues(typedData, ifp);
				string msg = new ConfigTemplate(template).Instantiate(args);
				return msg;
			}
			else
			{
				return template;
			}
		}

		private ImardaFormatProvider GetFormatter(Guid personID, Guid companyID, TimeZoneInfo tzi)
		{
			SimpleResponse<string> cultResp;
			var service = ImardaProxyManager.Instance.IImardaConfigurationProxy;
			string preferences = "";
			ChannelInvoker.Invoke(delegate(out IClientChannel channel)
			{
				channel = service as IClientChannel;

				cultResp = service.GetCulturePreferences(new GenericRequest(personID, companyID));
				ErrorHandler.Check(cultResp);
				preferences = cultResp.Value ?? "";
			});

			IDictionary prefMap = preferences.KeyValueMap(ValueFormat.Mix, true);
			string locale = (string)prefMap["Locale"];
			var ci = new CultureInfo(locale); // cannot use CultureInfo.GetCultureInfo() because we have to customize Infinity and NaN
			var mfi = new MeasurementFormatInfo(ci.NumberFormat);
			mfi.SetPreferences(prefMap);
			var ifp = new ImardaFormatProvider(ci, mfi, tzi);
			return ifp;
		}
	}
}
