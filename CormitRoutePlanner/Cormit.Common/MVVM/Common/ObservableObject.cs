#region

using System.ComponentModel;
using Imarda.Lib.MVVM.Common.Interfaces;

#endregion

namespace Imarda.Lib.MVVM.Common
{
    public abstract class ObservableObject : Disposable, IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void FirePropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}