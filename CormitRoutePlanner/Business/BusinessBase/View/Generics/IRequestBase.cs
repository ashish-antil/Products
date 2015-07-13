#region

using System;

#endregion

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
    public interface IRequestBase
    {
        Guid SessionID { get; set; }
        object SID { set; }
        object DebugInfo { get; set; }
    }
}