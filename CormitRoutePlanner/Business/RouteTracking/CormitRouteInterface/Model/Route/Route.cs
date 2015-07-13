//& IM-3558
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;
using Imarda.Lib;

namespace Cormit.Business.RouteTracking 
{
	[DataContract]
	public class Route : FullBusinessEntity 
	{				
		public Route()
		{
			_Waypoints = new List<Waypoint>();
		}

		public const int ExternalIDMaxLen = 50;
		[ValidLength(-1, ExternalIDMaxLen)]
		[DataMember]
		public string ExternalID { get; set; }

		public const int NameMaxLen = 50;
		[ValidLength(-1, NameMaxLen)]
		[DataMember]
		public string Name { get; set; }

		public const int DescriptionMaxLen = 150;
		[ValidLength(-1, DescriptionMaxLen)]
		[DataMember]
		public string Description { get; set; }

		[DataMember] 
		public bool Temporary { get; set; }

		[DataMember] 
		public bool UnderConstruction { get; set; }


		#region transient waypoint list

		private List<Waypoint> _Waypoints;

		public List<Waypoint> Waypoints
		{
			get
			{
				return _Waypoints ?? (_Waypoints = new List<Waypoint>());
			}
		}

		public Waypoint FirstWaypoint
		{
			get
			{
				return _Waypoints != null && _Waypoints.Count > 0 ? _Waypoints.Find(wp => wp.Active == true && wp.Deleted == false && wp.SeqNum == 1)  : null;
			}
		}

        public Waypoint SecondWaypoint
        {
            get
            {
                return _Waypoints != null && _Waypoints.Count > 0 ? _Waypoints.Find(wp => wp.Active == true && wp.Deleted == false && wp.SeqNum == 2) : null;
            }
        }

		public Waypoint LastWaypoint
		{
			get
			{
				return _Waypoints != null && _Waypoints.Count > 0 ? _Waypoints[Waypoints.Count - 1] : null;
			}
		}
		#endregion transient waypoint list



#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			ExternalID = GetValue<string>(dr, "ExternalID");
			Name = GetValue<string>(dr, "Name");
			Description = GetValue<string>(dr, "Description");
			Temporary = GetValue<bool>(dr, "Temporary");
			UnderConstruction = GetValue<bool>(dr, "UnderConstruction");

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		}

		public override string ToString()
		{
			return string.Format("Route({0}, {1}, {2}, {3}, {4}, {5})", ID, ExternalID, Name, Description, Temporary, UnderConstruction);
		}

		
	}
}