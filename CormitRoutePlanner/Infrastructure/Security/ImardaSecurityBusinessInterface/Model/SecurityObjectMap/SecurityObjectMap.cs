using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;
using System.Data;

namespace ImardaSecurityBusiness
{
	[DataContract]
	[Serializable]
	public class SecurityObjectMap : BusinessEntity
	{

		#region Instance Variables
		private Guid _ID;
		private Guid _ClientSecurityObjectID;
		private Guid _SolutionSecurityObjectID;
		private int _PermissionCovered;
		#endregion

		#region Properties
		[DataMember]
		public Guid ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		[DataMember]
		public Guid ClientSecurityObjectID
		{
			get { return _ClientSecurityObjectID; }
			set { _ClientSecurityObjectID = value; }
		}

		[DataMember]
		public Guid SolutionSecurityObjectID
		{
			get { return _SolutionSecurityObjectID; }
			set { _SolutionSecurityObjectID = value; }
		}

		[DataMember]
		public int PermissionCovered
		{
			get { return _PermissionCovered; }
			set { _PermissionCovered = value; }
		}
		#endregion

		public SecurityObjectMap()
		{
		}

		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			_ID = GetValue<Guid>(dr, "ID");
			_ClientSecurityObjectID = GetValue<Guid>(dr, "ClientSecurityObjectID");
			_SolutionSecurityObjectID = GetValue<Guid>(dr, "SolutionSecurityObjectID");
			_PermissionCovered = GetValue<int>(dr, "PermissionCovered");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		

		}
	}
}
