#region

using System;
using Imarda.Lib;

#endregion

// ReSharper disable once CheckNamespace
namespace ImardaBusinessBase
{
    public interface ITrackable
    {
        Angle Latitude { get; }
        Angle Longitude { get; }
        Speed Speed { get; }
        Angle Direction { get; }
        bool Ignition { get; }
        bool Idle { get; }
        DateTime LastUpdate { get; }
        bool HasAlert { get; }
    }
}