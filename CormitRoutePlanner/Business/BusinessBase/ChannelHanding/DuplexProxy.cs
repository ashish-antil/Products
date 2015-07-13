#region

using System.ServiceModel;

#endregion

namespace FernBusinessBase.ChannelHanding
{
    public sealed class DuplexProxy<TChannel> : DuplexClientBase<TChannel>
        where TChannel : class
    {
        public DuplexProxy(object callbackInstance, string endpointConfigurationName)
            : base(callbackInstance, endpointConfigurationName)
        {
        }
    }
}