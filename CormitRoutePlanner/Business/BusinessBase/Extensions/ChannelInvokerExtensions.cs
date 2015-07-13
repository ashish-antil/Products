#region

using System;
using System.ServiceModel;
using FernBusinessBase.Errors;

#endregion

namespace FernBusinessBase.Extensions
{
    public static class ChannelInvokerExtensions
    {
/*
        public static TResponse Invoke<TService, TResponse>(this TService me, Func<TService, TResponse> func)
            where TService : IServerFacadeBase
            where TResponse : IServiceMessageResponse
        {
            TResponse response = default(TResponse);
            ChannelInvoker.Invoke((out IClientChannel channel) =>
            {
                channel = me as IClientChannel;
                response = func(me);
            });

            return response;
        }
*/

/*
        public static TResponse Invoke<TService, TResponse>(this Proxy<TService> me, Func<TService, TResponse> func)
            where TService : IServerFacadeBase
            where TResponse : IServiceMessageResponse
        {
            var service = me.GetChannel();
            return Invoke(service, func);
        }
*/

        public static TResponse Check<TResponse>(this TResponse me)
            where TResponse : BusinessMessageResponse
        {
            ErrorHandler.Check(me);
            return me;
        }

        public static void SafeClose(this IClientChannel channel)
        {
            try
            {
                switch (channel.State)
                {
                    case CommunicationState.Faulted:
                        channel.Abort();
                        // see dispose comment below
                        channel.Dispose();
                        break;

                    case CommunicationState.Closing:
                        break;

                    case CommunicationState.Closed:
                        // see dispose comment below
                        channel.Dispose();
                        break;

                        // not closing and not faulted
                    default:
                        try
                        {
                            channel.Close();
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.Warn(ex);
                            channel.Abort();
                        }
                        // this might not be necessary, since it actually calls close. 
                        // But it is good practice to call dispose for disposable, if Microsoft change the channel.dispose method
                        channel.Dispose();
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Warn(ex);
            }
        }

        public static void SafeClose(this ChannelFactory channelFactory)
        {
            try
            {
                switch (channelFactory.State)
                {
                    case CommunicationState.Faulted:
                        //System.Diagnostics.Debug.WriteLine("Proxy.ChannelFactory.Abort");
                        channelFactory.Abort();
                        break;

                    case CommunicationState.Closing:
                    case CommunicationState.Closed:
                        break;

                    default:
                        //System.Diagnostics.Debug.WriteLine("Proxy.ChannelFactory.Close");
                        try
                        {
                            channelFactory.Close();
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.Warn(ex);
                            channelFactory.Abort();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Warn(ex);
            }
        }
    }
}