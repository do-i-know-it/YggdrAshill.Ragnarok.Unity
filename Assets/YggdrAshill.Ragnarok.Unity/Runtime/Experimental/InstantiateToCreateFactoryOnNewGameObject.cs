#nullable enable
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class InstantiateToCreateFactoryOnNewGameObject<T> : IInstantiation
        where T : notnull
    {
        private readonly IParentTransform parentTransform;
        private readonly Ownership ownership;
        private readonly IReadOnlyList<IInstallation> installationList;

        public InstantiateToCreateFactoryOnNewGameObject(IParentTransform parentTransform, IReadOnlyList<IInstallation> installationList, Ownership ownership)
        {
            this.parentTransform = parentTransform;
            this.installationList = installationList;
            this.ownership = ownership;
        }
        
        public object Instantiate(IObjectResolver resolver)
        {
            return ownership switch
            {
                Ownership.Internal => new InternalFactoryFromNewGameObject<T>(parentTransform, installationList),
                Ownership.External => new ExternalFactoryFromNewGameObject<T>(parentTransform, installationList),
                _ => throw new InvalidOperationException($"{ownership} is invalid."),
            };
        }
    }
    
    internal sealed class InstantiateToCreateFactoryOnNewGameObject<TInput, TOutput> : IInstantiation
        where TInput : notnull
        where TOutput : notnull
    {
        private readonly IParentTransform parentTransform;
        private readonly Ownership ownership;
        private readonly IReadOnlyList<IInstallation> installationList;

        public InstantiateToCreateFactoryOnNewGameObject(IParentTransform parentTransform, IReadOnlyList<IInstallation> installationList, Ownership ownership)
        {
            this.parentTransform = parentTransform;
            this.installationList = installationList;
            this.ownership = ownership;
        }
        
        public object Instantiate(IObjectResolver resolver)
        {
            return ownership switch
            {
                Ownership.Internal => new InternalFactoryFromNewGameObject<TInput, TOutput>(parentTransform, installationList),
                Ownership.External => new ExternalFactoryFromNewGameObject<TInput, TOutput>(parentTransform, installationList),
                _ => throw new InvalidOperationException($"{ownership} is invalid."),
            };
        }
    }
}
