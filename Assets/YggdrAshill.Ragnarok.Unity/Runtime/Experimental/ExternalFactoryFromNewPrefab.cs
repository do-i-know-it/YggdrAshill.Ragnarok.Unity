#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ExternalFactoryFromNewPrefab<T> : IFactory<T>
        where T : notnull
    {
        private readonly GameObjectLifecycle prefab;
        private readonly IAnchorTransform anchorTransform;
        private readonly IReadOnlyList<IInstallation> installationList;

        public ExternalFactoryFromNewPrefab(GameObjectLifecycle prefab, IAnchorTransform anchorTransform, IReadOnlyList<IInstallation> installationList)
        {
            this.prefab = prefab;
            this.anchorTransform = anchorTransform;
            this.installationList = installationList;
        }
        
        public T Create()
        {
            var parent = anchorTransform.GetAnchorTransform();
            
            var instance = GameObjectLifecycle.Create(prefab, parent, installationList);

            return instance.Resolver.Resolve<T>();
        }
    }

    internal sealed class ExternalFactoryFromNewPrefab<TInput, TOutput> : IFactory<TInput, TOutput>
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly GameObjectLifecycle prefab;
        private readonly IAnchorTransform anchorTransform;
        private readonly IEnumerable<IInstallation> installationList;
        
        public ExternalFactoryFromNewPrefab(GameObjectLifecycle prefab, IAnchorTransform anchorTransform, IEnumerable<IInstallation> installationList)
        {
            this.prefab = prefab;
            this.anchorTransform = anchorTransform;
            this.installationList = installationList;
        }
        
        public TOutput Create(TInput input)
        {
            var additionalInstallation = new InstallToRegisterInstance<TInput>(input);

            var totalInstallationList = installationList.Append(additionalInstallation).ToArray();
            
            var parent = anchorTransform.GetAnchorTransform();
            
            var instance = GameObjectLifecycle.Create(prefab, parent, totalInstallationList);

            return instance.Resolver.Resolve<TOutput>();
        }
    }
}
