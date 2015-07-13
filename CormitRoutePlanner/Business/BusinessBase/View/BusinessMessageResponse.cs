#region

using System;
using System.ServiceModel;
using FernBusinessBase.Errors;
using Imarda.Lib;

#endregion

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
    [MessageContract]
    [Serializable]
    public class BusinessMessageResponse : IServiceMessageResponse
    {
        private string _errorCode;
        private bool _status;
        private string _statusMessage;

        public BusinessMessageResponse()
        {
            _statusMessage = "OK";
            _status = true;
            _errorCode = string.Empty;
        }

        public BusinessMessageResponse(IServiceMessageResponse resp)
        {
            _status = resp.Status;
            _statusMessage = resp.StatusMessage;
            _errorCode = resp.ErrorCode;
        }

        [MessageBodyMember]
        public bool Status
        {
            get { return _status; }
            set { _status = value; }
        }

        [MessageBodyMember]
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { _statusMessage = value; }
        }

        [MessageBodyMember]
        public string ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }

        public virtual object Payload
        {
            get { return null; }
        }

        public bool IsSecurityException()
        {
            return ErrorHandler.IsSecurityException(ErrorCode);
        }

        public override string ToString()
        {
            return ServiceMessageHelper.ToString(this);
        }
    }
}