using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness
{
	[MessageContract]
	public class GetApplicationListResponse : BusinessMessageResponse
	{
		private List<Application> _ApplicationList;

		[MessageBodyMember]
		public List<Application> ApplicationList
		{
			get { return _ApplicationList; }
			set { _ApplicationList = value; }
		}
	}
}
