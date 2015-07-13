using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FernBusinessBase;

namespace ImardaAttributingBusiness
{
     [DataContract]
    public class AttributeHistory : FullBusinessEntity
    {
        #region Properties

        [DataMember]
        public Guid AttributeID;

        [DataMember]
        public Guid EntityID;

        [DataMember]
        public string Value;

        // Properties from AttributeDefinition

        [DataMember]
        public string VarName;

        [DataMember]
        public string Description;

        [DataMember]
        public Guid GroupID;

        [DataMember]
        public string VarType;

        [DataMember]
        public string Format;

        [DataMember]
        public bool CaptureHistory;

        [DataMember]
        public bool Viewable;

        [DataMember]
        public Guid EntityTypeID;

        #endregion

    }
}
