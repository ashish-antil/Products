#region

using System;
using System.ServiceModel;

#endregion

namespace FernBusinessBase.View.Generics
{
    [MessageContract]
    [Serializable]
    public class StringRequest : ParameterMessageBase
    {
        public StringRequest()
        {
        }

        public StringRequest(string value, Guid sessionId)
        {
            SessionID = sessionId;
            Parameters["stringValue"] = value;
        }

        public string Value
        {
            get { return Parameters["stringValue"]; }
        }
    }
}