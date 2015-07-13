#region

using System;
using System.ServiceModel;
using Imarda.Lib;

#endregion

namespace FernBusinessBase.View.Generics
{
    [MessageContract]
    public class UpdateMessageResponse : BusinessMessageResponse
    {
        // ReSharper disable once EmptyConstructor
        public UpdateMessageResponse()
        {
        }

        public UpdateMessageResponse(DateTime timeStamp)
        {
            TimeStamp = timeStamp;
        }

        public UpdateMessageResponse(IServiceMessageResponse resp)
            : base(resp)
        {
        }

        [MessageBodyMember]
        public DateTime? TimeStamp { get; set; }
    }
}