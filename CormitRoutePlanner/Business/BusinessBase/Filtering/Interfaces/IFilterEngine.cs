namespace FernBusinessBase.Filtering.Interfaces
{
    public interface IFilterEngine<TItem, TArgument>
        where TItem : IFilterItem<TArgument>
    {
        bool Test(TArgument argument);
    }
}