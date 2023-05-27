#nullable enable
using System;

namespace YggdrAshill.Ragnarok.Unity
{
    internal static class LifecycleExecutionOrder
    {
        public const int Project = Int32.MinValue;
        public const int Scene = Project + 1;
        public const int GameObject = Scene + 2;
    }
}
