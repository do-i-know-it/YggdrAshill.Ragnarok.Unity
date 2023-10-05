#nullable enable
using YggdrAshill.Ragnarok.Experimental;
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class RegisterComponentSpecification : MonoInstallation
    {
        [SerializeField] private GameObject? sender;
        private GameObject Sender
        {
            get
            {
                if (sender == null)
                {
                    throw new InvalidOperationException($"{nameof(sender)} is null.");
                }

                return sender;
            }
        }

        [SerializeField] private SearchOrder senderOrder = SearchOrder.Children;
        
        [SerializeField] private GameObject? receiver;
        private GameObject Receiver
        {
            get
            {
                if (receiver == null)
                {
                    throw new InvalidOperationException($"{nameof(receiver)} is null.");
                }

                return receiver;
            }
        }
        [SerializeField] private SearchOrder receiverOrder = SearchOrder.Parent;
        
        [SerializeField] private GameObject? service;
        private GameObject Service
        {
            get
            {
                if (service == null)
                {
                    throw new InvalidOperationException($"{nameof(service)} is null.");
                }

                return service;
            }
        }
        
        [SerializeField] private SearchOrder serviceOrder = SearchOrder.Scene;

        public override void Install(IObjectContainer container)
        {
            container.RegisterComponent<SenderComponent>(Sender, senderOrder);
            container.RegisterComponent<ReceiverComponent>(Receiver, receiverOrder);
            container.RegisterComponent<ServiceComponent>(Service, serviceOrder).WithFieldInjection();
        }
    }
}
