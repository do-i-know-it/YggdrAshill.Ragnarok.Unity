#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class UnitySubContainerResolutionExtension
    {
        public static ITypeAssignment Under(this IUnitySubContainerResolution resolution, Func<Transform> anchor)
        {
            var anchorTransform = new AnchorTransform(anchor);

            return resolution.Under(anchorTransform);
        }
        
        public static ITypeAssignment Under(this IUnitySubContainerResolution resolution, Transform parent)
        {
            var anchorTransform = new AnchorTransform(parent);

            return resolution.Under(anchorTransform);
        }
    }
}
