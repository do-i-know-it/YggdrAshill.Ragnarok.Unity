#nullable enable
namespace YggdrAshill.Ragnarok.Unity
{
    internal sealed class ReturnComponentDirectly :
        IInstantiation
    {
        private readonly object component;
        private readonly IInjection? injection;

        public ReturnComponentDirectly(object component, IInjection? injection)
        {
            this.component = component;
            this.injection = injection;
        }

        public object Instantiate(IResolver resolver)
        {
            injection?.Inject(resolver, component);

            return component;
        }
    }
}
