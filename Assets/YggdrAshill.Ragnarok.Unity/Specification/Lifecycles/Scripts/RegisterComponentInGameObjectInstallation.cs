#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class RegisterComponentInGameObjectInstallation : MonoInstallation
    {
        [SerializeField] private GameObject? senderParent;
        private GameObject SenderParent
        {
            get
            {
                if (senderParent == null)
                {
                    throw new InvalidOperationException($"{nameof(senderParent)} is null.");
                }

                return senderParent;
            }
        }
        
        [SerializeField] private GameObject? receiverChild;
        private GameObject ReceiverChild
        {
            get
            {
                if (receiverChild == null)
                {
                    throw new InvalidOperationException($"{nameof(receiverChild)} is null.");
                }

                return receiverChild;
            }
        }
        
        [SerializeField] private GameObject? handler;
        private GameObject Handler
        {
            get
            {
                if (handler == null)
                {
                    throw new InvalidOperationException($"{nameof(handler)} is null.");
                }

                return handler;
            }
        }

        public override void Install(IObjectContainer container)
        {
            container.RegisterComponentInHierarchy<IRequestSender>(SenderParent, SearchOrder.Children);
            container.RegisterComponentInHierarchy<IResponseReceiver>(ReceiverChild, SearchOrder.Parent);
            container.RegisterComponentInHierarchy<IServiceHandler>(Handler, SearchOrder.Scene);
        }
    }
}
