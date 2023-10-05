#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class ServiceComponent : MonoBehaviour
    {
        [InjectField] private SenderComponent? sender;
        [InjectField] private ReceiverComponent? receiver;

        private void Awake()
        {
            var content = sender!.Send();
            receiver!.Receive(content);
        }
    }
}
