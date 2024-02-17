#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ExternalFactoryFromNewPrefab<T> : IFactory<T>
        where T : notnull
    {
        private readonly GameObjectLifecycle prefab;
        private readonly IParentTransform parentTransform;
        private readonly IReadOnlyList<IInstallation> installationList;

        public ExternalFactoryFromNewPrefab(GameObjectLifecycle prefab, IParentTransform parentTransform, IReadOnlyList<IInstallation> installationList)
        {
            this.prefab = prefab;
            this.parentTransform = parentTransform;
            this.installationList = installationList;
        }
        
        public T Create()
        {
            var parent = parentTransform.GetParentTransform();
            
            var instance = GameObjectLifecycle.Create(prefab, parent, installationList);

            return instance.Resolver.Resolve<T>();
        }
    }

    internal sealed class ExternalFactoryFromNewPrefab<TInput, TOutput> : IFactory<TInput, TOutput>
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly GameObjectLifecycle prefab;
        private readonly IParentTransform parentTransform;
        private readonly IEnumerable<IInstallation> installationList;
        
        public ExternalFactoryFromNewPrefab(GameObjectLifecycle prefab, IParentTransform parentTransform, IEnumerable<IInstallation> installationList)
        {
            this.prefab = prefab;
            this.parentTransform = parentTransform;
            this.installationList = installationList;
        }
        
        public TOutput Create(TInput input)
        {
            var additionalInstallation = new InstallToRegisterInstance<TInput>(input);

            var totalInstallationList = installationList.Append(additionalInstallation).ToArray();
            
            var parent = parentTransform.GetParentTransform();
            
            var instance = GameObjectLifecycle.Create(prefab, parent, totalInstallationList);

            return instance.Resolver.Resolve<TOutput>();
        }
    }
}
