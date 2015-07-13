using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;
using Imarda.Lib;

namespace ImardaAttributingBusiness
{
    [DataContract]
    public class AttributeValue: FullBusinessEntity
    {
        #region Properties

        [DataMember]
        public Guid AttributeID;

        [DataMember]
        public Guid EntityID;

        [DataMember]
        public string Value;

        [DataMember]
        public string PrevValue;

        [DataMember]
        public DateTime PrevDateModified;

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

        [DataMember]
        public string EntityTypeName;

        #endregion


        #region Methods
        public override void AssignData(IDataReader dr)
        {
            //Attributes = HasColumn(dr, "Attributes") ? new EntityAttributes(GetValue<string>(dr, "Attributes")) : new EntityAttributes(string.Empty);

            base.AssignData(dr);
            AttributeID = GetValue<Guid>(dr, "AttributeID");
            EntityID = GetValue<Guid>(dr, "EntityID");
            Value = GetValue<string>(dr, "Value");
            PrevValue = GetValue<string>(dr, "PrevValue");
            PrevDateModified = GetDateTime(dr, "PrevDateModified");
            VarName = GetValue<string>(dr, "VarName");
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
