using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	/// <summary>
	/// Encapsulates a request to get number of objects that
	/// share some ID and have been updated since the time shown in
	/// TimeStamp.
	/// </summary>
	[MessageContract]
	[Serializable]
	public class GetUpdateCountRequest : IDRequest
	{
		private string _TypeName;
		private DateTime _TimeStamp;
		private Guid _LastRecordID;
		private int _Priority;

		[MessageBodyMember]
		public string TypeName
		{
			get { return _TypeName; }
			set { _TypeName = value; }
		}

		[MessageBodyMember]
		public Guid LastRecordID
		{
			get { return _LastRecordID; }
			set { _LastRecordID = value; }
		}

		[MessageBodyMember]
		public DateTime TimeStamp
		{
			get { return _TimeStamp; }
			set { _TimeStamp = value; }
		}

		[MessageBodyMember]
		public int Priority
		{
			get { return _Priority; }
			set { _Priority = value; }
		}

	}
}
