#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class ReceiverComponent : MonoBehaviour
    {
        public void Receive(string content)
        {
            Debug.LogError($"{nameof(ReceiverComponent)}.{nameof(Receive)}: {content}");
        }
    }
}
