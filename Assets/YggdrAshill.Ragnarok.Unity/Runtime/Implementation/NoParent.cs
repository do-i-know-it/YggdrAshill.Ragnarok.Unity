#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class NoParent : IAnchorTransform
    {
        public static NoParent Instance { get; } = new();

        private NoParent()
        {
            
        }
        
        public Transform? GetAnchorTransform()
        {
            return null;
        }
    }
}
