#nullable enable
namespace YggdrAshill.Ragnarok
{
    internal sealed class ReturnComponent : IInstantiation
    {
        private readonly object component;
        private readonly IInjection? injection;

        public ReturnComponent(object component, IInjection? injection)
        {
            this.component = component;
            this.injection = injection;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            injection?.Inject(resolver, component);

            return component;
        }
    }
}
