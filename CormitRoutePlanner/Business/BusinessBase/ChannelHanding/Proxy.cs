using System;
using System.ServiceModel;
using FernBusinessBase.Errors;
using FernBusinessBase.Extensions;

namespace FernBusinessBase
{
	public class Proxy<I> : IDisposable
	{
		#region private
		private ChannelFactory<I> _channelFactory = null;
		private string _endpointConfigurationName;

		private void CreateChannelFactory()
		{
			if (_channelFactory == null)
			{
				_channelFactory = new ChannelFactory<I>(_endpointConfigurationName);
				_channelFactory.Faulted += new EventHandler(ChannelFactoryFaulted);
			}
		}

		private void ChannelFactoryFaulted(Object sender, EventArgs e)
		{
			OnFaulted(e);
			DisposeChannelFactory();
		}

		private void DisposeChannelFactory()
		{
			if (_channelFactory != null)
			{
				try
				{
					_channelFactory.Faulted -= new EventHandler(ChannelFactoryFaulted);
                    _channelFactory.SafeClose();
				}
				catch (Exception ex)
				{
					ErrorHandler.HandleInternal(ex);
				}
				_channelFactory = null;
			}
		}

		private void Dispose(bool disposing)
		{
			DisposeChannelFactory();
		}

		private void OnFaulted(EventArgs e)
		{
			if (Faulted != null)
				Faulted(this, e);
		}

		private void Channel_Faulted(object sender, EventArgs e)
		{
			IClientChannel channel = sender as IClientChannel;
			if (channel != null)
			{
				channel.Faulted -= new EventHandler(Channel_Faulted);
				channel.Abort();
				channel.Dispose();
			}
		}
		#endregion

		#region public
		public Proxy(string endpointConfigurationName)
		{
			_endpointConfigurationName = endpointConfigurationName;
			CreateChannelFactory();
		}

		~Proxy()
		{
			Dispose(false);
		}

		public I GetChannel()
		{
			if ((_channelFactory != null) && (_channelFactory.State == CommunicationState.Faulted))
				DisposeChannelFactory();

			if (_channelFactory == null)
				CreateChannelFactory();

			I result = _channelFactory.CreateChannel();
			IClientChannel channel = result as IClientChannel;
			if (channel != null)
				channel.Faulted += new EventHandler(Channel_Faulted);

			return result;
		}

		public string EndpointConfigurationName
		{
			get { return _endpointConfigurationName; }
		}

		public event EventHandler Faulted;
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			Dispose(true);
		}
		#endregion
	}
}
