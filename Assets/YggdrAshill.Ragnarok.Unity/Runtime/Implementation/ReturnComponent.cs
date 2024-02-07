#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReturnComponent : IInstantiation
    {
        private readonly Component component;
        private readonly IInjection? injection;

        public ReturnComponent(Component component, IInjection? injection)
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
