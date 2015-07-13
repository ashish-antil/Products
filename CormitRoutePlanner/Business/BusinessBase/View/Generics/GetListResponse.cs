#region

using System.Collections.Generic;
using System.ServiceModel;
using Imarda.Lib;

#endregion

namespace FernBusinessBase
{
    /// <summary>
    ///     Encapsulates a response from the business layer with a list of objects of
    ///     the same type (eg. A list of Customers).
    /// </summary>
    /// <typeparam name="T">The type of the objects in the list</typeparam>
    [MessageContract]
    public class GetListResponse<T> : BusinessMessageResponse 
        where T : BaseEntity, new()
    {
        private List<T> _list;

        public GetListResponse(List<T> list)
        {
            _list = list;
        }

        public GetListResponse()
        {
            _list = new List<T>();
        }

        [MessageBodyMember]
        public List<T> List
        {
            get { return _list; }
            set { _list = value; }
        }

        public override object Payload
        {
            get { return _list; }
        }

        public override string ToString()
        {
            string detail = _list == null ? typeof (T).Name + ": null list" : typeof (T).Name + " " + _list.Count + " items";
            return ServiceMessageHelper.ToString(this, detail);
        }
    }
}