#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class ReturnComponentInGameObjectStatement : ISearchedComponentInjection, IStatement
    {
        private readonly GameObject instance;
        private readonly SearchOrder order;
        private readonly InstanceInjectionSource source;
        private readonly Lazy<IInstantiation> instantiation;

        public ReturnComponentInGameObjectStatement(ICompilation compilation, Type type, GameObject instance, SearchOrder order)
        {
            this.instance = instance;
            this.order = order;
            source = new InstanceInjectionSource(type, compilation);
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
        }

        private IInstantiation CreateInstantiation()
        {
            var injection = CreateInjection();

            return order switch
            {
                SearchOrder.Children => new ReturnComponentInChildren(instance, ImplementedType, includeInactive, injection),
                SearchOrder.Parent => new ReturnComponentInParent(instance, ImplementedType, includeInactive, injection),
                SearchOrder.Scene => new ReturnComponentInScene(instance, ImplementedType, includeInactive, injection),
                _ => throw new NotSupportedException($"{order} is invalid."),
            };
        }

        private IInjection? CreateInjection()
        {
            if (source.CanInjectIntoInstance(out var injection))
            {
                return injection;
            }

            return null;
        }

        private bool includeInactive;

        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;
        
        public Lifetime Lifetime => Lifetime.Global;
        
        public Ownership Ownership => Ownership.External;

        public IInstantiation Instantiation => instantiation.Value;
        
        public void AsOwnSelf()
        {
            source.AsOwnSelf();
        }

        public IInheritedTypeAssignment As(Type inheritedType)
        {
            return source.As(inheritedType);
        }

        public IOwnTypeAssignment AsImplementedInterfaces()
        {
            return source.AsImplementedInterfaces();
        }

        public IMethodInjection WithMethod(IParameter parameter)
        {
            return source.WithMethod(parameter);
        }

        public IMethodInjection WithMethodInjection()
        {
            return source.WithMethodInjection();
        }

        public IPropertyInjection WithProperty(IParameter parameter)
        {
            return source.WithProperty(parameter);
        }

        public IPropertyInjection WithPropertyInjection()
        {
            return source.WithPropertyInjection();
        }

        public IFieldInjection WithField(IParameter parameter)
        {
            return source.WithField(parameter);
        }

        public IFieldInjection WithFieldInjection()
        {
            return source.WithFieldInjection();
        }

        public IInstanceInjection IncludeInactive()
        {
            includeInactive = true;

            return this;
        }
    }
}
