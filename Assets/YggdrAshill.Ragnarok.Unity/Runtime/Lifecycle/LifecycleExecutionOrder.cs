#nullable enable
namespace YggdrAshill.Ragnarok
{
    internal static class LifecycleExecutionOrder
    {
        public const int Project = int.MinValue;
        public const int Scene = Project + 1;
        public const int GameObject = Scene + 2;
    }
}
