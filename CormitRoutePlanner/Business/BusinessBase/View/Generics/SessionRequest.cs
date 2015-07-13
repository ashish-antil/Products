using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imarda.Lib;
using ImardaBusinessBase;

namespace FernBusinessBase
{
	/// <summary>
	/// Encapsulates a request to perform some operation, whose function or target will
	/// differ based on the given ID (eg. get an object with the given ID)
	/// </summary>
	[MessageContract]
	[Serializable]
	public class SessionRequest : IRequestBase
	{
		private Guid _ApplicationID;

		private Guid _SessionID;
		private string _Username;
		private string _Password;
		private Guid _SecurityEntityID; // for impersonation
		private LoginMode _Mode;
		private string _HostIPAddress;
	    private int _EntityType;
		private string _AccessToken;

		[MessageBodyMember]
		public Guid ApplicationID
		{
			get { return _ApplicationID; }
			set { _ApplicationID = value; }
		}

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

		[MessageBodyMember]
		public Guid SecurityEntityID
		{
			get { return _SecurityEntityID; }
			set { _SecurityEntityID = value; }
		}

		/// <summary>
		/// 1 = Relogin
		/// 2 = IAC access
		/// 18 = Create iPhone Unit if not exists (for iPhone login)
		/// </summary>
		[MessageBodyMember]
		public LoginMode Mode
		{
			get { return _Mode; }
			set { _Mode = value; }
		}

        [MessageBodyMember]
        public int EntityType
        {
            get { return _EntityType; }
            set { _EntityType = value; }
        }

		[MessageBodyMember]
		public string HostIPAddress
		{
			get { return _HostIPAddress; }
			set { _HostIPAddress = value; }
		}

		[MessageBodyMember]
		public string AccessToken
		{
			get { return _AccessToken; }
			set { _AccessToken = value; }
		}

		public SessionRequest()
		{
		}

		public SessionRequest(Guid sessionID, string username, string password, int entityType = 0)
		{
			_SessionID = sessionID;
			_Username = username;
			_Password = password;
		    _EntityType = entityType;
		}

		public object SID
		{
			set { _SessionID = (Guid)value; }
		}

		/// <summary>
		/// Local use only. Not transported across services.
		/// </summary>
		public object DebugInfo { get; set; }


	    public override string ToString()
		{
			return string.Format("SessionReq({0} {1})", _SessionID.ShortString(), _Username);
		}
	}
}
