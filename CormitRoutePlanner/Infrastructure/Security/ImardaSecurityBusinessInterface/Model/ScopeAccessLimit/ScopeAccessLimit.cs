using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaSecurityBusiness
{
	[DataContract]
	public class ScopeAccessLimit : FullBusinessEntity
	{
		#region Constructor
		public ScopeAccessLimit()
		{
		}
		#endregion

		[DataMember]
		public Guid ScopeID { get; set; }

		[DataMember]
		public string ScopeName { get; set; }

		[DataMember]
		public Guid AssignedToID { get; set; }

		[DataMember]
		public byte AssignedToType { get; set; }

		[DataMember]
		public int Limit { get; set; }

		[DataMember]
		public int PerTimeSpan { get; set; }


#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			ScopeID = DatabaseSafeCast.Cast<Guid>(dr["ScopeID"]);
			AssignedToID = DatabaseSafeCast.Cast<Guid>(dr["AssignedToID"]);
			AssignedToType = DatabaseSafeCast.Cast<byte>(dr["AssignedToType"]);
			Limit = DatabaseSafeCast.Cast<int>(dr["Limit"]);
			PerTimeSpan = DatabaseSafeCast.Cast<int>(dr["PerTimeSpan"]);
			if (HasColumn(dr, "ScopeName"))
				ScopeName = DatabaseSafeCast.Cast<string>(dr["ScopeName"]);
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
		}

	}
}