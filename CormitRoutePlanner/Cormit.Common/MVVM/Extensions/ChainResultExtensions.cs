using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imarda.Lib.MVVM.Extensions
{
    public static class ChainResultExtensions
    {
        public static ChainResult<TResponse> ToChain<TResponse>(this TResponse me)
            where TResponse : class, IServiceMessageResponse
        {
            return new ChainResult<TResponse>(me, true);
        }

        public static ChainResult<TResponse> CheckAndContinue<TResponse>(this TResponse me)
              where TResponse : class, IServiceMessageResponse
        {
            if (ServiceMessageHelper.IsSessionProblem(me))
            {
                //throw new ResponseException(AjaxRequestResponder.ReloginToken);
            }

            return new ChainResult<TResponse>(me, true);
        }

        public static ChainResult<TResponse2> Continue<TResponse, TResponse2>(this ChainResult<TResponse> me, Func<TResponse, TResponse2> func)
            where TResponse : class, IServiceMessageResponse
            where TResponse2 : class, IServiceMessageResponse
        {
            if (!me.Success)
            {
                return new ChainResult<TResponse2>(null, false);
            }

            return new ChainResult<TResponse2>(func(me.Result), true);
        }

        public static TResponse ToResult<TResponse>(this ChainResult<TResponse> me)
            where TResponse : class, IServiceMessageResponse
        {
            return me.Result;
        }

        public class ChainResult<T>
            where T : class
        {
            public ChainResult(T result, bool success)
            {
                Result = result;
                Success = success;
            }

            public T Result { get; private set; }
            public bool Success { get; private set; }
        }
    }
}
