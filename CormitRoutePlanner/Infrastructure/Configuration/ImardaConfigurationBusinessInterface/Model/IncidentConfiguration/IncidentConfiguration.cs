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
	public class IncidentConfiguration : FullBusinessEntity
	{
		#region Constructor
		public IncidentConfiguration()
		{
		}
		#endregion

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public byte DeviceType { get; set; }

		[DataMember]
		public string Filename { get; set; }

		[DataMember]
		public Guid FileID { get; set; }

		[DataMember]
		public DateTime FileCreationDate { get; set; }

		[DataMember]
		public DateTime FileModificationDate { get; set; }

#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			Name = DatabaseSafeCast.Cast<string>(dr["Name"]);
			Description = DatabaseSafeCast.Cast<string>(dr["Description"]);
			DeviceType = DatabaseSafeCast.Cast<byte>(dr["DeviceType"]);
			Filename = DatabaseSafeCast.Cast<string>(dr["Filename"]);
			FileID = DatabaseSafeCast.Cast<Guid>(dr["FileID"]);

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif

			FileCreationDate = BusinessBase.ReadyDateForTransport(DatabaseSafeCast.Cast<DateTime>(dr["FileCreationDate"]));
			FileModificationDate = BusinessBase.ReadyDateForTransport(DatabaseSafeCast.Cast<DateTime>(dr["FileModificationDate"]));

#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
		}

	}
}