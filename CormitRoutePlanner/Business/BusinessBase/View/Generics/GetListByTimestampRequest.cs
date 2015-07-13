using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	/// <summary>
	/// Encapsulates a request to get a list of objects that
	/// share some ID and have been updated since the time shown in
	/// TimeStamp.
	/// </summary>
	[MessageContract]
	public class GetListByTimestampRequest : IDRequest
	{
		private int _Cap = 30;
		private Guid _BranchID;
		private Guid _LastRecordID;
		private DateTime _TimeStamp;
		
		[MessageBodyMember]
		public Guid BranchID
		{
			get { return _BranchID; }
			set { _BranchID = value; }
		}

		[MessageBodyMember]
		public Guid LastRecordID
		{
			get { return _LastRecordID; }
			set { _LastRecordID = value; }
		}

		[MessageBodyMember]
		public int Cap
		{
			get { return _Cap; }
			set { _Cap = value; }
		}

		[MessageBodyMember]
		public DateTime TimeStamp
		{
			get { return _TimeStamp; }
			set { _TimeStamp = value; }
		}

		public bool IncludeInactive
		{
			get { return HasSome(RetrievalOptions.IncludeInactive); }
		}

	}
}
