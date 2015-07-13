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

        #region Operation Contracts for AttributeDefinition

        [OperationContract]
        GetItemResponse<AttributeDefinition> GetAttributeDefinition(IDRequest request);

        [OperationContract]
        GetListResponse<AttributeDefinition> GetAttributeDefintionListWithHistoryByCompanyID(IDRequest request);

        [OperationContract]
        BusinessMessageResponse SaveAttributeDefinition(SaveRequest<FernBusinessBase.AttributeDefinition> request);

        [OperationContract]
        BusinessMessageResponse SaveAttributeDefinitionList(SaveListRequest<FernBusinessBase.AttributeDefinition> request);

        [OperationContract]
        BusinessMessageResponse DeleteAttributeDefinition(IDRequest request);
        
        #endregion

    }
}
