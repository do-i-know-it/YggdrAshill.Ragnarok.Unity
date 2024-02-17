#nullable enable
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class ParentTransformToReturnInstance : IParentTransform
    {
        private readonly Transform parent;

        public ParentTransformToReturnInstance(Transform parent)
        {
            this.parent = parent;
        }
        
        public Transform? GetParentTransform()
        {
            return parent;
        }
    }
}
