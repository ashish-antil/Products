using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace ImardaSecurityBusiness {

	[MessageContract]
	public class SaveApplicationRequest {
		private Application _Application;

		[MessageBodyMember]
		public Application Application {
			get { return _Application; }
			set { _Application = value; }
		}
	}
}
