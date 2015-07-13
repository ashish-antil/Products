//& IM-3927
using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;
using Imarda.Lib;

namespace ImardaNotificationBusiness
{
	[DataContract]
	public class FtpPending : FullBusinessEntity
	{
		private List<string> _AttachmentFileList;
		private string _AttachmentFiles;

		#region Constructor
		public FtpPending()
		{
			_AttachmentFileList = new List<string>();
		}

		public FtpPending(FtpFailed ff)
			: base()
		{
			this.ID = ff.ID;
			this.CompanyID = ff.CompanyID;
			this.UserID = ff.UserID;

			this.IPAddress = ff.IPAddress;
			this.Port = ff.Port;
			this.Username = ff.Username;
			this.Password = ff.Password;
			this.PSK = ff.PSK;
			this.AttachmentFiles = ff.AttachmentFiles;
			this.DestinationPath = ff.DestinationPath;

			this.LastRetryAt = ff.LastRetryAt;
			this.Retry = ff.Retry;
			this.Status = "Pending";
			this.TimeToSend = ff.TimeToSend;

			this.Priority = ff.Priority;
		}
		#endregion

		#region Properties

		[DataMember]
		public const int IPAddressMaxLen = 50;
		[ValidLength(-1, IPAddressMaxLen)]
		[DataMember]
		public string IPAddress
		{ get; set; }
		[DataMember]
		public Byte Port
		{ get; set; }
		public const int UsernameMaxLen = 50;
		[ValidLength(0, UsernameMaxLen)]
		[DataMember]
		public string Username { get; set; }
		public const int PasswordMaxLen = 100;
		[ValidLength(0, PasswordMaxLen)]
		[DataMember]
		public string Password { get; set; }
		public const int PSKMaxLen = 128;
		[ValidLength(-1, PSKMaxLen)]
		[DataMember]
		public string PSK { get; set; }

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

		public const int DestinationPathLen = 128;
		[ValidLength(-1, DestinationPathLen)]
		[DataMember]
		public string DestinationPath { get; set; }

		[DataMember]
		public DateTime TimeToSend { get; set; }

		[DataMember]
		public int Retry { get; set; }

		public const int StatusMaxLen = 50;
		[ValidLength(-1, StatusMaxLen)]
		[DataMember]
		public string Status { get; set; }

		[DataMember]
		public DateTime LastRetryAt { get; set; }

		[DataMember]
		public byte Priority { get; set; }

#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }

#endif

		#endregion

		#region Methods
		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			IPAddress = GetValue<string>(dr, "IPAddress");
			Port = GetValue<Byte>(dr, "Port");
			Username = GetValue<string>(dr, "Username");
			Password = GetValue<string>(dr, "Password");
			PSK = GetValue<string>(dr, "PSK");
			AttachmentFiles = GetValue<string>(dr, "AttachmentFiles");
			DestinationPath = GetValue<string>(dr, "DestinationPath");
			TimeToSend = GetDateTime(dr, "TimeToSend");
			Retry = GetValue<int>(dr, "Retry");
			Status = GetValue<string>(dr, "Status");
			LastRetryAt = GetDateTime(dr, "LastRetryAt");
			Priority = GetValue<byte>(dr, "Priority");
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

