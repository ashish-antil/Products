using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace FernBusinessBase {
	/// <summary>
	/// Encapsulates a request to the business layer to delete all
	/// Maps of the given Type with the given EntityID, and then save all given
	/// Maps to the database.
	/// </summary>
	[MessageContract]
	public class SetMapsForEntityRequest<TMap> where TMap : BusinessEntity {

		private Guid _OwnerID;
		private List<TMap> _Maps;

		public SetMapsForEntityRequest() {
			_Maps = new List<TMap>();
		}

		[MessageBodyMember]
		public List<TMap> Maps {
			get { return _Maps; }
			set { _Maps = value; }
		}

		[MessageBodyMember]
		public Guid OwnerID {
			get { return _OwnerID; }
			set { _OwnerID = value; }
		}

	}
}
