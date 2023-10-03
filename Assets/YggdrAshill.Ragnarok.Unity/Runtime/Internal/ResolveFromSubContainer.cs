#nullable enable
using System;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ResolveFromSubContainer : IInstantiation
    {
        private readonly Type type;
        private readonly IObjectResolver otherResolver;

        public ResolveFromSubContainer(Type type, IObjectResolver otherResolver)
        {
            this.type = type;
            this.otherResolver = otherResolver;
        }

        public object Instantiate(IObjectResolver resolver)
        {
            return otherResolver.Resolve(type);
        }
    }
}
