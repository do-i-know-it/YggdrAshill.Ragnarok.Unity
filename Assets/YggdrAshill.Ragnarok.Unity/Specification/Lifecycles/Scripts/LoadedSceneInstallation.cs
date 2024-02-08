#nullable enable
namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class LoadedSceneInstallation : MonoInstallation
    {
        public override void Install(IObjectContainer container)
        {
            container.RegisterEntryPoint<Service>();
            container.RegisterComponentOnNewGameObject<FieldInjectionServiceComponent>(Lifetime.Global).WithFieldInjection();
            container.RegisterComponentOnNewGameObject<PropertyInjectionServiceComponent>(Lifetime.Global).WithPropertyInjection();
            container.RegisterComponentOnNewGameObject<MethodInjectionServiceComponent>(Lifetime.Global).WithMethodInjection();
        }
    }
}
