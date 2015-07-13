using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	[MessageContract]
	public class AddRemoveRequest : IRequestBase
	{
		public AddRemoveRequest() { }

		public AddRemoveRequest(Guid id, List<Guid> add, List<Guid> delete)
		{
			ID = id;
			ToBeAdded = add;
			ToBeDeleted = delete;
		}

		[MessageBodyMember]
		public Guid CompanyID { get; set; }

		[MessageBodyMember]
		public Guid UserID { get; set; }


		[MessageBodyMember]
		public Guid ID { get; set; }

		[MessageBodyMember]
		public List<Guid> ToBeAdded { get; set; }

		[MessageBodyMember]
		public List<Guid> ToBeDeleted { get; set; }
		
		[MessageBodyMember]
		public Guid SessionID { get; set; }

		public object SID
		{
			set
			{
				if (value is string) SessionID = new Guid((string)value);
				else if (value is Guid) SessionID = (Guid)value;
				else SessionID = new Guid(value.ToString());
			}
		}

		/// <summary>
		/// Local use only. Not transported across services.
		/// </summary>
		public object DebugInfo { get; set; }

	}
}
