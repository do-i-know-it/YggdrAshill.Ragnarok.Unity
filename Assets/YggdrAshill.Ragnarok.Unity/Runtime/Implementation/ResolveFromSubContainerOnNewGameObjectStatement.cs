#nullable enable
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ResolveFromSubContainerOnNewGameObjectStatement : IUnitySubContainerResolution, IStatement
    {
        private readonly IRegistration registration;
        private readonly Lazy<IInstantiation> instantiation;
        private readonly TypeAssignmentSource source;
        
        public IObjectResolver Resolver { get; }

        public ResolveFromSubContainerOnNewGameObjectStatement(Type type, IObjectContainer container)
        {
            registration = container.Registration;
            source = new TypeAssignmentSource(type);
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
            Resolver = container.Resolver;
        }
        
        private IInstantiation CreateInstantiation()
        {
            var parent = parentTransform?.GetParentTransform();

            var instance = GameObjectLifecycle.Create(parent, installationList.ToArray());
            
            registration.Register(instance);

            return new ResolveFromSubContainer(ImplementedType, instance.Resolver);
        }

        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;
        
        public Lifetime Lifetime => Lifetime.Temporal;

        public Ownership Ownership => Ownership.External;

        public IInstantiation Instantiation => instantiation.Value;
        
        private readonly List<IInstallation> installationList = new();
        public ISubContainerResolution With(IInstallation installation)
        {
            if (!installationList.Contains(installation))
            {
                installationList.Add(installation);
            }

            return this;
        }

        private IParentTransform parentTransform = ParentTransformToReturnNothing.Instance;
        public ITypeAssignment Under(IParentTransform parent)
        {
            parentTransform = parent;

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
