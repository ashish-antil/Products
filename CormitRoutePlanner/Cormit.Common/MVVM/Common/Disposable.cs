#region

using System;

#endregion

namespace Imarda.Lib.MVVM.Common
{
    public class Disposable : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these  
            // operations, as well as in your methods that use the resource. 
            if (disposing)
            {
            }
        }
    }
}