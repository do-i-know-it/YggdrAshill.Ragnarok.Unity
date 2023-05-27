#nullable enable
using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Fabrication;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Unity.Internal
{
    // TODO: add document comments.
    public sealed class NewPrefabStatement :
        IComponentInjection,
        IStatement
    {
        private readonly Component prefab;
        private readonly InstanceInjection instanceInjection;

        public NewPrefabStatement(ICompilation compilation, Component prefab)
        {
            this.prefab = prefab;
            instanceInjection = new InstanceInjection(compilation, prefab.GetType());
        }
        
        private IAnchor? anchor;
        private bool dontDestroyOnLoad;
        
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
        
        public IInstanceInjection Under(IAnchor anchor)
        {
            this.anchor = anchor;

            return this;
        }
        public IInstanceInjection DontDestroyOnLoad()
        {
            dontDestroyOnLoad = true;

            return this;
        }

        private IInstantiation? instantiation;
        public IInstantiation Instantiation
        {
            get
            {
                if (instantiation == null)
                {
                    var injection = instanceInjection.GetInjection();

                    instantiation = new InstantiateNewPrefab(prefab, injection, anchor, dontDestroyOnLoad);
                }

                return instantiation;
            }
        }
    }
}
