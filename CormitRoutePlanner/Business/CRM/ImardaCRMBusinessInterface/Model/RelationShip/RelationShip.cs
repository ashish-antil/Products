using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;
using Imarda.Lib;

namespace ImardaCRMBusiness
{
    public enum RelationShipTypeEnum : byte
    {
        Manages = 0,
        SuppliesTo = 1
    }

    public enum SubjectTypeEnum : byte
    {
        Company = 0,
        Person = 1
    }


    [DataContract]
    public class RelationShip : FullBusinessEntity
    {
        #region Constructor
        public RelationShip()
        {
        }
        #endregion

        public static RelationShip NewManageRelationShip(Company company)
        {
            var rel = new RelationShip();
            rel.ID = SequentialGuid.NewDbGuid();
            rel.Active = true;
            rel.Deleted = false;
            rel.DateCreated = DateTime.UtcNow;
            rel.DateModified = DateTime.UtcNow;
            rel.CompanyID = company.CompanyID;
            rel.UserID = company.UserID;
            rel.Type = (int)RelationShipTypeEnum.Manages;
            rel.SubjectType = (int)SubjectTypeEnum.Company;
            rel.SubjectID = company.CompanyID;
            rel.ObjectType = (int)SubjectTypeEnum.Company;
            rel.ObjectID = company.ID;
            return rel;
        }

        [DataMember]
        public int Type { get; set; }

        [DataMember]
        public Guid TypeID { get; set; }

        [DataMember]
        public int SubjectType { get; set; }

        [DataMember]
        public Guid SubjectID { get; set; }

        [DataMember]
        public int ObjectType { get; set; }

        [DataMember]
        public Guid ObjectID { get; set; }




#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

        public override void AssignData(IDataReader dr)
        {
            base.AssignData(dr);
            TypeID = DatabaseSafeCast.Cast<Guid>(dr["TypeID"]);
            Type = DatabaseSafeCast.Cast<int>(dr["Type"]);
            SubjectType = DatabaseSafeCast.Cast<int>(dr["SubjectType"]);
            SubjectID = DatabaseSafeCast.Cast<Guid>(dr["SubjectID"]);
            ObjectType = DatabaseSafeCast.Cast<int>(dr["ObjectType"]);
            ObjectID = DatabaseSafeCast.Cast<Guid>(dr["ObjectID"]);

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr,"`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`"));
#endif
        }

    }
}