namespace Imarda.Lib.MVVM.Common.Interfaces
{
    public interface INotifyPropertyChangedHandler
    {
        void FirePropertyChanged(string propertyName);
    }
}