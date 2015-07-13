using System;
using System.Collections.Generic;
using System.Text;

namespace FernBusinessBase
{
	public class BusinessProxy<TChannel> : System.ServiceModel.ClientBase<TChannel> where TChannel : class
	{

		public static TChannel Proxy
		{
			get
			{
				return new BusinessProxy<TChannel>().Channel;
			}
		}

		private BusinessProxy() : base() { }
	}
}
