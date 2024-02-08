#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class PropertyInjectionServiceComponent : MonoBehaviour
    {
        [InjectProperty] private IRequestSender? Sender { get; set; }
        [InjectProperty] private IResponseReceiver? Receiver { get; set; }
        [InjectProperty] private IServiceHandler? Handler { get; set; }

        private void Awake()
        {
            Debug.LogError($"{nameof(PropertyInjectionServiceComponent)}.{nameof(Awake)}");

            if (Sender == null)
            {
                throw new InvalidOperationException($"{nameof(Sender)} is null.");
            }
            if (Receiver == null)
            {
                throw new InvalidOperationException($"{nameof(Receiver)} is null.");
            }
            if (Handler == null)
            {
                throw new InvalidOperationException($"{nameof(Handler)} is null.");
            }
            
            var request = Sender.Send();

            var response = Handler.Handle(request);
            
            Receiver.Receive(response);
        }
    }
}
