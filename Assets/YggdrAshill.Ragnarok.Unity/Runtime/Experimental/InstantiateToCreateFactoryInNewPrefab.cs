#nullable enable
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToCreateFactoryInNewPrefab<T> : IInstantiation
        where T : notnull
    {
        private readonly GameObjectLifecycle prefab;
        private readonly IAnchorTransform anchorTransform;
        private readonly Ownership ownership;
        private readonly IReadOnlyList<IInstallation> installationList;

        public InstantiateToCreateFactoryInNewPrefab(GameObjectLifecycle prefab, IAnchorTransform anchorTransform, IReadOnlyList<IInstallation> installationList, Ownership ownership)
        {
            this.prefab = prefab;
            this.anchorTransform = anchorTransform;
            this.installationList = installationList;
            this.ownership = ownership;
        }
        
        public object Instantiate(IObjectResolver resolver)
        {
            return ownership switch
            {
                Ownership.Internal => new InternalFactoryFromNewPrefab<T>(prefab, anchorTransform, installationList),
                Ownership.External => new ExternalFactoryFromNewPrefab<T>(prefab, anchorTransform, installationList),
                _ => throw new InvalidOperationException($"{ownership} is invalid."),
            };
        }
    }
    
    internal sealed class InstantiateToCreateFactoryInNewPrefab<TInput, TOutput> : IInstantiation
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly GameObjectLifecycle prefab;
        private readonly IAnchorTransform anchorTransform;
        private readonly Ownership ownership;
        private readonly IReadOnlyList<IInstallation> installationList;

        public InstantiateToCreateFactoryInNewPrefab(GameObjectLifecycle prefab, IAnchorTransform anchorTransform, IReadOnlyList<IInstallation> installationList, Ownership ownership)
        {
            this.prefab = prefab;
            this.anchorTransform = anchorTransform;
            this.installationList = installationList;
            this.ownership = ownership;
        }
        
        public object Instantiate(IObjectResolver resolver)
        {
            return ownership switch
            {
                Ownership.Internal => new InternalFactoryFromNewPrefab<TInput, TOutput>(prefab, anchorTransform, installationList),
                Ownership.External => new ExternalFactoryFromNewPrefab<TInput, TOutput>(prefab, anchorTransform, installationList),
                _ => throw new InvalidOperationException($"{ownership} is invalid."),
            };
        }
    }
}
