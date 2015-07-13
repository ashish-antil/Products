using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaSecurityBusiness {
	[DataContract]
	[Serializable]
	public class SecurityEntry : FullBusinessEntity {

		#region Instance Variables
		private Guid _ApplicationID;
		private Guid _EntityID;
		private Guid _SecurityObjectID;
		private int _PermissionsGranted;
		private int _PermissionsDenied;
		private int _EntryType;
		#endregion

		#region Properties
		[DataMember]
		public Guid ApplicationID
		{
			get { return _ApplicationID; }
			set { _ApplicationID = value; }
		}

		[DataMember]
		public Guid EntityID
		{
			get { return _EntityID; }
			set { _EntityID = value; }
		}

		[DataMember]
		public Guid SecurityObjectID
		{
			get { return _SecurityObjectID; }
			set { _SecurityObjectID = value; }
		}

		[DataMember]
		public int PermissionsGranted
		{
			get { return _PermissionsGranted; }
			set { _PermissionsGranted = value; }
		}

		[DataMember]
		public int PermissionsDenied
		{
			get { return _PermissionsDenied; }
			set { _PermissionsDenied = value; }
		}

		[DataMember]
		public int EntryType
		{
			get { return _EntryType; }
			set { _EntryType = value; }
		} 
		#endregion

		public SecurityEntry() {
		}

		public override void AssignData(IDataReader dr) { 
			base.AssignData(dr);
			_ApplicationID = GetValue<Guid>(dr, "ApplicationID");
			_EntityID = GetValue<Guid>(dr, "EntityID");
			_SecurityObjectID = GetValue<Guid>(dr, "SecurityObjectID");
			_PermissionsGranted = GetValue<int>(dr, "PermissionsGranted");
			_PermissionsDenied = GetValue<int>(dr, "PermissionsDenied");
			_EntryType = GetValue<int>(dr, "EntryType");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		

		}
	}
}
