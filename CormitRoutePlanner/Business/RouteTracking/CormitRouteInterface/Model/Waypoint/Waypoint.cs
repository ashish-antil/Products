//& IM-3558
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace Cormit.Business.RouteTracking
{
	[DataContract]
	public class Waypoint : FullBusinessEntity
	{
		public Waypoint()
		{
		}

		[ValidNonEmpty]
		[DataMember]
		public Guid RouteID { get; set; }

		[ValidInt(0, int.MaxValue)]
		[DataMember]
		public int SeqNum { get; set; }

		[ValidNonEmpty]
		[DataMember]
		public Guid CompanyGeofenceID { get; set; }

		public const int ExternalIDMaxLen = 50;
		[ValidLength(-1, ExternalIDMaxLen)]
		[DataMember]
		public string ExternalID { get; set; }

		public const int NameMaxLen = 50;
		[ValidLength(-1, NameMaxLen)]
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public bool Hidden { get; set; }

		[DataMember]
		public int CustomFlags { get; set; }
#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			RouteID = GetValue<Guid>(dr, "RouteID");
			SeqNum = GetValue<int>(dr, "SeqNum");
			CompanyGeofenceID = GetValue<Guid>(dr, "CompanyGeofenceID");
			ExternalID = GetValue<string>(dr, "ExternalID");
			Name = GetValue<string>(dr, "Name");
			Hidden = GetValue<bool>(dr, "Hidden");
			CustomFlags = GetValue<int>(dr, "CustomFlags");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		}

		public override string ToString()
		{
			return string.Format("Waypoint({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", ID, RouteID, SeqNum, CompanyGeofenceID, ExternalID, Name, Hidden, CustomFlags);
		}

	}
}