#nullable enable
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToCreateFactoryOnNewGameObject<T> : IInstantiation
        where T : notnull
    {
        private readonly IAnchorTransform anchorTransform;
        private readonly Ownership ownership;
        private readonly IReadOnlyList<IInstallation> installationList;

        public InstantiateToCreateFactoryOnNewGameObject(IAnchorTransform anchorTransform, IReadOnlyList<IInstallation> installationList, Ownership ownership)
        {
            this.anchorTransform = anchorTransform;
            this.installationList = installationList;
            this.ownership = ownership;
        }
        
        public object Instantiate(IObjectResolver resolver)
        {
            return ownership switch
            {
                Ownership.Internal => new InternalFactoryFromNewGameObject<T>(anchorTransform, installationList),
                Ownership.External => new ExternalFactoryFromNewGameObject<T>(anchorTransform, installationList),
                _ => throw new InvalidOperationException($"{ownership} is invalid."),
            };
        }
    }
    
    internal sealed class InstantiateToCreateFactoryOnNewGameObject<TInput, TOutput> : IInstantiation
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly IAnchorTransform anchorTransform;
        private readonly Ownership ownership;
        private readonly IReadOnlyList<IInstallation> installationList;

        public InstantiateToCreateFactoryOnNewGameObject(IAnchorTransform anchorTransform, IReadOnlyList<IInstallation> installationList, Ownership ownership)
        {
            this.anchorTransform = anchorTransform;
            this.installationList = installationList;
            this.ownership = ownership;
        }
        
        public object Instantiate(IObjectResolver resolver)
        {
            return ownership switch
            {
                Ownership.Internal => new InternalFactoryFromNewGameObject<TInput, TOutput>(anchorTransform, installationList),
                Ownership.External => new ExternalFactoryFromNewGameObject<TInput, TOutput>(anchorTransform, installationList),
                _ => throw new InvalidOperationException($"{ownership} is invalid."),
            };
        }
    }
}
