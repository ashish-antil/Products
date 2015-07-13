#region

using System;
using System.Linq;
using FernBusinessBase.Filtering.Interfaces;

#endregion

namespace FernBusinessBase.Filtering
{
    public sealed class FilterEngine<TItem, TArgument> : IFilterEngine<TItem, TArgument>
        where TItem : IFilterItem<TArgument>
    {
        private readonly FilterSettings<TItem, TArgument> _settings;

        public FilterEngine(FilterSettings<TItem, TArgument> settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            _settings = settings;
        }

        public bool Test(TArgument argument)
        {
            if (_settings == null)
            {
                return true;
            }

            return _settings.Items.Any(i => i.Test(argument));
        }
    }
}