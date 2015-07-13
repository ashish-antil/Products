using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FernBusinessBase;

namespace Cormit.Business.RouteTracking
{
    [DataContract]
    public class RouteWaypoint : FullBusinessEntity 
    {
        [DataMember]
        public Guid RouteId { get; set; }

        [DataMember]
        public Guid WayPointId { get; set; }

        public override void AssignData(IDataReader dr)
        {
            base.AssignData(dr);
            ID = GetValue<Guid>(dr, "ID");
            RouteId = GetValue<Guid>(dr, "RouteId");
            WayPointId = GetValue<Guid>(dr, "WayPointId");
        }
    }
}
