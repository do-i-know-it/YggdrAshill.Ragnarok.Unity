#nullable enable
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public sealed class ResolveFromUnitySubContainerStatement : IStatement
    {
        private readonly IRegistration registration;
        private readonly GameObjectLifecycle prefab;
        private readonly IAnchor? anchor;
        private readonly TypeAssignmentSource source;
        private readonly Lazy<IInstantiation> instantiation;

        public ResolveFromUnitySubContainerStatement(IRegistration registration, Type type, GameObjectLifecycle prefab, IAnchor? anchor)
        {
            this.registration = registration;
            this.prefab = prefab;
            this.anchor = anchor;
            source = new TypeAssignmentSource(type);
            instantiation = new Lazy<IInstantiation>(CreateInstantiation);
        }

        private IInstantiation CreateInstantiation()
        {
            var parent = anchor?.GetTransform();

            var instance = GameObjectLifecycle.Create(prefab, parent);
            
            registration.Register(instance);

            // TODO: get instantiation from YggdrAshill.Ragnarok implementation.
            return new ResolveFromSubContainer(ImplementedType, instance.Resolver);
        }

        public ITypeAssignment TypeAssignment => source;
        
        public Type ImplementedType => source.ImplementedType;

        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;
        
        public Lifetime Lifetime => Lifetime.Temporal;

        public Ownership Ownership => Ownership.External;

        public IInstantiation Instantiation => instantiation.Value;
    }
}