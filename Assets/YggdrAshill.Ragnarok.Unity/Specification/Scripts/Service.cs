#nullable enable
using UnityEngine;
using UnityEngine.Scripting;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class Service : IInitializable
    {
        private readonly IRequestSender sender;
        private readonly IResponseReceiver receiver;
        private readonly IServiceHandler handler;

        [Preserve]
        [Inject]
        public Service(IRequestSender sender, IResponseReceiver receiver, IServiceHandler handler)
        {
            this.sender = sender;
            this.receiver = receiver;
            this.handler = handler;
        }
        
        public void Initialize()
        {
            Debug.LogError($"{nameof(Service)}.{nameof(Initialize)}");
            
            var request = sender.Send();

            var response = handler.Handle(request);
            
            receiver.Receive(response);
        }
    }
}
