#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class ServiceHandlerComponent : MonoBehaviour, IServiceHandler
    {
        [SerializeField] private string header = string.Empty;
        [SerializeField] private string footer = string.Empty;
        
        public string Handle(string request)
        {
            var response = $"{header}{request}{footer}";
            
            Debug.LogError($"{nameof(ServiceHandlerComponent)}.{nameof(Handle)}: {request} -> {response}");

            return response;
        }
    }
}
