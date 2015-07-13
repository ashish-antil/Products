using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase {
	/// <summary>
	/// A request for objects that match a given ID and Date Range in some way.
	/// </summary>
	[MessageContract]
	public class IDAndDateRangeRequest : IDRequest {
		private DateTime _DateFrom = DateTime.MinValue;
		private DateTime _DateTo = DateTime.MaxValue;

		[MessageBodyMember]
		public DateTime DateTo {
			get { return _DateTo; }
			set { _DateTo = value; }
		}

		[MessageBodyMember]
		public DateTime DateFrom {
			get { return _DateFrom; }
			set { _DateFrom = value; }
		}
	}
}
