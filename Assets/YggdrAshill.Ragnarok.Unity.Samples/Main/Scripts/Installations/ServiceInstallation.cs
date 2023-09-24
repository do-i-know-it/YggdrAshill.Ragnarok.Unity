using YggdrAshill.Ragnarok.Experimental;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class ServiceInstallation : MonoInstallation
    {
        public override void Install(IContainer container)
        {
            container.Register<Service>(Lifetime.Global).AsImplementedInterfaces();
        }
    }
}
