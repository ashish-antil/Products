namespace FernBusinessBase.Filtering.Interfaces
{
    public interface IFilterItem<in TItem>
    {
        bool Test(TItem argument);
	    string ToString();
    }
}