//& IM-5178
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Imarda.Common.Cache;
using Imarda.Lib;
using Imarda.Lib.MVVM.Extensions;
//using ImardaAttributingBusiness;

namespace FernBusinessBase
{
    public class EntityCache : FullBusinessEntity
    {
        public string Name;
        public EntityItems Items = new EntityItems();

        public EntityCache(string name)
        {
            Name = name;
        }

        public bool AddItem(FullBusinessEntity entityItem)
        {
            return Items.AddEntry(entityItem);
        }

        public void AddItemList(List<FullBusinessEntity> entityItemList)
        {
            Items.AddList(entityItemList);
        }

        public FullBusinessEntity GetItem(Guid entityID)
        {
            return Items.GetEntry(entityID);
        }

        public FullBusinessEntity GetItemList(Guid entityID)    //IM-3747
        {
            return Items.GetEntry(entityID);
        }

        public void DeleteItem(Guid entityID)
        {
            Items.Remove(entityID);
        }

        public List<FullBusinessEntity> GetItemList(Guid companyID, bool inActive)
        {
            return
                Items.GetEntries(entry => (entry.CompanyID == companyID) && entry.Active || entry.Active == inActive)
                    .ToList();
        }

        //public List<FullBusinessEntity> GetAttributeValueItemList(string entityName, string businessServiceName)
        //{
        //    return
        //        Items.GetEntries(entry => (entry.CompanyID == companyID) && entry.Active || entry.Active == inActive)
        //            .ToList();
        //}

        //TODO - need method to retrieve an entity, based on certain values of certain fields of the entity
        public FullBusinessEntity GetItemByParams<T>(IDRequest request)
        {
            // Get the parameter names and values for this request
            var paramNames = new List<string>();
            var paramValues = new List<object>();
            foreach (var key in request.Keys)
            {
                object paramValue = null;
                if (request.Get(key, out paramValue))
                {
                    paramNames.Add(key);
                    paramValues.Add(paramValue);
                }
            }

            // Get the field names of this entity type
            var firstEntry = Items.GetFirstEntry();
            var propInfo1 = firstEntry.GetType().GetFields();
            var entityProperties = new List<string>();
            foreach (var pi in propInfo1)
            {
                entityProperties.Add(pi.Name.ToLower());
            }

            // Ensure that this entity type has members which correspond to the parameter names
            // (If it doesn't match 100%, then a LINQ query will not work. Note that they should match unless the caller has been careless.)
            foreach (var paramName in paramNames)
            {
                if (!entityProperties.Contains(paramName)) return null;
            }

            // Now select any entities in this specifc entity cache, which match the parameter values

            if (firstEntry.GetType().Name != "AttributeValue") return null;     // we only handle AttributeValue queries at present

            string lowerVarNameParam = paramValues[1].ToString().ToLower();

            // we only handle queries which provide these 3 params below
            var entityList =
                Items.GetEntries(
                    entry => entry.Active && !entry.Deleted &&
                        (entry as FernBusinessBase.AttributeValue).EntityID == request.ID    //WASDJ
                        && (entry as FernBusinessBase.AttributeValue).EntityTypeName == paramValues[0].ToString()    //WASDJ
                        && ((entry as FernBusinessBase.AttributeValue).VarName.ToLower() == lowerVarNameParam    //WASDJ
                            || (entry as FernBusinessBase.AttributeValue).FriendlyName.ToLower() == lowerVarNameParam));         //WASDJ   
            return entityList.FirstOrDefault();     // should only be one entity found, but in case none, return first or default
        }


        public List<FullBusinessEntity> GetListByParams<T>(IDRequest request)
        {
            // Get the parameter names and values for this request
            var paramNames = new List<string>();
            var paramValues = new List<object>();
            foreach (var key in request.Keys)
            {
                object paramValue = null;
                if (request.Get(key, out paramValue))
                {
                    paramNames.Add(key);
                    paramValues.Add(paramValue);
                }
            }

            // Get the field names of this entity type
            var firstEntry = Items.GetFirstEntry();
            var propInfo1 = firstEntry.GetType().GetFields();
            var entityProperties = new List<string>();
            foreach (var pi in propInfo1)
            {
                entityProperties.Add(pi.Name.ToLower());
            }

            // Ensure that this entity type has members which correspond to the parameter names
            // (If it doesn't match 100%, then a LINQ query will not work. Note that they should match unless the caller has been careless.)
            foreach (var paramName in paramNames)
            {
                if (!entityProperties.Contains(paramName)) return null;
            }

            // Now select any entities in this specifc entity cache, which match the parameter values

            if (firstEntry.GetType().Name != "AttributeValue") return null;     // we only handle AttributeValue queries at present

            var entityList =
                Items.GetEntries(
                    entry => entry.Active && !entry.Deleted &&
                        (entry as FernBusinessBase.AttributeValue).EntityID == request.ID    //WASDJ
                        && (entry as FernBusinessBase.AttributeValue).EntityTypeName == paramValues[0].ToString());    // we only handle queries which provide these 2 params  //WASDJ
            return entityList.ToList();     // should have found multiple attributes for the one entity and guid (e.g. all unit attributes for UnitID xxxx)
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
