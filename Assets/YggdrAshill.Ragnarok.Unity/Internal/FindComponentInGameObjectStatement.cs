#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Internal
{
    // TODO: add document comments.
    public sealed class FindComponentInGameObjectStatement :
        IInstanceInjection,
        IStatement
    {
        private readonly GameObject instance;
        private readonly InstanceInjection instanceInjection;

        public FindComponentInGameObjectStatement(ICompilation compilation, Type componentType, GameObject instance)
        {
            this.instance = instance;
            instanceInjection = new InstanceInjection(compilation, componentType);
        }

        public Type ImplementedType => instanceInjection.ImplementedType;
        public IReadOnlyList<Type> AssignedTypeList => instanceInjection.AssignedTypeList;
        
        public void AsSelf()
        {
            instanceInjection.AsSelf();
        }
        public IAssignImplementedInterface As(Type implementedInterface)
        {
            return instanceInjection.As(implementedInterface);
        }
        public IAssignImplementedType AsImplementedInterfaces()
        {
            return instanceInjection.AsImplementedInterfaces();
        }

        public IInjectIntoMethodExternally WithMethodInjected()
        {
            return instanceInjection.WithMethodInjected();
        }
        public IInjectIntoPropertiesExternally WithPropertiesInjected()
        {
            return instanceInjection.WithPropertiesInjected();
        }
        public IInjectIntoFieldsExternally WithFieldsInjected()
        {
            return instanceInjection.WithFieldsInjected();
        }

        private IInstantiation? instantiation;
        public IInstantiation Instantiation
        {
            get
            {
                if (instantiation == null)
                {
                    var injection = instanceInjection.GetInjection();
                    
                    instantiation = new FindComponentInGameObject(ImplementedType, instance, injection);
                }

                return instantiation;
            }
        }
    }
}
