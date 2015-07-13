#region

using System;
using FernBusinessBase.Errors;
using Imarda.Lib;

#endregion

namespace FernBusinessBase.Extensions
{
    public static class ProxyExtensions
    {
        /// <summary>
        ///     Invoke action on provided Proxy.
        /// </summary>
        /// <typeparam name="T">A service implementing IServerFacadeBase</typeparam>
        /// <param name="me">This instance of Proxy</param>
        /// <param name="action">Action to invoke</param>
        /// <param name="throwExceptions">Flag specifying if exceptions should be muffled</param>
        /// <returns>An exception occured during invokation of action if throwExceptions == false.</returns>
        public static Exception Invoke<T>(this Proxy<T> me, Action<T> action, bool throwExceptions = true)
            where T : IServerFacadeBase
        {
            return ChannelInvoker.Invoke(me, action, throwExceptions);
        }

        /// <summary>
        ///     Invoke function on provided Proxy.
        /// </summary>
        /// <typeparam name="T">A service implementing IServerFacadeBase</typeparam>
        /// <typeparam name="TResponse">A response implementing IServiceMessageResponse</typeparam>
        /// <param name="me">This instance of Proxy</param>
        /// <param name="func">Function to invoke</param>
        /// <returns>A non-null response.</returns>
        /// <remarks>Exceotions are not muffled when this method is invoked.</remarks>
        public static TResponse Invoke<T, TResponse>(this Proxy<T> me, Func<T, TResponse> func)
            where T : class, IServerFacadeBase
            where TResponse : class, IServiceMessageResponse
        { 
            TResponse response = null;
            ChannelInvoker.Invoke(me, s => { response = func(s); });
            return response;
        }

        /// <summary>
        ///     Invoke function on provided Proxy.
        /// </summary>
        /// <typeparam name="T">A service implementing IServerFacadeBase</typeparam>
        /// <typeparam name="TResponse">A response implementing IServiceMessageResponse</typeparam>
        /// <param name="me">This instance of Proxy</param>
        /// <param name="func">Function to invoke</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the invokation of func</param>
        /// <returns>A non-null response.</returns>
        /// <remarks>Exceotions are not muffled when this method is invoked.</remarks>
        public static TResponse Invoke<T, TResponse>(this Proxy<T> me, Func<T, TResponse> func, Action<Exception> onError)
            where T : class, IServerFacadeBase
            where TResponse : class, IServiceMessageResponse
        {
            TResponse response = null;

            try
            {
                ChannelInvoker.Invoke(me, s => { response = func(s); });
                return response;
            }
            catch (Exception e)
            {
                onError(e);
            }

            return null;
        }

        /// <summary>
        ///     Invoke action on provided Proxy and handle exceptions if thrown.
        /// </summary>
        /// <typeparam name="T">A service implementing IServerFacadeBase</typeparam>
        /// <param name="me">This instance of Proxy</param>
        /// <param name="action">Action to invoke</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the invokation of action</param>
        public static void Invoke<T>(this Proxy<T> me, Action<T> action, Action<Exception> onError)
            where T : IServerFacadeBase
        {
            try
            {
                // ReSharper disable once RedundantArgumentDefaultValue
                ChannelInvoker.Invoke(me, action, true);
            }
            catch (Exception e)
            {
                onError(e);
            }
        }

        /// <summary>
        ///     Invoke method on specified service and check response by ErrorHandler.Check
        /// </summary>
        /// <typeparam name="TService">A service implementing IServerFacadeBase</typeparam>
        /// <typeparam name="TResponse">A response implementing IServiceMessageResponse</typeparam>
        /// <param name="svc">This instance of service</param>
        /// <param name="func">Function to invoke</param>
        /// <returns>Service Response after error checking</returns>
        public static TResponse Invoke<TService, TResponse>(this TService svc, Func<TService, TResponse> func)
            where TService : class, IServerFacadeBase
            where TResponse : class, IServiceMessageResponse
        {
            var result = ChannelInvoker.Invoke(svc, func);
            ErrorHandler.Check(result);
            return result;
        }
    }
}