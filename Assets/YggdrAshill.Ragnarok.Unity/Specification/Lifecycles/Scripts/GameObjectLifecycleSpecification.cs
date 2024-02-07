#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class GameObjectLifecycleSpecification : MonoInstallation
    {
        [SerializeField] private GameObjectLifecycle? fieldInjectionServicePrefab;
        private GameObjectLifecycle FieldInjectionServicePrefab
        {
            get
            {
                if (fieldInjectionServicePrefab == null)
                {
                    throw new InvalidOperationException($"{nameof(fieldInjectionServicePrefab)} is null.");
                }

                return fieldInjectionServicePrefab;
            }
        }
        
        [SerializeField] private GameObjectLifecycle? propertyInjectionServicePrefab;
        private GameObjectLifecycle PropertyInjectionServicePrefab
        {
            get
            {
                if (propertyInjectionServicePrefab == null)
                {
                    throw new InvalidOperationException($"{nameof(propertyInjectionServicePrefab)} is null.");
                }

                return propertyInjectionServicePrefab;
            }
        }
        
        [SerializeField] private GameObjectLifecycle? methodInjectionServicePrefab;
        private GameObjectLifecycle MethodInjectionServicePrefab
        {
            get
            {
                if (methodInjectionServicePrefab == null)
                {
                    throw new InvalidOperationException($"{nameof(methodInjectionServicePrefab)} is null.");
                }

                return methodInjectionServicePrefab;
            }
        }
        
        public override void Install(IObjectContainer container)
        {
            container.RegisterEntryPoint<Service>();
            container.RegisterFromSubContainer<FieldInjectionServiceComponent>(FieldInjectionServicePrefab);
            container.RegisterFromSubContainer<PropertyInjectionServiceComponent>(PropertyInjectionServicePrefab, () => transform.parent);
            container.RegisterFromSubContainer<MethodInjectionServiceComponent>(MethodInjectionServicePrefab, transform.parent);
        }
    }
}
