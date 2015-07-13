using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Imarda360Base
{
	/// <summary>
	/// Encapsulates a response from the server with a single item.
	/// </summary>
	/// <typeparam name="T">The type of the item that's returned.</typeparam>
	[MessageContract]
	public class SolutionLoginResponse : SolutionMessageResponse  
	{
        //[MessageBodyMember]
        //public SessionObject CurrentSessionObject { get; set; }

        //public override object Payload
        //{
        //    get { return CurrentSessionObject; }
        //}
	}
}
