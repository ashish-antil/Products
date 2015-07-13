using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaSecurityBusiness
{
    partial interface IImardaSecurity
    {

        #region Operation Contracts for LogonLog

        [OperationContract]
        GetListResponse<LogonLog> GetTopNLogonLogListBySecurityEntityID(IDRequest request);


        #endregion

    }
}