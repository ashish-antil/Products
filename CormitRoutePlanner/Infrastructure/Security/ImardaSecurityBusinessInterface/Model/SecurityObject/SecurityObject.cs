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
	public class SecurityObject : BusinessEntity, IComparable<SecurityObject> {

		#region Instance Variables
		private Guid _ID;		
		private Guid _SolutionID;
		private Guid _ApplicationID;
		private string _DisplayName;
		private Guid _ParentID;
		private Guid _ObjectGroupID;
		private string _Description;
		private int _PermissionsConfigurable;
		private int _ObjectType;
		private Guid _FeatureID;
		#endregion

		#region Properties
		[DataMember]
		public Guid ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		[DataMember]
		public Guid UserID
		{ get;set;}

		[DataMember]
		public Guid SolutionID
		{
			get { return _SolutionID; }
			set { _SolutionID = value; }
		}

		[DataMember]
		public Guid ApplicationID
		{
			get { return _ApplicationID; }
			set { _ApplicationID = value; }
		}

		public const int DisplayNameMaxLen = 100;
		[ValidLength(-1, DisplayNameMaxLen)]
		[DataMember]
		public string DisplayName
		{
			get { return _DisplayName; }
			set { _DisplayName = value; }
		}

		[DataMember]
		public Guid ParentID
		{
			get { return _ParentID; }
			set { _ParentID = value; }
		}

		[DataMember]
		public Guid ObjectGroupID
		{
			get { return _ObjectGroupID; }
			set { _ObjectGroupID = value; }
		}

		[DataMember]
		public string Description
		{
			get { return _Description; }
			set { _Description = value; }
		}

		[DataMember]
		public int PermissionsConfigurable
		{
			get { return _PermissionsConfigurable; }
			set { _PermissionsConfigurable = value; }
		}

		[DataMember]
		public int ObjectType
		{
			get { return _ObjectType; }
			set { _ObjectType = value; }
		}
		#endregion

		public bool IsRoot {
			get { return _ID == Guid.Empty; }
		}
		[DataMember]
		public Guid FeatureID
		{
			get { return _FeatureID; }
			set { _FeatureID = value; }
		}
		public SecurityObject() {
		}

		public override void AssignData(IDataReader dr) { 
			base.AssignData(dr);
			_ID = GetValue<Guid>(dr, "ID");
			_SolutionID = GetValue<Guid>(dr, "SolutionID");
			_ApplicationID = GetValue<Guid>(dr, "ApplicationID");
			_DisplayName = GetValue<string>(dr, "DisplayName");
			_ParentID = GetValue<Guid>(dr, "ParentID");
			_ObjectGroupID = GetValue<Guid>(dr, "ObjectGroupID");
			_Description = GetValue<string>(dr, "Description");
			_PermissionsConfigurable = GetValue<int>(dr, "PermissionsConfigurable");
			_ObjectType = GetValue<int>(dr, "ObjectType");
			UserID  = GetValue<Guid>(dr, "UserID");
			FeatureID = GetValue<Guid>(dr, "FeatureID");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		
		}

		#region Make Root

		public void MakeRoot() {
			_ID = Guid.Empty;
			_SolutionID = Guid.Empty;
			_ApplicationID = Guid.Empty;
			_DisplayName = string.Empty;
			_ParentID = Guid.Empty;
			_ObjectGroupID = Guid.Empty;
		}

		#endregion

		#region IComparable<SecurityObject> Members

		public int CompareTo(SecurityObject other) {
			if (other == null)
				return 1;

			return string.CompareOrdinal(_DisplayName, other.DisplayName);
		}

		#endregion

		public override string ToString()
		{
			return string.Format("SecO({0}; {1})", _DisplayName, _ID.ToString("B"));
		}
	}
}
