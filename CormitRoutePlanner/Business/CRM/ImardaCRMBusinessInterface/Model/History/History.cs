using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaCRMBusiness
{
	[DataContract]
	public class History : FullBusinessEntity
	{
		#region Constructor
		public History()
		{
		}
		#endregion

		[DataMember]
		public DateTime Date { get; set; }

		[DataMember]
		public string Subject { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public int EventType { get; set; }

		[DataMember]
		public int OwnerType { get; set; }

		[DataMember]
		public Guid OwnerID { get; set; }

		[DataMember]
		public string UserName { get; set; }


		[DataMember]
		public Guid ContactID { get; set; }

		[DataMember]
		public Guid EmployeeID { get; set; }

		[DataMember]
		public Guid JobID { get; set; }

		[DataMember]
		public Guid TaskID { get; set; }

		[DataMember]
		public int TaskType { get; set; }

		[DataMember]
		public Guid AttachmentID { get; set; }

		[DataMember]
		public bool Completed { get; set; }

		[DataMember]
		public DateTime CompleteDate { get; set; }




#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			Date = BusinessBase.ReadyDateForTransport(DatabaseSafeCast.Cast<DateTime>(dr["Date"]));
			Subject = DatabaseSafeCast.Cast<string>(dr["Subject"]);
			Description = DatabaseSafeCast.Cast<string>(dr["Description"]);
			EventType = DatabaseSafeCast.Cast<int>(dr["EventType"]);
			OwnerType = DatabaseSafeCast.Cast<int>(dr["OwnerType"]);
			OwnerID = DatabaseSafeCast.Cast<Guid>(dr["OwnerID"]);

			ContactID = DatabaseSafeCast.Cast<Guid>(dr["ContactID"]);
			EmployeeID = DatabaseSafeCast.Cast<Guid>(dr["EmployeeID"]);
			JobID = DatabaseSafeCast.Cast<Guid>(dr["JobID"]);
			TaskID = DatabaseSafeCast.Cast<Guid>(dr["TaskID"]);
			TaskType = DatabaseSafeCast.Cast<int>(dr["TaskType"]);

			AttachmentID = DatabaseSafeCast.Cast<Guid>(dr["AttachmentID"]);
			Completed = DatabaseSafeCast.Cast<bool>(dr["Completed"]);
			CompleteDate = BusinessBase.ReadyDateForTransport(DatabaseSafeCast.Cast<DateTime>(dr["CompleteDate"]));

			if (HasColumn(dr, "UserName"))
				UserName = DatabaseSafeCast.Cast<string>(dr["UserName"]);

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
		}

	}
}