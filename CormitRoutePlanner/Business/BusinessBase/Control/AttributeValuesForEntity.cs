using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FernBusinessBase
{
    public class AttributeValuesForEntity
    {
        public AttributeValuesForEntity(List<AttributeValue> attributeValueList) //DJ_AAR
        {
            AttributeValues = attributeValueList;
        }

        public List<AttributeValue> AttributeValues;
    }
}
