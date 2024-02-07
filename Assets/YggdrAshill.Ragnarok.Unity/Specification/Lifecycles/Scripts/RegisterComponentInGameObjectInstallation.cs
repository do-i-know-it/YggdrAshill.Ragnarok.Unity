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
            container.RegisterComponentInGameObject<IRequestSender>(SenderParent, SearchOrder.Children);
            container.RegisterComponentInGameObject<IResponseReceiver>(ReceiverChild, SearchOrder.Parent);
            container.RegisterComponentInGameObject<IServiceHandler>(Handler, SearchOrder.Scene);
        }
    }
}
