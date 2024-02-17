#nullable enable
using System;
using UnityEngine;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    public static class UnityFactoryResolutionExtension
    {
        public static IFactoryResolution Under(this IUnityFactoryResolution resolution, Func<Transform> getParentTransform)
        {
            var parentTransform = new ParentTransformToReturnCache(getParentTransform);

            return resolution.Under(parentTransform);
        }
        
        public static IFactoryResolution Under(this IUnityFactoryResolution resolution, Transform parent)
        {
            var parentTransform = new ParentTransformToReturnInstance(parent);

            return resolution.Under(parentTransform);
        }
    }
}
