using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;

namespace ImardaSecurityBusiness {
	[DataContract]
	[Serializable]
	public class SecurityEntityParent : BusinessEntity {

		#region Instance Variables
		private Guid _RelationId;
		private Guid _EntityId;
		private Guid _ParentId;
		#endregion

		[DataMember]
		public Guid RelationId {
			get { return _RelationId; }
			set { _RelationId = value; }
		}

		[DataMember]
		public Guid EntityId {
			get { return _EntityId; }
			set { _EntityId = value; }
		}

		[DataMember]
		public Guid ParentId {
			get { return _ParentId; }
			set { _ParentId = value; }
		}

		public SecurityEntityParent() {
		}

		public override void AssignData(IDataReader dr) { 
			base.AssignData(dr);

			_RelationId = (Guid)dr["RelationId"];
			_EntityId = (Guid)dr["EntityId"];
			_ParentId = (Guid)dr["ParentId"];
		}
	}
}
