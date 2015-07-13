using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FernBusinessBase;
using System.ServiceModel;

namespace ImardaAttributingBusiness
{
    partial interface IImardaAttributingSimple
    {

        #region Operation Contracts for AttributeValue
        
        [OperationContract]
        //bool SaveAttributeValueSimple(SaveRequest<FernBusinessBase.AttributeValue> request);
        bool SaveAttributeValueSimple(FernBusinessBase.AttributeValue attributeValue);

        #endregion

    }
}
