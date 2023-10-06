#nullable enable
namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class LifecycleExtension
    {
        public static GameObjectLifecycle CreateChild(this Lifecycle lifecycle, GameObjectLifecycle prefab)
        {
            return GameObjectLifecycle.Create(prefab, lifecycle.transform);
        }
    }
}
