//& IM-5178
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FernBusinessBase.Control;
using Imarda.Common.Cache;

namespace FernBusinessBase
{
    public class EntityCacheManager
    {
        public string BusinessServiceName;

        public EntityCacheManager(string businessServiceName)
        {
            BusinessServiceName = businessServiceName;
            CacheList = new List<EntityCache>();
        }

        private List<EntityCache> CacheList;// = new List<EntityCache>();

        public void AddEntityItemToEntityCache(FullBusinessEntity entityItem)
        {
            string entityNameDetails = entityItem.ToString();
            var entityName = entityNameDetails.Substring(0, entityNameDetails.IndexOf("("));
            var foundEntity = CacheList.SingleOrDefault(obj => obj.Name == entityName);

            if (foundEntity == null)
            {
                var entityCache = new EntityCache(entityName);
                entityCache.AddItem(entityItem);
                CacheList.Add(entityCache);
            }
            else
            {
                foundEntity.AddItem(entityItem);
            }
        }

        public void AddEntityItemListToEntityCache(string entityName, List<FullBusinessEntity> entityItemList)
        {
            var foundEntity = CacheList.SingleOrDefault(obj => obj.Name == entityName);

            if (foundEntity == null)
            {
                var entityCache = new EntityCache(entityName);
                entityCache.AddItemList(entityItemList);
                CacheList.Add(entityCache);
            }
            else
            {
                foundEntity.AddItemList(entityItemList);
            }
        }

        public FullBusinessEntity GetEntityItemFromEntityCache(string entityName, Guid entityID)
        {
            var foundEntity = CacheList.SingleOrDefault(obj => obj.Name == entityName);

            if (foundEntity == null)
            {
                return null;
            }
            else
            {
                return foundEntity.GetItem(entityID);
            }
        }

        //TODO: work in progress
        public FullBusinessEntity GetEntityItemFromEntityCacheByParam<T>(string entityName, IDRequest request)
        {
            var foundEntity = CacheList.SingleOrDefault(obj => obj.Name == entityName);

            if (foundEntity == null)
            {
                return null;
            }
            else
            {
                return foundEntity.GetItemByParams<T>(request);
            }
        }

        public List<FullBusinessEntity> GetEntityListFromEntityCacheByParam<T>(string entityName, IDRequest request)
        {
            var foundEntity = CacheList.SingleOrDefault(obj => obj.Name == entityName);

            if (foundEntity == null)
            {
                //var entityCache = new EntityCache(entityName);
                //CacheList.Add(entityCache);
                //return new List<FullBusinessEntity>();
                return null;
            }
            else
            {
                return foundEntity.GetListByParams<T>(request);
            }
        }

        public List<FullBusinessEntity> GetEntityItemListFromEntityCache(string entityName, Guid CompanyID, bool inActive)
        {
            var foundEntity = CacheList.SingleOrDefault(obj => obj.Name == entityName);

            if (foundEntity == null)
            {
                var entityCache = new EntityCache(entityName);
                //entityCache.AddItemList(entityItemList);
                CacheList.Add(entityCache);
                return new List<FullBusinessEntity>();

                //return null;    // TODO: null or an empty list?
            }
            else
            {
                return foundEntity.GetItemList(CompanyID, inActive);
            }
        }

        public void DeleteEntityItemFromEntityCache(string entityName, Guid entityID)   // not called so far, as we do soft deletes almost always, which is a Save
        {
            var foundEntity = CacheList.SingleOrDefault(obj => obj.Name == entityName);

            if (foundEntity != null)
            {
                foundEntity.DeleteItem(entityID);
            }
        }
    }
}
