#nullable enable
using System;

namespace YggdrAshill.Ragnarok
{
    [Obsolete("Use IInstallation directly.")]
    public interface IEntryPoint
    {
        IInstallation Installation { get; }
    }
}
