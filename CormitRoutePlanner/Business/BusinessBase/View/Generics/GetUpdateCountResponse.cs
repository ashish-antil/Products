using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	/// <summary>
	/// Encapsulates a response to get number of objects that
	/// share some ID and have been updated since the time shown in
	/// TimeStamp.
	/// </summary>
	[MessageContract]
	[Serializable]
	public class GetUpdateCountResponse : BusinessMessageResponse
	{
		private string _TypeName;
		private int _Count;
		private int _Priority;

		public GetUpdateCountResponse()
			: base()
		{
		}

		public GetUpdateCountResponse(string name, int count, int priority)
			: base()
		{
			_TypeName = name;
			_Count = count;
			_Priority = priority;
		}

		[MessageBodyMember]
		public string TypeName
		{
			get { return _TypeName; }
			set { _TypeName = value; }
		}

		[MessageBodyMember]
		public int Count
		{
			get { return _Count; }
			set { _Count = value; }
		}

		[MessageBodyMember]
		public int Priority
		{
			get { return _Priority; }
			set { _Priority = value; }
		}

		public override object Payload
		{
			get { return _Count; }
		}

	}
}
