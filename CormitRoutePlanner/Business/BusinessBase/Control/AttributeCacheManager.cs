//& IM-3747
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using FernBusinessBase.Control;
using Imarda.Common.Cache;

namespace FernBusinessBase
{
    public class AttributeCacheManager
    {
        public string BusinessServiceName;

        public AttributeCacheManager(string businessServiceName)
        {
            BusinessServiceName = businessServiceName;
            AttributeDefinitionCacheList = new List<AttributeDefinitionCache>();
            AttributeValueCacheList = new List<AttributeValueCache>();
        }

        private List<AttributeDefinitionCache> AttributeDefinitionCacheList;// = new List<EntityCache>();
        private List<AttributeValueCache> AttributeValueCacheList;// = new List<EntityCache>();

        public void AddEntityItemListToAttributeDefinitionCache(string entityName, List<AttributeDefinition> entityItemList)
        {
            var foundEntity = AttributeDefinitionCacheList.SingleOrDefault(obj => obj.Name.ToLower() == entityName.ToLower());

            if (foundEntity == null)
            {
                var attributeDefinitionCache = new AttributeDefinitionCache(entityName);
                attributeDefinitionCache.AddItemList(entityItemList);
                AttributeDefinitionCacheList.Add(attributeDefinitionCache);
            }
            else
            {
                foundEntity.AddItemList(entityItemList);
            }
        }

        public void AddEntityItemToAttributeDefinitionCache(string entityName, AttributeDefinition entityItem)
        {
            var foundEntity = AttributeDefinitionCacheList.SingleOrDefault(obj => obj.Name.ToLower() == entityName.ToLower());

            if (foundEntity == null)
            {
                var attributeDefinitionCache = new AttributeDefinitionCache(entityName);
                attributeDefinitionCache.AddItem(entityItem);
                AttributeDefinitionCacheList.Add(attributeDefinitionCache);
            }
            else
            {
                foundEntity.AddItem(entityItem);
            }
        }

        public void AddEntityItemToAttributeValueCache(string entityName, AttributeValue entityItem)
        {
            var foundEntity = AttributeValueCacheList.SingleOrDefault(obj => obj.Name.ToLower() == entityName.ToLower());

            if (foundEntity == null)
            {
                var attributeValueCache = new AttributeValueCache(entityName);
                var avList = new List<AttributeValue>();
                avList.Add(entityItem);
                attributeValueCache.AddAttributeValues(entityItem.EntityID, avList);
                //attributeValueCache.AddAttribute(entityItem);
                AttributeValueCacheList.Add(attributeValueCache);
            }
            else
            {
                var avList = new List<AttributeValue>();
                avList.Add(entityItem);
                foundEntity.AddAttributeValues(entityItem.EntityID, avList);
                //foundEntity.AddAttribute(entityItem);
            }
        }

        public void AddEntityItemListToAttributeValueCache(string entityName, Guid entityID, List<AttributeValue> entityItemList)
        {
            var foundEntity = AttributeValueCacheList.SingleOrDefault(obj => obj.Name.ToLower() == entityName.ToLower());

            if (foundEntity == null)
            {
                var attributeValueCache = new AttributeValueCache(entityName);
                attributeValueCache.AddAttributeValues(entityID, entityItemList);
                AttributeValueCacheList.Add(attributeValueCache);
            }
            else
            {
                foundEntity.AddAttributeValues(entityID, entityItemList);
            }
        }

        public void ChangeEntityItemListToAttributeValueCache(string entityName, Guid entityID, List<AttributeValue> entityItemList)
        {
            var foundEntity = AttributeValueCacheList.SingleOrDefault(obj => obj.Name.ToLower() == entityName.ToLower());

            if (foundEntity == null)
            {
                var attributeValueCache = new AttributeValueCache(entityName);
                attributeValueCache.AddAttributeValues(entityID, entityItemList);
                AttributeValueCacheList.Add(attributeValueCache);
            }
            else
            {
                foundEntity.ChangeAttributeValues(entityID, entityItemList);
            }
        }

        public List<AttributeValue> GetListFromAttributeValueCacheByAttributeID(string entityName, AttributeDefinition ad)
        {
            if (ad == null || ad.ID == Guid.Empty) return null;

            var foundEntity = AttributeValueCacheList.SingleOrDefault(obj => obj.Name.ToLower() == entityName.ToLower());

            if (foundEntity == null)
            {
                return null;
            }
            else
            {
                return foundEntity.GetAttributeValueListForAttributeID(ad);
            }
        }

        /// <summary>
        /// An attribute definition has changed, so we must go through all entities for this entity type,
        /// and update the definition fields where the entity has that attribute.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="ad"></param>
        /// <returns></returns>
        public bool UpdateAttributeValueCacheByAttributeIDWithAttributeDefintion(string entityName, AttributeDefinition ad)
        {
            AttributeValueCache avCacheForEntity = AttributeValueCacheList.SingleOrDefault(obj => obj.Name.ToLower() == entityName.ToLower());

            if (avCacheForEntity == null)
            {
                return false;
            }
            else
            {
                return avCacheForEntity.UpdateAttributeValueForAttributeIDWithAttributeDefintion(ad);
            }
        }


        public AttributeDefinition GetEntityItemFromAttributeDefinitionCache(string entityName, Guid companyID, string attributeName)
        {
            var foundEntity = AttributeDefinitionCacheList.SingleOrDefault(obj => obj.Name.ToLower() == entityName.ToLower());

            if (foundEntity == null)
            {
                var attributeDefinitionCache = new AttributeDefinitionCache(entityName);
                AttributeDefinitionCacheList.Add(attributeDefinitionCache);
                return null;
            }
            else
            {
                return foundEntity.GetItemByCompanyIDAndAttributeName(companyID, attributeName);
            }
        }

        public List<AttributeDefinition> GetEntityListFromAttributeDefinitionCache(string entityName, Guid companyID)
        {
            if (entityName == "All")
            {
                var adList = new List<AttributeDefinition>();
                foreach (var entityType in AttributeDefinitionCacheList)
                {
                    adList.AddRange(entityType.GetListByCompanyID(companyID));
                }
                return adList;
            }
            else
            {
                var foundEntity = AttributeDefinitionCacheList.SingleOrDefault(obj => obj.Name.ToLower() == entityName.ToLower());

                if (foundEntity == null)
                {
                    var attributeDefinitionCache = new AttributeDefinitionCache(entityName);
                    AttributeDefinitionCacheList.Add(attributeDefinitionCache);
                    return new List<AttributeDefinition>();
                }
                else
                {
                    return foundEntity.GetListByCompanyID(companyID);
                }
            }
        }

        public List<AttributeValue> GetEntityAttributeListFromAttributeValueCache(string entityName, Guid entityID)
        {
            if (entityID == Guid.Empty) return null;
                
            var foundEntity = AttributeValueCacheList.SingleOrDefault(obj => obj.Name.ToLower() == entityName.ToLower());

            if (foundEntity == null)
            {
                return null;
            }
            else
            {
                return foundEntity.GetAttributeValueListForEntityID(entityName, entityID);
            }
        }

        //public List<AttributeDefinition> GetAttributeDefinitionListFromAttributeDefinitionCacheForBusinessService(string businessServiceName)
        //{
        //    var foundEntity = AttributeDefinitionCacheList.SingleOrDefault(obj => obj.BusinessServiceName == businessServiceName);

        //    if (foundEntity == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return foundEntity.GetAllItems();
        //    }
        //}

        public bool DeleteEntityItemFromAttributeDefinitionCache(Guid companyID, string entityName, string attributeName)
        {
            var foundEntity = AttributeDefinitionCacheList.SingleOrDefault(obj => obj.Name.ToLower() == entityName.ToLower());

            if (foundEntity != null)
            {
                return foundEntity.DeleteItemByCompanyIDAndAttributeName(companyID, attributeName);
            }
            return false;
        }

        public bool DeleteAttributeValuesFromAttributeValueCache(Guid companyID, string entityName, string attributeName)
        {
            var foundEntity = AttributeValueCacheList.SingleOrDefault(obj => obj.Name.ToLower() == entityName.ToLower());

            if (foundEntity != null)
            {
                return foundEntity.DeleteAttributeValuesByCompanyIDAndAttributeName(companyID, attributeName);
            }
            return false;
        }
    }
}
