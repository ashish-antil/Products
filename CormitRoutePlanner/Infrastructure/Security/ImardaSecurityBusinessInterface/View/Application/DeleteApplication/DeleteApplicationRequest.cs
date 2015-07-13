using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace ImardaSecurityBusiness {
	[MessageContract]
	public class DeleteApplicationRequest {

		private Guid _ID;

		[MessageBodyMember]
		public Guid ID {
			get { return _ID; }
			set { _ID = value; }
		}

	}
}
