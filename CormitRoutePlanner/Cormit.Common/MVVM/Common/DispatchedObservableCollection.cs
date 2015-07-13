#region

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Imarda.Lib.MVVM.Common.Interfaces;

#endregion

namespace Imarda.Lib.MVVM.Common
{
    //ToDo: Implement a thread safe collection
    public class DispatchedObservableCollection<T> : ObservableCollection<T>, INotifyPropertyChangedHandler
    {
        public DispatchedObservableCollection()
        {
        }

        public DispatchedObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public void FirePropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected override event PropertyChangedEventHandler PropertyChanged;

        protected override sealed void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            FirePropertyChanged("Count");
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                return;
            }

            foreach (T item in items)
            {
                Add(item);
            }
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                return;
            }

            foreach (T item in items)
            {
                Remove(item);
            }            
        }
    }
}