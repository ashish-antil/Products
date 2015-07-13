using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaConfigurationBusiness
{
	[DataContract]
	public class Profile : FullBusinessEntity
	{
		#region Constructor
		public Profile()
		{
		}
		#endregion

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public DateTime StartDate { get; set; }

		[DataMember]
		public DateTime ExpiryDate { get; set; }

		[DataMember]
		public int AreaType { get; set; }

		[DataMember]
		public int ProfileType { get; set; }

		[DataMember]
		public bool AllowCustomersToUse { get; set; }

		[DataMember]
		public Guid SettingsLinkID { get; set; }

		[DataMember]
		public string Settings { get; set; }

		[DataMember]
		public string Owner { get; set; }  



#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			Name = DatabaseSafeCast.Cast<string>(dr["Name"]);
			Description = DatabaseSafeCast.Cast<string>(dr["Description"]);
			AreaType = DatabaseSafeCast.Cast<int>(dr["AreaType"]);
			ProfileType = DatabaseSafeCast.Cast<int>(dr["ProfileType"]);
			AllowCustomersToUse = DatabaseSafeCast.Cast<bool>(dr["AllowCustomersToUse"]);
			SettingsLinkID = DatabaseSafeCast.Cast<Guid>(dr["SettingsLinkID"]);
			Settings = DatabaseSafeCast.Cast<string>(dr["Settings"]);
			if (HasColumn(dr, "Owner"))
			{
				Owner = DatabaseSafeCast.Cast<string>(dr["Owner"]);
			}
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif

			StartDate = BusinessBase.ReadyDateForTransport(DatabaseSafeCast.Cast<DateTime>(dr["StartDate"]));
			ExpiryDate = BusinessBase.ReadyDateForTransport(DatabaseSafeCast.Cast<DateTime>(dr["ExpiryDate"]));

#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
		}

	}
}