using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FernBusinessBase;
using System.ServiceModel;

namespace ImardaAttributingBusiness
{
    partial interface IImardaAttributing
    {
        #region Operation Contracts for AttributeValue

        //[OperationContract]
        //AttributeValue GetAttributeValue(FullBusinessEntity entity, string attributeName);

        //[OperationContract]
        //GetListResponse<AttributeValue> GetAttributeValueList(IDRequest request);

        //[OperationContract]
        //GetListResponse<AttributeValue> GetAttributeValueListByEntityType_EntityID(IDRequest request);

        //[OperationContract]
        //GetItemResponse<AttributeValue> GetAttributeValueByID(IDRequest request);

        //[OperationContract]
        //GetItemResponse<AttributeValue> GetAttributeValueByEntityType_EntityID_Name(IDRequest request);

        [OperationContract]
        BusinessMessageResponse SaveAttributeValueList(SaveListRequest<FernBusinessBase.AttributeValue> request);

        [OperationContract]
        BusinessMessageResponse SaveAttributeValue(SaveRequest<FernBusinessBase.AttributeValue> request);

        #endregion

    }
}
