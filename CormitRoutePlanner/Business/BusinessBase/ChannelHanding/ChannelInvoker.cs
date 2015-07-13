using System;
using System.ServiceModel;
using FernBusinessBase.Extensions;
using Imarda.Lib;

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
	public static class ChannelInvoker
	{
		#region public
		public delegate void SafeChannelInvocationHandler(out IClientChannel channel);

        /// <summary>
        ///     Invoke action and ensure that opened service's channels are closed afterwards.
        /// </summary>
        /// <param name="proxy">The proxy used to open a new channel</param>
        /// <param name="action">The action to be invoked</param>
        /// <param name="throwExceptions">The flag that enables exceptions throwing if set to true, otherwise an exception is returned as a result. Default is true. </param>
        /// <returns>
        ///     A non-null Exception if throwExceptions is set to false and there is exception during action invokation.
        /// </returns>
        public static Exception Invoke<T>(Proxy<T> proxy, Action<T> action, bool throwExceptions = true)
            where T : IServerFacadeBase
        {
            var service = proxy.GetChannel();
            var handler = new SafeChannelInvocationHandler((out IClientChannel channel) =>
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                channel = service as IClientChannel;
                action(service);
            });

            // ReSharper disable once CSharpWarnings::CS0618
            return Invoke(handler, throwExceptions);
        }

        public static TResponse Invoke<TService, TResponse>(TService svc, Func<TService, TResponse> func)
            where TService : class, IServerFacadeBase
            where TResponse : class, IServiceMessageResponse
        {
            var result = default(TResponse);
            var handler = new SafeChannelInvocationHandler((out IClientChannel channel) =>
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                // ReSharper disable once ExpressionIsAlwaysNull
                channel = svc as IClientChannel;
                result = func(svc);
            });

            // ReSharper disable once CSharpWarnings::CS0618
            Invoke(handler, true);
            return result;
        }

		/// <summary>
		/// Invokes the 'channel delegate' and will throw CommunicationExceptions.
		/// </summary>
		/// <param name="handler"></param>
		/// <returns></returns>
		public static Exception Invoke(SafeChannelInvocationHandler handler)
		{
			return Invoke(handler, true);
		}

		/// <summary>
		/// Invokes the 'channel delegate' and will throw general and CommunicationExceptions 
		/// if the 'throwExceptions' flag is set to true.
		/// </summary>
		/// <param name="handler"></param>
		/// <param name="throwExceptions"></param>
		/// <returns></returns>
		public static Exception Invoke(SafeChannelInvocationHandler handler, bool throwExceptions)
		{
		    IClientChannel channel = null;
		    try
		    {
		        // Call delegate
		        handler(out channel);
		        return null;
		    }
			catch (TimeoutException ex)
		    {
				/*
		        // for historical reason we suppress this exception so that it doesn't get rethrown
		        return null;*/

				//IM-5885 also throw and return TimeoutException
				if (throwExceptions)
				{
					throw;
				}
				return ex;

		    }
		    catch (Exception ex)
		    {
		        if (throwExceptions)
		        {
		            throw;
		        }
                return ex;
            }
		    finally
		    {
                if (channel != null)
                {
                    channel.SafeClose();
                }
            }
		}
		#endregion
	}
}
