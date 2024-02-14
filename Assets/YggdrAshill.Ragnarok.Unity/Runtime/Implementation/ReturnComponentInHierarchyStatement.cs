#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ReturnComponentInHierarchyStatement : ISearchedComponentInjection, IStatement
    {
        private readonly GameObject instance;
        private readonly SearchOrder order;
        private readonly InstanceInjectionSource source;
        private readonly Lazy<IInstantiation> instantiation;

        public ReturnComponentInHierarchyStatement(IObjectContainer container, Type type, GameObject instance, SearchOrder order)
        {
            this.instance = instance;
            this.order = order;
            source = new InstanceInjectionSource(type, container);
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
        }

        private IInstantiation CreateInstantiation()
        {
            var injection = CreateInjection();

            return order switch
            {
                SearchOrder.Children => new InstantiateToReturnComponentInChildren(instance, ImplementedType, includeInactive, injection),
                SearchOrder.Parent => new InstantiateToReturnComponentInParent(instance, ImplementedType, includeInactive, injection),
                SearchOrder.Scene => new InstantiateToReturnComponentInScene(instance, ImplementedType, includeInactive, injection),
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
        
        public IInstanceInjection IncludeInactive()
        {
            includeInactive = true;

            return this;
        }
        
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

        public IParameterMethodInjection WithMethod(IParameter parameter)
        {
            return source.WithMethod(parameter);
        }

        public ITypeAssignment WithMethodInjection()
        {
            return source.WithMethodInjection();
        }

        public IParameterPropertyInjection WithProperty(IParameter parameter)
        {
            return source.WithProperty(parameter);
        }

        public IMethodInjection WithPropertyInjection()
        {
            return source.WithPropertyInjection();
        }

        public IParameterFieldInjection WithField(IParameter parameter)
        {
            return source.WithField(parameter);
        }

        public IPropertyInjection WithFieldInjection()
        {
            return source.WithFieldInjection();
        }

        public IFieldInjection ResolvedImmediately()
        {
            return source.ResolvedImmediately();
        }
    }
}
