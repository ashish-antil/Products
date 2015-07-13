#region

using System;

#endregion

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
    /// <summary>
    ///     Set of flags that influence the retrieval of data
    /// </summary>
    [Flags]
    public enum RetrievalOptions : byte
    {
        /// <summary>
        ///     No options provided.
        /// </summary>
        None = 0x0,

        /// <summary>
        ///     Retrieve EntityAttributes.
        /// </summary>
        Attributes = 0x1,

        /// <summary>
        ///     Retrieve records regardless of active flag value, if SP supports it.
        /// </summary>
        IncludeInactive = 0x2,

        // Retrieve records regardless of deleted flag value, if SP supports it.
        //IncludeDeleted = 0x4,

        /// <summary>
        ///     Retrieve All AttributeValues
        /// </summary>
        AllAttributes = 0x4,
    }

    /// <summary>
    ///     To be implemented by Get-requests parameters to filter the records to retrieve.
    /// </summary>
    public interface IWithOptions
    {
        RetrievalOptions Options { get; set; }

        bool HasAll(RetrievalOptions options);

        bool HasSome(RetrievalOptions options);
    }
}