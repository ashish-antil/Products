#region

using System.ComponentModel;

#endregion

namespace Imarda.Lib.MVVM.Common.Interfaces
{
    public interface IViewModel : INotifyPropertyChanged, INotifyPropertyChangedHandler
    {
    }
}