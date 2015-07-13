using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FernBusinessBase
{
    [DataContract]
    public class AttributeDefinition : FullBusinessEntity
    {

        #region Properties

        [DataMember]
        public string VarName;

        [DataMember]
        public string FriendlyName;

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

        [DataMember]
        public string EntityTypeName;

        #endregion


        #region Methods

        public override void AssignData(IDataReader dr)
        {
            base.AssignData(dr);
            VarName = GetValue<string>(dr, "VarName");
            FriendlyName = GetValue<string>(dr, "FriendlyName");
            Description = GetValue<string>(dr, "Description");
            GroupID = GetValue<Guid>(dr, "GroupID");
            VarType = GetValue<string>(dr, "VarType");
            Format = GetValue<string>(dr, "Format");
            CaptureHistory = GetValue<bool>(dr, "CaptureHistory");
            Viewable = GetValue<bool>(dr, "Viewable");
            EntityTypeID = GetValue<Guid>(dr, "EntityTypeID");
            EntityTypeName = GetValue<string>(dr, "EntityTypeName");
        }
        #endregion
    }


}
