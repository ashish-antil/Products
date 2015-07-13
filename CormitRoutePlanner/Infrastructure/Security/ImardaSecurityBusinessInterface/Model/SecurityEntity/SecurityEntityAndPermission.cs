using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;

namespace ImardaSecurityBusiness
{
	/* GS: 9/4/2008 i think this is obsolete ?
	/// <summary>
	/// This class adds a temporary store of a permission along with a business entity.
	/// The permission is not stored in the database.
	/// </summary>
	[DataContract]
	[Serializable]
	public class SecurityEntityAndPermission : SecurityEntity
	{

		#region Instance Variables
		private int _PermissionsTemp = 0;
		#endregion

		/// <summary>
		/// This is a temporary store of a permission, somthing to do with this entity.
		/// This is not stored in the database.
		/// </summary>
		[DataMember]
		public PermissionKind PermissionTemp
		{
			get { return (PermissionKind)_PermissionsTemp; }
			set { _PermissionsTemp = (int)value; }
		}

		public SecurityEntityAndPermission()
		{
		}

		public SecurityEntityAndPermission(SecurityEntity se)
			: this(se, (PermissionKind)0)
		{
		}

		public SecurityEntityAndPermission(SecurityEntity se, PermissionKind permission)
		{
			this.ID = se.ID;
			_EntityName = se.EntityName;
			_EntityType = se.EntityType;
			_LoginEnabled = se.LoginEnabled;
			_LoginUsername = se.LoginUsername;
			_LoginPassword = se.LoginPassword;
			this.FernID = se.FernID;
			this.FernUser = se.FernUser;
			_CRMId = se.CRMId;

			_PermissionsTemp = (int)permission;
		}
	}
	*/
}
