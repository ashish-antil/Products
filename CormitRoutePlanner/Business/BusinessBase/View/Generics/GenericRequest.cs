using System.Runtime.Serialization;

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
    ///     differ based on the given ID and parameters.
    /// </summary>
    [MessageContract]
    [Serializable]
    public class GenericRequest : IRequestBase
    {
        public GenericRequest()
        {
        }

        public GenericRequest(Guid id) : this(id, null)
        {
        }

        public GenericRequest(Guid id, params object[] parameters)
        {
            ID = id;
            Parameters = parameters;
        }

        [MessageBodyMember]
        public Guid ID { get; set; }

        [MessageBodyMember]
        public object[] Parameters { get; set; }

        public object this[int index]
        {
            get { return Parameters[index]; }
            set { Parameters[index] = value; }
        }

        public T Get<T>(int index)
        {
            return (T) Parameters[index];
        }

        public bool TryGet<T>(int index, out T value)
        {
            value = default(T);

            if (Parameters.Length <= index)
            {
                return false;
            }

            var parameter = Parameters[index];
            if (!(parameter is T))
            {
                return false;
            }

            value = (T) parameter;
            return true;
        }

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

        public override string ToString()
        {
            return string.Format("GenericRequest(ID={0}, {1} params)",
                                 ID,
                                 Parameters == null ? "null" : Parameters.Length.ToString(CultureInfo.InvariantCulture));
        }
    }
}