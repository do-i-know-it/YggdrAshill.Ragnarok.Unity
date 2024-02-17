#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ParentTransformToReturnNothing : IParentTransform
    {
        public static ParentTransformToReturnNothing Instance { get; } = new();

        private ParentTransformToReturnNothing()
        {
            
        }
        
        public Transform? GetParentTransform()
        {
            return null;
        }
    }
}
