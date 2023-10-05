#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok.Specification
{
    internal sealed class SenderComponent : MonoBehaviour
    {
        [SerializeField] private string content = string.Empty;
        
        public string Send()
        {
            return content;
        }
    }
}
