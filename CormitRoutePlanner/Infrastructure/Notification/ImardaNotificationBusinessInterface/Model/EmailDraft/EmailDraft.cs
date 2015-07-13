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
	public class EmailDraft : FullBusinessEntity
	{
		#region Constructor
		public EmailDraft()
		{
		}
		#endregion

		#region Properties

		public const int SubjectMaxLen = 200;
		[ValidLength(-1, SubjectMaxLen)]
		[DataMember]
		public string Subject { get; set; }

		public const int FromAddressMaxLen = 50;
		[ValidLength(-1, FromAddressMaxLen)]
		[DataMember]
		public string FromAddress { get; set; }

		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public Guid NotificationID { get; set; }
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
			Subject = GetValue<string>(dr, "Subject");
			FromAddress = GetValue<string>(dr, "FromAddress");
			Message = GetValue<string>(dr, "Message");
			NotificationID = GetValue<Guid>(dr, "NotificationID");
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