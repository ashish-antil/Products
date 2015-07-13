#region

using System;
using System.ServiceModel;
using Imarda.Lib;

#endregion

namespace Imarda360Base
{
    [MessageContract]
    [Serializable]
    public class SolutionMessageResponse : IServiceMessageResponse
    {
        #region private declarations

        private string _ErrorCode;
        private bool _Status;
        private string _StatusMessage;

        #endregion

        public SolutionMessageResponse()
        {
            _StatusMessage = "OK";
            _Status = true;
            _ErrorCode = string.Empty;
        }

        [MessageBodyMember]
        public bool Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        [MessageBodyMember]
        public string StatusMessage
        {
            get { return _StatusMessage; }
            set { _StatusMessage = value; }
        }

        [MessageBodyMember]
        public string ErrorCode
        {
            get { return _ErrorCode; }
            set { _ErrorCode = value; }
        }

        public virtual object Payload
        {
            get { return null; }
        }

        public override string ToString()
        {
            return ServiceMessageHelper.ToString(this);
        }
    }
}