#nullable enable
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal sealed class CreateComponentOnNewGameObjectStatement : INamedComponentInjection, IStatement
    {
        private readonly InstanceInjectionSource source;
        private readonly Lazy<IInstantiation> instantiationCache;
        
        public Lifetime Lifetime { get; }

        public CreateComponentOnNewGameObjectStatement(IObjectContainer container, Type type, Lifetime lifetime)
        {
            source = new InstanceInjectionSource(type, container);
            instantiationCache = new Lazy<IInstantiation>(CreateInstantiation);
            Lifetime = lifetime;
        }

        private IInstantiation CreateInstantiation()
        {
            var injection = CreateInjection();

            return new CreateComponentOnNewGameObject(ImplementedType, injection, parentTransform, objectName, dontDestroyOnLoad);
        }

        private IInjection? CreateInjection()
        {
            if (source.CanInjectIntoInstance(out var injection))
            {
                return injection;
            }

            return null;
        }

        public Type ImplementedType => source.ImplementedType;
        
        public IReadOnlyList<Type> AssignedTypeList => source.AssignedTypeList;
        
        public Ownership Ownership => Ownership.Internal;
        
        public IInstantiation Instantiation => instantiationCache.Value;
        
        private IObjectName objectName = ObjectNameToReturnNothing.Instance;
        public ICreatedComponentInjection Named(IObjectName name)
        {
            objectName = name;

            return this;
        }
        
        private IParentTransform parentTransform = ParentTransformToReturnNothing.Instance;
        public IInstanceInjection Under(IParentTransform parent)
        {
            parentTransform = parent;

            return this;
        }
        
        private bool dontDestroyOnLoad;
        public IInstanceInjection DontDestroyOnLoad()
        {
            dontDestroyOnLoad = true;

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

        public IParameterMethodInjection WithMethod(IParameter parameter)
        {
            return source.WithMethod(parameter);
        }

        public ITypeAssignment WithMethodInjection()
        {
            return source.WithMethodInjection();
        }

        public IParameterPropertyInjection WithProperty(IParameter parameter)
        {
            return source.WithProperty(parameter);
        }

        public IMethodInjection WithPropertyInjection()
        {
            return source.WithPropertyInjection();
        }

        public IParameterFieldInjection WithField(IParameter parameter)
        {
            return source.WithField(parameter);
        }

        public IPropertyInjection WithFieldInjection()
        {
            return source.WithFieldInjection();
        }

        public IFieldInjection ResolvedImmediately()
        {
            return source.ResolvedImmediately();
        }
    }
}
