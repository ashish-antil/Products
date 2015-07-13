#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#endregion

namespace FernBusinessBase.Model
{
    [DataContract]
    public class DisplaySettings
    {
        public DisplaySettings(Guid headUnitId, IEnumerable<DisplayOrderItem> displayOrder)
        {
            HeadUnitId = headUnitId;
            DisplayOrder = displayOrder.ToList();
        }

        [DataMember]
        public Guid HeadUnitId { get; private set; } 

        [DataMember]
        public List<DisplayOrderItem> DisplayOrder { get; private set; }
    }

    [DataContract]
    public class DisplayOrderItem
    {
        public DisplayOrderItem(Guid id, bool isAccessible)
        {
            Id = id;
            IsAccessible = isAccessible;
        }

        [DataMember]
        public Guid Id { get; private set; }

        [DataMember]
        public bool IsAccessible { get; private set; }
    }
}