using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FernBusinessBase;
using Imarda.Lib;
using System.Globalization;


namespace ImardaSecurityBusiness
{
	/// <summary>
	/// A class that contains session details for current user
	/// </summary>
	/// 
	[DataContract]
	[Serializable]
	public class SessionObject : BusinessEntity
	{
		public SessionObject()
		{
		}

		#region Properties

		public virtual ImardaFormatProvider FormatProvider { get { return null; } }

		/// <summary>
		/// Used in security system. Applications are e.g. I360 Web Services, Public SOAP Service, Public REST Service, IAC Service...
		/// </summary>
		[DataMember]
		public Guid ApplicationID { get; set; }

		[DataMember]
		public Guid SessionID { get; set; }

		[DataMember]
		public Guid CRMID { get; set; }

		[DataMember]
		public Guid SecurityEntityID { get; set; }

		[DataMember]
		public Guid CompanyID { get; set; }

		[DataMember]
		public string EntityName { get; set; }

        [DataMember]
        public int EntityType { get; set; }

		[DataMember]
		public string Username { get; set; }

		[DataMember]
		public string Password { get; set; }

		[DataMember]
		public bool Impersonation { get; set; }

		[DataMember]
		public List<Guid> PermissionsList { get; set; }

		[DataMember]
		public bool EnableTimeZoneSelect { get; set; }

		[DataMember]
		public string TimeZoneKey { get; set; }

		public override string ToString()
		{
			return string.Format("{0}({1:s} User=`{2}` Impersonation={3} SID={4})", GetType().Name, DateModified, Username, Impersonation, SessionID);
		}

		public SessionObject Clone()
		{
			//do a deep copy, fastest copy is just one on one 
			var res = new SessionObject();
			res.Active = Active;
			res.DateCreated = DateCreated;
			res.DateModified = DateModified;
			res.ApplicationID = ApplicationID;
			res.SessionID = SessionID;
			res.CRMID = CRMID;
			res.SecurityEntityID = SecurityEntityID;
			res.CompanyID = CompanyID;
			res.EntityName = EntityName;
			res.Username = Username;
			res.Password = Password;
			res.Impersonation = Impersonation;
			res.EnableTimeZoneSelect = EnableTimeZoneSelect;
			res.TimeZoneKey = TimeZoneKey;
			res.PermissionsList = new List<Guid>(PermissionsList);
			return res;
		}

	}
		#endregion
}
