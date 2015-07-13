/***********************************************************************
Auto Generated Code.

Generated by   : IMARDAINC\qian.chen
Date Generated : 07/02/2011 11:32 a.m.
Copyright (c)2009 CodeGenerator 1.2
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaSecurityBusiness 
{
	[DataContract]
	public class LogonLog : FullBusinessEntity 
	{				
		#region Constructor
		public LogonLog()
		{
		}
		#endregion
		
		#region Properties

		[DataMember]
		public Guid SecurityEntityID { get; set; }

		[DataMember]
		public bool Success { get; set; }

		public const int HostIPAddressMaxLen = 30;
		[ValidLength(-1, HostIPAddressMaxLen)]
		[DataMember]
		public string HostIPAddress { get; set; }

		[DataMember]
		public Guid ApplicationID { get; set; }

		[DataMember]
		public Guid SessionObjectID { get; set; }

		[DataMember]
		public Byte Logon { get; set; }

		[DataMember]
		public int FailureCode { get; set; }

        // IM-5245 invariant culture format
		[DisplayFormat("dd/MM/yyyy HH:mm:ss")]
		public DateTime LogonDate
		{
			get { return DateCreated; }
			set { DateCreated = value; }
		}

		public const int LoginUsernameMaxLen = 50;
		[ValidLength(-1, LoginUsernameMaxLen)]
		[DataMember]
		public string LoginUsername { get; set; }
#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		#endregion
		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			SecurityEntityID = GetValue<Guid>(dr, "SecurityEntityID");
			Success = GetValue<bool>(dr, "Success");
			HostIPAddress = GetValue<string>(dr, "HostIPAddress");
			ApplicationID = GetValue<Guid>(dr, "ApplicationID");
			SessionObjectID = GetValue<Guid>(dr, "SessionObjectID");
			Logon = GetValue<Byte>(dr, "Logon");
			FailureCode = GetValue<int>(dr, "FailureCode");

			LoginUsername = GetValue<string>(dr, "LoginUsername");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		}

		

	
	}
}

