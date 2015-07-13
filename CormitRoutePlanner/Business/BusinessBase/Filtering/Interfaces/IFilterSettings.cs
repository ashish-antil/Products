#region

using System;
using System.Collections.Generic;

#endregion

namespace FernBusinessBase.Filtering.Interfaces
{
    public interface IFilterSettings<out TItem, TArgument>
        where TItem : IFilterItem<TArgument>
    {
        Guid Id { get; }
        IEnumerable<TItem> Items { get; }

        IFilterSettings<TItem, TArgument> GetEmptyCopy();

	    string ToString();
    }
}