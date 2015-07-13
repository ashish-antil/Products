using System;
using System.ComponentModel.Design;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using Imarda.Lib;
using Imarda.Logging;

//using ImardaAttributingBusiness;

namespace FernBusinessBase
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class AttributeValue : FullBusinessEntity
    {
        #region Properties

        [DataMember] public Guid AttributeID;

        [DataMember] public Guid EntityID;

        [DataMember] public string Value;

        [DataMember] public string PrevValue;

        [DataMember] public DateTime PrevDateModified;

        // Properties from AttributeDefinition

        [DataMember] public string VarName;

        [DataMember] public string FriendlyName;

        [DataMember] public string Description;

        [DataMember] public Guid GroupID;

        [DataMember] public string VarType;

        [DataMember] public string Format;

        [DataMember] public bool CaptureHistory;

        [DataMember] public bool Viewable;

        [DataMember] public Guid EntityTypeID;

        [DataMember] public string EntityTypeName;

        [DataMember]
        public bool Added = false;
        [DataMember]
        public bool Changed = false;

        #endregion


        #region Methods

        public override void AssignData(IDataReader dr)
        {
            base.AssignData(dr);
            AttributeID = GetValue<Guid>(dr, "AttributeID");
            EntityID = GetValue<Guid>(dr, "EntityID");
            Value = GetValue<string>(dr, "Value");
            PrevValue = GetValue<string>(dr, "PrevValue");
            PrevDateModified = GetDateTime(dr, "PrevDateModified");
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

        public T GetValue<T>()
        {
            try
            {
                if (string.IsNullOrEmpty(Value)) return default(T);
                object valueAsObject = Value;
                if (valueAsObject is T) return (T)valueAsObject;

                if (typeof(T) == typeof(bool))
                {
                    if ("0".Equals(valueAsObject))
                    {
                        valueAsObject = false;
                    }
                    else if ("1".Equals(valueAsObject))
                    {
                        valueAsObject = true;
                    }
                }

                if (typeof(T) == typeof(DateTime))
                {
                    var dateTime = ParseDateTime((string)valueAsObject);
                    if (dateTime != null) valueAsObject = dateTime.Value;
                }

                if (typeof(IMeasurement).IsAssignableFrom(typeof(T)))
                {
                    return (T)Measurement.Parse(typeof(T), valueAsObject.ToString());
                }

                if (valueAsObject is IConvertible)
                {
                    return (T)Convert.ChangeType(valueAsObject, typeof(T));
                }
                return default(T);
            }
            catch (Exception ex)
            {
                DebugLog.Write(ex);
                return default(T);
            }
        }

        public static DateTime? ParseDateTime(string s)
        {
            DateTime dt;
            return DateTime.TryParseExact(s, "s", null, DateTimeStyles.None, out dt)
                       ? (DateTime?)DateTime.SpecifyKind(dt, DateTimeKind.Utc)
                       : null;
        }

        #endregion
    }

}
