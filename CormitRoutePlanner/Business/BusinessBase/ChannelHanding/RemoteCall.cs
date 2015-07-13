using System;
using System.ServiceModel;

namespace FernBusinessBase
{
	/*
	 * using (var c = Imarda360GatewayProxy.Instance.Configuration)
	 * {
	 *	  var resp = c.Call.GetConfig(new ConfigRequest(x, null));
	 *	  c.Call.UpdateConfig(..........);
	 * }
	 * 
	 */


	public class RemoteCall<T> : IDisposable 
	{
		private Proxy<T> _Proxy;
		private IClientChannel _Channel;

		public RemoteCall(Proxy<T> proxy)
		{
			_Proxy = proxy;
		}

		public T Call
		{
			get
			{
				T service = _Proxy.GetChannel();
				_Channel = service as IClientChannel;
				return service;
			}
		}
		

		public void Dispose()
		{
			try
			{
				if (_Channel != null)
				{
					if (_Channel.State == CommunicationState.Faulted)
					{
						_Channel.Abort();
					}
					else if (_Channel.State != CommunicationState.Closed && _Channel.State != CommunicationState.Closing)
					{
						_Channel.Close();
					}
					_Channel.Dispose();
					_Channel = null;
					_Proxy = null;
				}
			}
			catch {}
		}
	}
}