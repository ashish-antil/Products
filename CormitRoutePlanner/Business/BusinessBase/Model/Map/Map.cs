using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using FernBusinessBase;
using System.Runtime.Serialization;
using FernBusinessBase.Control;

namespace FernBusinessBase {
	[DataContract]
	public class Map : BusinessEntity {
		private Guid _RelationID;
		private int _RelationType;

		[DataMember]
		public Guid RelationID {
			get { return _RelationID; }
			set { _RelationID = value; }
		}

		[DataMember]
		public int RelationType
		{
			get { return _RelationType; }
			set { _RelationType = value; }
		}

		public override void AssignData(IDataReader dr) { 
			base.AssignData(dr);
			int i = dr.GetOrdinal("RelationID");
			if (i >= 0)
			{
				RelationID = (Guid)dr[i];
			}

			i = dr.GetOrdinal("RelationType");
			if (i >= 0)
			{
				RelationType = DatabaseSafeCast.Cast<int>(dr[i]);
			}
		}
	}
}
