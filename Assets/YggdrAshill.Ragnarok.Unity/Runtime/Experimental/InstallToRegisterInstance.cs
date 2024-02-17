#nullable enable
namespace YggdrAshill.Ragnarok
{
    internal sealed class InstallToRegisterInstance<T> : IInstallation
        where T : notnull
    {
        private readonly T instance;

        public InstallToRegisterInstance(T instance)
        {
            this.instance = instance;
        }
        
        public void Install(IObjectContainer container)
        {
            container.Register(() => instance, Lifetime.Global, Ownership.External);
        }
    }
}
