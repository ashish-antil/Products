using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaSecurityBusiness
{
	[DataContract]
	[Serializable]
	public class SecurityEntity : FullBusinessEntity, IComparable<SecurityEntity>
	{
		#region Properties

		public const int EntityNameMaxLen = 50;
		[ValidLength(0, EntityNameMaxLen)]
		[DataMember]
		public string EntityName
		{ get; set; }

		[DataMember]
		public int EntityType
		{ get; set; }

		[DataMember]
		public bool LoginEnabled
		{ get; set; }

		public const int LoginUsernameMaxLen = 50;
		[ValidLength(-1, LoginUsernameMaxLen)]
		[DataMember]
		public string LoginUsername
		{ get; set; }

		public const int LoginPasswordMaxLen = 100;
		[ValidLength(0, LoginPasswordMaxLen)]
		[DataMember]
		public string LoginPassword
		{ get; set; }

		[DataMember]
		public Guid CRMId
		{ get; set; }

		[DataMember]
		public string Description
		{ get; set; }

		[DataMember]
		public Guid BranchID
		{ get; set; }

		[DataMember]
		public DateTime LastLogonDate
		{ get; set; }

		[DataMember]
		[DisplayFormat(null)]
		public DateTime LastLogonDateDisplay
		{
			get { return LastLogonDate; }
			set { }
		}

		/// <summary>
		/// Contains a list of the ids of the immediate parents of this entity.
		/// This is the way to add/remove child and parent relationships.
		/// </summary>
		[DataMember]
		public List<Guid> ImmediateParentsIds
		{ get; set; }

		/// <summary>
		/// Contains a list of the securityPermissions (SecurityObjectID - PermissionGranted)
		/// </summary>
		[DataMember]
		public List<SecurityPermission> PermissionList
		{ get; set; }

		/// <summary>
		/// e.g. "New Zealand Standard Time", as found in registry
		/// </summary>
		public const int TimeZoneMaxLen = 40;
		[ValidLength(-1, TimeZoneMaxLen)]
		[DataMember]
		public string TimeZone
		{ get; set; }

		/// <summary>
		/// e.g. "en-US"
		/// </summary>
		public const int LocaleMaxLen = 10;
		[ValidLength(-1, LocaleMaxLen)]
		[DataMember]
		public string Locale
		{ get; set; }
		[DataMember]
		public Guid PreferredUnitSystemID
		{ get; set; }
		[DataMember]
		public bool Template
		{ get; set; }

		//Optional Props
		[DataMember]
		public bool EnableTimeZoneSelect
		{ get; set; }
		[DataMember]
		public bool IsAdmin { get; set; }
		[DataMember]
		public Guid Salt { get; set; }
#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif


		#endregion

		public SecurityEntity()
		{
			EnableTimeZoneSelect = false;
			//_ImmediateParentsIds = new List<Guid>();
			//_PermissionList = new List<SecurityPermission>();
		}

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);

			EntityName = GetValue<string>(dr, "EntityName");
			EntityType = GetValue<int>(dr, "EntityType");
			LoginEnabled = GetValue<bool>(dr, "LoginEnabled");
			LoginUsername = GetValue<string>(dr, "LoginUsername");
			LoginPassword = GetValue<string>(dr, "LoginPassword");
			CRMId = GetValue<Guid>(dr, "CRMId");
			Description = GetValue<string>(dr, "Description");
			BranchID = GetValue<Guid>(dr, "BranchID");
			LastLogonDate = GetDateTime(dr, "LastLogonDate");
			TimeZone = GetValue<string>(dr, "TimeZone");
			Locale = GetValue<string>(dr, "Locale");
			EnableTimeZoneSelect = GetValue<bool>(dr, "EnableTimeZoneSelect");
			IsAdmin = GetValue<bool>(dr, "IsAdmin");
			Salt = GetValue<Guid>(dr, "Salt");
			if (HasColumn(dr, "PreferredUnitSystemID"))
			{
				PreferredUnitSystemID = GetValue<Guid>(dr, "PreferredUnitSystemID");
			}

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif

		}

		#region Make Root

		/// <summary>
		/// Fills in the entity's properties with values to represent the imaginary root
		/// element.
		/// </summary>
		public void MakeRoot()
		{
			this.ID = Guid.Empty;
			EntityName = "Root";
			EntityType = 0;
			LoginEnabled = false; ;
			LoginUsername = string.Empty;
			LoginPassword = string.Empty;
			CRMId = Guid.Empty; ;
			ImmediateParentsIds = new List<Guid>();
			PermissionList = new List<SecurityPermission>();
			TimeZone = "UTC";
			Locale = "en";
		}

		#endregion

		#region IComparable<SecurityEntity> Members

		public int CompareTo(SecurityEntity other)
		{
			return other == null ? 1 : string.CompareOrdinal(EntityName, other.EntityName);
		}

		#endregion
	}
}
