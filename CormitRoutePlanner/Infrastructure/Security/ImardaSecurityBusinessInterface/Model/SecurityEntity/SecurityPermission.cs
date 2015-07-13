using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;

namespace ImardaSecurityBusiness
{
	[DataContract]
	public class SecurityPermission : FullBusinessEntity
	{
		#region Instant variables
		private Guid _SecurityObjectID;
		private int _PermissionGranted;
		private int _PermissionDenied;
		private Guid _ApplicationID;
		private int _EntryType;
		#endregion

		#region Constructors
		public SecurityPermission()
		{
		}

		public SecurityPermission(Guid securityObjID, int permissionGranted)
		{
		}
		#endregion

		#region Properties
		[DataMember]
		public Guid SecurityObjectID
		{
			get { return _SecurityObjectID; }
			set { _SecurityObjectID = value; }
		}

		[DataMember]
		public int PermissionGranted
		{
			get { return _PermissionGranted; }
			set { _PermissionGranted = value; }
		}

		[DataMember]
		public int PermissionDenied
		{
			get { return _PermissionDenied; }
			set { _PermissionDenied = value; }
		}

		[DataMember]
		public Guid ApplicationID
		{
			get { return _ApplicationID; }
			set { _ApplicationID = value; }
		}

		[DataMember]
		public int EntryType
		{
			get { return _EntryType; }
			set { _EntryType = value; }
		}
		#endregion

		public void AssignFromEntry(SecurityEntry entry)
		{
			this.ID = entry.ID;
			this.CompanyID  = entry.CompanyID ;
			this.UserID = entry.UserID ;
			this.Active = entry.Active;
			this.Deleted = entry.Deleted;
			this.DateModified = entry.DateModified ;

			this.SecurityObjectID = entry.SecurityObjectID;
			this.PermissionGranted = entry.PermissionsGranted;
			this.PermissionDenied = entry.PermissionsDenied;
			this.ApplicationID = entry.ApplicationID;
			this.EntryType = entry.EntryType;
		}

		public SecurityEntry AssignToEntry()
		{
			SecurityEntry entry = new SecurityEntry();

			entry.ID = this.ID;
			entry.CompanyID = this.CompanyID ;
			entry.UserID = this.UserID ;
			entry.Active = this.Active;
			entry.Deleted = this.Deleted;
			entry.DateModified  = this.DateModified ;

			entry.SecurityObjectID = this.SecurityObjectID;
			entry.PermissionsGranted = this.PermissionGranted;
			entry.PermissionsDenied = this.PermissionDenied;
			entry.ApplicationID = this.ApplicationID;
			entry.EntryType = this.EntryType;

			return entry;
		}
	}
}
