#nullable enable
using YggdrAshill.Ragnarok.Experimental;

namespace YggdrAshill.Ragnarok.Unity.Specification
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
