//& IM-3747
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FernBusinessBase.Control;
using Imarda.Common.Cache;
using Imarda.Lib.MVVM.Extensions;

namespace FernBusinessBase
{
    public class AttributeValues : BaseConcurrentCache<Guid, AttributeValuesForEntity>
    {
        public bool AddAttributeList(Guid entityID, AttributeValuesForEntity avl)
        {
            Dictionary.AddOrUpdate(entityID, avl, (id, value) => avl);
            return true;
        }

        public bool UpdateAttributeValueListFromDefinition(AttributeDefinition ad)
        {
            var avList = GetAttributeValueListViaAttributeID(ad);
            foreach (var av in avList)
            {
                av.FriendlyName = ad.FriendlyName;
                av.Description = ad.Description;
                av.GroupID = ad.GroupID;
                av.VarType = ad.VarType;
                av.Format = ad.Format;
                av.CaptureHistory = ad.CaptureHistory;
                av.Viewable = ad.Viewable;
                av.DateModified = DateTime.UtcNow;
                AddAttribute(av);
            }
            return true;
        }

        public bool AddAttribute(AttributeValue val)
        {
            AttributeValuesForEntity attributeValuesForEntity;
            if (Dictionary.TryGetValue(val.EntityID, out attributeValuesForEntity))
            {
                int posn = attributeValuesForEntity.AttributeValues.IndexOf(val);
                if (posn > -1)
                    attributeValuesForEntity.AttributeValues[posn] = val;   // replace attribute with new attributeValue
                else
                {
                    attributeValuesForEntity.AttributeValues.Add(val);      // not present, so add attributeValue to this list
                }
                return AddAttributeList(val.EntityID, attributeValuesForEntity);
            }
            else
            {
                // This entity is not represented in cache yet, so add it
                var attributeValueList = new List<AttributeValue> {val};
                attributeValuesForEntity = new AttributeValuesForEntity(attributeValueList);                    //DJ_AAR
                return AddAttributeList(val.EntityID, attributeValuesForEntity);
            }
        }

        public bool AddEntry(Guid entityId, AttributeValuesForEntity avl)
        {
            return AddAttributeList(entityId, avl);
        }

        public override bool AddEntry(AttributeValuesForEntity val)
        {
            throw new NotImplementedException();
        }

        // Look across entire AttributeValue cache and retrieve all AttributeValue rows which are for the attribute wanted
        public List<AttributeValue> GetAttributeValueListViaAttributeID(AttributeDefinition ad)
        {
            var avList = new List<AttributeValue>();
            var foundList = Dictionary.Values.SelectMany(v => v.AttributeValues.Where(av => av.AttributeID == ad.ID));
            if (foundList.Any())
            {
                foundList.ForEach(av =>
                {
                    avList.Add(av);
                });
            }
            return avList;
        }

        // Look across entire AttributeValue cache and retrieve all AttributeValue rows which are for the attribute wanted
        public List<AttributeValue> GetAttributeValueListViaAttributeNameAndCompany(Guid companyID, string varName)
        {
            var avList = new List<AttributeValue>();
            var foundList = Dictionary.Values.SelectMany(v => v.AttributeValues.Where(av => av.CompanyID == companyID && (av.VarName.ToLower() == varName.ToLower()) && av.Active && !av.Deleted));
            if (foundList.Any())
            {
                foundList.ForEach(av =>
                {
                    avList.Add(av);
                });
            }
            return avList;
        }

        public AttributeValuesForEntity GetAttributeValuesForEntity(Guid entityId)
        {
            AttributeValuesForEntity attributeValuesForEntity;
            if (Dictionary.TryGetValue(entityId, out attributeValuesForEntity))
                return attributeValuesForEntity;
            return null;
        }
    }
}
