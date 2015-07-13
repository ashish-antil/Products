using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using System.Threading;
using System.Collections;
using FernBusinessBase.Errors;
using System.Net.Mail;
using Imarda.Lib;
using Imarda.Logging;
using Imarda360.Infrastructure.ConfigurationService;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net;
using Renci.SshNet;


namespace ImardaNotificationBusiness
{
	partial class ImardaNotification
	{
		private static ErrorLogger _Log = ErrorLogger.GetLogger("Notification");

		private int[] _Retries;
		private SmtpClient _SmtpClient;
		private readonly string _EmailSenderAddress = "i360@imardainc.com";
		private readonly string _EmailSenderName = "Imarda360";
		private string _SMSSenderAddress = "";
		private string _SMSReceiverSuffix = "";
		private readonly bool _SuppressNotification;
		//private FaxServer _FaxServer = new FaxServerClass(); 

		private Thread _Sender;		//# IM-3927
		private Thread _Resender;//check failed items and resend them	//# IM-3927

		private readonly string _AttachmentFolder;

		public ImardaNotification()
		{
			_Retries = ConfigUtils.GetArray<int>("RetryIntervalsMinutes", 1) ?? new int[] { 3, 10, 20, 60, 2 * 60, 6 * 60, 24 * 60 };
            _SuppressNotification = ConfigUtils.GetFlag("SuppressNotification");

            _AttachmentFolder = ConfigUtils.GetString("AttachmentFolder") ?? @"C:\";
			//_EventLog = LogManager.GetLogger("EventLogger");
			InitialiseSmtpSettings();
            string emailSenderAddress = ConfigUtils.GetString("EmailSenderAddress");
			if (!string.IsNullOrEmpty(emailSenderAddress))
			{
				_EmailSenderAddress = emailSenderAddress;
			}
            string emailSender = ConfigUtils.GetString("EmailSenderName");
			if (!string.IsNullOrEmpty(emailSender))
			{
				_EmailSenderName = emailSender;
			}

			InitialiseSMSSettings();
			Start();
		}


		private void InitialiseSmtpSettings()
		{
			_SmtpClient = new SmtpClient();
			try
			{
                _SmtpClient.Host = ConfigUtils.GetString("SmtpServer");

                _SmtpClient.Port = ConfigUtils.GetInt("SmtpPort");
                string smtpServerCred = ConfigUtils.GetString("SmtpServerCredentials");
				if (!string.IsNullOrEmpty(smtpServerCred))
				{
                    var credentials = ConfigUtils.GetString("SmtpServerCredentials").KeyValueMap(ValueFormat.Strings, true);
					string user = (string)credentials["user"];
					string pass = AuthenticationHelper.Decrypt((string)credentials["password"], true, user);
					//string domain = (string)credentials["domain"];
					_SmtpClient.Credentials = new System.Net.NetworkCredential(user, pass);//, domain);
				}
				//_SmtpClient.Timeout = 100;// timeout for sending email

			}
			catch (Exception e)
			{
				_Log.Info("SMTP Server Setting Error - " + (e.InnerException != null ? e.InnerException.Message : e.Message));
			}
		}

		private void InitialiseSMSSettings()
		{
            string smsSender = ConfigUtils.GetString("SMSSenderEmail");
			if (!string.IsNullOrEmpty(smsSender))
			{
				_SMSSenderAddress = smsSender;
			}
            string smsReceiver = ConfigUtils.GetString("SMSReceiverEmailSuffix");
			if (!string.IsNullOrEmpty(smsReceiver))
			{
				_SMSReceiverSuffix = smsReceiver;
			}

		}

		public void Start()
		{
			//# IM-3927
			_Sender = new Thread(SendPendings) { Name = "SendPendingEmailSmsFtp" };
			_Sender.Start();

			_Resender = new Thread(CheckFailed) { Name = "ResendPendingEmailSmsFtp" };
			_Resender.Start();
			//. IM-3927
		}

		//private void TestSendEmailAndSMS()
		//{

		//  //Send test email
		//  string[] emailPara = new string[8];
		//  emailPara[0] = "78C46D66-B886-44D0-A3C2-3AA9B12C4D98";
		//  emailPara[1] = "08C46D66-B886-44D0-A3C2-3AA9B12C4D98";
		//  emailPara[2] = "Test Email Sending for Imarda360 Notification.";
		//  emailPara[3] = "Hello From I360";
		//  emailPara[4] = "c:\\2.1.KML";
		//  emailPara[5] = "Qian Chen|q.chen.nz@hotmail.com||Qian Chen|qian.chen@imardainc.com";
		//  emailPara[6] = "jfqin@hotmail.com";
		//  emailPara[7] = "qian.chen.nz@gmail.com";
		//  GenericRequest emailReq = new GenericRequest(Guid.NewGuid(), emailPara);
		//  SendEmail(emailReq);

		//  //Send test SMS
		//  string[] smsPara = new string[8];
		//  smsPara[0] = "78C46D66-B886-44D0-A3C2-3AA9B12C4D98";
		//  smsPara[1] = "08C46D66-B886-44D0-A3C2-3AA9B12C4D98";
		//  smsPara[2] = "Test SMS Sending for Imarda360 Notification.";
		//  smsPara[3] = "Hello From I360";
		//  smsPara[4] = "Qian Chen|642102733758||Lily Chen|64210676101";
		//  GenericRequest smsReq = new GenericRequest(Guid.NewGuid(), smsPara);
		//  SendSMS(smsReq);

		//}



		private void SendPendings()
		{

			while (true)
			{
				try
				{
					SendPendingEmails();
					SendPendingSMS();
					SendPendingFtps();		//& IM-3927
					//SendPendingFax();

					Thread.Sleep(3000);

				}
				catch (Exception e)
				{
					ErrorHandler.HandleInternal(e);
					_Log.Info("NOTIFYError - " + (e.InnerException != null ? e.InnerException.Message : e.Message));
					Thread.Sleep(100000);
				}
			}
		}


		private void SendPendingEmails()
		{
			try
			{
				var response = GetEmailPendingList(new IDRequest(Guid.Empty));
				foreach (EmailPending ep in response.List)
				{
					if (ep.TimeToSend < DateTime.UtcNow)
					{
						SendSingleEmail(ep);
					}
				}
			}
			catch (Exception e)
			{
				ErrorHandler.HandleInternal(e);
				_Log.Info("NOTIFYError - SendPendingEmails - " + (e.InnerException != null ? e.InnerException.Message : e.Message));
			}
		}

		//& IM-3927
		private void SendPendingFtps()
		{
			try
			{
				var response = GetFtpPendingList(new IDRequest(Guid.Empty));
				foreach (FtpPending fp in response.List)
				{
					if (fp.TimeToSend < DateTime.UtcNow)
					{
						SendSingleFtp(fp);
					}
				}
			}
			catch (Exception e)
			{
				ErrorHandler.HandleInternal(e);
				_Log.Info("NOTIFYError - SendPendingFtps - " + (e.InnerException != null ? e.InnerException.Message : e.Message));
			}
		}
		//. IM-3927

		private bool IsHtml(string message)
		{
			string lower = message.ToLower();
			return
				(lower.Contains("<p")) ||
				(lower.Contains("<b")) ||
				(lower.Contains("<i")) ||
				(lower.Contains("<font")) ||
				(lower.Contains("<html>")); // fixed, was <HTML> which can never occur in a lowercase string
		}

		private string StripOffHtmlTags(string message)
		{
			return Regex.Replace(message, @"<(.|\n)*?>", string.Empty);
		}

		private void SendSingleEmail(EmailPending ep)
		{
			bool succeeded = false;
			if (ep == null) return;
			try
			{
				//generate mail
				var mail = new MailMessage(new MailAddress(ep.FromAddress, _EmailSenderName), new MailAddress(ep.ToAddress, ep.RecipientName));
				mail.Subject = StripOffHtmlTags(ep.Subject.Replace('\r', ' ').Replace('\n', ' '));
				_Log.Info("Subject in real mail: " + mail.Subject);
				if (ep.Retry > 1) _Log.Debug("Retry mail attachmentFiles: " + ep.AttachmentFiles);
				mail.Body = ep.Message;
				if (IsHtml(mail.Body))
					mail.IsBodyHtml = true;

				mail.Priority = ep.Priority == 0 ? MailPriority.Low : ep.Priority == 2 ? MailPriority.High : MailPriority.Normal;

				//Add attachment if any
				if (ep.AttachmentFileList != null && ep.AttachmentFileList.Count > 0)
				{
					foreach (string s in ep.AttachmentFileList)
					{
						var attData = new Attachment(s);
						mail.Attachments.Add(attData);
					}
				}

				//Add CC if any, assume CC contains email address only
				if (ep.CCList != null && ep.CCList.Count > 0)
				{
					foreach (string s in ep.CCList)
					{
						mail.CC.Add(new MailAddress(s));
					}
				}
				//Add Bcc if any, assume Bcc contains email address only
				if (ep.BccList != null && ep.BccList.Count > 0)
				{
					foreach (string s in ep.BccList)
					{
						mail.Bcc.Add(new MailAddress(s));
					}
				}

				//send the mail
				if (_SmtpClient != null && !_SuppressNotification)
				{
					_Log.InfoFormat("Sending email {0}", mail.Body);
					_SmtpClient.Send(mail);
				}
				succeeded = true;
				//mail.Dispose();
			}
			catch (Exception ex)
			{
				var msg = string.Format("Email ID={0} subject=[{1}] to=[{2}]: {3}", ep.ID, ep.Subject.Truncate(30), ep.ToAddress, ex.Message);
                AttentionUtils.Attention(new Guid("480EBC76-9EAF-BADD-823A-E8BF83D2A518"), msg);
				_Log.Warn(msg);
				succeeded = false;
			}
			//if send successfully, save to EmailSent and delete from EmailPending
			if (succeeded)
			{
				DeleteEmailPending(new IDRequest(ep.ID));
				var req = new SaveRequest<EmailSent>();
				var es = new EmailSent(ep);
				es.LastRetryAt = ep.DateModified;
				es.Retry++;
				es.TimeToSend = DateTime.MaxValue;//never to send again
				req.Item = es;
				SaveEmailSent(req);

			}
			//if failed, save to EmailFailed and delete from EmailPending
			else
			{
				DeleteEmailPending(new IDRequest(ep.ID));
				var request = new SaveRequest<EmailFailed>();
				var es = new EmailFailed(ep);
				if (es.ToAddress.HasValidEmailAddress() && es.Retry < _Retries.Length)
				{
					es.TimeToSend = DateTime.UtcNow.AddMinutes(_Retries[es.Retry]);
					es.Retry++;
				}
				else
				{
					es.TimeToSend = DateTime.MaxValue; // don't send again
					es.Deleted = true;
				}
				request.Item = es;
				SaveEmailFailed(request);
			}
		}

		//& IM-3927
		private void SendSingleFtp(FtpPending fp)
		{
			bool succeeded = false;
			if (fp == null) return;
            string destinationFilename = "";
			try
			{
				if (fp.Retry > 1) _Log.Debug("Retry ftp attachmentFiles: " + fp.AttachmentFiles);

				string destinationPath = "";
				
				if (fp.DestinationPath != null)
				{
					// Ensure destination path is in Windows format i.e. "\" between folders
					string validWindowsPath = fp.DestinationPath;
					validWindowsPath = validWindowsPath.Replace("/","\\");

					destinationPath = Path.GetDirectoryName(validWindowsPath);
					if (destinationPath != "" && !destinationPath.StartsWith("C:") && !destinationPath.StartsWith("\\"))		//& IM-4227
						destinationPath = "\\" + destinationPath;																//& IM-4227

					destinationFilename = Path.GetFileName(validWindowsPath);
				}

				if (destinationFilename.Contains("YYMMDD_HHMMSS"))
				{
					string tmp = destinationFilename.Replace("YYMMDD_HHMMSS", DateTime.UtcNow.ToString("yyyyMMdd_HHmmss"));		//# IM-4227
					destinationFilename = tmp.Replace(":", "");
				}

                if (destinationFilename.EndsWith("YYYYMMDD")) //PP-206
                {
                    string tmp = destinationFilename.Replace("YYYYMMDD", DateTime.UtcNow.ToString("yyyyMMdd"));	//PP-206
                    destinationFilename = tmp.Replace(":", "");
                }

				if (destinationFilename != "")
				{
					// User has a custom filename they want to use - so just add the appropriate extension from the source report name
					destinationFilename += Path.GetExtension(fp.AttachmentFiles);		// use extension from report file generated e.g. CSV	//# IM-4227
				}
				else
				{
					// use the default report name that the report writer assigned when creating the report
					destinationFilename = Path.GetFileName(fp.AttachmentFiles);
				}

				// Unencrypt username and password	// TODO

				var sftp = new SftpClient(fp.IPAddress, fp.Port, fp.Username, fp.Password); //# PP-223 ---Added Port Number
				sftp.Connect();

				using (var file = File.OpenRead(fp.AttachmentFiles))
				{
					if (destinationPath != "")
					{
						destinationPath = FormatPathForOSTalkingTo(destinationPath, sftp.WorkingDirectory);
						sftp.ChangeDirectory(destinationPath);
					}
					sftp.UploadFile(file, destinationFilename);
				}

				sftp.Disconnect();
				succeeded = true;
			}
			catch (Exception ex)
			{
				var msg = string.Format("Ftp ID={0} file=[{1}] server=[{2}]: error {3} : Stack {4} destination :{5}",
                    fp.ID, Path.GetFileName(fp.AttachmentFiles).Truncate(30), fp.IPAddress, ex.Message, ex.StackTrace, destinationFilename);
                AttentionUtils.Attention(new Guid("63dd8220-d6a8-badd-8158-bed1aa10d130"), msg);
				_Log.Warn(msg);
				succeeded = false;
			}
			//if ftp'ed successfully, save to FtpSent and delete from FtpPending
			if (succeeded)
			{
				DeleteFtpPending(new IDRequest(fp.ID));
				var req = new SaveRequest<FtpSent>();
				var fs = new FtpSent(fp);
				fs.LastRetryAt = fp.DateModified;
				fs.Retry = fp.Retry + 1;
				fs.TimeToSend = DateTime.MaxValue;//never to send again
				req.Item = fs;
				SaveFtpSent(req);

			}
			//if failed, save to FtpFailed and delete from FtpPending
			else
			{
				DeleteFtpPending(new IDRequest(fp.ID));
				var request = new SaveRequest<FtpFailed>();
				var fs = new FtpFailed(fp);
				fs.LastRetryAt = fp.DateModified;
				if (!string.IsNullOrEmpty(fs.DestinationPath) && fs.Retry < _Retries.Length) //TODO check for path valid syntax
				{
					fs.TimeToSend = DateTime.UtcNow.AddMinutes(_Retries[fs.Retry]);
					fs.Retry++;
				}
				else
				{
					fs.TimeToSend = DateTime.MaxValue; // don't send again
					fs.Deleted = true;
				}
				request.Item = fs;
				SaveFtpFailed(request);
			}
		}

		/// <summary>
		/// Format path in Unix or Windows format, depending on what system we're talking to
		/// </summary>
		/// <param name="path"></param>
		/// <param name="samplePath"></param>
		private string FormatPathForOSTalkingTo(string path, string samplePath)
		{
			if (samplePath.Contains("/"))
			{
				// This is a Unix system we're talking to, so alter our path to match this
				path = path.Replace("\\", "/");
				return path;
			}
			else
			{
				// Assume this is a Windows system we're talking to, so path can remain the same
				return path;
			}
		}
		//. IM-3927

		private void SendPendingSMS()
		{
			try
			{
				var response = GetSMSPendingList(new IDRequest(Guid.Empty));
				foreach (SMSPending sp in response.List)
				{
					SendSingleSMS(sp);
				}
			}
			catch (Exception e)
			{
				ErrorHandler.HandleInternal(e);
				_Log.Info("NOTIFYError - SendPendingSMS - " + (e.InnerException != null ? e.InnerException.Message : e.Message));
			}
		}

		private void SendSingleSMS(SMSPending sp)
		{
			//send SMS as email
			bool succeeded = false;
			if (sp == null) return;
			try
			{
				//generate mail
				MailMessage mail = new MailMessage(new MailAddress(_SMSSenderAddress, _EmailSenderName), new MailAddress(sp.ToPhoneNumber + _SMSReceiverSuffix));
				mail.Subject = StripOffHtmlTags(sp.Subject);
				mail.Body = StripOffHtmlTags(sp.Message);

				//send the mail
				if (_SmtpClient != null && !_SuppressNotification)
				{
					_SmtpClient.Send(mail);
				}
				succeeded = true;
				//mail.Dispose();
			}
			catch (FormatException ex)
			{
				_Log.Info(ex.Message);
				succeeded = false;
			}
			catch (SmtpException ex)
			{
				_Log.Info(ex.Message);
				succeeded = false;
			}
			catch (Exception e)
			{
				_Log.Info(e.Message);
				succeeded = false;
			}
			//if send successfully, save to SMSSent and delete from SMSPending
			if (succeeded)
			{
				SaveRequest<SMSSent> req = new SaveRequest<SMSSent>();
				SMSSent ss = new SMSSent(sp);
				ss.LastRetryAt = sp.DateModified;
				ss.Retry = sp.Retry + 1;
				ss.TimeToSend = DateTime.MaxValue;//never to send again
				req.Item = ss;
				SaveSMSSent(req);
				DeleteSMSPending(new IDRequest(sp.ID));

			}
			//if failed, save to SMSFailed and delete from SMSPending
			else
			{
				SaveRequest<SMSFailed> request = new SaveRequest<SMSFailed>();
				SMSFailed sf = new SMSFailed(sp);
				sf.LastRetryAt = sp.DateModified;
				if (!string.IsNullOrEmpty(sf.ToPhoneNumber) && sf.Retry < _Retries.Length)
				{
					sf.TimeToSend = DateTime.UtcNow.AddMinutes(_Retries[sf.Retry]);
					sf.Retry++;
				}
				else
				{
					sf.TimeToSend = DateTime.MaxValue; // don't send again
					sf.Deleted = true;
				}
				request.Item = sf;
				SaveSMSFailed(request);
				DeleteSMSPending(new IDRequest(sp.ID));
			}
		}


		private void SendPendingFax()
		{
			//try
			//{
			//	var response = GetFaxPendingList(new IDRequest(Guid.Empty));
			//	foreach (FaxPending fp in response.List)
			//	{
			//		SendSingleFax(fp);
			//	}
			//}
			//catch (Exception e)
			//{
			//	ErrorHandler.HandleInternal(e);
			//	_EventLog.Info("NOTIFYError - SendPendingFax - " + (e.InnerException != null ? e.InnerException.Message : e.Message));
			//}
		}

		/*private void SendSingleFax(FaxPending fp)
		{
				bool succeeded = false;
				if (fp == null) return;
				try
				{
						//generate FaxDoc
						FAXCOMLib.FaxDoc doc = null;
						int response = -11; 

						if (_FaxServer != null)
						{
								_FaxServer.Connect(Environment.MachineName);
								doc = (FAXCOMLib.FaxDoc)_FaxServer.CreateDocument(fp.fileName);
				
								doc.FaxNumber = fp.faxnumber;
								doc.RecipientName = fp.RecipientName;
								doc.DisplayName = "Imarda360 Notification"; 
				
								//send the fax
								response = doc.Send();
					
								//TODO: check fax status before set succeeded boolean to true
								//FaxJobs jobs = (FaxJobs)_FaxServer.GetJobs();
								//FaxJob job = (FaxJob)jobs.Item[1];
								//string faxStatus = job.QueueStatus;
								//if(faxStatus == true)   
										succeeded = true;
						}
				}
				catch (Exception e)
				{
						_EventLog.Info("Fax ERROR - " + e.Message);
						succeeded = false;
				}
				//if send successfully, save to FaxSent and delete from FaxPending
				if (succeeded)
				{
						SaveRequest<FaxSent> req = new SaveRequest<FaxSent>();
						FaxSent ss = new FaxSent(fp);
						ss.LastRetryAt = fp.DateModified;
						ss.Retry = fp.Retry + 1;
						ss.TimeToSend = DateTime.MaxValue;//never to send again
						req.Item = ss;
						SaveFaxSent(req);
						DeleteFaxPending(new IDRequest(fp.ID));

				}
				//if failed, save to FaxFailed and delete from FaxPending
				else
				{
						SaveRequest<FaxFailed> request = new SaveRequest<FaxFailed>();
						FaxFailed ff = new FaxFailed(fp);
						ff.LastRetryAt = fp.DateModified;
						ff.Retry = fp.Retry + 1;
						ff.TimeToSend = CalculateTimeToSend(ff.Retry);
						request.Item = ff;
						SaveFaxFailed(request);
						DeleteFaxPending(new IDRequest(fp.ID));
				}
		}*/


		private void CheckFailed()
		{
			while (true)
			{
				try
				{
					CheckFailedEmails();
					CheckFailedSMS();
					CheckFailedFtps();		//& IM-3927
					//CheckFailedFax();

					Thread.Sleep(3000);

				}
				catch (Exception e)
				{
					ErrorHandler.HandleInternal(e);
					_Log.Info("NOTIFYError - " + (e.InnerException != null ? e.InnerException.Message : e.Message));
					Thread.Sleep(100000);
				}
			}

		}

		private void CheckFailedEmails()
		{
			try
			{
				var response = GetEmailFailedDueList(new IDRequest(Guid.Empty));
				if (response.List != null && response.List.Count > 0)
				{
					SaveListRequest<EmailPending> request = new SaveListRequest<EmailPending>();
					request.List = new List<EmailPending>();
					foreach (EmailFailed ef in response.List)
					{
						//put all due failed emails to pending
						EmailPending ep = new EmailPending(ef);

						request.List.Add(ep);
						DeleteEmailFailed(new IDRequest(ef.ID));
					}
					SaveEmailPendingList(request);//save to db pending table

				}
			}
			catch (Exception e)
			{
				ErrorHandler.HandleInternal(e);
				_Log.Info("NOTIFYError - CheckFailedEmails - " + (e.InnerException != null ? e.InnerException.Message : e.Message));
			}
		}

		//& IM-3927
		private void CheckFailedFtps()
		{
			try
			{
				var response = GetFtpFailedDueList(new IDRequest(Guid.Empty));
				if (response.List != null && response.List.Count > 0)
				{
					SaveListRequest<FtpPending> request = new SaveListRequest<FtpPending>();
					request.List = new List<FtpPending>();
					foreach (FtpFailed ff in response.List)
					{
						//put all due failed Ftps to pending
						FtpPending fp = new FtpPending(ff);

						request.List.Add(fp);
						DeleteFtpFailed(new IDRequest(ff.ID));
					}
					SaveFtpPendingList(request);//save to db pending table
				}
			}
			catch (Exception e)
			{
				ErrorHandler.HandleInternal(e);
				_Log.Info("NOTIFYError - CheckFailedFtps - " + (e.InnerException != null ? e.InnerException.Message : e.Message));
			}
		}
		//. IM-3927

		private void CheckFailedSMS()
		{
			try
			{
				var response = GetSMSFailedDueList(new IDRequest(Guid.Empty));
				if (response.List != null && response.List.Count > 0)
				{
					SaveListRequest<SMSPending> request = new SaveListRequest<SMSPending>();
					request.List = new List<SMSPending>();
					foreach (SMSFailed sf in response.List)
					{
						//put all due failed emails to pending
						SMSPending ep = new SMSPending(sf);

						request.List.Add(ep);
						DeleteSMSFailed(new IDRequest(sf.ID));
					}
					SaveSMSPendingList(request);//save to db pending table
				}
			}
			catch (Exception e)
			{
				ErrorHandler.HandleInternal(e);
				_Log.Info("NOTIFYError - CheckFailedSMSs - " + (e.InnerException != null ? e.InnerException.Message : e.Message));
			}
		}

		private void CheckFailedFax()
		{
			throw new NotImplementedException();
		}


		//#region SendPlan
		///// <summary>
		///// Send email or SMS or Fax for a NotificationPlan
		///// </summary>
		///// <param name="req">[0]=NotificationPlan ID, [1]=CompanyID, [2]=UserID, [3]=NotificationPlan Name, 
		///// [4]=TemplateID, [5]=Delivery Method (Email, SMS, Fax etc), 
		///// [6]=list of recipients, in format "name1|destination1||name2|destination2||name3|destination3||..."
		///// [7]=CC addresses for email, [8]=Bcc addresses for email</param>
		///// <returns>always return a response with status = ok</returns>
		//public BusinessMessageResponse SendPlan(GenericRequest req)
		//{
		//  try
		//  {
		//	var p = req.Parameters;
		//	string planID = GetValue(p, 0);
		//	string companyID = GetValue(p, 1);
		//	string userID = GetValue(p, 2);
		//	string subject = GetValue(p, 3);
		//	string templateID = GetValue(p, 4);
		//	string deliveryMethod = GetValue(p, 5);
		//	string recipientList = GetValue(p, 6);
		//	string cc = "";//GetValue(p, 7);
		//	string bcc = "";//GetValue(p, 8);
		//	//string attachments = GetValue(p, 9);in format "c:\attach1.doc;c:\attach2.kml;...",
		//	string attachments = "";
		//	string template = "";
		//	string fromAddress

		//	string data = new StringBuilder(GetValue(p, 10))
		//	  .AppendKV("Plan", subject)
		//	  .AppendKV("Recipients", recipientList.Replace("||", ", ").Replace('|', ' '))
		//	  .AppendKV("Deliv", deliveryMethod)
		//	  .ToString();


		//	//create Notification object
		//	Guid notifyID = CreateNotification(planID, subject, companyID, userID);
		//	switch (deliveryMethod.ToLowerInvariant())
		//	{
		//	  case "email":
		//		template = GetTemplate(templateID, companyID, "Email", out attachments);
		//		SendEmail(template, recipientList, fromAddress, subject, notifyID, companyID, userID, cc, bcc, attachments, data);
		//		break;
		//	  case "sms":
		//		template = GetTemplate(templateID, companyID, "SMS");
		//		SendSMS(template, recipientList, subject, notifyID, companyID, userID, data);
		//		break;
		//	  case "fax":
		//		template = GetTemplate(templateID, companyID, "Fax");
		//		SendFax(template, recipientList, subject, notifyID, companyID, userID, data);
		//		break;
		//	}
		//  }
		//  catch (Exception ex)
		//  {
		//	return ErrorHandler.Handle(ex);
		//  }
		//  return new BusinessMessageResponse();
		//}

		//private string GetTemplate(string templateID, string companyID, string type)
		//{
		//  System.Text.StringBuilder builder = new System.Text.StringBuilder();
		//  //builder.Append("[");
		//  Guid[] param = new Guid[1];
		//  param[0] = new Guid(companyID);
		//  IImardaConfiguration service = ImardaProxyManager.Instance.IImardaConfigurationProxy;
		//  ChannelInvoker.Invoke(delegate(out IClientChannel channel)
		//  {
		//	channel = service as IClientChannel;
		//	var request = new ConfigRequest(new Guid(templateID), null, param) { IgnoreCache = true };
		//	var response = service.GetConfigValue(request);
		//	string rawResult = response.Item.As<string>("");
		//	rawResult = rawResult.Replace("\"", "'");
		//	if (response.Item != null)
		//	{
		//	  //builder.Append("[\"" + templateID.ToString() + "\"],");
		//	  //builder.Append("[\"" + GetUID(response.Item.UID) + "\"],");
		//	  builder.Append(rawResult);
		//	}
		//  });
		//  //builder.Append("]");
		//  return builder.ToString();
		//}
		//private string GetTemplate(string templateID, string companyID, string type, out string attachments)
		//{
		//  string template = GetTemplate(templateID, companyID, type);
		//  attachments = GetAttachments(template);
		//  return template;
		//}

		private string GetAttachments(string template)
		{
			return "";
		}


		/// <summary>
		/// Send email or SMS or Fax for a NotificationItem
		/// </summary>
		/// <param name="req">[0]=NotificationPlan ID, [1]=CompanyID, [2]=UserID, [3]=NotificationPlan Name, 
		/// [4]=Content, [5]=Delivery Method (Email, SMS, Fax etc), 
		/// [6]=recipient, in format "name1|destination1||"
		/// [7]=CC addresses for email, [8]=Bcc addresses for email, [9]=attachment, [10]=parameters, [11]=from [12]=tzid [13]=priority</param>
		/// <returns>always return a response with status = ok</returns>
		public BusinessMessageResponse SendSingle(GenericRequest req)
		{
			try
			{
				_Log.InfoFormat("SendSingle - enter");
				var p = req.Parameters;
				string planID = GetString(p, 0);
				string companyID = GetString(p, 1);
				string userID = GetString(p, 2);
				string subject = GetString(p, 3);
				string content = GetString(p, 4);
				string deliveryMethod = GetString(p, 5);
				string recipientList = GetString(p, 6);
				string cc = "";//GetString(p, 7);
				string bcc = "";//GetString(p, 8);
				string attachments = GetString(p, 9);
				string data = GetString(p, 10);
				string fromAddress = GetString(p, 11);
				string tzid = GetString(p, 12);
				string priority = GetString(p, 13);
				byte prio;
				if (!byte.TryParse(priority, out prio)) prio = 1;
				Guid notifyID = Guid.Empty;
				_Log.InfoFormat("SendSingle: plan {0}, to {1}, by {2}, data {3}", planID, recipientList, deliveryMethod, data);

				if (planID != Guid.Empty.ToString())
				{
					notifyID = CreateNotification(planID, subject, companyID, userID);
				}
				//IM-5884 check recipient list on sending email
				var response = new BusinessMessageResponse();
				switch (deliveryMethod.ToUpperInvariant())
				{
					case "EMAIL":
						response = SendEmailInternal(content, recipientList, null, subject, notifyID, companyID, userID, cc, bcc, attachments, tzid, data, prio);
						break;

					case "SMS":
						subject = null;
						SendSMSInternal(content, recipientList, fromAddress, subject, notifyID, companyID, userID, tzid, data);
						break;

					case "FAX":
						SendFaxInternal(content, recipientList, subject, notifyID, companyID, userID, tzid, data);
						break;
				}
				_Log.InfoFormat("SendSingle end.");
				return response;
			}
			catch (Exception ex)
			{
				_Log.ErrorFormat("Exception caught in SendSingle: {0}", ex);
				return ErrorHandler.Handle(ex);
			}
		}


		/// <summary>
		/// Send email 
		/// </summary>
		/// <param name="req">[0]=CompanyID, [1]=UserID, [2]=Content, [3]=Subject, 
		/// [4]=AttachmentFilesFullPaths, in format "c:\attach1.doc;c:\attach2.kml;...",
		/// [5]=list of recipients, in format "name1|destination1||name2|destination2||name3|destination3||..."
		/// [6]=CC addresses for email, [7]=Bcc addresses for email [8] is mailmerge parameters [9] timezone [10] priority</param>
		/// <returns>always return a response with status = ok</returns>
		public BusinessMessageResponse SendEmail(GenericRequest req)
		{
			try
			{
				_Log.Debug("Enter SendEmail");
				var p = req.Parameters;
				string companyID = GetString(p, 0);
				string userID = GetString(p, 1);
				string content = GetString(p, 2);
				string subject = GetString(p, 3);
				string attachments = GetString(p, 4);
				string recipientList = GetString(p, 5);
				string cc = GetString(p, 6);
				string bcc = GetString(p, 7);
				string data = GetString(p, 8);
				string tzid = GetString(p, 9);
				string priority = GetString(p, 10);
				byte prio;
				if (!byte.TryParse(priority, out prio)) prio = 1;

				_Log.DebugFormat("SendEmail comp={0} content={1} subject={2} attach={3} recip={4}, data={5}, tzid={6}",
					companyID, content, subject, attachments.Truncate(30),
					recipientList, data.Truncate(100), tzid);

				//IM-5884 check recipient list on sending email
				return SendEmailInternal(content, recipientList, null, subject, SequentialGuid.NewDbGuid(), companyID, userID, cc, bcc, attachments, tzid, data, prio);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		//& IM-3927
		public BusinessMessageResponse SendsFTP(GenericRequest req)
		{
			try
			{
				_Log.Debug("Enter SendsFTP");
				var p = req.Parameters;
				string companyID = GetString(p, 0);
				string userID = GetString(p, 1);
				string server = GetString(p, 2);
				string port = GetString(p, 3);
				string username = GetString(p, 4);
				string password = GetString(p, 5);
				string psk = GetString(p, 6);
				string destinationPath = GetString(p, 7);
				string attachments = GetString(p, 8);
				string priority = GetString(p, 9);
				byte prio;
				if (!byte.TryParse(priority, out prio)) prio = 1;

				_Log.DebugFormat("SendsFTP comp={0} server={1} username={2} attachments={3}",
					companyID, server, username, attachments.Truncate(30));

				SendFtpInternal(companyID, userID, server, port, username, password, psk, destinationPath, attachments, prio);
				return new BusinessMessageResponse();
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		//. IM-3927


		/// <summary>
		/// Send SMS 
		/// </summary>
		/// <param name="req">[0]=CompanyID, [1]=UserID, [2]=Content, [3]=Subject, 
		/// [4]=list of recipients, in format "name1|destination1||name2|destination2||name3|destination3||..."</param>
		/// <returns>always return a response with status = ok</returns>
		public BusinessMessageResponse SendSMS(GenericRequest req)
		{
			var p = req.Parameters;
			string companyID = GetString(p, 0);
			string userID = GetString(p, 1);
			string content = GetString(p, 2);
			string subject = GetString(p, 3);
			string recipientList = GetString(p, 4);
			string data = GetString(p, 5);
			string fromAddress = GetString(p, 6);
			string tzid = GetString(p, 7);

			SendSMSInternal(content, recipientList, fromAddress, subject, SequentialGuid.NewDbGuid(), companyID, userID, tzid, data);
			return new BusinessMessageResponse();
		}


		public SimpleResponse<string> HttpPost(GenericRequest req)
		{
			try
			{
				Guid companyID = req.ID;
				var p = req.Parameters;
				string url = GetString(p, 0);
				string template = GetString(p, 1);
				string data = GetString(p, 2);
				string tzid = GetString(p, 3);
				string message = FillInTemplate(companyID, Guid.Empty, tzid, template, data);

				WebRequest webRequest = WebRequest.Create(url);
				webRequest.ContentType = "application/x-www-form-urlencoded";
				webRequest.Method = "POST";
				byte[] bytes = Encoding.ASCII.GetBytes(message);
				webRequest.ContentLength = bytes.Length;
				using (Stream stream = webRequest.GetRequestStream())
				{
					stream.Write(bytes, 0, bytes.Length);
				}

				WebResponse webResponse = webRequest.GetResponse();
				if (webResponse == null) return new SimpleResponse<string>(string.Empty);
				StreamReader sr = new StreamReader(webResponse.GetResponseStream());
				string responseMessage = sr.ReadToEnd().Trim();
				return new SimpleResponse<string>(responseMessage);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SimpleResponse<string>>(ex);
			}
		}



		private static string GetString(object[] par, int index)
		{
			if (index >= par.Length) return null;
			object o = par[index];
			if (o == null || string.Empty.Equals(o)) return null;
			if (o is string) return (string)o;
			return null;
		}

		private static string MakeSubjectLineFromContent(string prefix, string content)
		{
			string suffix = "...";
			int max = 50;
			int p = content.IndexOf("\n");
			string subject = prefix + (p == -1 ? content : content.Remove(p)).TrimEnd();
			if (subject.Length > max) subject = subject.Remove(max - suffix.Length) + suffix;
			return subject;
		}




		private Guid CreateNotification(string planID, string planName, string companyID, string userID)
		{
			DateTime now = DateTime.UtcNow;
			Notification notification = new Notification();
			notification.ID = SequentialGuid.NewDbGuid();
			notification.CompanyID = new Guid(companyID);
			notification.UserID = new Guid(userID);
			notification.NotificationPlanID = new Guid(planID);
			notification.Description = "Notification " + planName + " at " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
			notification.Status = "Start";
			notification.Active = true;
			notification.Deleted = false;
			notification.DateCreated = now;
			notification.DateModified = now;
			SaveNotification(new SaveRequest<Notification>(notification));
			return notification.ID;
		}

		private BusinessMessageResponse SendEmailInternal(
			string template,
			string recipientList,
			string fromAddress,
			string subject,
			Guid notifyID,
			string companyID,
			string userID,
			string cc,
			string bcc,
			string attachments,
			string tzid,
			string data,
			byte priority)
		{
			//IM-5884 check recipient list on sending email
			if (null == recipientList || string.IsNullOrEmpty(recipientList))
			{
				var msg = string.Format("{0} - Recipient list cannot be null or empty", MethodBase.GetCurrentMethod().Name);
				return new BusinessMessageResponse { Status = false, StatusMessage = msg };
			}

			string attachmentFiles = SaveAttachmentData(_AttachmentFolder, attachments);
			EmailDraft ed = CreateEmailDraft(template, fromAddress, subject, notifyID, companyID, userID, tzid, data, priority);
            var destinationMap = recipientList.KeyValueMap(ValueFormat.Strings, true);
			foreach (string recipientName in destinationMap.Keys)
			{
				EmailPending ep = new EmailPending(ed);
				ep.ToAddress = (string)destinationMap[recipientName];
				ep.RecipientName = recipientName == ep.ToAddress ? string.Empty : recipientName.ToString();
				ep.Message = ed.Message;
				ep.CC = cc;
				ep.Bcc = bcc;
				ep.AttachmentFiles = attachmentFiles;
				ep.Priority = priority;
				SaveEmailPending(new SaveRequest<EmailPending>(ep));//save to db as pending email
			}

			return new BusinessMessageResponse();
		}

		//& IM-3927
		private void SendFtpInternal(
			string companyID,
			string userID,
			string server,
			string port,
			string username,
			string password,
			string psk,
			string destinationPath,
			string attachments,
			byte priority)
		{
			string attachmentFiles = SaveAttachmentData(_AttachmentFolder, attachments);
			FtpPending fp = new FtpPending();

			fp.ID = SequentialGuid.NewDbGuid();
			fp.CompanyID = new Guid(companyID);
			fp.UserID = new Guid(userID);

			fp.IPAddress = server;
			fp.Port = (byte) Int32.Parse(port);
			fp.Username = username;
			fp.Password = password;
			fp.PSK = psk;
			fp.DestinationPath = destinationPath;
			fp.AttachmentFiles = attachmentFiles;
			fp.Priority = priority;
			SaveFtpPending(new SaveRequest<FtpPending>(fp));		//save to db as pending ftp
		}
		//. IM-3927

		private EmailDraft CreateEmailDraft(string template, string fromAddress, string subject, Guid notifyID, string companyID, string userID, string tzid, string data, byte priority)
		{
			EmailDraft draft = new EmailDraft();
			draft.ID = SequentialGuid.NewDbGuid();
			draft.CompanyID = new Guid(companyID);
			draft.UserID = new Guid(userID);
			draft.FromAddress = string.IsNullOrEmpty(fromAddress) ? _EmailSenderAddress : fromAddress;
			draft.NotificationID = notifyID;
			draft.Message = FillInTemplate(draft.CompanyID, draft.UserID, tzid, template, data);
			string prefix = draft.Message.StartsWith("i360") ? string.Empty : "i360 - ";
			if (string.IsNullOrEmpty(subject)) subject = MakeSubjectLineFromContent(prefix, draft.Message);
			draft.Subject = subject;
			draft.Priority = priority;
			SaveEmailDraft(new SaveRequest<EmailDraft>(draft));
			return draft;
		}

		private void SendSMSInternal(string template, string recipientList, string fromAddress, string subject, Guid notifyID, string companyID, string userID, string tzid, string data)
		{
			EmailDraft ed = CreateEmailDraft(template, fromAddress, subject, notifyID, companyID, userID, tzid, data, 0);
            var destinationMap = recipientList.KeyValueMap(ValueFormat.Strings, true);
			foreach (var recipientName in destinationMap.Keys)
			{
				EmailPending ep = new EmailPending(ed);
				ep.ToAddress = (string)destinationMap[recipientName] + _SMSReceiverSuffix;
				ep.RecipientName = (string)recipientName == ep.ToAddress ? string.Empty : recipientName.ToString();
				ep.Message = ed.Message;
				ep.CC = "";
				ep.Bcc = "";
				ep.AttachmentFiles = "";
				SaveEmailPending(new SaveRequest<EmailPending>(ep));//save to db as pending email
			}

			//SMSDraft sd = CreateSMSDraft(template, subject, notifyID, companyID, userID, data);
			//var destinationMap = recipientList.KeyValueMap(ValueFormat.Strings, true);
			//foreach (var recipientName in destinationMap.Keys)
			//{
			//  SMSPending sp = new SMSPending(sd);
			//  //sp.Message = TranslateTemplate(template, recipientName); 
			//  sp.Message = sd.Message;
			//  sp.Subject = subject;
			//  sp.ToPhoneNumber = (string)destinationMap[recipientName];
			//  sp.RecipientName = recipientName.ToString();
			//  //Todo: add greetings to message.
			//  SaveSMSPending(new SaveRequest<SMSPending>(sp));//save to db as pending SMS
			//}
		}

		private SMSDraft CreateSMSDraft(string template, string subject, Guid notifyID, string companyID, string userID, string tzid, string data)
		{
			SMSDraft draft = new SMSDraft();
			draft.ID = SequentialGuid.NewDbGuid();
			draft.CompanyID = new Guid(companyID);
			draft.UserID = new Guid(userID);
			draft.NotificationID = notifyID;
			draft.Subject = subject;
			draft.Message = FillInTemplate(draft.CompanyID, draft.UserID, tzid, template, data);
			return draft;
		}

		private void SendFaxInternal(string template, string recipientList, string subject, Guid notifyID, string companyID, string userID, string tzid, string data)
		{
			throw new NotImplementedException();
		}

		//private Regex _rxTemplateParameter = new Regex(@"<([A-Za-z_]\w*)>", RegexOptions.Compiled);

		/// <summary>
		/// Fill in the template, replace identifiers in the template in angular brackets by $(identifier).
		/// Flatten the arrays in the data to simple identifiers, DRV#1 becomes DRV11, DRV21, DRV31
		/// </summary>
		/// <returns></returns>
		private string FillInTemplate(Guid companyID, Guid personID, string timeZoneId, string template, string typedData)
		{
			if (string.IsNullOrEmpty(template)) return string.Empty;
			template = template.Replace("<br>", "\r\n").Replace("&nbsp;", " ");
			var ifp = GetFormatter(companyID, personID, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
			IDictionary args = EAHelper.MakeFormattedValues(typedData, ifp);
			string msg = new ConfigTemplate(template).Instantiate(args);
			return msg;
		}

		public static ImardaFormatProvider GetFormatter(Guid companyID, Guid personID, TimeZoneInfo tzi)
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
			//TODO optimize: use a cache locale->CultureInfo
			var ci = new CultureInfo(locale); // cannot use CultureInfo.GetCultureInfo() because we have to customize Infinity and NaN
			var mfi = new MeasurementFormatInfo(ci.NumberFormat);
			mfi.SetPreferences(prefMap);
			var ifp = new ImardaFormatProvider(ci, mfi, tzi);
			ifp.ForceDefaultDateFormat = true; // don't use 'w' or 'z' format in emails, sms and other notifications.
			return ifp;
		}


		/// <summary>
		/// Save the data contained in the attachments as files and return the file names. First look if a | is contained in the
		/// 'attachments' string. If that's not the case, just return the input string: it is provided in the old format
		/// file1;file2;... with files assumed to exist already. If there is a | then chop the attachments string into parts on 
		/// | and interpret odd/even pairs: odd is file name, even is data or empty. The file name will be prefixed with the default
		/// folder if no folder provided. If data exists interpret as base64, convert to binary and save into a file with the
		/// given name.
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="attachments">"filename1|base64data1|filename2|base64data2|..." or "filepath1;filepath2;..."</param>
		/// <returns></returns>
		private static string SaveAttachmentData(string folder, string attachments)
		{
			if (string.IsNullOrEmpty(attachments) || !attachments.Contains("|"))
			{
				return attachments;
			}
			else
			{
				var list = new List<string>();
				string[] parts = attachments.Split('|');
				for (int i = 0; i < parts.Length; i += 2)
				{
					string fn = parts[i];
					string data64 = parts[i + 1];
					string dir = Path.GetDirectoryName(fn);
					if (dir == string.Empty) fn = Path.Combine(folder, fn);

					if (data64 != string.Empty)
					{
						File.WriteAllBytes(fn, Convert.FromBase64String(data64));
					}
					list.Add(fn);
				}
				return string.Join(";", list.ToArray());
			}
		}

	}
}
