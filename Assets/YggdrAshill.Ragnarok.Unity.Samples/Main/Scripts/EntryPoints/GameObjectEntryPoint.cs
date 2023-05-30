namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class GameObjectEntryPoint : MonoEntryPoint
    {
        protected override void Configure(IContainer container)
        {
            container.Register<Service>(Lifetime.Global).AsImplementedInterfaces();
        }
    }
}
