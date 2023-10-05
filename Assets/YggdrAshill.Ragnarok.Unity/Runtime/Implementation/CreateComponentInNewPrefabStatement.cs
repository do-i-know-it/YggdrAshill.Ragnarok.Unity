#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class CreateComponentInNewPrefabStatement : ICreatedComponentInjection, IStatement
    {
        private readonly Component prefab;
        private readonly InstanceInjectionSource source;
        private readonly Lazy<IInstantiation> instantiationCache;

        public Lifetime Lifetime { get; }

        public CreateComponentInNewPrefabStatement(ICompilation compilation, Lifetime lifetime, Component prefab)
        {
            this.prefab = prefab;
            source = new InstanceInjectionSource(prefab.GetType(), compilation);
            instantiationCache = new Lazy<IInstantiation>(CreateInstantiation);
            Lifetime = lifetime;
        }

        private IInstantiation CreateInstantiation()
        {
            var injection = CreateInjection();
            
            return new CreateComponentInNewPrefab(prefab, injection, candidateAnchor, dontDestroyOnLoad);
        }

        private IInjection? CreateInjection()
        {
            if (source.CanInjectIntoInstance(out var injection))
            {
                return injection;
            }

            return null;
        }

        private IAnchor? candidateAnchor;
        private bool dontDestroyOnLoad;
        
        public Type ImplementedType => source.ImplementedType;
        
        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;
        
        public Ownership Ownership => Ownership.Internal;
        
        public IInstantiation Instantiation => instantiationCache.Value;

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
        
        public IInstanceInjection Under(IAnchor anchor)
        {
            candidateAnchor = anchor;

            return this;
        }
        
        public IInstanceInjection DontDestroyOnLoad()
        {
            dontDestroyOnLoad = true;

            return this;
        }
    }
}
