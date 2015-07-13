#region

using System;
using System.Collections.Generic;
using Imarda.Lib.MVVM.Common.Interfaces;

#endregion

namespace Imarda.Lib.MVVM.Common
{
    public abstract class DisposableObservableObject : ObservableObject, IDisposeManager
    {
        private readonly object _lock = new object();
        private List<IDisposable> _managedDisposables;

        public void AddToDisposalList(IDisposable disposable)
        {
            if (disposable == null)
            {
                return;
            }

            if (_managedDisposables == null)
            {
                _managedDisposables = new List<IDisposable>();
            }

            lock (_lock)
            {
                _managedDisposables.Add(disposable);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Dispose disposables on request
                if (_managedDisposables != null)
                {
                    lock (_lock)
                    {
                        for (int i = _managedDisposables.Count - 1; i > -1; i--)
                        {
                            //As we don't have control over the references
                            //check that the patient is still alive
                            var d = _managedDisposables[i];
                            if (d != null)
                            {
                                d.Dispose();
                            }
                        }

                        _managedDisposables.Clear();
                        _managedDisposables = null;
                    }
                }
            }

            base.Dispose(disposing);
        }
    }
}