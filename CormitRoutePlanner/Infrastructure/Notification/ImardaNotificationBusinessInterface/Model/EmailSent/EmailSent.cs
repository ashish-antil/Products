/***********************************************************************
Auto Generated Code.

Generated by   : IMARDAINC\Qian.Chen
Date Generated : 12/02/2010 3:40 p.m.
Copyright (c)2009 CodeGenerator 1.2
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaNotificationBusiness
{
	[DataContract]
	public class EmailSent : FullBusinessEntity
	{
		private List<string> _CCList;
		private List<string> _BccList;
		private List<string> _AttachmentFileList;
		private string _CC;
		private string _Bcc;
		private string _AttachmentFiles;

		#region Constructor
		public EmailSent()
		{
			_CCList = new List<string>();
			_BccList = new List<string>();
			_AttachmentFileList = new List<string>();
		}
		public EmailSent(EmailPending ep)
			: base()
		{
			_CCList = new List<string>();
			_BccList = new List<string>();
			_AttachmentFileList = new List<string>();

			this.ID = ep.ID;
			this.CompanyID = ep.CompanyID;
			this.UserID = ep.UserID;

			this.EmailDraftID = ep.EmailDraftID;
			this.FromAddress = ep.FromAddress;
			this.LastRetryAt = ep.LastRetryAt;
			this.Message = ep.Message;
			this.NotificationID = ep.NotificationID;
			this.Retry = ep.Retry;
			this.Status = "Sent";
			this.Subject = ep.Subject;
			this.TimeToSend = ep.TimeToSend;
			this.ToAddress = ep.ToAddress;
			this.RecipientName = ep.RecipientName;
			this.CC = ep.CC;
			this.Bcc = ep.Bcc;
			this.AttachmentFiles = ep.AttachmentFiles;
			
		}
		#endregion

		#region Properties

		[DataMember]
		public Guid EmailDraftID { get; set; }

		public const int SubjectMaxLen = 200;
		[ValidLength(-1, SubjectMaxLen)]
		[DataMember]
		public string Subject { get; set; }

		public const int FromAddressMaxLen = 50;
		[ValidLength(0, FromAddressMaxLen)]
		[DataMember]
		public string FromAddress { get; set; }

		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public Guid NotificationID { get; set; }

		[DataMember]
		public string ToAddress { get; set; }

		[DataMember]
		public int Retry { get; set; }

		[DataMember]
		public DateTime TimeToSend { get; set; }

		public const int StatusMaxLen = 50;
		[ValidLength(-1, StatusMaxLen)]
		[DataMember]
		public string Status { get; set; }

		[DataMember]
		public DateTime LastRetryAt { get; set; }

		[DataMember]
		public string RecipientName { get; set; }

		[DataMember]
		public string CC
		{
			get { return _CC; }
			set
			{
				_CC = value;
				if (_CC != null && _CCList != null)
				{
					_CCList.Clear();
					string[] splitResult = _CC.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in splitResult)
					{
						_CCList.Add(s);
					}
				}
			}
		}

		[DataMember]
		public string Bcc
		{
			get { return _Bcc; }
			set
			{
				_Bcc = value;
				if (_Bcc != null && _BccList != null)
				{
					_BccList.Clear();
					string[] splitResult = _Bcc.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in splitResult)
					{
						_BccList.Add(s);
					}
				}
			}
		}

		[DataMember]
		public string AttachmentFiles
		{
			get
			{
				return _AttachmentFiles;
			}
			set
			{
				_AttachmentFiles = value;
				if (_AttachmentFiles != null && _AttachmentFileList != null)
				{
					_AttachmentFileList.Clear();
					string[] splitResult = _AttachmentFiles.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in splitResult)
					{
						_AttachmentFileList.Add(s);
					}
				}
			}
		}
		[DataMember]
		public List<string> AttachmentFileList
		{
			get { return _AttachmentFileList; }
		}

		[DataMember]
		public List<string> CCList
		{
			get { return _CCList; }
		}

		[DataMember]
		public List<string> BccList
		{
			get { return _BccList; }
		}

#if EntityProperty
	[DataMember]
	public `cstype` `field` { get; set; }
#endif
		#endregion

		#region Methods
		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			EmailDraftID = GetValue<Guid>(dr, "EmailDraftID");
			Subject = GetValue<string>(dr, "Subject");
			FromAddress = GetValue<string>(dr, "FromAddress");
			Message = GetValue<string>(dr, "Message");
			NotificationID = GetValue<Guid>(dr, "NotificationID");
			ToAddress = GetValue<string>(dr, "ToAddress");
			Retry = GetValue<int>(dr, "Retry");
			TimeToSend = GetDateTime(dr, "TimeToSend");
			Status = GetValue<string>(dr, "Status");
			LastRetryAt = GetDateTime(dr, "LastRetryAt");
			RecipientName = GetValue<string>(dr, "RecipientName");
			CC = GetValue<string>(dr, "CC");
			Bcc = GetValue<string>(dr, "Bcc");
			AttachmentFiles = GetValue<string>(dr, "AttachmentFiles");
#if EntityProperty_NoDate
			`field` = GetColumn<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		
		}

		#endregion


	}
}
