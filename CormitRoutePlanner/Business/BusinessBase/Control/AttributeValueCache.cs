//& IM-3747
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Imarda.Common.Cache;
using Imarda.Lib;
using Imarda.Lib.MVVM.Extensions;

namespace FernBusinessBase
{
    public class AttributeValueCache // one per entity type e.g. there's a Unit Attribute Cache, a Driver Attribute Cache, etc.
    {
        public string Name;
        public string BusinessServiceName;
        public AttributeValues attributeValues = new AttributeValues();
        public bool LoadingInProgress;

        public AttributeValueCache(string name)
        {
            Name = name.ToLower();
            LoadingInProgress = true;
        }

        public bool AddAttributeValues(Guid entityID, List<AttributeValue> attributeValueList)
        {
            if (entityID != Guid.Empty && attributeValueList != null && attributeValueList.Count > 0)
            {
                var avForEntity = attributeValues.GetAttributeValuesForEntity(entityID);
                if (avForEntity == null)
                {
                    var avl = new AttributeValuesForEntity(attributeValueList); //DJ_AAR
                    return attributeValues.AddAttributeList(entityID, avl);   //DJ_RAR
                }
                else
                {
                    avForEntity.AttributeValues.AddRange(attributeValueList);
                    return true;
                }
            }
            return false;
        }

        public bool ChangeAttributeValues(Guid entityID, List<AttributeValue> attributeValueList)
        {
            if (entityID != Guid.Empty && attributeValueList != null && attributeValueList.Count > 0)
            {
               foreach (var attributeValue in attributeValueList)
               {
                   attributeValues.AddAttribute(attributeValue);
               }
               return true;
            }
            return false;
        }

        // This is redundant as each attribute value must go under it's entity ID
        //public bool AddAttribute(AttributeValue av)
        //{
        //    return attributeValues.AddAttribute(av);
        //}

        public List<AttributeValue> GetAttributeValueListForAttributeID(AttributeDefinition ad)
        {
            return attributeValues.GetAttributeValueListViaAttributeID(ad);
        }

        public List<AttributeValue> GetAttributeValueListForAttributeNameAndCompnay(Guid companyID, string varName)
        {
            return attributeValues.GetAttributeValueListViaAttributeNameAndCompany(companyID, varName);
        }

        public bool UpdateAttributeValueForAttributeIDWithAttributeDefintion(AttributeDefinition ad)
        {
            return attributeValues.UpdateAttributeValueListFromDefinition(ad);
        }

        public bool DeleteAttributeValuesByCompanyIDAndAttributeName(Guid companyID, string varName)
        {
            // Multiple entities, e.g. Units, may have this attribute and so need it removing
            // Retrieve all attributeValue items across all entities, for this attribute name
            var avList = GetAttributeValueListForAttributeNameAndCompnay(companyID, varName);

            foreach (var av in avList)
            {
                // Retrieve each entity's attribute value list, where there is an attribute value to remove
                var avListForEntity = attributeValues.GetEntry(av.EntityID);
                // Remove the attribute value that matches the company and attribute name
                avListForEntity.AttributeValues.Remove(av);
                // Put the attribute value list back, minus the deleted attribute
                attributeValues.AddEntry(av.EntityID, avListForEntity);
            }
            return true;
        }
       
        public List<AttributeValue> GetAttributeValueListForEntityID(string entityName, Guid entityID)
        {
            var result = attributeValues.GetEntry(entityID);
            if (result != null) return result.AttributeValues;
            return null;
        }

        public T CastExamp1<T>(object input)
        {
            return (T)input;
        }

        public T ConvertExamp1<T>(object input)
        {
            return (T)Convert.ChangeType(input, typeof(T));
        }
    }
}
