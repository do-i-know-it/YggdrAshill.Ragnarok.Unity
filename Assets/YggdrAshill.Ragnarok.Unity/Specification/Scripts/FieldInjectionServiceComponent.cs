#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class FieldInjectionServiceComponent : MonoBehaviour
    {
        [InjectField] private IRequestSender? sender;
        private IRequestSender Sender
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

        [InjectField] private IResponseReceiver? receiver;
        private IResponseReceiver Receiver
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

        [InjectField] private IServiceHandler? handler;
        private IServiceHandler Handler
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

        private void Awake()
        {
            Debug.LogError($"{nameof(FieldInjectionServiceComponent)}.{nameof(Awake)}");

            var request = Sender.Send();

            var response = Handler.Handle(request);
            
            Receiver.Receive(response);
        }
    }
}
