//& IM-3747
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imarda.Common.Cache;

namespace FernBusinessBase
{
    public class AttributeDefinitionItems : BaseConcurrentCache<Guid, AttributeDefinition>
    {
        public override bool AddEntry(AttributeDefinition val)
        {
            Dictionary.AddOrUpdate(val.ID, val, (id, value) => val);
            return true;
        }

        public bool AddList(List<AttributeDefinition> list)
        {
            foreach (var item in list)
            {
                AddEntry(item);
            }
            return true;
        }

        internal bool RemoveEntry(AttributeDefinition val)
        {
            return Remove(val.ID);
        }
    }
}
