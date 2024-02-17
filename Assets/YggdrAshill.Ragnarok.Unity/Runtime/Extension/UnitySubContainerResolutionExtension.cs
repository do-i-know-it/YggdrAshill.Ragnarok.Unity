#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class UnitySubContainerResolutionExtension
    {
        public static ITypeAssignment Under(this IUnitySubContainerResolution resolution, Func<Transform> getParentTransform)
        {
            var parentTransform = new ParentTransformToReturnCache(getParentTransform);
            
            return resolution.Under(parentTransform);
        }
        
        public static ITypeAssignment Under(this IUnitySubContainerResolution resolution, Transform parent)
        {
            var parentTransform = new ParentTransformToReturnInstance(parent);

            return resolution.Under(parentTransform);
        }
    }
}
