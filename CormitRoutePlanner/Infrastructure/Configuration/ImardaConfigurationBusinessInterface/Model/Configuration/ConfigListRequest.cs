using System;
using System.ServiceModel;

namespace ImardaConfigurationBusiness
{
	[MessageContract]
	public class ConfigListRequest : ConfigRequest
	{
		public ConfigListRequest()
		{
		}

		public ConfigListRequest(Guid[] ids, params Guid[] levels)
			: base(Guid.Empty, null, levels)
		{
			IDs = ids;
		}

		[MessageBodyMember]
		public Guid[] IDs { get; set; }
	}
}
