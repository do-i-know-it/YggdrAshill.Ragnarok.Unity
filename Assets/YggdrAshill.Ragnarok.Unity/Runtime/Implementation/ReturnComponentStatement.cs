#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReturnComponentStatement : IStatement
    {
        private readonly Component component;
        private readonly Lazy<IInstantiation> instantiation;
        public InstanceInjectionSource Source { get; }

        public ReturnComponentStatement(Component component, IObjectContainer container)
        {
            this.component = component;
            Source = new InstanceInjectionSource(component.GetType(), container);
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
        }
        
        private IInstantiation CreateInstantiation()
        {
            var returnComponent = new ReturnComponent(component);
            
            if (!Source.CanInjectIntoInstance(out var injection))
            {
                return returnComponent;
            }

            return injection.ToInstantiate(returnComponent);
        }

        public Type ImplementedType => Source.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => Source.AssignedTypeList;
        public Lifetime Lifetime => Lifetime.Global;
        public Ownership Ownership => Ownership.External;
        public IInstantiation Instantiation => instantiation.Value;
    }
}
