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
	public class UsageLog : FullBusinessEntity
	{
		#region Constructor
		public UsageLog()
		{
		}
		#endregion

		[DataMember]
		public Guid ApiID { get; set; }

		[DataMember]
		public string ApiName { get; set; }

		[DataMember]
		public string ApiDescription { get; set; }	

		[DataMember]
		public string ApiVersion { get; set; }

		[DataMember]
		public string Method { get; set; }
		
		[DataMember]
		public int ExecutionTime { get; set; }

		[DataMember]
		public int Count { get; set; }

		[DataMember]
		public string Company { get; set; }

#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			ApiID = DatabaseSafeCast.Cast<Guid>(dr["ApiID"]);
			if (HasColumn(dr, "Name"))
				ApiName = DatabaseSafeCast.Cast<string>(dr["Name"]);
			if (HasColumn(dr, "Description"))
				ApiDescription = DatabaseSafeCast.Cast<string>(dr["Description"]);
			if (HasColumn(dr, "ApiVersion"))
				ApiVersion = DatabaseSafeCast.Cast<string>(dr["ApiVersion"]);
			if (HasColumn(dr, "Method"))
				Method = DatabaseSafeCast.Cast<string>(dr["Method"]);
			if (HasColumn(dr, "ExecutionTime"))
				ExecutionTime = DatabaseSafeCast.Cast<int>(dr["ExecutionTime"]);
			if (HasColumn(dr, "Count"))
				Count = DatabaseSafeCast.Cast<int>(dr["Count"]);
			if (HasColumn(dr, "Company"))
				Company = DatabaseSafeCast.Cast<string>(dr["Company"]);


#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
		}

	}

	public enum APINumbers
	{
		CompanyApi = 0,
		UserApi = 1,
		DeviceAPI = 2,
		VehicleAPI = 3,
		FleetAPI = 4,
		DriverAPI = 5,
		LocationAPI = 6,
		JobAPI = 7,
		FatigueAPI = 8,
		MessageAPI = 9,
		TrackingAPI = 10
	}


}