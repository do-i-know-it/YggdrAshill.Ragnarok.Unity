#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReturnComponentStatement : IStatement
    {
        private readonly Component component;
        private readonly InstanceInjectionSource source;
        private readonly Lazy<IInstantiation> instantiation;

        public ReturnComponentStatement(Component component, ICompilation compilation)
        {
            this.component = component;
            source = new InstanceInjectionSource(component.GetType(), compilation);
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
        }
        
        private IInstantiation CreateInstantiation()
        {
            var injection = CreateInjection();

            return new ReturnComponent(component, injection);
        }

        private IInjection? CreateInjection()
        {
            if (source.CanInjectIntoInstance(out var injection))
            {
                return injection;
            }

            return null;
        }

        public IInstanceInjection InstanceInjection => source;
        public Type ImplementedType => source.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;
        public Lifetime Lifetime => Lifetime.Global;
        public Ownership Ownership => Ownership.External;
        public IInstantiation Instantiation => instantiation.Value;
    }
}
