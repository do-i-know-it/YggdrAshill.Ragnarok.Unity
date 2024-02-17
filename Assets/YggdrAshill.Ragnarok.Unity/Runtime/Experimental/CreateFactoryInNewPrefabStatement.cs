#nullable enable
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CreateFactoryInNewPrefabStatement<T> : IUnityFactoryResolution, IStatement
        where T : notnull
    {
        private readonly GameObjectLifecycle prefab;
        private readonly Lazy<IInstantiation> instantiation;
        private readonly FactoryResolutionSource source;
        
        public Ownership Ownership { get; }

        public CreateFactoryInNewPrefabStatement(IObjectContainer container, GameObjectLifecycle prefab, Ownership ownership)
        {
            this.prefab = prefab;
            source = new FactoryResolutionSource(container.Resolver);
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
            Ownership = ownership;
        }

        private IInstantiation CreateInstantiation()
        {
            return new InstantiateToCreateFactoryInNewPrefab<T>(prefab, anchorTransform, source.InstallationList, Ownership);
        }
        
        private IAnchorTransform anchorTransform = NoParent.Instance;
        
        public IFactoryResolution Under(IAnchorTransform transform)
        {
            anchorTransform = transform;

            return this;
        }

        public Type ImplementedType { get; } = typeof(IFactory<T>);

        public IReadOnlyList<Type> AssignedTypeList { get; } = Array.Empty<Type>();

        public Lifetime Lifetime => Lifetime.Global;
        
        public IInstantiation Instantiation => instantiation.Value;

        public IObjectResolver Resolver => source.Resolver;
        
        public IFactoryResolution With(IInstallation installation)
        {
            return source.With(installation);
        }
    }
    
    internal sealed class CreateFactoryInNewPrefabStatement<TInput, TOutput> : IUnityFactoryResolution, IStatement
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly GameObjectLifecycle prefab;
        private readonly Lazy<IInstantiation> instantiation;
        private readonly FactoryResolutionSource source;
        
        public Ownership Ownership { get; }

        public CreateFactoryInNewPrefabStatement(IObjectContainer container, GameObjectLifecycle prefab, Ownership ownership)
        {
            this.prefab = prefab;
            source = new FactoryResolutionSource(container.Resolver);
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
            Ownership = ownership;
        }

        private IInstantiation CreateInstantiation()
        {
            return new InstantiateToCreateFactoryInNewPrefab<TInput, TOutput>(prefab, anchorTransform, source.InstallationList, Ownership);
        }
        
        private IAnchorTransform anchorTransform = NoParent.Instance;
        
        public IFactoryResolution Under(IAnchorTransform transform)
        {
            anchorTransform = transform;

            return this;
        }

        public Type ImplementedType { get; } = typeof(IFactory<TInput, TOutput>);

        public IReadOnlyList<Type> AssignedTypeList { get; } = Array.Empty<Type>();

        public Lifetime Lifetime => Lifetime.Global;
        
        public IInstantiation Instantiation => instantiation.Value;

        public IObjectResolver Resolver => source.Resolver;
        
        public IFactoryResolution With(IInstallation installation)
        {
            return source.With(installation);
        }
    }
}
