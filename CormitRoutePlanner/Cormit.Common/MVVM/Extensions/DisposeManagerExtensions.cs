#region

using System;
using Imarda.Lib.MVVM.Common.Interfaces;

#endregion

namespace Imarda.Lib.MVVM.Extensions
{
    public static class DisposeManagerExtensions
    {
        public static IDisposable DisposeWith(this IDisposable me, IDisposeManager disposeManager)
        {
            if (disposeManager != null)
            {
                disposeManager.AddToDisposalList(me);
            }

            return me;
        }
    }
}