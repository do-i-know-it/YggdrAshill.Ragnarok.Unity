#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class MethodInjectionServiceComponent : MonoBehaviour
    {
        [InjectMethod]
        public void Construct(IRequestSender sender, IResponseReceiver receiver, IServiceHandler handler)
        {
            Debug.LogError($"{nameof(MethodInjectionServiceComponent)}.{nameof(Construct)}");
            
            var request = sender.Send();

            var response = handler.Handle(request);
            
            receiver.Receive(response);
        }
    }
}
