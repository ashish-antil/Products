using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	/// <summary>
	/// Encapsulates a request to reset password
	/// </summary>
	[MessageContract]
	[Serializable]
	public class ResetPasswordRequest : IRequestBase
	{
		private string _Username;
		private string _Email;

		public ResetPasswordRequest()
		{
		}

		public ResetPasswordRequest(string username, string email)
		{
			_Username = username;
			_Email = email;
		}

		[MessageBodyMember]
		public Guid SessionID { get; set; }

		public object SID
		{
			set { SessionID = new Guid(value.ToString()); }
		}

		[MessageBodyMember]
		public string Username
		{
			get { return _Username; }
			set { _Username = value; }
		}

		[MessageBodyMember]
		public string Email
		{
			get { return _Email; }
			set { _Email = value; }
		}

		/// <summary>
		/// Local use only. Not transported across services.
		/// </summary>
		public object DebugInfo { get; set; }

	}
}
