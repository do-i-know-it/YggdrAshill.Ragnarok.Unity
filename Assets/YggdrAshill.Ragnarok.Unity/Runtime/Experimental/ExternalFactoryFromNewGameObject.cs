#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ExternalFactoryFromNewGameObject<T> : IFactory<T>
        where T : notnull
    {
        private readonly IParentTransform parentTransform;
        private readonly IReadOnlyList<IInstallation> installationList;

        public ExternalFactoryFromNewGameObject(IParentTransform parentTransform, IReadOnlyList<IInstallation> installationList)
        {
            this.parentTransform = parentTransform;
            this.installationList = installationList;
        }
        
        public T Create()
        {
            var parent = parentTransform.GetParentTransform();
            
            var instance = GameObjectLifecycle.Create(parent, installationList);

            return instance.Resolver.Resolve<T>();
        }
    }
    
    internal sealed class ExternalFactoryFromNewGameObject<TInput, TOutput> : IFactory<TInput, TOutput>
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly IParentTransform parentTransform;
        private readonly IEnumerable<IInstallation> installationList;
        
        public ExternalFactoryFromNewGameObject(IParentTransform parentTransform, IEnumerable<IInstallation> installationList)
        {
            this.parentTransform = parentTransform;
            this.installationList = installationList;
        }
        
        public TOutput Create(TInput input)
        {
            var additionalInstallation = new InstallToRegisterInstance<TInput>(input);

            var totalInstallationList = installationList.Append(additionalInstallation).ToArray();
            
            var parent = parentTransform.GetParentTransform();
            
            var instance = GameObjectLifecycle.Create(parent, totalInstallationList);

            return instance.Resolver.Resolve<TOutput>();
        }
    }
}
