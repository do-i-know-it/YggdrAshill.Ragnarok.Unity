#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReturnComponent : IInstantiation
    {
        private readonly Component component;

        public ReturnComponent(Component component)
        {
            this.component = component;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return component;
        }
    }
}
