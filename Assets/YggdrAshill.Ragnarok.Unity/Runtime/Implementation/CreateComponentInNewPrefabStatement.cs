#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CreateComponentInNewPrefabStatement : ICreatedComponentInjection, IStatement
    {
        private readonly Component prefab;
        private readonly InstanceInjectionSource source;
        private readonly Lazy<IInstantiation> instantiationCache;

        public Lifetime Lifetime { get; }

        public CreateComponentInNewPrefabStatement(IObjectContainer container, Lifetime lifetime, Component prefab)
        {
            this.prefab = prefab;
            source = new InstanceInjectionSource(prefab.GetType(), container);
            instantiationCache = new Lazy<IInstantiation>(CreateInstantiation);
            Lifetime = lifetime;
        }

        private IInstantiation CreateInstantiation()
        {
            var injection = CreateInjection();
            
            return new CreateComponentInNewPrefab(prefab, injection, parentTransform, dontDestroyOnLoad);
        }

        private IInjection? CreateInjection()
        {
            if (source.CanInjectIntoInstance(out var injection))
            {
                return injection;
            }

            return null;
        }
        
        public Type ImplementedType => source.ImplementedType;
        
        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;
        
        public Ownership Ownership => Ownership.Internal;
        
        public IInstantiation Instantiation => instantiationCache.Value;
        
        private IParentTransform parentTransform = ParentTransformToReturnNothing.Instance;
        public IInstanceInjection Under(IParentTransform parent)
        {
            parentTransform = parent;

            return this;
        }
        
        private bool dontDestroyOnLoad;
        public IInstanceInjection DontDestroyOnLoad()
        {
            dontDestroyOnLoad = true;

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
