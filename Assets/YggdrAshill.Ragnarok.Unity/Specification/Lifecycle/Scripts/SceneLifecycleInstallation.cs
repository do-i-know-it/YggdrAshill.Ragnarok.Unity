#nullable enable
namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class SceneLifecycleInstallation : MonoInstallation
    {
        public override void Install(IObjectContainer container)
        {
            container.Register<HelloWorldWithGameObject>(Lifetime.Global);
            container.Register(resolver => resolver.Resolve<HelloWorldWithGameObject>().Execute());
        }
    }
}
