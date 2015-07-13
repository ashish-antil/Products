#region

using System.Collections.Generic;
using System.ServiceModel;

#endregion

// ReSharper disable once CheckNamespace
namespace Imarda360Base
{
    [MessageContract]
    public class GenericList360Response<T> : SolutionMessageResponse
    {
        public GenericList360Response()
        {
            List = new List<T>();
        }

        public override object Payload
        {
            get { return List; }
        }

        [MessageBodyMember]
        public List<T> List { get; set; }
    }
}