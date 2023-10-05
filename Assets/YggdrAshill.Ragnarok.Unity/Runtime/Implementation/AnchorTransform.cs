#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    internal sealed class AnchorTransform : IAnchorTransform
    {
        private readonly Func<Transform?> getAnchorTransform;

        public AnchorTransform(Func<Transform?> getAnchorTransform)
        {
            this.getAnchorTransform = getAnchorTransform;
        }

        public AnchorTransform(Transform? anchorTransform) : this(() => anchorTransform)
        {
            
        }

        public Transform? GetAnchorTransform()
        {
            return getAnchorTransform.Invoke();
        }
    }
}
