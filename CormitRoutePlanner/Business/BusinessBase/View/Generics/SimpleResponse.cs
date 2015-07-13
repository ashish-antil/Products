#region

using System;
using System.ServiceModel;
using Imarda.Lib;

#endregion

namespace FernBusinessBase
{
    [MessageContract]
    [Serializable]
    public class SimpleResponse<T> : IServiceMessageResponse
    {
        public SimpleResponse()
        {
            Status = true;
        }

        public SimpleResponse(IServiceMessageResponse resp)
        {
            ServiceMessageHelper.Copy(resp, this);
        }

        public SimpleResponse(T val)
        {
            Value = val;
            Status = true;
        }

        [MessageBodyMember]
        public T Value { get; set; }

        [MessageBodyMember]
        public bool Status { get; set; }

        [MessageBodyMember]
        public string StatusMessage { get; set; }

        [MessageBodyMember]
        public string ErrorCode { get; set; }

        public object Payload
        {
            get { return Value; }
        }

        public override string ToString()
        {
            return ServiceMessageHelper.ToString(this, string.Format("SimpleResponse: {0}", Value));
        }
    }
}