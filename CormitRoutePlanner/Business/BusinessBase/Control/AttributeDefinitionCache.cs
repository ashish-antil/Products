//& IM-3747
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
    public class AttributeDefinitionCache //: AttributeDefinition
    {
        public string Name;
        public string BusinessServiceName;
        public AttributeDefinitionItems Items = new AttributeDefinitionItems();

        public AttributeDefinitionCache(string name)
        {
            Name = name.ToLower();
        }

        public bool AddItem(AttributeDefinition entityItem)
        {
            return Items.AddEntry(entityItem);
        }

        public void AddItemList(List<AttributeDefinition> entityItemList)
        {
            Items.AddList(entityItemList);
        }

        public AttributeDefinition GetItem(Guid entityID)
        {
            return Items.GetEntry(entityID);
        }

        public AttributeDefinition GetItemByCompanyIDAndAttributeName(Guid companyID, string varName)
        {
            return
                Items.GetEntries(entry => (entry.CompanyID == companyID) && (entry.VarName.ToLower() == varName.ToLower()) && entry.Active && !entry.Deleted)
                    .ToList().FirstOrDefault();
        }

        public List<AttributeDefinition> GetListByCompanyID(Guid companyID)
        {
            var systemCompanyID = new Guid("11111111-1111-1111-1111-111111111111");
            return Items.GetEntries(entry => ((entry.CompanyID == companyID || entry.CompanyID == systemCompanyID) && entry.Active && !entry.Deleted)).ToList();
        }

        public bool DeleteItemByCompanyIDAndAttributeName(Guid companyID, string varName)
        {
            var ad = Items.GetEntries(entry => (entry.CompanyID == companyID) && (entry.VarName.ToLower() == varName.ToLower()) && entry.Active && !entry.Deleted)
                    .ToList().FirstOrDefault();
            if (ad != null) return Items.RemoveEntry(ad);
            return false;
        }

        public AttributeDefinition GetItemList(Guid entityID)    //IM-3747
        {
            return Items.GetEntry(entityID);
        }

        public List<AttributeDefinition> GetAllItems()
        {
            return Items.GetEntries(entry => true).ToList();
        }

        public void DeleteItem(Guid entityID)
        {
            Items.Remove(entityID);
        }

        public List<AttributeDefinition> GetItemList(Guid companyID, bool inActive)
        {
            return
                Items.GetEntries(entry => (entry.CompanyID == companyID) && entry.Active || entry.Active == inActive)
                    .ToList();
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
