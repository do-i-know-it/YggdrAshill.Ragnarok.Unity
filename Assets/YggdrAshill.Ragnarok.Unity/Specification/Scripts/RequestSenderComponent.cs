#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class RequestSenderComponent : MonoBehaviour, IRequestSender
    {
        [SerializeField] private string request = string.Empty;
        
        public string Send()
        {
            Debug.LogError($"{nameof(RequestSenderComponent)}.{nameof(Send)}: {request}");

            return request;
        }
    }
}
