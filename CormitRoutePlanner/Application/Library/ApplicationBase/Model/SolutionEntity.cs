#region

using System;
using System.Runtime.Serialization;
using FernBusinessBase;

#endregion

// ReSharper disable once CheckNamespace
namespace Imarda360Base
{
    [DataContract]
    [Serializable]
    public class SolutionEntity
    {
        [DataMember]
        public bool Active { get; set; }

        [DataMember]
        public bool Deleted { get; set; }

        // ReSharper disable once InconsistentNaming
        [DataMember]
        public Guid ID { get; set; }

        public virtual string[] Validate(bool all)
        {
            return BaseEntity.Validate(this, all);
        }
    }
}