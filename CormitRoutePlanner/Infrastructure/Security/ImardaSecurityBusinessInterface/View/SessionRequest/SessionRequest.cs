using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	/// <summary>
	/// Encapsulates a request to perform some operation, whose function or target will
	/// differ based on the given ID (eg. get an object with the given ID)
	/// </summary>
	[MessageContract]
	[Serializable]
	public class SessionRequest
	{
		private Guid _SessionID;
		private string _Username;
		private string _Password;

		[MessageBodyMember]
		public Guid SessionID
		{
			get { return _SessionID; }
			set { _SessionID = value; }
		}

		[MessageBodyMember]
		public string Username
		{
			get { return _Username; }
			set { _Username = value; }
		}

		[MessageBodyMember]
		public string Password
		{
			get { return _Password; }
			set { _Password = value; }
		}

		public SessionRequest()
		{
		}

		public SessionRequest(Guid sessionID, string username, string password)
		{
			_SessionID = sessionID;
			_Username = username;
			_Password = password;
		}
	}
}
