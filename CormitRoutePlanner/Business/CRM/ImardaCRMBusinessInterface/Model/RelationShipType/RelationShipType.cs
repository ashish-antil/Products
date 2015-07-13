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
    public class RelationShipType : FullBusinessEntity
    {
        #region Constructor
        public RelationShipType()
        {
        }
        #endregion

        [DataMember]
        public int Type { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }




#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

        public override void AssignData(IDataReader dr)
        {
            base.AssignData(dr);
            Type = DatabaseSafeCast.Cast<int>(dr["Type"]);
            Name = DatabaseSafeCast.Cast<string>(dr["Name"]);
            Description = DatabaseSafeCast.Cast<string>(dr["Description"]);

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
        }

    }
}