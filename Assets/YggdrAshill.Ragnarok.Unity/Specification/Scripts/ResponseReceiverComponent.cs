#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class ResponseReceiverComponent : MonoBehaviour, IResponseReceiver
    {
        public void Receive(string response)
        {
            Debug.LogError($"{nameof(ResponseReceiverComponent)}.{nameof(Receive)}: {response}");
        }
    }
}
