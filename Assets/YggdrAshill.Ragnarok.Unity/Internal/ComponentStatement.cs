#nullable enable
using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Fabrication;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Unity.Internal
{
    // TODO: add document comments.
    public sealed class ComponentStatement :
        IInstanceInjection,
        IStatement
    {
        private readonly object instance;
        private readonly InstanceInjection instanceInjection;

        public ComponentStatement(ICompilation compilation, object instance)
        {
            this.instance = instance;
            instanceInjection = new InstanceInjection(compilation, instance.GetType());
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

                    instantiation = new ReturnComponentDirectly(instance, injection);
                }

                return instantiation;
            }
        }
    }
}
