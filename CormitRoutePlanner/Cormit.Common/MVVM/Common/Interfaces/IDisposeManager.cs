#region

using System;

#endregion

namespace Imarda.Lib.MVVM.Common.Interfaces
{
    public interface IDisposeManager : IDisposable
    {
        /// <summary>
        ///     Add to the immediate disposal list.
        /// </summary>
        /// <remarks>
        ///     A disposable added into the list is disposed when this IDisposeManager derived class is disposed.
        /// </remarks>
        /// <remarks>
        ///     Use DisposeManagerExtensions.DisposeWith() extension method to dispose critical observable subscriptions.
        /// </remarks>
        /// <example>this.AddToDisposalList(Model.GetSomeDataAsObservable().Subscribe());</example>
        /// <example>Model.GetSomeDataAsObservable().Subscribe().DisposeWith(this);</example>
        /// <param name="disposable">The object to be added in the disposable list</param>
        void AddToDisposalList(IDisposable disposable);
    }
}