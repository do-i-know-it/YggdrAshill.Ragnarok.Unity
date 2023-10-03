using YggdrAshill.Ragnarok.Experimental;

namespace YggdrAshill.Ragnarok.Unity.Samples
{
    internal sealed class ServiceInstallation : MonoInstallation
    {
        public override void Install(IObjectContainer container)
        {
            container.RegisterEntryPoint<Service>();
        }
    }
}
