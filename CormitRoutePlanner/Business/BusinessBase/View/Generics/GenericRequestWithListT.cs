using System.Collections.Generic;

#region

using System;
using System.Globalization;
using System.ServiceModel;

#endregion

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
    /// <summary>
    ///     Encapsulates a request to perform some operation, whose function or target will
    ///     differ based on the given object.
    /// </summary>
    [MessageContract]
    [Serializable]
    public class GenericRequestWithListT<T> : IRequestBase
    {
        public GenericRequestWithListT()
        {
        }

        public GenericRequestWithListT(List<T> entity, Guid id, params object[] parameters)
        {
            Entity = entity;
            ID = id;
            Parameters = parameters;
        }

        [MessageBodyMember]
        public List<T> Entity { get; set; }

        [MessageBodyMember]
        public Guid ID { get; set; }

        [MessageBodyMember]
        public object[] Parameters { get; set; }

        [MessageBodyMember]
        public Guid SessionID { get; set; }

        public object SID
        {
            set { SessionID = new Guid(value.ToString()); }
        }

        /// <summary>
        ///     Local use only. Not transported across services.
        /// </summary>
        public object DebugInfo { get; set; }
    }
}
