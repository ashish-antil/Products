using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Reflection;

namespace FernBusinessBase
{
	/// <summary>
	/// Encapsulates a request to save a given string.
	/// </summary>
	/// <typeparam name="T">The content that needs to be saved.</typeparam>
	[MessageContract]
	public class SaveStringRequest : ParameterMessageBase
	{
		private string _Content;

		public SaveStringRequest()
		{
		}

		public SaveStringRequest(string content)
		{
			Content = content;
		}

		[MessageBodyMember]
		public string Content
		{
			get { return _Content; }
			set { _Content = value; }
		}


	}
}
