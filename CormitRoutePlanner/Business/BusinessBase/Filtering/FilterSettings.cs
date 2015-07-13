#region

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using FernBusinessBase.Filtering.Interfaces;
using Imarda.Lib;
using Imarda.Lib.MVVM.Extensions;

#endregion

namespace FernBusinessBase.Filtering
{
    [DataContract]
    public class FilterSettings<TItem, TArgument> : IFilterSettings<TItem, TArgument>
        where TItem : IFilterItem<TArgument>
    {
        protected FilterSettings(IEnumerable<TItem> items)
            : this(Guid.NewGuid(), items)
        {
        }

        private FilterSettings(Guid id, IEnumerable<TItem> items)
        {
            Id = id;
            Items = items == null
                        ? new List<TItem>()
                        : new List<TItem>(items);
        }

        [DataMember]
        public Guid Id { get; private set; }

        [DataMember]
        public IEnumerable<TItem> Items { get; private set; }

        /// <summary>
        ///     Create an empty copy of this FilterSettings instance.
        /// </summary>
        /// <returns>An empty copy of FilterSettings with the same Id and an empty Items collection.</returns>
        public IFilterSettings<TItem, TArgument> GetEmptyCopy()
        {
            return new FilterSettings<TItem, TArgument>(Id, null);
        }

	    public override string ToString()
	    {
		    var sb = new StringBuilder(Id.ToString());
			Items.ForEach(i=>sb.AppendLine(i.ToString()));
		    return sb.ToString();
	    }
    }
}