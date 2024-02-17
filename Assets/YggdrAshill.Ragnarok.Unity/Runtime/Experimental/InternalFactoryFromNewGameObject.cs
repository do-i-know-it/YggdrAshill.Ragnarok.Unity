#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InternalFactoryFromNewGameObject<T> : IFactory<T>, IDisposable
        where T : notnull
    {
        private readonly IAnchorTransform anchorTransform;
        private readonly IReadOnlyList<IInstallation> installationList;

        public InternalFactoryFromNewGameObject(IAnchorTransform anchorTransform, IReadOnlyList<IInstallation> installationList)
        {
            this.anchorTransform = anchorTransform;
            this.installationList = installationList;
        }
        
        private readonly List<IDisposable> disposableList = new();

        public T Create()
        {
            var parent = anchorTransform.GetAnchorTransform();
            
            var instance = GameObjectLifecycle.Create(parent, installationList);

            disposableList.Add(instance);

            return instance.Resolver.Resolve<T>();
        }

        public void Dispose()
        {
            foreach (var disposable in disposableList)
            {
                disposable.Dispose();
            }

            disposableList.Clear();
        }
    }
    
    internal sealed class InternalFactoryFromNewGameObject<TInput, TOutput> : IFactory<TInput, TOutput>, IDisposable
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly IAnchorTransform anchorTransform;
        private readonly IEnumerable<IInstallation> installationList;
        
        public InternalFactoryFromNewGameObject(IAnchorTransform anchorTransform, IEnumerable<IInstallation> installationList)
        {
            this.anchorTransform = anchorTransform;
            this.installationList = installationList;
        }
        
        private readonly List<IDisposable> disposableList = new();

        public TOutput Create(TInput input)
        {
            var additionalInstallation = new InstallToRegisterInstance<TInput>(input);

            var totalInstallationList = installationList.Append(additionalInstallation).ToArray();
            
            var parent = anchorTransform.GetAnchorTransform();
            
            var instance = GameObjectLifecycle.Create(parent, totalInstallationList);
            
            disposableList.Add(instance);

            return instance.Resolver.Resolve<TOutput>();
        }
        
        public void Dispose()
        {
            foreach (var disposable in disposableList)
            {
                disposable.Dispose();
            }

            disposableList.Clear();
        }
    }
}
