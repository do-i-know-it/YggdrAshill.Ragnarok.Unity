#nullable enable
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ResolveFromSubContainerInNewPrefabStatement : IUnitySubContainerResolution, IStatement
    {
        private readonly IRegistration registration;
        private readonly GameObjectLifecycle prefab;
        private readonly Lazy<IInstantiation> instantiation;
        private readonly TypeAssignmentSource source;
        
        public IObjectResolver Resolver { get; }

        public ResolveFromSubContainerInNewPrefabStatement(Type type, GameObjectLifecycle prefab, IObjectContainer container)
        {
            this.prefab = prefab;
            registration = container.Registration;
            source = new TypeAssignmentSource(type);
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
            Resolver = container.Resolver;
        }

        private IInstantiation CreateInstantiation()
        {
            var parent = anchorTransform?.GetAnchorTransform();

            var instance = GameObjectLifecycle.Create(prefab, parent, installationList.ToArray());
            
            registration.Register(instance);

            return new ResolveFromSubContainer(ImplementedType, instance.Resolver);
        }
        
        private IAnchorTransform? anchorTransform;

        private readonly List<IInstallation> installationList = new();

        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;
        
        public Lifetime Lifetime => Lifetime.Temporal;

        public Ownership Ownership => Ownership.External;

        public IInstantiation Instantiation => instantiation.Value;
        
        public ISubContainerResolution With(IInstallation installation)
        {
            if (!installationList.Contains(installation))
            {
                installationList.Add(installation);
            }

            return this;
        }
        
        public ITypeAssignment Under(IAnchorTransform anchor)
        {
            anchorTransform = anchor;

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
    }
}
