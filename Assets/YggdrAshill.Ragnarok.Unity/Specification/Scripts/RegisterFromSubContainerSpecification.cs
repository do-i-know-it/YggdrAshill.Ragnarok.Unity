#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class RegisterFromSubContainerSpecification : MonoInstallation
    {
        [SerializeField] private GameObjectLifecycle? senderPrefab;
        private GameObjectLifecycle SenderPrefab
        {
            get
            {
                if (senderPrefab == null)
                {
                    throw new InvalidOperationException($"{nameof(senderPrefab)} is null.");
                }

                return senderPrefab;
            }
        }
        
        [SerializeField] private GameObjectLifecycle? receiverPrefab;
        private GameObjectLifecycle ReceiverPrefab
        {
            get
            {
                if (receiverPrefab == null)
                {
                    throw new InvalidOperationException($"{nameof(receiverPrefab)} is null.");
                }

                return receiverPrefab;
            }
        }
        
        [SerializeField] private GameObjectLifecycle? componentPrefab;
        private GameObjectLifecycle ComponentPrefab
        {
            get
            {
                if (componentPrefab == null)
                {
                    throw new InvalidOperationException($"{nameof(componentPrefab)} is null.");
                }

                return componentPrefab;
            }
        }
        
        public override void Install(IObjectContainer container)
        {
            container.RegisterFromSubContainer<NoDependencyComponent>(ComponentPrefab);
            container.RegisterFromSubContainer<SenderComponent>(SenderPrefab, () => transform.parent);
            container.RegisterFromSubContainer<ReceiverComponent>(ReceiverPrefab, transform.parent);
        }
    }
}
