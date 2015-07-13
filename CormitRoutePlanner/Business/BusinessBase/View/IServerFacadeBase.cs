#region

using System;
using System.ServiceModel;

#endregion

namespace FernBusinessBase
{
    [ServiceContract]
    public interface IServerFacadeBase
    {
        /// <summary>
        ///     A contract to receive the current date and time from the Server.
        /// </summary>
        [OperationContract]
        DateTime CurrentServerTime();

        [OperationContract]
        SimpleResponse<string> GetAttributes(IDRequest req);

        [OperationContract]
        SimpleResponse<string> GetVehicleAttributesForEntityId(IDRequest req);

        [OperationContract]
        SimpleResponse<string[]> GetVehicleAttributesForEntities(IDListRequest req);

        [OperationContract]
        SimpleResponse<string> GetDriverAttributesForEntityId(IDRequest req);

        [OperationContract]
        SimpleResponse<string[]> GetDriverAttributesForEntities(IDListRequest req);

        [OperationContract]
        KeyValueListResponse Ping(IDRequest req);
    }
}