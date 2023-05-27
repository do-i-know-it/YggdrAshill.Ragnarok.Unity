using YggdrAshill.Ragnarok.Construction;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class GameObjectEntryPoint : MonoEntryPoint
    {
        protected override void Configure(IContainer container)
        {
            container.RegisterGlobal<Service>().AsImplementedInterfaces();
        }
    }
}
