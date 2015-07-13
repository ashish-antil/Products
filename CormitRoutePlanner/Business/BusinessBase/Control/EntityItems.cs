//& IM-5178
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imarda.Common.Cache;

namespace FernBusinessBase
{
    public class EntityItems : BaseConcurrentCache<Guid, FullBusinessEntity>
    {
        public override bool AddEntry(FullBusinessEntity val)
        {
            if (Dictionary.ContainsKey(val.ID))
            {
                Dictionary[val.ID] = val;
                return true;
            }
            else
                return Dictionary.TryAdd(val.ID, val);
        }

        public bool AddList(List<FullBusinessEntity> list)
        {
            foreach (var item in list)
            {
                AddEntry(item);
            }
            return true;
        }

        internal void RemoveEntry(FullBusinessEntity val)
        {
            Remove(val.ID);
        }
    }
}
